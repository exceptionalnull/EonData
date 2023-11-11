using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EonData.ContactForm.Models
{
    public class SendMessageModel
    {
        [Required]
        [StringLength(250)]
        public string ContactName { get; set; }

        [Required]
        [StringLength(250)]
        public string ContactAddress { get; set; }

        [Required]
        [StringLength(7500)]
        public string MessageContent { get; set; }

        [Required]
        [StringLength(25)]
        public string FormSource { get; set; }
    }
}
