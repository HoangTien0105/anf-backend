using ANF.Core.Commons;
using ANF.Core;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class CommonsController : BaseApiController
    {
        private static List<DomesticBeneficiaryBank> banks =
        [
            new DomesticBeneficiaryBank
            {
                BankCode = "01310001",
                BankName = "NH TMCP KY THUONG VN",
                Location = "Ca nuoc",
                BranchName = "NH TMCP KY THUONG VN-01310001"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "79307001",
                BankName = "A CHAU (ACB)",
                Location = "Ca nuoc",
                BranchName = "A CHAU (ACB)-79307001"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "01664001",
                BankName = "AGRICULTURAL BANK OF CHINA LIMITED CN HA NOI",
                Location = "Ca nuoc",
                BranchName = "AGRICULTURAL BANK OF CHINA LIMITED CN HA NOI-01664001"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "01323001",
                BankName = "AN BINH (AB BANK)",
                Location = "Ca nuoc",
                BranchName = "AN BINH (AB BANK)-01323001"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "01359001",
                BankName = "BAO VIET (BAOVIETBANK)",
                Location = "Ca nuoc",
                BranchName = "BAO VIET (BAOVIETBANK)-01359001"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "01201001",
                BankName = "CONG THUONG VN (VIETINBANK)",
                Location = "Ca nuoc",
                BranchName = "CONG THUONG VN (VIETINBANK)-01201001"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "01202001",
                BankName = "DAU TU VA PHAT TRIEN VN ( BIDV)",
                Location = "Ca nuoc",
                BranchName = "DAU TU VA PHAT TRIEN VN ( BIDV)-01202001"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "79317002",
                BankName = "DONG NAM A (SEA BANK)",
                Location = "Ca nuoc",
                BranchName = "DONG NAM A (SEA BANK)-79317002"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "79627001",
                BankName = "JP Morgan Chase",
                Location = "Ca nuoc",
                BranchName = "JP Morgan Chase-79627001"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "79353001",
                BankName = "KIEN LONG (KIENLONGBANK)",
                Location = "Ca nuoc",
                BranchName = "KIEN LONG (KIENLONGBANK)-79353001"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "79306001",
                BankName = "NAM A",
                Location = "Ca nuoc",
                BranchName = "NAM A-79306001"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "01203001",
                BankName = "NGOAI THUONG VN (VCB)",
                Location = "Ca nuoc",
                BranchName = "NGOAI THUONG VN (VCB)-01203001"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "79321001",
                BankName = "PHAT TRIEN TP HCM (HDBANK)",
                Location = "Ca nuoc",
                BranchName = "PHAT TRIEN TP HCM (HDBANK)-79321001"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "01311003",
                BankName = "QUAN DOI (MB)",
                Location = "Ca nuoc",
                BranchName = "QUAN DOI (MB)-01311003"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "79314013",
                BankName = "QUOC TE (VIB)",
                Location = "Ca nuoc",
                BranchName = "QUOC TE (VIB)-79314013"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "01309001",
                BankName = "VIET NAM THINH VUONG (VPBANK)",
                Location = "Ca nuoc",
                BranchName = "VIET NAM THINH VUONG (VPBANK)-01309001"
            },
            new DomesticBeneficiaryBank
            {
                BankCode = "01348002",
                BankName = "SAI GON - HA NOI (SHB)",
                Location = "Ca nuoc",
                BranchName = "SAI GON - HA NOI (SHB)-01348002"
            },
        ];

        private static List<BankingInformation> bankingInformation = new List<BankingInformation>()
        {
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
                Name = "MB Bank",
                LogoImgUrl = "https://logos.fandom.com/wiki/File:MB_Bank_logo.svg"
            },
            new BankingInformation
            {
                Name = "Sacombank",
                LogoImgUrl = "https://logos.fandom.com/wiki/File:Sacombank_logo.svg"
            },
        };

        /// <summary>
        /// Get all pricing models
        /// </summary>
        /// <returns></returns>
        [HttpGet("pricing-models")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPricingModels()
        {
            var pricingModels = PricingModelConstant.pricingModels;
            return Ok(new ApiResponse<List<PricingModel>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = pricingModels
            });
        }

        /// <summary>
        /// Get pricing model by id
        /// </summary>
        /// <param name="id">Pricing model id</param>
        /// <returns></returns>
        [HttpGet("pricing-models/{id}")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPricingModel(long id)
        {
            var pricingModel = PricingModelConstant.pricingModels.Where(e => e.Id == id).FirstOrDefault();
            return Ok(new ApiResponse<PricingModel>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = pricingModel
            });
        }

        /// <summary>
        /// Get data of domestic beneficiary banks (from Techcombank template)
        /// </summary>
        /// <returns></returns>
        [HttpGet("domestic-beneficiary-banks")]
        [MapToApiVersion(1)]
        [ProducesResponseType(200)]
        public IActionResult Get()
        {
            return Ok(banks);
        }

        /// <summary>
        /// Get banks' name and image (svg format)
        /// </summary>
        /// <returns></returns>
        [HttpGet("banks")]
        [MapToApiVersion(1)]
        [ProducesResponseType(200)]
        public IActionResult GetBankingInfo()
        {
            return Ok(bankingInformation);
        }
    }

    internal class BankingInformation
    {
        public string Name { get; set; } = null!;

        public string? LogoImgUrl { get; set; }
    }
}
