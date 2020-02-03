using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models.ViewModels
{
    public class ReportDetailsViewModel
    {
        public ReportModel Report { get; set; }
        public List<ScanModel> Scans { get; set; }
        public List<YaraResultAndYaraMetasViewModel> YaraResults { get; set; }
        public List<MessageModel> Messages { get; set; }
    }
}
