# Personal Troller

<img src="https://raw.githubusercontent.com/dialex/PersonalTroller/master/TrollerProject/Resources/ogre.png" width="150">

This C# code builds an executable that enables you to troll someone by:

- Showing a dialog message
- Opening a new tab on for a specific URL
- Opening and closing the CD/DVD drive several times

## How to use

1. Copy the executable (`Troller.exe`) and the trolling actions file (`Tasks.txt`) to your victim's pc.
2. Open the executable.
3. Enjoy.

The executable searches for a `Tasks.txt` file containing the trolling actions to perform. If it doesn't exists, the executable will create one with default actions. Make sure you hide these files in a well hidden folder.

## How to configure Troller's tasks

Each line is a command. The syntax is `action|parameter`. Don't use spaces, use the `|` separator.

- Start by specifying the time to start (`BEGIN` action) and suspend (`END` action) the troller.
- Then specify the time interval between trolling actions (`EVERY` action).
- All of these `actions` receive a time `parameter` in the format `HH:mm:ss`. 

Trolling actions:

- To show a dialog message, `action` is **`MSG`** and `parameter` the **`message`** to display.
- To open a new tab, `action` is **`URL`** and `parameter` the **`link`** to open (include `http://`).
- To open the disk drive, `action` is **`DSK`** and `parameter` the **`number`** of times to close and open the drive.
 
Example:

```
BEGIN|09:42:57
EVERY|00:19:23
END|17:47:14

URL|http://www.sanger.dk/
URL|http://www.ringingtelephone.com/
URL|http://cachemonet.com/
DSK|1
URL|http://cat-bounce.com/
URL|http://giantbatfarts.com/
URL|http://www.ooooiiii.com/
URL|http://www.iiiiiiii.com/
DSK|3
URL|http://iamawesome.com/
URL|http://www.nelson-haha.com/
DSK|5
URL|http://www.muchbetterthanthis.com/
URL|http://baconsizzling.com/
URL|http://leekspin.com/
DSK|7
URL|http://www.sadtrombone.com/?autoplay=true
MSG|I can't wait for tomorrow :)
```

## How it works

Once started, the executable keeps running until the pc is turned off. If you really hate your victim you can add the executable to the startup folder, that way the Troller.exe will start on every time the pc is turned on.

The Troller reads the time configurations and keeps running between the `BEGIN` and `END` time, executing a trolling action on each `EVERY` time configuration.

Even if the victim opens the **Task Manager** searching for your process, it appears listed as **Host Process For Driver Compatibility**. He/she will never guess that's a trolling process.

:green_heart: The Troller is *environmentally friendly*, suspeding while idle and hibernating to the next day, keeping resource consumption to a minimum.

## New features & License

Personal Troller Copyright (C) 2015 Diogo Nunes This program is licensed under the terms of the MIT License and it comes with ABSOLUTELY NO WARRANTY. For more details check LICENSE.

If you liked this software, consider making a donation :angel: or contributing with a new trolling action :smiling_imp:.
