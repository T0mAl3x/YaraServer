﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models.Network
{
    public class InfoModel
    {
        public string ScandId { get; set; }
        public DateTime Date { get; set; }
        public string SHA1 { get; set; }
        public string SHA256 { get; set; }
        public string FilePath { get; set; }
        public int Positives { get; set; }
        public int Total { get; set; }
        public List<Scan> Scans { get; set; }
        public List<YaraResult> YaraResults { get; set; }
        public List<string> Messages = new List<string>();
        public Tag ReportTag { get; set; }
    }
}
