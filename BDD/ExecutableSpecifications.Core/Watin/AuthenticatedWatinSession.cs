using WatiN.Core;

namespace ExecutableSpecifications.Core.Watin
{
    public class AuthenticatedWatinSession : WatinSession
    {
        public AuthenticatedWatinSession()
        {
            UserName = "";
            Password = "";
        }

        protected AuthenticatedWatinSession(IE browser) : base(browser)
        {
            UserName = "";
            Password = "";
        }

        protected AuthenticatedWatinSession(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        protected internal string Password { get; internal set; }

        protected internal string UserName { get; internal set; }

        protected static bool LoggedIn { get; private set; }

        public void Login(bool validateTitle = false)
        {
            LoggedIn = true;
        }

        public void Logout()
        {
            LoggedIn = false;
        }
    }
}