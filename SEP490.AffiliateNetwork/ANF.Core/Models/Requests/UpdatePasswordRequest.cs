using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class UpdatePasswordRequest
    {
        [Required(ErrorMessage = "Field is required", AllowEmptyStrings = false)]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Field is required", AllowEmptyStrings = false)]
        public string ConfirmedPassword { get; set; } = null!;
    }
}
