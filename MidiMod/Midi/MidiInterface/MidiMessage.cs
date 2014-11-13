using System;

namespace CoreMidiLib
{
	public class MidiMessage
	{

		public MidiMessageType MessageType { get; set; }
		public Int32 MidiChannel { get; set; }
		public Int32 NoteNumber { get; set; }
		public Int32 NoteVelocity { get; set; }

		public long TimeStamp { get; set;}

		public byte[] SourceData { get; set;}

		public MidiMessage ()
		{
		}

		public MidiMessage(MidiMessageType MessageType, Int32 MidiChannel, Int32 NoteNumber, Int32 NoteVelocity)
		{
			this.MessageType = MessageType;
			this.MidiChannel = MidiChannel;
			this.NoteNumber = NoteNumber;
			this.NoteVelocity = NoteVelocity;
		}

		public void ParseRawData(byte[] midiData)
		{
			byte StatusByte = midiData [0];
			byte DataByte = midiData [1];
			byte USEMByte = midiData [2];

			byte typeData = (byte)((StatusByte & 0xF0) >> 4);
			byte channelData = (byte)(StatusByte & 0x0F);

			MidiMessageType mType = (MidiMessageType)typeData;

			//note on + value zero > note off
			if (mType == MidiMessageType.NoteOn && USEMByte == 0) {
				mType = MidiMessageType.NoteOff;
			}

			MessageType = mType;

			this.MidiChannel = channelData;

			this.NoteNumber = DataByte;
			this.NoteVelocity = USEMByte;
			this.SourceData = midiData;
		}

		public MonoMac.CoreMidi.MidiPacket ToMidiPacket()
		{
			byte[] data = new byte[3];

			//cast
			byte messageType = (byte)MessageType;
			byte midiChannel = (byte)MidiChannel;

			//NoteNumber
			data [0] = (byte)((messageType << 4) | midiChannel);
			data [1] = (byte)NoteNumber;
			data [2] = (byte)NoteVelocity;

			return new MonoMac.CoreMidi.MidiPacket (this.TimeStamp, data);
		}

		public override string ToString ()
		{
			return string.Format ("[MidiMessage: MessageType={0}, MidiChannel={1}, NoteNumber={2}, NoteVelocity={3}]", MessageType, MidiChannel, NoteNumber, NoteVelocity);
		}
	}
}

