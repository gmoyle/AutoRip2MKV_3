using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace AutoRip2MKV
{
    public class OpenOrCloseCDDrive
    {
        static void Main(string[] args)
        {
            ConsoleKey key;
            while (true)
            {
                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.O:
                        Open();
                        break;
                    case ConsoleKey.C:
                        Close();
                        break;
                    default:
                        return;
                }
            }
        }

        public static void Open()
        {
            int ret = mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);
        }

        public static void Close()
        {
            int ret = mciSendString("set cdaudio door closed", null, 0, IntPtr.Zero);
        }

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi)]
        protected static extern int mciSendString(string lpstrCommand,
                                                    StringBuilder lpstrReturnString,
                                                    int uReturnLength,
                                                    IntPtr hwndCallback);

    }
 }
