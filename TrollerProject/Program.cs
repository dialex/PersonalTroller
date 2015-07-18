using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Troller
{
    public class Program
    {
        #region Configurations

        private const string ErrorFileName = "Error.log";
        private const string TasksFileName = "Tasks.txt";
        private const string CommentString = "//";

        private const string INTERVAL_TAG = "EVERY";
        private const string BEGIN_TAG = "BEGIN";
        private const string END_TAG = "END";
        private const char SEPARATOR = '|';

        private static DateTime BeginTime;
        private static DateTime EndTime;
        private static TimeSpan Interval;

        #endregion

        public static void Main(string[] args)
        {
            DebugConsole.WriteLine("=== Troller Unleashed ===");
            DebugConsole.WriteLine("");

            try
            {
                while (true)
                {
                    if (!File.Exists(TasksFileName)) CreateDefaultTasks();

                    // ==============================================================================
                    // Performs all trolling tasks, waiting the specified interval between each tasks
                    // ==============================================================================

                    using (StreamReader file = new StreamReader(TasksFileName))
                    {
                        string line;
                        while ((line = file.ReadLine()) != null)
                        {
                            // Ignore empty lines or comments
                            if ((line == string.Empty)) { continue; }
                            else if ((line.Length >= CommentString.Length) && (line.Substring(0, CommentString.Length) == CommentString)) { continue; }

                            // Otherwise, execute command
                            string[] task = line.Split(SEPARATOR);
                            string action = task[0];
                            string parameter = task[1];

                            if (action == INTERVAL_TAG)
                            {
                                SetTime(INTERVAL_TAG, parameter);
                                Thread.Sleep(Interval); // To avoid suspicions, waits before first trolling
                            }
                            else if ((action == BEGIN_TAG) || (action == END_TAG))
                            {
                                SetTime(action, parameter);
                            }
                            else
                            {
                                DoTrolling(action, parameter);
                                Thread.Sleep(Interval);
                            }
                        }

                        // If execution produced no errors, deletes file to save space
                        File.Delete(ErrorFileName);
                    }

                    // ============================================================================
                    // After completing all tasks, checks if it should restart or wait for tomorrow
                    // ============================================================================

                    if (DateTime.Now > EndTime)
                    {
                        DateTime wakeupTime = BeginTime.AddDays(1);
                        int sleepTime = Convert.ToInt32((wakeupTime - EndTime).TotalMilliseconds);
                        DebugConsole.WriteLine("Soon...");
                        Thread.Sleep(sleepTime);
                    }
                }
            }
            catch (Exception error)
            {
                // Appends error message to file
                using (StreamWriter errorLog = File.AppendText(ErrorFileName))
                {
                    errorLog.WriteLine(string.Format("{0} | ERROR {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), error.Message));
                    DebugConsole.WriteLine("ERROR: View log file for details.");
                }
            }

            DebugConsole.WriteLine("");
            DebugConsole.WriteLine("=== END ===");
            Thread.Sleep(1000 * 5); // Keeps the DebugConsole open so you can read it before closing
        }

        /// <summary>
        /// I solemnly swear I'm up to no good.
        /// </summary>
        /// <param name="actionCode">Code of trolling action to execute.</param>
        /// <param name="parameter">Input for trolling action.</param>
        /// <returns>True on success, False otherwise.</returns>
        private static bool DoTrolling(string actionCode, string parameter)
        {
            OpenUrlOnBrowser asdf = new OpenUrlOnBrowser();
            
            switch (actionCode)
            {
                case ShowMessage.ActionCode:                return ShowMessage.DoAction(parameter);
                case OpenDisksDrive.ActionCode:             return OpenDisksDrive.DoAction(parameter);
                case OpenUrlOnBrowser.ActionCode:           return OpenUrlOnBrowser.DoAction(parameter);
                case ChangeCursorToWait.ActionCode:         return ChangeCursorToWait.DoAction(parameter);
                case ChangeCursorToAnimation.ActionCode:    return ChangeCursorToAnimation.DoAction(parameter);
                case ScheduleShutdown.ActionCode:           return ScheduleShutdown.DoAction(parameter);
                case ScheduleLogoff.ActionCode:             return ScheduleLogoff.DoAction(parameter);
                default: return true;
            }
        }

        #region Auxiliary methods

        /// <summary>
        /// Creates a file with default trolling tasks.
        /// </summary>
        private static void CreateDefaultTasks()
        {
            string[] lines = {
                "BEGIN|09:42:57",
                "EVERY|00:23:56",
                "END|17:47:14",
                "DISKDRV|1",
                "OPENURL|http://www.sanger.dk/",
                "OPENURL|http://www.ringingtelephone.com/",
                "OPENURL|http://cachemonet.com/",
                "DISKDRV|2",
                "OPENURL|http://cat-bounce.com/",
                "OPENURL|http://giantbatfarts.com/",
                "OPENURL|http://www.ooooiiii.com/",
                "OPENURL|http://www.iiiiiiii.com/",
                "DISKDRV|3",
                "OPENURL|http://iamawesome.com/",
                "OPENURL|http://www.nelson-haha.com/",
                "DISKDRV|5",
                "OPENURL|http://leekspin.com/",
                "OPENURL|http://baconsizzling.com/",
                "OPENURL|http://www.muchbetterthanthis.com/",
                "DISKDRV|7",
                "OPENURL|http://www.sadtrombone.com/?autoplay=true",
            };
            File.WriteAllLines(TasksFileName, lines);
            DebugConsole.WriteLine("Created Tasks.txt using defaults.");
        }

        /// <summary>
        /// Sets Begin, End and Interval times.
        /// </summary>
        /// <param name="tag">String specifying if it's a Begin, End or Interval setting.</param>
        /// <param name="parameter">Time value (format HH:mm:ss).</param>
        private static void SetTime(string tag, string parameter)
        {
            string[] stringInput = parameter.Split(':');
            int[] timeInput = { int.Parse(stringInput[0]), int.Parse(stringInput[1]), int.Parse(stringInput[2]) };
            DateTime time = DateTime.Today.AddHours(timeInput[0]).AddMinutes(timeInput[1]).AddSeconds(timeInput[2]);

            switch (tag)
            {
                case BEGIN_TAG: BeginTime = time; break;
                case END_TAG: EndTime = time; break;
                case INTERVAL_TAG: Interval = time.TimeOfDay; break;
            }
        }

        #endregion
    }

    #region Trolling actions (template)

    /// <summary>
    /// All trolling actions must implement this interface. Use it as a template.
    /// </summary>
    internal class TrollingAction
    {
        /// <summary>
        /// Code made of 7 chars that will be used on Tasks.txt.
        /// </summary>
        public const string ActionCode = "XXXXXXX";

        /// <summary>
        /// Method that executes the trolling action.
        /// </summary>
        /// <param name="parameter">Your method may receive one input.</param>
        /// <returns>True if action was executed with success; False otherwise.</returns>
        public static bool DoAction(string parameter) { return true; }
    }

    #endregion
    #region Trolling actions (implementations)

    /// <summary>
    /// Shows a dialog message.
    /// </summary>
    internal class ShowMessage
    {
        public const string ActionCode  = "MESSAGE";

        /// <summary>
        /// Shows a dialog message. Keep in mind that showing a message will let the victim know the name of your exe.
        /// </summary>
        /// <param name="text">Text do display.</param>
        /// <returns>True on success.</returns>
        public static bool DoAction(string text)
        {
            DebugConsole.WriteLine("Showing message.");

            string title = "Personal Troller";
            MessageBox.Show(text, title);
            return true;
        }
    }

    /// <summary>
    /// Opens a URL on a browser.
    /// </summary>
    internal class OpenUrlOnBrowser
    {
        /// <summary>
        /// Code that will be used on Tasks.txt to trigger the action.
        /// </summary>
        public const string ActionCode = "OPENURL";

        /// <summary>
        /// Opens a URL on a browser.
        /// </summary>
        /// <param name="url">Url (including http://) to open on a new tab.</param>
        /// <returns>True on success.</returns>
        public static bool DoAction(string url)
        {
            DebugConsole.WriteLine("Opening tab.");

            try
            {
                Process.Start(url);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Opens the CD/DVD drive.
    /// </summary>
    internal class OpenDisksDrive
    {
        /// <summary>
        /// Code that will be used on Tasks.txt to trigger the action.
        /// </summary>
        public const string ActionCode = "DISKDRV";

        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        public static extern int mciSendStringA(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        /// <summary>
        /// Opens the CD/DVD drive.
        /// </summary>
        /// <param name="numTimes">Number of times to open and close.</param>
        /// <returns>True on success.</returns>
        public static bool DoAction(string numTimes)
        {
            DebugConsole.WriteLine("Opening cd/dvd drive.");
            int times = Convert.ToInt32(numTimes);

            string returnString, driveLetter;
            returnString = driveLetter = "";
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.CDRom)
                    driveLetter = drive.Name;
            }

            if (driveLetter != "")
            {
                for (int i = 0; i < times; i++)
                {
                    // Open drive
                    mciSendStringA("open " + driveLetter + ": type CDaudio alias drive" + driveLetter, returnString, 0, 0);
                    mciSendStringA("set drive" + driveLetter + " door open", returnString, 0, 0);
                    // Wait for it to open
                    Thread.Sleep(1000 * 1);
                    // Close it
                    mciSendStringA("open " + driveLetter + ": type CDaudio alias drive" + driveLetter, returnString, 0, 0);
                    mciSendStringA("set drive" + driveLetter + " door closed", returnString, 0, 0);
                }
                return true;
            }
            else return false;
        }
    }

    /// <summary>
    /// Schedules the computer shutdown.
    /// </summary>
    internal class ScheduleShutdown
    {
        /// <summary>
        /// Code that will be used on Tasks.txt to trigger the action.
        /// </summary>
        public const string ActionCode = "SHUTDWN";

        /// <summary>
        /// Schedules the computer shutdown.
        /// </summary>
        /// <param name="shutdownMessage">Notification message after scheduling.</param>
        /// <returns>True on success.</returns>
        public static bool DoAction(string shutdownMessage)
        {
            DebugConsole.WriteLine("Scheduling computer shutdown.");
            int MAX_CHARS = 127;    //FROM: http://ss64.com/nt/shutdown.html

            int secondsToShutdown = 60 * 15;
            if (shutdownMessage.Length > MAX_CHARS)
                shutdownMessage = shutdownMessage.Substring(0, MAX_CHARS - 1);

            Process.Start("shutdown", string.Format("/s /t {0} /d p:0:0 /c \"{1}\"", secondsToShutdown, shutdownMessage));
            return true;
        }
    }

    /// <summary>
    /// Schedules the computer shutdown.
    /// </summary>
    internal class ScheduleLogoff
    {
        /// <summary>
        /// Code that will be used on Tasks.txt to trigger the action.
        /// </summary>
        public const string ActionCode = "LOGUOFF";

        /// <summary>
        /// Schedules the computer shutdown.
        /// </summary>
        /// <param name="logoffMessage">Notification message after scheduling.</param>
        /// <returns>True on success.</returns>
        public static bool DoAction(string logoffMessage)
        {
            DebugConsole.WriteLine("Scheduling user logoff.");
            int MAX_CHARS = 127;    //FROM: http://ss64.com/nt/shutdown.html

            int secondsToLogoff = 60 * 15;
            if (logoffMessage.Length > MAX_CHARS)
                logoffMessage = logoffMessage.Substring(0, MAX_CHARS - 1);

            Process.Start("shutdown", string.Format("/l /t {0} /c \"{1}\"", secondsToLogoff, logoffMessage));
            return true;
        }
    }

    /// <summary>
    /// Change mouse cursor to wait state.
    /// </summary>
    internal class ChangeCursorToWait
    {
        /// <summary>
        /// Code that will be used on Tasks.txt to trigger the action.
        /// </summary>
        public const string ActionCode = "WAITCUR";

        /// <summary>
        /// Change mouse cursor to wait state.
        /// </summary>
        /// <param name="numSeconds">Number of seconds to display the wait cursor.</param>
        /// <returns>True on success.</returns>
        public static bool DoAction(string numSeconds)
        {
            DebugConsole.WriteLine("Change cursor to wait state.");
            int totalSeconds = int.Parse(numSeconds);

            //TODO: This doesn't change the cursor, I don't know why, I tried everything!
            for (int currentSec = 0; currentSec < totalSeconds; currentSec++)
            {
                Cursor.Current = Cursors.WaitCursor;
                Application.UseWaitCursor = true;
                Thread.Sleep(1000 * 1);
            }
            return true;
        }
    }

    /// <summary>
    /// Change mouse cursor to custom animated gif.
    /// </summary>
    internal class ChangeCursorToAnimation
    {
        /// <summary>
        /// Code that will be used on Tasks.txt to trigger the action.
        /// </summary>
        public const string ActionCode = "CUSTCUR";

        [DllImport("User32.dll")]
        private static extern IntPtr LoadCursorFromFile(String str);
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

        /// <summary>
        /// Change mouse cursor to custom animated gif.
        /// </summary>
        /// <param name="numSeconds">Number of seconds to display the custom animation.</param>
        /// <returns>True on success.</returns>
        public static bool DoAction(string numSeconds)
        {
            DebugConsole.WriteLine("Change cursor to custom animation.");
            int totalSeconds = int.Parse(numSeconds);

            try
            {
                // Choose custom cursor
                // TODO: choose randomly from available resources
                byte[] cursorResource = Properties.Resources.dog;

                // From Resource to temp file
                string tempFileName = "temp_cursor.ani";
                if (File.Exists(tempFileName)) File.Delete(tempFileName);
                File.WriteAllBytes(tempFileName, cursorResource);

                // Create cursor
                tempFileName = Path.Combine(Application.StartupPath, tempFileName);
                Cursor customCursor = CreateCursor(tempFileName);

                // Force the cursor during totalSeconds
                //TODO: This doesn't change the cursor, I don't know why, I tried everything! (http://stackoverflow.com/a/2902336/675577)
                for (int currentSec = 0; currentSec < totalSeconds; currentSec += 5)
                {
                    Cursor.Current = customCursor;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a cursor from a file.
        /// </summary>
        private static Cursor CreateCursor(string filename)
        {
            return CreateUsingLoadCursor(filename);
        }

        private static Cursor CreateUsingLoadCursor(string filename)
        {
            IntPtr hCursor = LoadCursorFromFile(filename);

            if (!IntPtr.Zero.Equals(hCursor))
            {
                return new Cursor(hCursor);
            }
            else
            {
                throw new ApplicationException("Could not create cursor from file " + filename);
            }
        }

        private static Cursor CreateUsingLoadImage(string filename)
        {
            const int IMAGE_CURSOR = 2;
            const uint LR_LOADFROMFILE = 0x00000010;
            IntPtr ipImage = LoadImage(IntPtr.Zero, filename, IMAGE_CURSOR, 0, 0, LR_LOADFROMFILE);
            return new Cursor(ipImage);
        }
    }

    #endregion

    #region Auxiliary classes

    /// <summary>
    /// Class that writes debug messages into a Console.
    /// </summary>
    internal static class DebugConsole
    {
        /// <summary>
        /// Console.WriteLine.
        /// </summary>
        /// <param name="text"></param>
        internal static void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        /// <summary>
        /// Console.Write.
        /// </summary>
        /// <param name="text"></param>
        internal static void Write(string text)
        {
            Console.Write(text);
        }
    }

    #endregion
}
