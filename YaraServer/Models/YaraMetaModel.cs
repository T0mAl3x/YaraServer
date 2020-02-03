using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models
{
    public class YaraMetaModel
    {
        [Key]
        public int Id { get; set; }
        public string MetaKey { get; set; }
        public string MetaValue { get; set; }

        public int YaraResultId { get; set; }
        [ForeignKey("YaraResultId")]
        public virtual YaraResultModel YaraResult { get; set; }
    }
}
