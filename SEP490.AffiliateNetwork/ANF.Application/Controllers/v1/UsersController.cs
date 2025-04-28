using Microsoft.AspNetCore.Mvc;
using ANF.Core.Services;
using ANF.Core.Commons;
using ANF.Core.Models.Requests;
using Asp.Versioning;
using ANF.Core.Models.Responses;
using Microsoft.AspNetCore.Authorization;

namespace ANF.Application.Controllers.v1
{
    public class UsersController(IUserService userService) : BaseApiController
    {
        private readonly IUserService _userService = userService;

        /// <summary>
        /// Authenticates a user with the provided email and password.
        /// </summary>
        /// <param name="value">The login request containing email and password.</param>
        /// <returns>An access token</returns>
        [HttpPost("users/login")]
        [MapToApiVersion(1)]
        [AllowAnonymous]
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
            var token = await _userService.Login(value.Email, value.Password);
            return Ok(new ApiResponse<DetailedUserResponse>
            {
                IsSuccess = true,
                Message = "Login successfully.",
                Value = token
            });
        }

        /// <summary>
        /// Change user's account status 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch("users/{id}/status")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeAccountStatus(long id, [FromQuery] string status)
        {
            var result = await _userService.UpdateAccountStatus(id, status);
            if (result is not null)
            {
                return Ok(new ApiResponse<UserStatusResponse>
                {
                    IsSuccess = true,
                    Message = "Success.",
                    Value = result
                });
            }
            else return BadRequest();

        }

        /// <summary>
        /// Get all advertisers
        /// </summary>
        /// <param name="request">Pagination request model</param>
        /// <returns></returns>
        [HttpGet("users/advertisers")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAdvertisers([FromQuery] PaginationRequest request)
        {
            var advertisers = await _userService.GetAdvertisers(request);
            return Ok(new ApiResponse<PaginationResponse<AdvertiserResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = advertisers
            });
        }

        /// <summary>
        /// Get all publishers
        /// </summary>
        /// <param name="request">Pagination request model</param>
        /// <returns></returns>
        [HttpGet("users/publishers")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPublishers([FromQuery] PaginationRequest request)
        {
            var publishers = await _userService.GetPublishers(request);
            return Ok(new ApiResponse<PaginationResponse<PublisherResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = publishers
            });
        }

        /// <summary>
        /// Get information of the user
        /// </summary>
        /// <returns>User's information</returns>
        [HttpGet("users/me")]
        [MapToApiVersion(1)]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailedUser()
        {
            var user = await _userService.GetUserInformation();
            return Ok(new ApiResponse<DetailedUserResponse>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = user
            });
        }

        /// <summary>
        /// Get bank account name by bank code and account number
        /// </summary>
        /// <returns>User bank account's name</returns>
        [HttpPost("users/bank-account-name")]
        [MapToApiVersion(1)]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBankAccountName(BankLookupRequest request)
        {
            var accountName = await _userService.LookupBankAccount(request.BankCode, request.AccountNo);
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = accountName
            });
        }

        /// <summary>
        /// Create new account
        /// </summary>
        /// <param name="value">Account data</param>
        /// <returns></returns>
        [HttpPost("users/account")]
        [AllowAnonymous]
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
                Message = "Register account successfully. Please wait for admin to accept the registration."
            });
        }

        [HttpDelete("users/{id}")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var result = await _userService.DeleteUser(id);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success."
            });
        }

        /// <summary>
        /// Verify user's account
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns></returns>
        [HttpGet("users/{id}/verify-account")]
        [MapToApiVersion(1)]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VerifyUserAccount(long id)
        {
            var result = await _userService.ChangeEmailStatus(id);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success."
            });
        }

        /// <summary>
        /// Reset passsword for user
        /// </summary>
        /// <param name="id">User's id</param>
        /// <param name="token">Reset token</param>
        /// <param name="request">Data to reset the password</param>
        /// <returns></returns>
        [HttpPatch("users/{id}/reset-token/{token}")]
        [MapToApiVersion(1)]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePassword(long id, string token, UpdatePasswordRequest request)
        {
            var result = await _userService.UpdatePassword(token, id, request);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success."
            });
        }

        /// <summary>
        /// Send email to user to get the link to reset the password
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns></returns>
        [HttpPost("users/{email}")]
        [MapToApiVersion(1)]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SendEmailForForgetPassword(string email)
        {
            var result = await _userService.ChangePassword(email);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "An email for reset the password is sent to you. Please check the inbox and spam email to get the link!"
            });
        }

        [HttpPatch("users/{code}/wallet")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser, Publisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActivateWallet(string code)
        {
            var result = await _userService.ActivateWallet(code);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Wallet is actived successfully!"
            });
        }

        /// <summary>
        /// Add banking information for a user.
        /// </summary>
        /// <param name="requests">A list of banking information to be added.</param>
        /// <returns>
        /// A response indicating the success or failure of the add operation.
        /// </returns>
        /// <response code="200">If the banking information was added successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If the user was not found.</response>
        [HttpPost("users/bank-accounts")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser, Publisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddBankingInformation(List<UserBankCreateRequest> requests)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
                return validationResult;

            var result = await _userService.AddBankingInformation(requests);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success."
            });
        }


        /// <summary>
        /// Update the banking information of a user.
        /// </summary>
        /// <param name="id">User bank's id (ub_id).</param>
        /// <param name="request">The request containing the updated banking information.</param>
        /// <returns>
        /// A response indicating the success or failure of the update operation.
        /// </returns>
        /// <response code="200">If the banking information was updated successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If the user's bank account was not found.</response>
        [HttpPut("users/bank-accounts/{id}")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser, Publisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBankingInformation(long id, UserBankUpdatedRequest request)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
                return validationResult;

            var result = await _userService.UpdateBankingInformation(id, request);
            if (!result) return BadRequest();

            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Update successfully!"
            });
        }
    }
}
