namespace TV.MeanChords.ModelViews.MVUser
{
    public class MVGetUserRequest
    {
        public int UserId { get; set; }
    }

    public class MVPostUserRequest
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class MVPutUserRequest
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CurrentEmail { get; set; }
        public string NewEmail { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
