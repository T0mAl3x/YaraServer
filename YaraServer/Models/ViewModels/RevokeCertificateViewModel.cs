using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models.ViewModels
{
    public class RevokeCertificateViewModel
    {
        public CertificateDetailsModel Certificate { get; set; }
        [Required]
        public string CAPassword { get; set; }
        public string ErrorMessage { get; set; }
    }
}
