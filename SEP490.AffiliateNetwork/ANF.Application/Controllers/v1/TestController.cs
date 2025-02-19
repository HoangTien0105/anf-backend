using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("publishers/authorize-hello")]
        [Authorize(Roles = "Publisher")]
        public string HelloForPublisher() => "Hello mtfk!";

        [HttpGet("admin/authorize-hello")]
        [Authorize(Roles = "Admin")]
        public string HelloForAdmin() => "Hello mtfk!";
    }
}
