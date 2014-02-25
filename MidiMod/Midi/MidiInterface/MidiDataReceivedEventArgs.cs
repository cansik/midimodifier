using System;
using MonoMac.CoreMidi;
using System.Runtime.InteropServices;

namespace CoreMidiLib
{
	public class MidiDataReceivedEventArgs : EventArgs
	{
		public MidiMessage Data { get; set; }

		public MidiDataReceivedEventArgs(MidiMessage Data)
		{
			this.Data = Data;
		}
	}
}

