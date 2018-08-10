using Service.Interface;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI.Filters;
using WebAPI.Requests;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/user")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        protected readonly IUserService UserService;

        public UserController(IUserService userService)
        {
            UserService = userService;
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("userlist")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Return to the user list")]
        public IHttpActionResult GetusetList()
        {
            var ret = UserService.GetUserList();

            if (ret == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return Json(ret);
        }

        /// <summary>
        /// Generate new Token / User Validate
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Create Token")]
        public IHttpActionResult ValidateUser([FromBody]UserLogin request)
        {
            var ret = UserService.GetToken(request.UserName, request.Password);

            if (ret == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return Json(ret);
        }

        /// <summary>
        /// Insert new user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="confirmpassword"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Enter user")]
        public IHttpActionResult InsertUser(string username, string password, string confirmpassword)
        {
            if (password == confirmpassword)
            {
                UserService.InsertUser(username, password);
                return Json("User Created Successfully! :)");
            }
            throw new HttpResponseException(HttpStatusCode.ExpectationFailed);
        }

        /// <summary>
        /// Clear User Cache
        /// </summary>
        [AllowAnonymous]
        [HttpPut]
        [Route("clearcache")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Clear the cache")]
        public IHttpActionResult ClearCache()
        {
            UserService.ClearFullCache();
            return Ok();
        }
    }
}