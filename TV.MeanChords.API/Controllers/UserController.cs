using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using TV.MeanChords.API.Helpers;
using TV.MeanChords.Handlers.UserHandler;
using TV.MeanChords.ModelViews.MVUser;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        public IUserService GetService()
        {
            return UserService.Create();
        }

        [Authorize]
        [HttpGet]
        [Route("~/api/User/GET")]
        public IHttpActionResult GetUser([FromUri]MVGetUserRequest request)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.GetUser(MapperHelper.Map<GetUserRequest>(request));

                    var mvReponse = new ResponseBase<MVGetUserResponse>()
                    {
                        Data = MapperHelper.Map<MVGetUserResponse>(response.Data),
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<MVGetUserResponse>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("~/api/User/POST")]
        public IHttpActionResult PostUser(MVPostUserRequest request)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.PostUser(MapperHelper.Map<PostUserRequest>(request));

                    var mvReponse = new ResponseBase<MVPostUserResponse>()
                    {
                        Data = MapperHelper.Map<MVPostUserResponse>(response.Data),
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<MVPostUserResponse>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }

        [Authorize]
        [HttpPut]
        [Route("~/api/User/PUT")]
        public IHttpActionResult PutUser(MVPutUserRequest request)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.PutUser(MapperHelper.Map<PutUserRequest>(request));

                    var mvReponse = new ResponseBase<MVPutUserResponse>()
                    {
                        Data = MapperHelper.Map<MVPutUserResponse>(response.Data),
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<MVPutUserResponse>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }
    }
}