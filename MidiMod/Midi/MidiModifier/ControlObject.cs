using System;
using CoreMidiLib;

namespace MidiMod
{
	public abstract class ControlObject
	{		
		public MidiMessageType MessageType { get; set; }
		public Int32 MidiChannel { get; set; }
		public Int32 NoteNumber { get; set; }

		public abstract MidiMessage HandleInput(MidiMessage msg);
		public abstract MidiMessage HandleOutput(MidiMessage msg);

		public MidiMessage InitControl()
		{
			MidiMessage msg = new MidiMessage ();
			msg.MidiChannel = this.MidiChannel;
			msg.MessageType = this.MessageType;
			msg.NoteNumber = this.NoteNumber;
			msg.NoteVelocity = 0;

			return msg;
		}
	}
}

