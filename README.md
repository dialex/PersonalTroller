# Personal Troller

<img src="https://raw.githubusercontent.com/dialex/PersonalTroller/master/TrollerProject/Resources/ogre.png" width="150">

This code builds an executable that enables you to troll someone by:

- Showing a dialog message
- Opening a new tab on for a specific URL
- Opening and closing the CD/DVD drive several times
- Shutting down or logging off displaying a message

## How to use

1. Copy the executable (`Troller.exe`) and the trolling actions file (`Tasks.txt`) to your victim's pc.
2. Open the executable.
3. Enjoy.

The executable searches for a `Tasks.txt` file containing the trolling actions to perform. If it doesn't exists, the executable will create one with default actions.

## Download

You can build the source files to get the latest `Troller.exe` or download this [one ready to use](http://www.diogonunes.com/assets/downloadmanager/click.php?id=11).

## How to configure Troller's tasks

Each line is a command. The syntax is `action|parameter`. Don't surround the `|` separator with spaces.

- Start by specifying the time to start (`BEGIN` action) and suspend (`END` action) the troller.
- Then specify the time interval between trolling actions (`EVERY` action).
- All of these `actions` receive a time `parameter` in the format `HH:mm:ss`. 

Trolling actions:

- To show a dialog message, `action` is **`MESSAGE`** and `parameter` the **`message`** to display.
- To open a new tab, `action` is **`OPENURL`** and `parameter` the **`link`** to open (include `http://`).
- To open the disk drive, `action` is **`DISKDRV`** and `parameter` the **`number`** of times to close and open the drive.
- To shutdown the computer, `action` is **`SHUTDWN`** and `parameter` the **`message`** to display 15min before doing it.
- To logoff the user, `action` is **`LOGUOFF`** and `parameter` the **`message`** to display 15min before doing it.
 
Example:

```
BEGIN|09:42:57
EVERY|00:19:23
END|17:47:14

OPENURL|http://www.sanger.dk/
OPENURL|http://www.ringingtelephone.com/
OPENURL|http://cachemonet.com/
DISKDRV|1
OPENURL|http://giantbatfarts.com/
OPENURL|http://www.ooooiiii.com/
OPENURL|http://cat-bounce.com/
OPENURL|http://www.iiiiiiii.com/
DISKDRV|3
OPENURL|http://leekspin.com/
OPENURL|http://iamawesome.com/
OPENURL|http://www.nelson-haha.com/
DISKDRV|5
OPENURL|http://www.muchbetterthanthis.com/
OPENURL|http://baconsizzling.com/
DISKDRV|7
OPENURL|http://www.sadtrombone.com/?autoplay=true
MESSAGE|I can't wait for tomorrow :)
```

## How it works

Once started, the executable keeps running until the pc is turned off. If you really hate your victim you can add the executable to the startup folder, that way the Troller.exe will start on every time the pc is turned on.

The Troller reads the time configurations and keeps running between the `BEGIN` and `END` time, executing a trolling action on each `EVERY` time configuration.

Even if the victim opens the **Task Manager** searching for your process, it appears listed as **Host Process For Driver Compatibility**. He/she will never guess that's a trolling process.

:green_heart: The Troller is *environmentally friendly*, suspending while idle and hibernating to the next day, keeping resource consumption to a minimum.

## New features & License

Personal Troller Copyright (C) 2015 [Diogo Nunes](http://www.diogonunes.com/)
This program is licensed under the terms of the MIT License and it comes with ABSOLUTELY NO WARRANTY. For more details check LICENSE.

**Be responsible - this program is for fun, not for harm.** Both creator and contributors of this program cannot be held responsible for the consequences of how others use it.

If you liked this software, consider making a [donation](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=DGR2KAV5RLGBW) :angel: or contributing with a [new trolling action](https://github.com/dialex/PersonalTroller/pulls) :smiling_imp:.
