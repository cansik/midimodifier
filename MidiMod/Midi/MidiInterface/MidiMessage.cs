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

			//split data byte (maybe not so nice!)
			string bitData = Convert.ToString(StatusByte,2);

			//fill with zeros
			while (bitData.Length < 8) {
				bitData = 0 + bitData;
			}

			string typeData = bitData.Substring (0, 4);
			string channelData = bitData.Substring (4, 4);

			MidiMessageType mType = (MidiMessageType)BitToByte(typeData);

			//note on + value zero > note off
			if (mType == MidiMessageType.NoteOn && USEMByte == 0) {
				mType = MidiMessageType.NoteOff;
			}

			this.MessageType = mType;

			this.MidiChannel = BitToByte(channelData);

			this.NoteNumber = DataByte;
			this.NoteVelocity = USEMByte;
			this.SourceData = midiData;
		}

		public MonoMac.CoreMidi.MidiPacket ToMidiPacket()
		{
			byte[] data = new byte[3];

			//NoteNumber
			string binary = Convert.ToString ((byte)this.MessageType, 2) + Convert.ToString ((byte)this.MidiChannel, 2);
			data [0] = BitToByte(binary);
			data [1] = (byte)this.NoteNumber;
			data [2] = (byte)this.NoteVelocity;

			return new MonoMac.CoreMidi.MidiPacket (this.TimeStamp, data);
		}

		public override string ToString ()
		{
			return string.Format ("[MidiMessage: MessageType={0}, MidiChannel={1}, NoteNumber={2}, NoteVelocity={3}]", MessageType, MidiChannel, NoteNumber, NoteVelocity);
		}

		private byte BitToByte(string bit)
		{
			int data = 0;
			int dataslot = 1;

			for (int i = bit.Length - 1; i >= 0; i--) {
				if (bit [i].ToString () == "1") {
					data += dataslot;
				}
				
				dataslot = dataslot * 2;
			}

			return (byte)data;
		}
	}
}

