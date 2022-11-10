namespace TV.MeanChords.Handlers.UserHandler
{
    public class PostUserRequest
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class PostUserResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int ID { get; set; }
    }
    public class PutUserRequest
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CurrentEmail { get; set; }
        public string NewEmail { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class PutUserResponse
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
