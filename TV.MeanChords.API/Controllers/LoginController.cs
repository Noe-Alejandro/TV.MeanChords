using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using TV.MeanChords.API.Helpers;
using TV.MeanChords.Handlers.LoginHandler;
using TV.MeanChords.ModelViews.MVLogin;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        ILoginService GetService()
        {
            return LoginService.Create();
        }
        [HttpPost]
        [Route("~/api/Login/POST")]
        public IHttpActionResult PostLogin(MVPostLoginRequest request)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.PostLogin(MapperHelper.Map<PostLoginRequest>(request));

                    var mvReponse = new ResponseBase<MVPostLoginResponse>()
                    {
                        Data = MapperHelper.Map<MVPostLoginResponse>(response.Data),
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