namespace TV.MeanChords.Handlers.LoginHandler
{
    public class PostLoginResponse
    {
        public bool Status { get; set; }
        public int UserId { get; set; }
    }

    public class PostLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
