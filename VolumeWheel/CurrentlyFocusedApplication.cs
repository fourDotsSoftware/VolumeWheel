using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace VolumeWheel
{
    public class CurrentlyFocusedApplication
    {
        public string CurrentApplicationFilename="";

        public CurrentlyFocusedApplication()
        {

        }

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventProc lpfnWinEventProc, int idProcess, int idThread, uint dwflags);
        [DllImport("user32.dll")]
        internal static extern int UnhookWinEvent(IntPtr hWinEventHook);
        internal delegate void WinEventProc(IntPtr hWinEventHook, uint iEvent, IntPtr hWnd, int idObject, int idChild, int dwEventThread, int dwmsEventTime);

        const uint WINEVENT_OUTOFCONTEXT = 0;
        const uint EVENT_SYSTEM_FOREGROUND = 3;
        private IntPtr winHook;
        private WinEventProc listener;

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();


        public void StartListeningForWindowChanges()
        {
            try
            {
                listener = new WinEventProc(EventCallback);
                //setting the window hook
                winHook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, listener, 0, 0, WINEVENT_OUTOFCONTEXT);
            }
            catch { }
        }

        public void StopListeningForWindowChanges()
        {
            try
            {
                UnhookWinEvent(winHook);
            }
            catch { }
        }

        private void EventCallback(IntPtr hWinEventHook, uint iEvent, IntPtr hWnd, int idObject, int idChild, int dwEventThread, int dwmsEventTime)
        {
            try
            {                
                uint pid;
                GetWindowThreadProcessId(hWnd, out pid);
                Process p = Process.GetProcessById((int)pid);
                //3string program = System.IO.Path.GetFileNameWithoutExtension(p.MainModule.FileName).ToLower();

                string program = System.IO.Path.GetFileNameWithoutExtension(ProcessFilenameRetriever.GetExecutablePath(p)).ToLower();

                if (program != "volumewheel")
                {
                    CurrentApplicationFilename = program;
                }
                
            }
            catch {

                CurrentApplicationFilename = "";
            }
            finally
            {

            }
        }
    }
}
