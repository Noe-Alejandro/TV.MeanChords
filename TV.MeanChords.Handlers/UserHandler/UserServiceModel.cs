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
}
