using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendKeysChecker
{
    class Program
    {
        [System.Security.SuppressUnmanagedCodeSecurity()]
        internal sealed class NativeMethods
        {
            private NativeMethods() { }

            internal static bool IsWindowInForeground(IntPtr hWnd)
            {
                return hWnd == GetForegroundWindow();
            }

            [DllImport("user32.dll")]
            internal static extern IntPtr GetForegroundWindow();
        }
        static class KeyboardSend
        {
            [DllImport("user32.dll")]
            private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

            private const int KEYEVENTF_EXTENDEDKEY = 1;
            private const int KEYEVENTF_KEYUP = 2;

            public static void KeyDown(Keys vKey)
            {
                keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY, 0);
            }

            public static void KeyUp(Keys vKey)
            {
                keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            }
        }

        private static void PressKey(Keys key)
        {
            KeyboardSend.KeyDown(key);
            Thread.Sleep(10);
            KeyboardSend.KeyUp(key);
        }

        static void Main(string[] args)
        {
            Console.Title = "Ingame cheat scanner by kurwanator1337";
            while (true)
            {
                var hl2 = Process.GetProcessesByName("hl2");
                foreach (var process in hl2)
                {
                    if (process.MainWindowTitle == "Counter-Strike Source")
                    {
                        if (NativeMethods.IsWindowInForeground(process.MainWindowHandle))
                        {
                            Console.WriteLine("Found CS:S window\nProcess ID " + process.Id + "\nScanner will start through 5 secs...");
                            Thread.Sleep(5000);
                            for (int i = 0; i < 1000; i++)
                            {
                                PressKey(Keys.F12);
                                PressKey(Keys.Insert);
                                PressKey(Keys.Left);
                                PressKey(Keys.Down);
                                Console.WriteLine("Step: " + i + "/" + 1000);
                                Thread.Sleep(10);
                            }
                            Environment.Exit(0);
                        }
                    }
                }
                Console.WriteLine("No active CS:S window found");
                Thread.Sleep(1000);
            }
        }
    }
}
