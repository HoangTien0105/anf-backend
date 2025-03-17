using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class BanksController : BaseApiController
    {
        private static List<BankingInformation> banks =
        [
            new BankingInformation
            {
                Name = "Vietcombank",
                LogoImgUrl = "https://logos.fandom.com/wiki/File:Vietcombank_logo.svg"
            },
            new BankingInformation
            {
                Name = "VietinBank",
                LogoImgUrl = "https://logos.fandom.com/wiki/File:VietinBank_logo.svg"
            },
            new BankingInformation
            {
                Name = "Agribank",
                LogoImgUrl = "https://logos.fandom.com/wiki/File:Agribank_logo.svg"
            },
            new BankingInformation
            {
                Name = "BIDV",
                LogoImgUrl = "https://logos.fandom.com/wiki/File:BIDV_logo.svg"
            },
            new BankingInformation
            {
                Name = "Techcombank",
                LogoImgUrl = "https://logos.fandom.com/wiki/File:Techcombank_logo.svg"
            },
            new BankingInformation
            {
                Name = "MB Bank",
                LogoImgUrl = "https://logos.fandom.com/wiki/File:MB_Bank_logo.svg"
            },
            new BankingInformation
            {
                Name = "Sacombank",
                LogoImgUrl = "https://logos.fandom.com/wiki/File:Sacombank_logo.svg"
            },
        ];

        /// <summary>
        /// Bank information data
        /// </summary>
        /// <returns></returns>
        [HttpGet("banks")]
        [MapToApiVersion(1)]
        [ProducesResponseType(200)]
        public IActionResult Get()
        {
            return Ok(banks);
        }   
    }
    
    internal class BankingInformation
    {
        public string Name { get; set; } = null!;

        public string? LogoImgUrl { get; set; }
    }
}
