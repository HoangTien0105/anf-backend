using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAuthorizationController : ControllerBase
    {
        [HttpGet("publishers/authorize-hello")]
        [Authorize(Roles = "Publisher")]
        [MapToApiVersion(1)]
        public string HelloForPublisher() => "Hello publisher mtfk!";

        [HttpGet("admin/authorize-hello")]
        [Authorize(Roles = "Admin")]
        [MapToApiVersion(1)]
        public string HelloForAdmin() => "Hello mtfk!";

        [HttpGet("advertisers/authorize-hello")]
        [Authorize(Roles = "Advertiser")]
        [MapToApiVersion(1)]
        public string HelloForAdvertiser() => "Hello advertiser mtfk!";
    }
}
