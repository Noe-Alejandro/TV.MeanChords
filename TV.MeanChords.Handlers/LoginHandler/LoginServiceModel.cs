using TV.MeanChords.Handlers.UserHandler;

namespace TV.MeanChords.Handlers.LoginHandler
{
    public class PostLoginResponse
    {
        public bool Status { get; set; }
        public int UserId { get; set; }
        public int UserType { get; set; }
        public GetUserResponse User { get; set; }
    }

    public class PostLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
