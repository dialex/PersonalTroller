using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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

        private const char SEPARATOR = '|';
        private const string INTERVAL_TAG = "EVERY";
        private const string BEGIN_TAG = "BEGIN";
        private const string END_TAG = "END";

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
            // Use action codes with 7 characters
            switch (actionCode)
            {
                case "DISKDRV": return OpenDisksDrive(parameter);
                case "MESSAGE": return ShowMessage(parameter);
                case "OPENURL": return OpenLinkOnBrowser(parameter);
                case "WAITCUR": return ChangeCursorToWait(parameter);
                case "CUSTCUR": return ChangeCursorToAnimation(parameter);
                default: return true;
            }
        }

        #region Trolling actions

        /// <summary>
        /// Shows a dialog message. Keep in mind that showing a message will let the victim know the name of your exe.
        /// </summary>
        /// <param name="text">Text do display.</param>
        /// <returns>True on success.</returns>
        private static bool ShowMessage(string text)
        {
            DebugConsole.WriteLine("Showing message.");

            string title = "Personal Troller";
            MessageBox.Show(text, title);
            return true;
        }

        /// <summary>
        /// Opens a URL on a browser.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>True on success. False otherwise.</returns>
        private static bool OpenLinkOnBrowser(string url)
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

        /// <summary>
        /// Required by OpenDisksDrive.
        /// </summary>
        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        public static extern int mciSendStringA(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        /// <summary>
        /// Opens the CD/DVD drive (FROM: http://stackoverflow.com/a/3797166/675577).
        /// </summary>
        /// <param name="numTimes">Number of times to open and close.</param>
        /// <returns>True on success. False otherwise.</returns>
        private static bool OpenDisksDrive(string numTimes)
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

        /// <summary>
        /// Change mouse cursor to wait state.
        /// </summary>
        /// <param name="numSeconds">Number of seconds to display the wait cursor.</param>
        /// <returns>True on success. False otherwise.</returns>
        private static bool ChangeCursorToWait(string numSeconds)
        {
            DebugConsole.WriteLine("Change cursor to wait state.");
            int totalSeconds = int.Parse(numSeconds);

            for (int currentSec = 0; currentSec < totalSeconds; currentSec++)
            {
                Cursor.Current = Cursors.WaitCursor;
                Application.UseWaitCursor = true;
                Thread.Sleep(1000 * 1);
            }
            return true;
        }

        /// <summary>
        /// Change mouse cursor to custom animated gif (FROM: http://stackoverflow.com/a/2902336/675577).
        /// </summary>
        /// <param name="numSeconds">Number of seconds to display the custom animation.</param>
        /// <returns>True on success. False otherwise.</returns>
        private static bool ChangeCursorToAnimation(string numSeconds)
        {
            DebugConsole.WriteLine("Change cursor to custom animation.");
            int totalSeconds = int.Parse(numSeconds);

            try
            {
                // Choose custom cursor
                byte[] cursorResource = Properties.Resources.dog;   // TODO: use random to choose from available resources

                // From Resource to temp file
                string tempFileName = "temp_cursor.ani";
                if (File.Exists(tempFileName)) File.Delete(tempFileName);
                File.WriteAllBytes(tempFileName, cursorResource);

                // Create cursor
                tempFileName = Path.Combine(Application.StartupPath, tempFileName);
                Cursor customCursor = AdvancedCursor.Create(tempFileName);

                // Force the cursor during totalSeconds
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

        private static bool ScheduleShutdown(string shutdownMessage)
        {
            int timeToShutdown = 0; //minutes
            //run command line
            //shutdown -s -t 1925000 -c “System error: overloaded porn folder”
            return true;
        }

        #endregion

        #region Aux methods

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

    #region Auxiliary classes

    /// <summary>
    /// Class that reads resources files and creates Cursors.
    /// </summary>
    internal static class AdvancedCursor
    {
        [DllImport("User32.dll")]
        private static extern IntPtr LoadCursorFromFile(String str);
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

        /// <summary>
        /// Creates a cursor from a file.
        /// </summary>
        public static Cursor Create(string filename)
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
