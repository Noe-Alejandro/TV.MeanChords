using System.Collections.Generic;
using System.Linq;
using TV.MeanChords.Data.Db.UnitOfWork;
using TV.MeanChords.Handlers.UserHandler;
using TV.MeanChords.Utils;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.LoginHandler
{
    public class LoginService : ILoginService
    {
        private UoWDiscosChowell UoWDiscosChowell { get; set; }

        public static LoginService Create() => new LoginService();

        public LoginService()
        {
            UoWDiscosChowell = UoWDiscosChowell.Create();
        }
        public void Dispose()
        {
            UoWDiscosChowell.Dispose();
            UoWDiscosChowell = null;
        }

        public ResponseBase<PostLoginResponse> PostLogin(PostLoginRequest request)
        {
            var user = UoWDiscosChowell.UserRepository.Get(x => x.Email.Equals(request.Email)).FirstOrDefault();
            if (user == null)
                return ResponseBase<PostLoginResponse>.Create(new List<string>()
                {
                    "El correo proporcionado no está registrado"
                });
            return request.Password.Equals(user.Password.DecryptString()) ? ResponseBase<PostLoginResponse>.Create(new PostLoginResponse
            {
                Status = true,
                UserId = user.UserId,
                UserType = user.Type,
                User = new GetUserResponse
                {
                    Name = user.Name,
                    LastName = user.LastName,
                    Email = user.Email,
                }
            }) : ResponseBase<PostLoginResponse>.Create(new List<string>
            {
                "Correo o contraseña inválida"
            });
        }
    }
}
