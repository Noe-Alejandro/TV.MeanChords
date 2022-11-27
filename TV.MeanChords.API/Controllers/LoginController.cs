using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using TV.MeanChords.API.Helpers;
using TV.MeanChords.API.JWT;
using TV.MeanChords.Handlers.LoginHandler;
using TV.MeanChords.ModelViews.MVLogin;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AllowAnonymous]
    public class LoginController : ApiController
    {
        ILoginService GetService()
        {
            return LoginService.Create();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("~/api/Login/POST")]
        public IHttpActionResult PostLogin(MVPostLoginRequest request)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.PostLogin(MapperHelper.Map<PostLoginRequest>(request));
                    if (!response.Status)
                        throw new Exception("Correo o contraseña inválida");

                    var mvReponse = new ResponseBase<MVPostLoginResponse>()
                    {
                        Data = new MVPostLoginResponse{ 
                            token = TokenGenerator.GenerateTokenJwt(request.Email),
                            UserID = response.Data.UserId,
                            User = response.Data.User
                        },
                        Errors = response.Errors,
                        Status = response.Status
                    };

                    if (mvReponse.Status)
                        return Content(HttpStatusCode.OK, mvReponse);

                    return Content(HttpStatusCode.BadRequest, mvReponse);
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, ResponseBase<MVPostLoginResponse>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }
    }
}