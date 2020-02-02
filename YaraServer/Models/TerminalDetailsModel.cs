using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models
{
    public class TerminalDetailsModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "System Name")]
        public string SystemName { get; set; }
        [Display(Name = "OS Name")]
        public string OsName { get; set; }
        public string Version { get; set; }
        public string MAC { get; set; }
        public string Processor { get; set; }
        public string Motherboard { get; set; }
        public string RAM { get; set; }
        [Display(Name = "Certificate")]
        public int CertificateId { get; set; }
        [ForeignKey("CertificateId")]
        public virtual CertificateDetailsModel Certificate { get; set; }
    }
}
