using SS13Stats.Frontend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SS13Stats.Frontend.Models {
    public class ServerViewModel {
        public Server server { get; set; }
        public string day_snapshots_json { get; set; }
        public string week_snapshots_json { get; set; }
    }
}
