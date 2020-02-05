using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models.ViewModels
{
    public class HomeViewModel
    {
        public class DictObject
        {
            public string Key { get; set; }
            public int Count { get; set; }
        };

        [Display(Name = "Total Reports")]
        public int TotalScans { get; set; }
        [Display(Name = "Potential Malware")]
        public int TotalPotentialInfected { get; set; }
        [Display(Name = "Total Terminals")]
        public int TotalTerminals { get; set; }

        public List<DictObject> Scans { get; set; }
        public List<DictObject> YaraResults { get; set; }
        public List<DictObject> Reports { get; set; }
        public List<DictObject> ScansPerDay { get; set; }
    }
}
