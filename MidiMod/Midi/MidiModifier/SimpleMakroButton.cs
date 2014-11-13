using System;
using CoreMidiLib;
using System.ComponentModel;

namespace MidiMod
{
	public class SimpleMakroButton : ControlObject
	{
		BackgroundWorker worker;
		bool running;

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
			running = true;

			while (running) {
				Flash (router);
			}

			//even count
			/*for (int i = 0; i < 6; i++) {
				router.TransmitMidiData (router.Endpoint2, new MidiMessage (MidiMessageType.NoteOn, 14, 59, 127));
				System.Threading.Thread.Sleep (50);
			}*/
		}

		void Flash(MidiRouter router)
		{
			router.TransmitMidiData (router.Endpoint2, new MidiMessage (MidiMessageType.NoteOn, 14, 59, 127));
			router.TransmitMidiData (router.Endpoint1, new MidiMessage (MidiMessageType.NoteOn, 14, NoteNumber + 72, 127)); // color
			System.Threading.Thread.Sleep (50);
			router.TransmitMidiData (router.Endpoint2, new MidiMessage (MidiMessageType.NoteOff, 14, 59, 0));
			router.TransmitMidiData (router.Endpoint1, new MidiMessage (MidiMessageType.NoteOn, 14, NoteNumber + 36, 127)); // color
			System.Threading.Thread.Sleep (50);
		}

		public override MidiMessage HandleInput(MidiMessage msg, MidiRouter router)
		{
			if(msg.MessageType == MidiMessageType.NoteOn)
				if (!worker.IsBusy)
					worker.RunWorkerAsync (router);

			if (msg.MessageType == MidiMessageType.NoteOff)
				running = false;

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

