using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models.Network
{
    public class Scan
    {
        public string EngineName { get; set; }
        public bool Detected { get; set; }
        public string Version { get; set; }
        public string Result { get; set; }
    }
}
