
using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using System.Diagnostics;
using System.ComponentModel;

namespace MidiMod
{
	public partial class AppDelegate : NSApplicationDelegate
	{
		//MainWindowController mainWindowController;

		MidiModulator mod;
		NSMenuItem statusMenuItem;
		NSStatusItem sItem;

		public AppDelegate ()
		{
		}

		public override void FinishedLaunching (NSObject notification)
		{
//			mainWindowController = new MainWindowController ();
//			mainWindowController.Window.MakeKeyAndOrderFront (this);

			// Construct menu that will be displayed when tray icon is clicked
			var notifyMenu = new NSMenu();
			var exitMenuItem = new NSMenuItem("Quit", 
				(a,b) => { System.Environment.Exit(0); });

			var startMidiModMenuItem = new NSMenuItem("Run", 
				(a,b) => { RunMidiMod(); });

			var mappingmodMidiModMenuItem = new NSMenuItem("Mapping Mod", 
				(a,b) => { ActivateMappingMod(); });

			statusMenuItem = new NSMenuItem("STATUS", 
				(a,b) => {  });
			statusMenuItem.Enabled = false;

			var versionMenuItem = new NSMenuItem("Version 1.0", 
				(a,b) => {  });
			versionMenuItem.Enabled = false;

			var seperatorItem = NSMenuItem.SeparatorItem;

			notifyMenu.AutoEnablesItems = false;


			// Just add 'Quit' command
			notifyMenu.AddItem (statusMenuItem);
			notifyMenu.AddItem (versionMenuItem);
			notifyMenu.AddItem (seperatorItem);
			notifyMenu.AddItem (startMidiModMenuItem);
			notifyMenu.AddItem (mappingmodMidiModMenuItem);
			notifyMenu.AddItem(exitMenuItem);

			// Display tray icon in upper-right-hand corner of the screen
			sItem = NSStatusBar.SystemStatusBar.CreateStatusItem(16); //def 16
			sItem.Menu = notifyMenu;
			sItem.Title = "MidiMod";
			sItem.ToolTip = "Midi Mod";
			sItem.Image = NSImage.FromStream(System.IO.File.OpenRead(NSBundle.MainBundle.ResourcePath + @"/9b244f1232672041.icns"));
			sItem.HighlightMode = true;

			UpdateStatus ("Mod not startet");

			// Remove the system tray icon from upper-right hand corner of the screen
		 	// (works without adjusting the LSUIElement setting in Info.plist)
			NSApplication.SharedApplication.ActivationPolicy = 
				NSApplicationActivationPolicy.Accessory;
		}

		void UpdateStatus(string msg)
		{
			statusMenuItem.InvokeOnMainThread (delegate {
				statusMenuItem.Title = msg;
			});
		}

		void UpdateTitle(string msg)
		{
			statusMenuItem.InvokeOnMainThread (delegate {
				sItem.Length = msg.Length * 6 + 16;
				sItem.Title = msg;
			});
		}

		void RunMidiMod()
		{
			UpdateStatus ("starting mod...");
			UpdateTitle("starting...");
			BackgroundWorker worker = new BackgroundWorker ();
			worker.DoWork += (object sender, DoWorkEventArgs e) => {
				mod = new MidiModulator ();
				UpdateStatus ("mod started!");
				UpdateTitle("");
			};
				
			worker.RunWorkerAsync ();
		}

		void ActivateMappingMod()
		{
			if (mod != null) {
				mod.MappingMod = !mod.MappingMod;

				if (mod.MappingMod) {
					UpdateStatus ("mapping modus activ");
				} else {
					UpdateStatus ("Mod started");
				}
			} else {
				UpdateStatus ("Mod not started yet");
			}
		}
	}
}

