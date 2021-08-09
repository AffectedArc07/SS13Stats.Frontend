using System;
using System.Collections.Generic;

#nullable disable

namespace SS13Stats.Frontend.Database
{
    public partial class Server
    {
        public Server()
        {
            Snapshots = new HashSet<Snapshot>();
        }

        public int Id { get; set; }
        public string ServerName { get; set; }
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }

        public virtual ICollection<Snapshot> Snapshots { get; set; }
    }
}
