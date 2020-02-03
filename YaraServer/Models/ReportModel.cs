using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models
{
    public class ReportModel
    {
        [Key]
        public int Id { get; set; }
        public string ScanId { get; set; }
        public DateTime Date { get; set; }
        public string SHA1 { get; set; }
        public string SHA256 { get; set; }
        [Display(Name = "File")]
        public string FilePath { get; set; }
        [Display(Name = "VirusTotal Positives")]
        public int Positives { get; set; }
        [Display(Name = "VirusTotal Scans")]
        public int Total { get; set; }
        public int Tag { get; set; }

        public int TerminalId { get; set; }
        [ForeignKey("TerminalId")]
        public virtual TerminalDetailsModel Terminal { get; set; }
    }
}
