using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models
{
    public class CertificateDetailsModel
    {
        [Key]
        public int Id { get; set; }

        public string Subject { get; set; }
        public string Issuer { get; set; }
        public string Version { get; set; }
        [Display(Name = "Valid Date")]
        [Required]
        public DateTime ValidDate { get; set; }
        [Display(Name = "Expiry Date")]
        [Required]
        public DateTime ExpiryDate { get; set; }
        public string Thumbprint { get; set; }
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }
        [Display(Name = "Friendly Name")]
        public string FriendlyName { get; set; }
        public string PublicKeyFormat { get; set; }
        [Display(Name = "Raw Data Length")]
        public string RawDataLength { get; set; }
        public bool IsRevoked { get; set; }

    }
}
