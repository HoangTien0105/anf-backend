using Microsoft.AspNetCore.Mvc;
using ANF.Core.Services;
using ANF.Core.Commons;
using ANF.Core.Models.Requests;
using Asp.Versioning;
using ANF.Core.Models.Responses;

namespace ANF.Application.Controllers.v1
{
    public class UsersController(IUserService userService) : BaseApiController
    {
        private readonly IUserService _userService = userService;

        /// <summary>
        /// Authenticates a user with the provided email and password.
        /// </summary>
        /// <param name="value">The login request containing email and password.</param>
        /// <returns>An ApiResponse containing the login response with user details and access token.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/users/login
        ///     {
        ///         "email": "user@example.com",
        ///         "password": "password123"
        ///     }
        /// 
        /// Sample response:
        /// 
        ///     {
        ///         "isSuccess": true,
        ///         "message": "Login successfully.",
        ///         "value": {
        ///             "id": "d290f1ee-6c54-4b01-90e6-d701748f0851",
        ///             "firstName": "John",
        ///             "lastName": "Doe",
        ///             "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
        ///         }
        ///     }
        /// </remarks>
        [HttpPost("users/login")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginRequest value)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var user = await _userService.Login(value.Email, value.Password);
            return Ok(new ApiResponse<LoginResponse>
            {
                IsSuccess = true,
                Message = "Login successfully.",
                Value = user
            });
        }

        // GET: api/Users
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        //{
        //    return await _context.Users.ToListAsync();
        //}

        // GET: api/Users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(Guid id)
        //{
        //    var user = await _context.Users.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(Guid id, User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        /// <summary>
        /// Create new account
        /// </summary>
        /// <param name="value">Account data</param>
        /// <returns></returns>
        [HttpPost("users/account")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAccount(AccountCreateRequest value)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _userService.RegisterAccount(value);
            if (!result) return BadRequest();

            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Register account successfully. Please wait a while for admin to accept the registration."
            });
        }

        // DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(Guid id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
