namespace MV.Shell.ServerInteraction
{
    public class SignUpArgs
    {
        public string SessionId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string RootCode { get; set; }
    }

    public class LoginArgs
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
