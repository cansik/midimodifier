using System;
using MonoMac.CoreMidi;
using System.Runtime.InteropServices;

namespace CoreMidiLib
{
	public class MidiRouter
	{
		private MidiClient client;

		public MidiPort InputPort1 { get; set; }
		public MidiPort InputPort2 { get; set; }

		private MidiPort outputPort;

		public MidiEndpoint Endpoint1 { get; set; }
		public MidiEndpoint Endpoint2 { get; set; }

		public event MidiDataReceivedEventHandler MidiDataReceivedInput1;
		public event MidiDataReceivedEventHandler MidiDataReceivedInput2;

		public MidiRouter ()
		{
			Midi.Restart ();
			client = new MidiClient ("MidiRouter");

			InputPort1 = client.CreateInputPort ("mdrInput1");
			InputPort2 = client.CreateInputPort ("mdrInput2");

			outputPort = client.CreateOutputPort ("mdrOutput");

			InputPort1.MessageReceived += HandleMessageReceivedInput1;
			InputPort2.MessageReceived += HandleMessageReceivedInput2;
		}

		public void SetMidiDevice(int epNumber, MidiPort inputport, string name)
		{
			for (int i = 0; i < Midi.SourceCount; i++) {
				var ep = MidiEndpoint.GetSource (i);
				if (ep.Name.Contains (name)) {
					var inCode = inputport.ConnectSource (ep);

					if (inCode != MidiError.Ok) {
						Console.WriteLine (i.ToString () + " - " + ep.DisplayName + " failed to add midi in device");
					} else {
						Console.WriteLine (i.ToString () + " - " + ep.DisplayName + " in device added");
					}
				}
			}

			for (int i = 0; i < Midi.DestinationCount; i++) {
				var ep = MidiEndpoint.GetDestination (i);
				if (ep.Name.Contains (name)) {
					if (epNumber == 1) {
						this.Endpoint1 = ep;
					} else {
						this.Endpoint2 = ep;
					}

				}
			}
		}

		void HandleMessageReceivedInput2 (object sender, MidiPacketsEventArgs e)
		{
			HandleMessageReceived (MidiDataReceivedInput2, e);
		}

		void HandleMessageReceivedInput1 (object sender, MidiPacketsEventArgs e)
		{
			HandleMessageReceived (MidiDataReceivedInput1, e);
		}

		void HandleMessageReceived(MidiDataReceivedEventHandler eventHandler, MidiPacketsEventArgs e)
		{
			foreach (MidiPacket mPacket in e.Packets) {
				var midiData = new byte[mPacket.Length];
				Marshal.Copy (mPacket.Bytes, midiData, 0, mPacket.Length);

				MidiMessage message = new MidiMessage ();
				message.ParseRawData (midiData);
				message.TimeStamp = mPacket.TimeStamp;

				//rais event
				if (eventHandler != null) {
					eventHandler (this, new MidiDataReceivedEventArgs (message));
				}
			}
		}

		public void TransmitMidiData(MidiEndpoint endpoint, MidiMessage msg)
		{
			var packages = new MidiPacket[] { msg.ToMidiPacket () };
			outputPort.Send (endpoint, packages);
		}
	}
}

