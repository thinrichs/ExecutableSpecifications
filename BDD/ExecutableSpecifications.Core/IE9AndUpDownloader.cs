using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Automation;
using WatiN.Core;
using WatiN.Core.Native.Windows;

namespace ExecutableSpecifications.Core
{
    internal class IE9AndUpDownloader
    {
        private readonly Browser _browser;

        public IE9AndUpDownloader(Browser browser)
        {
            _browser = browser;
        }

        // from http://stackoverflow.com/questions/7500339/how-to-test-file-download-with-watin-ie9/8532222#8532222
        public void Download()
        {
            var windowDialog = GetWindowDialog();
            // if doesn't work try to increase sleep interval or write your own waitUntill method
            Thread.Sleep(3000);
            windowDialog.SetActivate();
            var amc = AutomationElement.FromHandle(windowDialog.Hwnd).FindAll(TreeScope.Children, Condition.TrueCondition);

            foreach (var element in amc.Cast<AutomationElement>().Where(element => element.Current.Name.Equals("Save"))) 
            {
                // if doesn't work try to increase sleep interval or write your own waitUntil method
                Thread.Sleep(3000);
                var pats = element.GetSupportedPatterns();

                var element1 = element;
                var invokePatterns = pats
                    .Where(pat => pat.Id == 10000)
                    .Select(pat => (InvokePattern) element1.GetCurrentPattern(pat));
                foreach (var click in invokePatterns) 
                {
                    click.Invoke();
                }
            }
        }

        private Window GetWindowMain()
        {
            return new Window(NativeMethods.GetWindow(_browser.hWnd, 5));
        }

        private Window GetWindowDialog()
        {
            var windowMain = GetWindowMain();
            return new Window(NativeMethods.GetWindow(windowMain.Hwnd, 5));
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out string pszPath);

        internal static string DownloadsFolder()
        {
            var downloadsKnownFolder = new Guid("374DE290-123F-4565-9164-39C4925E467B");
            // get downloads folder
            string downloads;
            SHGetKnownFolderPath(downloadsKnownFolder, 0, IntPtr.Zero, out downloads);
            return downloads;
        }
    }
}