﻿namespace TV.MeanChords.ModelViews.MVUser
{
    public class MVPostUserResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int ID { get; set; }
    }

    public class MVPutUserResponse
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}