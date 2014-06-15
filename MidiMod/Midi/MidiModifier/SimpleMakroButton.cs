using System;
using CoreMidiLib;
using System.ComponentModel;

namespace MidiMod
{
	public class SimpleMakroButton : ControlObject
	{
		BackgroundWorker worker;

		public SimpleMakroButton(MidiMessageType MessageType, int MidiChannel, int NoteNumber)
		{
			this.MessageType = MessageType;
			this.MidiChannel = MidiChannel;
			this.NoteNumber = NoteNumber;

			worker = new BackgroundWorker ();
			worker.DoWork += HandleDoWork;
		}

		void HandleDoWork (object sender, DoWorkEventArgs e)
		{
			MidiRouter router = (MidiRouter)e.Argument;
			//even count
			for (int i = 0; i < 6; i++) {
				router.TransmitMidiData (router.Endpoint2, new MidiMessage (MidiMessageType.NoteOn, 14, 59, 127));
				System.Threading.Thread.Sleep (50);
			}
		}

		public override MidiMessage HandleInput(MidiMessage msg, MidiRouter router)
		{
			if(!worker.IsBusy)
				worker.RunWorkerAsync (router);

			return null;
		}

		public override MidiMessage HandleOutput(MidiMessage msg, MidiRouter router)
		{
			//Green (Orange = + 36)
			msg.NoteNumber = this.NoteNumber + 36;
			msg.MessageType = MidiMessageType.NoteOn;
			msg.NoteVelocity = 127;
			return msg;
		}
	}
}

