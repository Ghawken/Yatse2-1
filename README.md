# Yatse2-1
YATSE 2 - A touch screen remote controller for XBMC (.NET 3.5)

Hi

Welcome to the newer Yatse2

For those not familiar Yatse2 was created as a remote and 'nowplaying' info service for Xbmc and now Kodi.

It was created by Tolriq who has continued with Android remote control called Yatse.

Yatse2 fulls a huge need of info on screen screen TFT for those with HTPC cases that include a second TFT/Monitor.
(Note this differs from LCD/VFD displays which are small one or two line single color devices, there is ongoing support for these through other software.  Not of the VFD/LCD software supports a TFT/Full screen monitor like Yatse)

I've made some minor modifications to the original code
The Binaries are included in the release - sorry no install
Just copy the Program File and run (does need .net installed)

Most Settings are available through the Yatse2.xml file:
Should be straightforward.

My changes
- Undims on Pause
- MinimiseAlways Setting - in yatse2.xml file
If enable will always minimise Yatse2 to taskbar if not playing.
Ideal if wish to use Yatse2 for nowplaying information - but would like other information at other times.
ie. webpage, other software etc.
If double click taskbar icon - MinimiseAlways is disabled to enable settings to be altered.
If double click again - and Yatse2 shrinks MinimiseAlways is reenabled.
- Minor bug fixes - minimising how many times it checks for dim/undim



- Fanart
Moderated change to JSON and Display
If like me you use your displaying only to look pretty and not as a remote - the default screen was a bit average
I have modifed the JSON code to let Yatse2 know what menu is being browsed by the user and to play fanart accordingly.
Unfortunately I was hopeful to distinguish between TV and Movie menus but I believe this is not possible currently through JSON.

None-the-less
New Setting: Fanart - Enable
This will when not playing always play a slide show of your fanart.
The Directories are selected through the Yatse2.xml file as below.
The must be within %appdata%\Kodi\userdata

eg. as below

  <FanartDirectoryTV>addon_data\script.artworkorganizer\TVShowFanart\</FanartDirectoryTV>
  <FanartDirectoryMovie>addon_data\script.artworkorganizer\ArtistFanart\</FanartDirectoryMovie>
  <FanartDirectoryWeather>addon_data\skin.aeonmq5.extrapack\backgrounds_weather\</FanartDirectoryWeather>
  <FanartDirectoryMyImages>addon_data\script.artworkorganizer\OwnFanart\</FanartDirectoryMyImages>
  <FanartDirectoryMusic>addon_data\script.artworkorganizer\ArtistFanart\</FanartDirectoryMusic>
  
 FanartDirectoryTV - Defaults when on main menu
 FanartDirectoryMovie - Defaults when browswing Video folder, unfortunately also TV
 FanartDirectoryWeather - Weather images when in weather menu
 FanartDirectoryMyImages - if in pictures - show these images
 FanartDirectoryMusic - if browsing Music
 
 If no images in a directory - will default to control screen.
 Also if browsing settings menu will default to control screen if need access to change or disable settings.
 
 To disable Fanart - go to Settings menu on Kodi - enter - Fanart will stop, and can be disabled from Settings within Yatse2
 
