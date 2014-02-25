using System;
using CoreMidiLib;

namespace MidiMod
{
	public class ToggleButton : ControlObject
	{
		public bool Value { get; set;}

		public ToggleButton(MidiMessageType MessageType, int MidiChannel, int NoteNumber)
		{
			this.MessageType = MessageType;
			this.MidiChannel = MidiChannel;
			this.NoteNumber = NoteNumber;

			this.Value = false;
		}

		public override MidiMessage HandleInput(MidiMessage msg)
		{
			if (msg.MessageType == MidiMessageType.NoteOn) {
				//note ON

				if (Value == true) {
#warning Default is NoteOff!
					msg.MessageType = MidiMessageType.NoteOn;
					msg.NoteVelocity = 0;
				}

			} else {
				//note OFF
				msg = null;
			}

			return msg;
		}

		public override MidiMessage HandleOutput(MidiMessage msg)
		{
			if (msg.MessageType == MidiMessageType.NoteOn) {
				this.Value = true;

				//Green (Orange = + 36 RED = + 72)
				msg.NoteNumber = this.NoteNumber + 72;
				msg.MessageType = MidiMessageType.NoteOn;
				msg.NoteVelocity = 127;
			
			} else {
				this.Value = false;

				//Red
				msg.MessageType = MidiMessageType.NoteOn;
				msg.NoteNumber = this.NoteNumber + 36;
				msg.NoteVelocity = 127;
			}
			return msg;
		}
	}
}

