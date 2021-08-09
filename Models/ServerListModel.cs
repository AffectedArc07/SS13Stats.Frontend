using SS13Stats.Frontend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SS13Stats.Frontend.Models {
    public class ServerListModel {
        // Yes I know this is technically a list of snapshots, but this is easier. Trust me.
        public List<Snapshot> servers { get; set; } = new List<Snapshot>();
    }
}
