using System;
using System.Linq;
using CoreMidiLib;
using MonoMac.CoreMidi;
using System.Collections.Generic;

namespace MidiMod
{
	public class MidiModifier
	{
		private MidiEndpoint endpoint;
		private MidiHandler client;

		List<ControlObject> controls;

		public MidiModifier ()
		{
			client = new MidiHandler ();
			client.SetupMidi ();
			client.CreateVirtualEndPoint ("MidiMod Output 1");

			controls = new List<ControlObject> ();
			controls.AddRange (Xone.GetXoneK2Controls ());

			client.MidiDataReceived += HandleMidiDataReceived;
		}

		void HandleMidiDataReceived (object sender, MidiDataReceivedEventArgs e)
		{
			ValidateMidiData (Xone.FilterMessages(e.Data));
		}

		void ValidateMidiData(MidiMessage msg)
		{
			//search msg in controls
			var matchedControl = controls.SingleOrDefault (c =>
				c.MidiChannel == msg.MidiChannel &&
				c.NoteNumber == msg.NoteNumber);

			//handle event
			if (matchedControl != null) {
				msg = matchedControl.HandleInput (msg);	
			}

			if (msg != null) {
				client.SendMidiMessage (msg);
			}
		}
	}
}

