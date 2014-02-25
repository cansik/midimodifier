using System;

namespace CoreMidiLib
{
	[Flags]
	public enum MidiMessageType
	{
		NoteOff = 0x8,
		NoteOn = 0x9,
		PolyphonicKeyPressure = 0xA,
		ControlChange = 0xB,
		ProgramChange = 0xC,
		ChannelPressure = 0xD,
		PitchWheelChange = 0xE,
		SystemCommon = 0xF,
	}
}

