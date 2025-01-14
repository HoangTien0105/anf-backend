using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ANF.Core.Models.Entities;
using ANF.Infrastructure;
using Asp.Versioning;
using ANF.Core.Services;
using ANF.Core.Models.Requests;
using ANF.Core.Commons;

namespace ANF.Application.Controllers.v1
{
    public class UsersController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public UsersController(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new publisher.
        /// </summary>
        /// <param name="request">The request model containing publisher details.</param>
        /// <returns>Returns an ApiResponse indicating the result of the operation.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1/users/publisher
        ///     {
        ///        "firstName": "John",
        ///        "lastName": "Doe",
        ///        "phoneNumber": "1234567890",
        ///        "citizenNo": "A1234567",
        ///        "email": "john.doe@example.com",
        ///        "password": "Password123!",
        ///        "passwordConfirmed": "Password123!"
        ///     }
        ///
        /// </remarks>
        [MapToApiVersion(1)]
        [HttpPost("users/publisher")]
        public async Task<ActionResult<ApiResponse<string>>> CreatePublisher(PublisherCreateRequest request)
        {
            var result = await _userService.CreateUser(request);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Publisher created successfully."
            });
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
