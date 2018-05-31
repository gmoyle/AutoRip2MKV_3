# AutoRip2MKV_3

A complete rewrite of the AutoRip2MKV originally written in Autoit.  This is my first attempt at a C# progam.

User assumes all risks and liabilities for use of this applicaton.

It is intended as a headless ripping application for your disc library. It relies on MakeMKV for ripping and Handbrake for conversion to mp4.

This is the document from the orginal. **** items are currently not completed.

What it does for you:

* Rip any DVD or Bluray (MakeMKV License for Blueray required) to an MKV file at full quality, including chapters and dolby 5.1. 
* No decryptor software required (handled by MakeMKV)
* ****Optionally convert the MKV to an Apple Universal file format .MP4 file to the same location.
* ****Optionally delete the MKV file if you do not want it (it is required for the initial rip)
* Copy the completed files to a final location (if specified, see readme)
* Eject your DVD and wait for another 
* If you assign Rip using Autorip2MKV as your DVD or Bluray default insert functionality, it is completely user interaction free once installed.

You can leave the Temp Folder empty and it will rip directly to the FinalPath.

Temp and directory folders now function as intended.  Put whatever folders you want there.  The only caveat is they MUST be a subfolder and not a root drive.

You can customize the HandbrakeCMDLine to convert to match the hardware you will play it on.
Some examples are available here. ONLY include the settings from the file extension onward.  The rest is hardcoded.
https://trac.handbrake.fr/wiki/BuiltInPresets

MinimumTitleLength determines if features or other items on the DVD are included in the rip.  The default value prevents most extraneous stuff from being included (about 20 minutes).  If you rip Episodic TV shows or videos shorter than that, you may need to adjust this value.

