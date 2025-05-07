using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class DateRangeRequest
    {
        [BindProperty(Name = "from")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime From { get; set; }

        [BindProperty(Name = "to")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime To { get; set; }
    }
}
