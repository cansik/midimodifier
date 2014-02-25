using System;
using MonoMac.CoreMidi;
using System.Runtime.InteropServices;

namespace CoreMidiLib
{
	public delegate void MidiDataReceivedEventHandler(object sender, MidiDataReceivedEventArgs e);

	public class MidiHandler
	{
		private MidiClient client;
		private MidiPort outputPort;
		private MidiPort inputPort;
		private MidiEndpoint endpoint;

		public event MidiDataReceivedEventHandler MidiDataReceived;

		public MidiHandler ()
		{
		}

		public void SetupMidi()
		{
			//Midi.Restart ();

			//create new client
			client = new MidiClient ("MidiMod");

			//create input and output port
			outputPort = client.CreateOutputPort ("MidiMod Output Port");
			inputPort = client.CreateInputPort ("MidiMod Input Port");

			//create event handlers
//			client.ObjectAdded += HandleObjectAdded;
//			client.ObjectRemoved += HandleObjectRemoved;
//			client.PropertyChanged += HandlePropertyChanged;
//			client.ThruConnectionsChanged += HandleThruConnectionsChanged;
//			client.SerialPortOwnerChanged += HandleSerialPortOwnerChanged;
//			client.IOError += HandleIOError;
			inputPort.MessageReceived += HandleMessageReceived;

			//connect midi devices
			ConnectMidiDevices ();
		}

		void HandleIOError (object sender, IOErrorEventArgs e)
		{
			Console.WriteLine (e.ErrorCode.ToString () + " - " + e.Device.DisplayName);
		}

		void ConnectMidiDevices()
		{
			for (int i = 0; i < Midi.SourceCount; i++) {
				var ep = MidiEndpoint.GetSource (i);
				var inCode = inputPort.ConnectSource (ep);

				if (inCode != MidiError.Ok) {
					Console.WriteLine (i.ToString() + " - " + ep.DisplayName + " failed to add midi in device");
				} else {
					Console.WriteLine (i.ToString() + " - " + ep.DisplayName + ". in device added");
				}
			}

//			for (int i = 0; i < Midi.SourceCount; i++) {
//				var ep = MidiEndpoint.GetDestination (i);
//				var outCode = outputPort.ConnectSource (ep);
//
//				if (outCode != MidiError.Ok) {
//					Console.WriteLine (i.ToString() + " - " + ep.DisplayName + " failed to add midi out device");
//				} else {
//					Console.WriteLine (i.ToString() + " - " + ep.DisplayName + ". out device added");
//				}
//
//				endpoint = ep;
//			}

			//TEST ------------------------------------------
		}

		public void CreateVirtualEndPoint(string name)
		{
			endpoint = client.CreateVirtualSource (name);
			endpoint.ReceiveChannels = 1;
			endpoint.TransmitChannels = 1;
			outputPort.ConnectSource (endpoint);
			endpoint.MessageReceived += endpointMessageReceived;
		}

		void endpointMessageReceived (object sender, MidiPacketsEventArgs e)
		{
			Console.WriteLine ("EP: received data");
		}

		public void SendMidiMessage(MidiMessage msg)
		{
			var packages = new MidiPacket[] { msg.ToMidiPacket () };
			//endpoint.Received (packages);
			this.outputPort.Send (endpoint, packages);
		}

		void HandleMessageReceived (object sender, MidiPacketsEventArgs e)
		{
			foreach (MidiPacket mPacket in e.Packets) {
				var midiData = new byte[mPacket.Length];
				Marshal.Copy (mPacket.Bytes, midiData, 0, mPacket.Length);

				MidiMessage message = new MidiMessage ();
				message.ParseRawData (midiData);
				message.TimeStamp = mPacket.TimeStamp;

				//Console.WriteLine("type: {0} channel: {1} note: {2} velocity: {3}", message.MessageType.ToString(), message.MidiChannel, message.NoteNumber, message.NoteVelocity);

				//rais event
				if (MidiDataReceived != null) {
					MidiDataReceived (this, new MidiDataReceivedEventArgs (message));
				}
			}
		}

		void HandleSerialPortOwnerChanged (object sender, EventArgs e)
		{
			
		}

		void HandleThruConnectionsChanged (object sender, EventArgs e)
		{
			
		}

		void HandlePropertyChanged (object sender, ObjectPropertyChangedEventArgs e)
		{
			
		}

		void HandleObjectAdded (object sender, ObjectAddedOrRemovedEventArgs e)
		{
			
		}

		void HandleObjectRemoved (object sender, ObjectAddedOrRemovedEventArgs e)
		{
			
		}

	}
}

