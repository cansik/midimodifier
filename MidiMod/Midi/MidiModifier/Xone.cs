using System;
using CoreMidiLib;
using System.Collections.Generic;

namespace MidiMod
{
	public static class Xone
	{
		static int channel = 14;
		static int ctrlCount = 60;

		public static List<ControlObject> GetXoneK2Controls()
		{
			List<ControlObject> controls = new List<ControlObject> ();

			//Top Encoder
			for(int i = 0; i <= 3; i++)
			{
				controls.Add (new TwoWayEncoder (MidiMessageType.ControlChange, channel, i, ctrlCount));
			}

			//Bottom Encoder
			for(int i = 20; i <= 21; i++)
			{
				controls.Add (new TwoWayEncoder (MidiMessageType.ControlChange, channel, i, ctrlCount));
			}

			//Upper buttons
			for(int i = 24; i <= 39; i++)
			{
				controls.Add (new ToggleButton (MidiMessageType.NoteOn, channel, i));
			}

			//mid
			for(int i = 40; i <= 41; i++)
			{
				controls.Add (new SimpleButton (MidiMessageType.NoteOn, channel, i));
			}

			//Upper buttons
			for(int i = 42; i <= 47; i++)
			{
				controls.Add (new ToggleButton (MidiMessageType.NoteOn, channel, i));
			}

			//mid
			for(int i = 48; i <= 51; i++)
			{
				controls.Add (new SimpleButton (MidiMessageType.NoteOn, channel, i));
			}

			//Upper buttons
			for(int i = 52; i <= 55; i++)
			{
				controls.Add (new ToggleButton (MidiMessageType.NoteOn, channel, i));
			}

			//lowest
			for(int i = 56; i <= 59; i++)
			{
				controls.Add (new SimpleButton (MidiMessageType.NoteOn, channel, i));
			}

			return controls;
		}

		public static MidiMessage FilterMessages(MidiMessage msg)
		{
			//Pitch up bottom buttons
			if ((msg.MessageType == MidiMessageType.NoteOn || msg.MessageType == MidiMessageType.NoteOff)
				&& (msg.NoteNumber >= 12 && msg.NoteNumber <= 15)) {
				msg.NoteNumber += 44;
			}

			return msg;
		}
	}
}

