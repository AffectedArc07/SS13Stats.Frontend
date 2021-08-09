using System;
using System.Collections.Generic;

#nullable disable

namespace SS13Stats.Frontend.Database
{
    public partial class Heartbeat
    {
        public string Type { get; set; }
        public DateTime Time { get; set; }
    }
}
