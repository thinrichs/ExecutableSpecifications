using System;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ExecutableSpecifications.Core.ExtensionMethods;
using TechTalk.SpecFlow;
using WatiN.Core.DialogHandlers;

namespace ExecutableSpecifications.Core.Watin
{
    public class BaseWatinActions<TSession, TFirstPage> : IDisposable
        where TSession : AuthenticatedWatinSession, new() where TFirstPage : WatinWebPage, new()
    {
        // ReSharper disable once StaticFieldInGenericType
        private static bool? _screenShotOnlyErrors;

        private static bool ScreenShotOnlyErrors
        {
            get
            {
                if (_screenShotOnlyErrors.HasValue)
                {
                    return _screenShotOnlyErrors.Value;
                }
                _screenShotOnlyErrors = GetScreenShotOnlyErrors();
                return _screenShotOnlyErrors.Value;
            }
        }

        protected static TSession TestSession
        {
            get
            {
                var session = BaseSpecFlowSteps.GetPossiblyNullFeatureData<TSession>();
                if (session != null)
                {
                    return session;
                }
                session = new TSession();
                TestSession = session;
                return session;
            }
            set { FeatureContext.Current.Set(value); }
        }

        protected static TFirstPage TestPage
        {
            get { return BaseSpecFlowSteps.GetPossiblyNullFeatureData<TFirstPage>() ?? (TestPage = TestSession.Browser.Page<TFirstPage>()); }
            set { FeatureContext.Current.Set(value); }
        }

        public void Dispose()
        {
            if (BaseSpecFlowSteps.GetPossiblyNullFeatureData<TSession>() != null)
            {
                DisposeSession();
                TestSession = null;
            }
            TestPage = null;
        }

        private static bool GetScreenShotOnlyErrors()
        {
            if (!ConfigurationManager.AppSettings.AllKeys.Contains("onlyScreenshotErrors"))
            {
                return false;
            }
            bool onlyScreenshotErrors;
            Boolean.TryParse(ConfigurationManager.AppSettings["onlyScreenshotErrors"], out onlyScreenshotErrors);
            return onlyScreenshotErrors;
        }

        protected static void Initialize(string userName = "", string password = "", bool validateTitle = true)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                TestSession.UserName = userName;
            }
            if (!String.IsNullOrEmpty(password))
            {
                TestSession.Password = password;
            }
            TestSession.Login(validateTitle);
        }

        protected static void DisposeSession()
        {
            if (BaseSpecFlowSteps.GetPossiblyNullFeatureData<TSession>() == null)
            {
                return;
            }
            TestSession.Dispose();
            TestSession = null;
        }

        protected static void LoadTestPage()
        {
            TestPage = TestSession.Browser.Load(TestSession.Browser.Page<TFirstPage>());
        }

        protected static void HandleDialogTriggeredBy<TDialog>(Action action, bool closeUnhandledDialogs = false) 
            where TDialog : JavaDialogHandler, new()
        {
            TestSession.Browser.DialogWatcher.CloseUnhandledDialogs = closeUnhandledDialogs;

            var dialog = new TDialog();

            using (new UseDialogOnce(TestSession.Browser.DialogWatcher, dialog))
            {
                action();
                dialog.WaitUntilExists(45);
                dialog.OKButton.Click();
            }
        }

        protected static void HandleLeaveDialogTriggeredBy(Action action)
        {
            var dialog = new ReturnDialogHandlerIe9();

            using (new UseDialogOnce(TestSession.Browser.DialogWatcher, dialog))
            {
                action();
                dialog.WaitUntilExists(45);
                dialog.OKButton.Click();
            }
        }

        protected static void ProcessAction(Action action, [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string caller = "")
        {
            if (!TestSession.ShowBrowserWindow)
            {
                action();
                return;
            }
            
            var fullPath = BaseSpecFlowSteps.GetFullPath(sourceFilePath, caller);

            try
            {
                if (!ScreenShotOnlyErrors)
                {
                    TestSession.Browser.CaptureWebPageToFile(fullPath.Replace(".png", ".0.before.png"));
                }
                action();
                try
                {
                    if (!ScreenShotOnlyErrors)
                    {
                        TestSession.Browser.CaptureWebPageToFile(fullPath.Replace(".png", ".1.after.png"));
                    }
                }
                catch (COMException) { }      //com      exception can happen when action() closes the browser... is there something better to do?
                catch (ArgumentException) {}  //argument exception can happen when action() closes the browser... is there something better to do?
            }
            catch (Exception)
            {
                try
                {
                    TestSession.BringToFront();
                    TestSession.Browser.CaptureWebPageToFile(fullPath.Replace(".png", ".ERROR.png"));
                    TestSession.Logout();
                }
                    // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception) {}

                DisposeSession();
                throw;
            }
        }

        protected T PerformAction<T>(Action action, [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string caller = "") where T : class
        {
            ProcessAction(action, sourceFilePath, caller);
            return this as T;
        }

        private static string FullPath(string sourceFilePath, string caller)
        {
            return BaseSpecFlowSteps.GetFullPath(sourceFilePath, caller);
        }

        protected static void ProcessAssertion(Action assertion, [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string caller = "")
        {
            if (!TestSession.ShowBrowserWindow)
            {
                assertion();
                return;
            }

            var fullPath = FullPath(sourceFilePath, caller);

            try
            {
                assertion();
            }
            catch (Exception)
            {
                TestSession.Browser.CaptureWebPageToFile(fullPath.Replace(".png", ".ASSERTION.FAILURE.png"));
                TestSession.Logout();
                DisposeSession();
                throw;
            }
        }
    }
}