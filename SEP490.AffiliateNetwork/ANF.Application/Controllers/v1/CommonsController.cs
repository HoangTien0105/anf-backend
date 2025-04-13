using ANF.Core.Commons;
using ANF.Core;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class CommonsController(HttpClient httpClient, ILogger<CommonsController> logger) : BaseApiController
    {
        private static List<DomesticBeneficiaryBank> _banks =
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
        private static List<string> _noOfExperienceForPublisher = [
            "1 years",
            "2 years",
            "3 years",
            "4 years",
            "5 years",
            "6+ years",
        ];
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<CommonsController> _logger = logger;

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
        /// Get supported domestic beneficiary banks from Techcombank template
        /// </summary>
        /// <returns></returns>
        [HttpGet("domestic-beneficiary-banks")]
        [MapToApiVersion(1)]
        [ProducesResponseType(200)]
        public IActionResult Get()
        {
            return Ok(_banks);
        }

        /// <summary>
        /// Integrate third-party endpoint from Bank Lookup API to get supported banks 
        /// </summary>
        /// <returns></returns>
        [HttpGet("supported-banks")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSupportedBanks()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://api.banklookup.net/api/bank/list");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to fetch bank list. Status code: {response.StatusCode}");
                    return StatusCode((int)response.StatusCode, "Failed to fetch bank list");
                }

                var content = await response.Content.ReadAsStringAsync();
                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred while fetching bank list: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Get experience value to filter publisher
        /// </summary>
        /// <returns></returns>
        [HttpGet("publisers/no-of-experience")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetExperienceForPublisher()
        {
            return Ok(_noOfExperienceForPublisher);
        }
    }
}
