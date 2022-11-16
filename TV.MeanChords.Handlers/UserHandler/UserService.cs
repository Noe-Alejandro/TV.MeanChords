using System;
using System.Linq;
using System.Text.RegularExpressions;
using TV.MeanChords.Data.Db.Context.DiscosChowell;
using TV.MeanChords.Data.Db.UnitOfWork;
using TV.MeanChords.Utils;
using TV.MeanChords.Utils.Enum;
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

        public ResponseBase<GetUserResponse> GetUser(GetUserRequest request)
        {
            var user = UoWDiscosChowell.UserRepository.Get(X => X.UserId.Equals(request.UserId)).FirstOrDefault();
            if (user == null)
                throw new Exception("No se encontró el usuario");
            return ResponseBase<GetUserResponse>.Create(new GetUserResponse
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email
            });
        }

        public ResponseBase<PostUserResponse> PostUser(PostUserRequest request)
        {
            if (ValidateParams(request))
            {
                if (IsEmailInUse(request.Email))
                    throw new Exception("El correo ya está en uso por otra cuenta");
                var user = new User
                {
                    Name = request.Name,
                    LastName = request.LastName,
                    Email = request.Email,
                    Password = request.Password.EncryptString(),
                    Type = (int)UserType.NORMAL_USER,
                    CreatedDate = DateTime.Now,
                    ModificationDate = DateTime.Now
                };
                UoWDiscosChowell.UserRepository.Insert(user);
                UoWDiscosChowell.Save();
                if (user.UserId > 0)
                    return ResponseBase<PostUserResponse>.Create(new PostUserResponse
                    {
                        Name = user.Name,
                        Email = user.Email,
                        ID = (int)user.UserId
                    });
                throw new Exception("Falla al insertar el usuario en la base de datos");
            }
            else
            {
                throw new Exception("Alguno de sus campos no es válido");
            }
        }

        public ResponseBase<PutUserResponse> PutUser(PutUserRequest request)
        {
            var user = UoWDiscosChowell.UserRepository
                .Get(x => x.Email.Equals(request.CurrentEmail)).FirstOrDefault();
            if(!user.Password.DecryptString().Equals(request.CurrentPassword))
                throw new Exception("La contraseña es inválida");
            if (request.NewEmail != null)
            {
                if (IsEmailInUse(request.NewEmail))
                    throw new Exception("El correo ya se encuentra en uso por otra cuenta");
                user.Email = request.NewEmail;
            }
            if (request.NewPassword != null)
            {
                if (!ValidatePassword(request.NewPassword))
                    throw new Exception("La nueva contraseña no es válida");
                user.Password = request.NewPassword.EncryptString();
            }
            if (request.Name != null && request.Name.Equals(""))
                user.Name = request.Name;
            if (request.LastName != null && request.LastName.Equals(""))
                user.LastName = request.LastName;
            user.ModificationDate = DateTime.Now;
            UoWDiscosChowell.Save();
            return ResponseBase<PutUserResponse>.Create(new PutUserResponse
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email
            });
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

        private bool IsEmailInUse(string email)
        {
            if (UoWDiscosChowell.UserRepository.Get(x => x.Email.Equals(email)).FirstOrDefault() != null)
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
