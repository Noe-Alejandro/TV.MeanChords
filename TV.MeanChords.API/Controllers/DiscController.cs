using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using TV.MeanChords.API.Helpers;
using TV.MeanChords.Handlers.DiscHandler;
using TV.MeanChords.Handlers.UserHandler;
using TV.MeanChords.ModelViews.MVDisc;
using TV.MeanChords.ModelViews.MVUser;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AllowAnonymous]
    public class DiscController : ApiController
    {
        public IDiscService GetService()
        {
            return DiscService.Create();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/Disc/GET")]
        public IHttpActionResult GetDisc([FromUri] MVGetDiscRequest request)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.GetDisc(MapperHelper.Map<GetDiscRequest>(request));

                    var mvReponse = new ResponseBase<MVGetDiscResponse>()
                    {
                        Data = MapperHelper.Map<MVGetDiscResponse>(response.Data),
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<MVGetDiscResponse>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/Disc/GETBYTITLE")]
        public IHttpActionResult GetDiscByTitle(string Title)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.GetDiscByTitle(Title);

                    var mvReponse = new ResponseBase<List<GetDiscResponse>>()
                    {
                        Data = MapperHelper.Map<List<GetDiscResponse>>(response.Data),
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<List<GetDiscResponse>>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/Disc/GETBYCATEGORY")]
        public IHttpActionResult GetDiscByCategory(int CategoryId)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.GetDiscByCategory(CategoryId);

                    var mvReponse = new ResponseBase<List<GetDiscResponse>>()
                    {
                        Data = MapperHelper.Map<List<GetDiscResponse>>(response.Data),
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<List<GetDiscResponse>>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/Disc/GETALL")]
        public IHttpActionResult GetAllDisc()
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.GetAllDisc();

                    var mvReponse = new ResponseBase<List<MVGetDiscResponse>>()
                    {
                        Data = MapperHelper.Map<List<MVGetDiscResponse>>(response.Data),
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<List<MVGetDiscResponse>>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/Disc/GETLAST")]
        public IHttpActionResult GetLastDisc(int? Quantity = null)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.GetLastDisc(Quantity);

                    var mvReponse = new ResponseBase<List<MVGetDiscResponse>>()
                    {
                        Data = MapperHelper.Map<List<MVGetDiscResponse>>(response.Data),
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<List<MVGetDiscResponse>>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("~/api/Disc/POST")]
        public IHttpActionResult PostDisc(MVPostDiscRequest request)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.PostDisc(MapperHelper.Map<PostDiscRequest>(request));

                    var mvReponse = new ResponseBase<MVPostDiscResponse>()
                    {
                        Data = MapperHelper.Map<MVPostDiscResponse>(response.Data),
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<MVPostDiscResponse>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("~/api/Disc/PUT")]
        public IHttpActionResult PutDisc([FromUri] int DiscId, [FromBody] PutDiscRequest request)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.PutDisc(DiscId, request);

                    return Ok();
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, ResponseBase<PostDiscResponse>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }
    }
}