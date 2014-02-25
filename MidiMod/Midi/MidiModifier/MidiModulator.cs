using System;
using CoreMidiLib;
using System.Collections.Generic;
using System.Linq;

namespace MidiMod
{
	public class MidiModulator
	{
		MidiRouter router;
		List<ControlObject> controls;

		public bool MappingMod { get; set; }

		public MidiModulator ()
		{
			router = new MidiRouter ();
			router.MidiDataReceivedInput1 += HandleMidiDataReceivedInput1;
			router.MidiDataReceivedInput2 += HandleMidiDataReceivedInput2;

			//Connect Hardware XONE:k2
			router.SetMidiDevice (1,
				router.InputPort1,
				"XONE");

			//Connect ICA Driver
			router.SetMidiDevice (2,
				router.InputPort2,
				"MidiModLoop");

			controls = new List<ControlObject> ();
			controls.AddRange (Xone.GetXoneK2Controls ());

			MappingMod = false;

			InitControls ();
			Console.WriteLine ("modulator finished");
		}

		void InitControls()
		{
			Console.WriteLine ("init controls...");
			foreach (ControlObject co in controls) {
				System.Threading.Thread.Sleep (25);
				MidiMessage msg = co.InitControl ();
				if (msg != null) {
					router.TransmitMidiData (router.Endpoint2, msg);
				}
			}
		}

		void HandleMidiDataReceivedInput2 (object sender, MidiDataReceivedEventArgs e)
		{
			//Console.WriteLine ("I2: " + e.Data.ToString ());
			if (!MappingMod) {
				ValidateBackwardMidiData (e.Data);
			}
		}

		void HandleMidiDataReceivedInput1 (object sender, MidiDataReceivedEventArgs e)
		{
			//Hardware Input
			ValidateForwardMidiData (Xone.FilterMessages (e.Data));
		}

		void ValidateForwardMidiData(MidiMessage msg)
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
				router.TransmitMidiData (router.Endpoint2, msg);
			}
		}

		void ValidateBackwardMidiData(MidiMessage msg)
		{
			//search msg in controls
			var matchedControl = controls.SingleOrDefault (c =>
				c.MidiChannel == msg.MidiChannel &&
				c.NoteNumber == msg.NoteNumber);

			//handle event
			if (matchedControl != null) {
				msg = matchedControl.HandleOutput (msg);	
			}

			if (msg != null) {
				router.TransmitMidiData (router.Endpoint1, msg);
			}
		}
	}
}

