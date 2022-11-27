using TV.MeanChords.Handlers.UserHandler;

namespace TV.MeanChords.ModelViews.MVLogin
{
    public class MVPostLoginResponse
    {
        public string token { get; set; }
        public int UserID { get; set; }
        public GetUserResponse User { get; set; }
    }
}
