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

		public override MidiMessage HandleInput(MidiMessage msg, MidiRouter router)
		{
			System.Threading.Thread.Sleep (200);
			if (msg.NoteVelocity == 1) {
				//right
				msg.NoteNumber += this.IncRange;
				msg.NoteVelocity = 127;
			}
			return msg;
		}

		public override MidiMessage HandleOutput(MidiMessage msg, MidiRouter router)
		{
			return msg;
		}
	}
}

