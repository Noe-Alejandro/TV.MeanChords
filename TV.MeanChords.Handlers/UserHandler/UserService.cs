using System;
using System.Text.RegularExpressions;
using TV.MeanChords.Data.Db.UnitOfWork;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.UserHandler
{
    public class UserService : IUserService
    {
        private UoWDiscosChowell UoWDiscosChowell { get; set; }

        public static UserService Create() => new UserService();

        public UserService()
        {
            UoWDiscosChowell = UoWDiscosChowell.Create();
        }

        public ResponseBase<PostUserResponse> PostUser(PostUserRequest request)
        {
            if (ValidateParams(request))
                return ResponseBase<PostUserResponse>.Create(new PostUserResponse
                {
                    Name = request.Name,
                    Email = request.Email,
                    ID = 1
                });
            return null;
        }

        private bool ValidateParams(PostUserRequest request)
        {
            if (String.IsNullOrEmpty(request.Name))
                return false;
            if (String.IsNullOrEmpty(request.LastName))
                return false;
            if (String.IsNullOrEmpty(request.Email))
                return false;
            if (string.IsNullOrEmpty(request.Password))
                return false;
            return ValidatePassword(request.Password);
        }

        private bool ValidatePassword(string password)
        {
            if (Regex.IsMatch(password, "[A-Z]"))
                return true;
            return false;
        }

        public void Dispose()
        {
            UoWDiscosChowell.Dispose();
            UoWDiscosChowell = null;
        }
    }
}
