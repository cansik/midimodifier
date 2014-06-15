using System;
using CoreMidiLib;

namespace MidiMod
{
	public class SimpleButton : ControlObject
	{
		public SimpleButton(MidiMessageType MessageType, int MidiChannel, int NoteNumber)
		{
			this.MessageType = MessageType;
			this.MidiChannel = MidiChannel;
			this.NoteNumber = NoteNumber;
		}

		public override MidiMessage HandleInput(MidiMessage msg, MidiRouter router)
		{
			return msg;
		}

		public override MidiMessage HandleOutput(MidiMessage msg, MidiRouter router)
		{
			//Green (Orange = + 36)
			msg.NoteNumber = this.NoteNumber;
			msg.MessageType = MidiMessageType.NoteOn;
			msg.NoteVelocity = 127;
			return msg;
		}
	}
}

