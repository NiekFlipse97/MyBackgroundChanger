using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;

namespace MyBackgroundChanger
{
    internal class Program
    {
        private static TimeChecker _timeChecker;

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(UAction uAction, int uParam, StringBuilder lpvParam, int fuWinIni);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {
            _timeChecker = new TimeChecker();
            var timer = new Timer();

            var h = Process.GetCurrentProcess().MainWindowHandle;
            ShowWindow(h, 0);

            CheckTime();

            timer.Elapsed += OnTimeElapsed;
            timer.Interval = 60000;
            timer.Enabled = true;

            Console.ReadLine();
        }

        private static void OnTimeElapsed(object source, ElapsedEventArgs e)
        {
            CheckTime();
        }

        private static void CheckTime()
        {
            const string morning = @"C:\Users\Niek\Pictures\ServiceBackgrounds\sunrise.png";
            const string afternoon = @"C:\Users\Niek\Pictures\ServiceBackgrounds\afternoon.png";
            const string evening = @"C:\Users\Niek\Pictures\ServiceBackgrounds\sunset.jpg";
            const string midnight = @"C:\Users\Niek\Pictures\ServiceBackgrounds\midnight.jpg";
            
            if (_timeChecker.LocalTime() >= _timeChecker.GetSunriseTime() && _timeChecker.LocalTime() < _timeChecker.GetSolarNoonTime())
            {
                SetBackground(morning);
            }
            else if (_timeChecker.LocalTime() >= _timeChecker.GetSolarNoonTime() && _timeChecker.LocalTime() < _timeChecker.GetTwilightEnd())
            {
                SetBackground(afternoon);
            }
            else if (_timeChecker.LocalTime() >= _timeChecker.GetTwilightEnd() && _timeChecker.LocalTime().Hour < 22)
            {
                SetBackground(evening);
            }
            else
            {
                SetBackground(midnight);
            }
        }

        private static void SetBackground(string filename)
        {
            var classesRoot = Registry.CurrentUser;
            var registryKey = classesRoot.OpenSubKey(@"Control Panel\Desktop", true);
            var s = new StringBuilder(filename);

            try
            {
                SystemParametersInfo(UAction.SPI_SETDESKWALLPAPER, 0, s, 0x2);
                registryKey?.SetValue("WallPaper", s);
            }
            finally
            {
                classesRoot.Close();
                registryKey?.Close();
            }
        }

        public enum UAction
        {
            /// <summary>
            /// set the desktop background image
            /// </summary>
            SPI_SETDESKWALLPAPER = 0x0014,
            /// <summary>
            /// set the desktop background image
            /// </summary>
            SPI_GETDESKWALLPAPER = 0x0073,
        }
    }
}
