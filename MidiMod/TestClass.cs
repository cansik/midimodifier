using System;
using MonoMac.CoreMidi;

namespace MidiMod
{
	public class TestClass
	{
		public TestClass ()
		{
		}

		public void SimpleMidiTest()
		{
			//create midi client to use core midi features
			MidiClient client = new MidiClient ("MidiMod");

			//create input port to listen to incoming midi data
			MidiPort inputPort = client.CreateInputPort ("MidiMod Input Port");
			inputPort.MessageReceived += delegate(object sender, MidiPacketsEventArgs e) {
				Console.WriteLine("message received");
			};

			//connect midi devices to inputport (only source 0)
			var ep = MidiEndpoint.GetSource (0);
			inputPort.ConnectSource (ep);
		}
	}
}

