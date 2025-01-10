using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    [Route("api/affiliate-network")]
    [ApiController]
    [ApiVersion(1)]
    public class BaseApiController : ControllerBase
    {
    }
}
