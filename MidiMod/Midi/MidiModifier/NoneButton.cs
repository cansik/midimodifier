using System;
using CoreMidiLib;

namespace MidiMod
{
	public class NoneButton : SimpleButton
	{
		public NoneButton(MidiMessageType MessageType, int MidiChannel, int NoteNumber) : base(MessageType, MidiChannel, NoteNumber)
		{

		}

		public override MidiMessage HandleInput (MidiMessage msg, MidiRouter router)
		{
			return null;
		}
	}
}

