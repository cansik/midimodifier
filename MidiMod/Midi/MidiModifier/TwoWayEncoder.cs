using System;
using CoreMidiLib;

namespace MidiMod
{
	public class TwoWayEncoder : ControlObject
	{
		public int IncRange { get; set;}

		public TwoWayEncoder (MidiMessageType MessageType, int MidiChannel, int NoteNumber, int IncRange)
		{
			this.MessageType = MessageType;
			this.MidiChannel = MidiChannel;
			this.NoteNumber = NoteNumber;
			this.IncRange = IncRange;
		}

		public override MidiMessage HandleInput(MidiMessage msg)
		{
			if (msg.NoteVelocity == 1) {
				//right
				msg.NoteNumber += this.IncRange;
				msg.NoteVelocity = 127;
			}
			return msg;
		}

		public override MidiMessage HandleOutput(MidiMessage msg)
		{
			return msg;
		}
	}
}

