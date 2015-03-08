using System;
using System.Configuration;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Native.Windows;

namespace ExecutableSpecifications.Core.Watin
{
    public class WatinSession : IDisposable
    {
        private bool? _showBrowserWindow;

        protected WatinSession()
        {
            InitializeSession();
        }

        protected WatinSession(IE browser)
        {
            InitializeSession(browser);
        }

        protected internal bool ShowBrowserWindow
        {
            get
            {
                if (_showBrowserWindow.HasValue)
                {
                    return _showBrowserWindow.Value;
                }
                _showBrowserWindow = GetShowBrowserWindow();
                return _showBrowserWindow.Value;
            }
        }

        public IE Browser { get; private set; }

        public void Dispose()
        {
            if (Browser != null)
            {
                Browser.Dispose();
            }
        }

        private static bool GetShowBrowserWindow()
        {
            if (!ConfigurationManager.AppSettings.AllKeys.Contains("showBrowserWindow"))
            {
                return false;
            }
            bool showBrowserWindow;
            Boolean.TryParse(ConfigurationManager.AppSettings["showBrowserWindow"], out showBrowserWindow);
            return showBrowserWindow;
        }

        private void InitializeSession(IE browser = null)
        {
            Settings.AttachToBrowserTimeOut = 240;
            Settings.WaitUntilExistsTimeOut = 240;
            Settings.WaitForCompleteTimeOut = 240;
            Settings.SleepTime = 5;

            Settings.Instance.MakeNewIeInstanceVisible = ShowBrowserWindow;

            Browser = browser ?? new IE(true) {AutoClose = true};

            if (!ShowBrowserWindow)
            {
                return;
            }

            Settings.AttachToBrowserTimeOut = 20;
            Settings.WaitUntilExistsTimeOut = 240;
            Settings.WaitForCompleteTimeOut = 240;
            BringToFront();
        }

        protected internal void BringToFront()
        {
            Browser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);
            Browser.WaitForComplete();
        }

        ~WatinSession()
        {
            Dispose();
        }
    }
}