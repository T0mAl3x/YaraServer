using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models
{
    public class ScanModel
    {
        [Key]
        public int Id { get; set; }
        public string EngineName { get; set; }
        public bool Detected { get; set; }
        public string Version { get; set; }
        public string Result { get; set; }

        public int ReportId { get; set; }
        [ForeignKey("ReportId")]
        public virtual ReportModel Report { get; set; }
    }
}
