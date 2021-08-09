using System;
using System.Collections.Generic;

#nullable disable

namespace SS13Stats.Frontend.Database
{
    public partial class Snapshot
    {
        public long SnapshotId { get; set; }
        public int ServerId { get; set; }
        public int PlayerCount { get; set; }
        public DateTime SnapshotTime { get; set; }

        public virtual Server Server { get; set; }
    }
}
