using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

using TechTalk.SpecFlow;

namespace ExecutableSpecifications.Core
{
    internal class BaseSpecFlowSteps
    {
        private const string DefaultScreenshotFilePath = @".";

        // ReSharper disable once StaticFieldInGenericType
        private static string _screenShotPath = string.Empty;
        private static string _testRunNumber;

        private static string ScreenshotPath
        {
            get { return (!String.IsNullOrEmpty(_screenShotPath)) ? _screenShotPath : (_screenShotPath = GetScreenshotPath()); }
        }

        internal static ITestRunner Runner
        {
            get { return TestRunnerManager.GetTestRunner(); }
        }

        private static string TestRunPath
        {
            get
            {
                var formattedDateTokens = DateTime.Now.ToString("s").Split('T');
                var date = formattedDateTokens[0];
                var shotPath = String.Format("{1}{0}{2}", Path.DirectorySeparatorChar, ScreenshotPath, date);
                shotPath = Path.GetFullPath(shotPath);
                Directory.CreateDirectory(shotPath);
                var trnum = TestRunNumber(shotPath);
                shotPath += Path.DirectorySeparatorChar + trnum.ToString(CultureInfo.InvariantCulture);
                SetupDirectory(shotPath);
                return shotPath;
            }
        }

        private static string GetScreenshotPath()
        {
            return ConfigurationManager.AppSettings.AllKeys.Contains("saveScreenShotsTo")
                       ? ConfigurationManager.AppSettings["saveScreenShotsTo"]
                       : DefaultScreenshotFilePath;
        }

        internal static T GetPossiblyNull<T>(string keyName = null)
        {
            var value = default(T);
            try
                // this try \ catch shouldnt be needed but specflow throws an error when you .Get<> and it has a null value
            {
                value = !(String.IsNullOrEmpty(keyName)) ? ScenarioContext.Current.Get<T>(keyName) : ScenarioContext.Current.Get<T>();
            }
                // ReSharper disable EmptyGeneralCatchClause 
            catch {}
            // ReSharper restore EmptyGeneralCatchClause            
            return value;
        }

        protected internal static T GetPossiblyNullFeatureData<T>(string keyName = null)
        {
            var value = default(T);
            try
                // this try \ catch shouldnt be needed but specflow throws an error when you .Get<> and it has a null value
            {
                value = !(String.IsNullOrEmpty(keyName)) ? FeatureContext.Current.Get<T>(keyName) : FeatureContext.Current.Get<T>();
            }
                // ReSharper disable EmptyGeneralCatchClause 
            catch {}
            // ReSharper restore EmptyGeneralCatchClause            
            return value;
        }

        private static string TestRunNumber(string path)
        {
            if (!String.IsNullOrEmpty(_testRunNumber))
            {
                return _testRunNumber;
            }
            var directories = Directory.GetDirectories(path);
            _testRunNumber = (directories.Count() + 1).ToString(CultureInfo.InvariantCulture);
            return _testRunNumber;
        }

        private static void SetupDirectory(string path)
        {
            Directory.CreateDirectory(path);
            var info = new DirectoryInfo(path);
            var dirsec = info.GetAccessControl();
            //Remove inherited permissions
            dirsec.SetAccessRuleProtection(true, false);

            //create rights, include subfolder and files to be inherited by this
            var everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            var full = new FileSystemAccessRule(everyone, FileSystemRights.Modify | FileSystemRights.Synchronize,
                                                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None,
                                                AccessControlType.Allow);
            dirsec.AddAccessRule(full);

            info.SetAccessControl(dirsec);
        }

        internal static string GetFullPath(string sourceFilePath, string caller)
        {
            var formattedDateTokens = DateTime.Now.ToString("s").Split('T');
            var time = formattedDateTokens[1].Replace(":", "-");
            var file = sourceFilePath.Split(Path.DirectorySeparatorChar).Last().Replace(".cs", "." + caller);
            var fileName = String.Format("{0}{1}{2}{1}{3}", time, ".", file, "png");
            var shotPath = TestRunPath;
          
            // we are implicitly assuming the method with the longest name is our test method. HACK
            // Instead we should be looking for existence of Specflow given / when / then attributes
            // ReSharper disable once AssignNullToNotNullAttribute
            var unitTestCaller = new StackTrace()
                .GetFrames()
                .OrderByDescending(frame => frame.GetMethod().Name.Length)
                .FirstOrDefault();

            if (unitTestCaller != null)
            {
                shotPath += Path.DirectorySeparatorChar + unitTestCaller.GetMethod().Name;
                SetupDirectory(shotPath);
            }
            var fullPath = String.Format("{1}{0}{2}", Path.DirectorySeparatorChar, shotPath, fileName);
            return fullPath;
        }
    }
}