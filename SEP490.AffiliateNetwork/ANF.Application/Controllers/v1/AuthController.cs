using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class AuthController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>
        /// 
        /// </remarks>
        [HttpPost("user/login")]
        public void Login([FromBody] string value)
        {
        }
    }
}
