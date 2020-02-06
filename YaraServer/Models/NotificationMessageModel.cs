using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models
{
    public class NotificationMessageModel
    {
        public int Tag { get; set; }
        public string SystemName { get; set; }
        public string OsName { get; set; }
        public int ReportId { get; set; }
    }
}
