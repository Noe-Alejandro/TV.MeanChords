using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using TV.MeanChords.API.Helpers;
using TV.MeanChords.Handlers.TagHandler;
using TV.MeanChords.Handlers.UserHandler;
using TV.MeanChords.ModelViews.MVUser;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TagController : ApiController
    {
        public ITagService GetService()
        {
            return TagService.Create();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/Category/GET")]
        public IHttpActionResult GetCategories()
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.GetCategories();

                    var mvReponse = new ResponseBase<List<GetTagResponse>>()
                    {
                        Data = MapperHelper.Map<List<GetTagResponse>>(response.Data),
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<List<GetTagResponse>>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }
    }
}