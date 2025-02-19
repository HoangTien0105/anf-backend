namespace ANF.Core.Models.Responses
{
    public class LoginResponse
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string AccessToken { get; set; } = null!;
    }
}
