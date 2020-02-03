using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Models.Network
{
    public class YaraResult
    {
        public Dictionary<string, string> Meta { get; set; }
        public string Identifier { get; set; }
    }
}
