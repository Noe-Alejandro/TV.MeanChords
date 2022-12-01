using TV.MeanChords.Handlers.UserHandler;

namespace TV.MeanChords.ModelViews.MVLogin
{
    public class MVPostLoginResponse
    {
        public string token { get; set; }
        public int UserID { get; set; }
        //public int UserType { get; set; } 0 = admin y  1 = normal
        public GetUserResponse User { get; set; }
    }
}
