using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models
{
    public class MessageModel
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }

        public int ReportId { get; set; }
        [ForeignKey("ReportId")]
        public virtual ReportModel Report { get; set; }
    }
}
