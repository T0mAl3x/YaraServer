using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models.ViewModels
{
    public class CreateCertificateViewModel
    {
        public CertificateDetailsModel Certificate { get; set; }
        [Display(Name = "Password")]
        [Required]
        public string CAPassword { get; set; }
        public string ErrorMessage { get; set; }

        public CreateCertificateViewModel()
        {
            Certificate = new CertificateDetailsModel();
            Certificate.ValidDate = DateTime.Now.Date;
            Certificate.ExpiryDate = Certificate.ValidDate.AddYears(1);
        }
    }
}
