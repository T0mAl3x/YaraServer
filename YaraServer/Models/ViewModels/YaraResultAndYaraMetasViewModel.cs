using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models.ViewModels
{
    public class YaraResultAndYaraMetasViewModel
    {
        public YaraResultModel YaraResult { get; set; }
        [Display(Name = "Metadata")]
        public List<YaraMetaModel> YaraMetas { get; set; }
    }
}
