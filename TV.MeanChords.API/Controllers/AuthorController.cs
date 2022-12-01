using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using TV.MeanChords.API.Helpers;
using TV.MeanChords.Handlers.AuthorHandler;
using TV.MeanChords.Handlers.DiscHandler;
using TV.MeanChords.Handlers.UserHandler;
using TV.MeanChords.ModelViews.MVDisc;
using TV.MeanChords.ModelViews.MVUser;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AllowAnonymous]
    public class AuthorController : ApiController
    {
        public IAuthorService GetService()
        {
            return AuthorService.Create();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/Author/GETALL")]
        public IHttpActionResult GetAllAuthor()
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.GetAllAuthor();

                    var mvReponse = new ResponseBase<List<AuthorResponse>>()
                    {
                        Data = response.Data,
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<List<AuthorResponse>>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }
    }
}