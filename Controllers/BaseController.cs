using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SS13Stats.Frontend.Database;
using SS13Stats.Frontend.Helpers;
using SS13Stats.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SS13Stats.Frontend.Controllers {
    [Route("")]
    public class BaseController : Controller {
        private readonly ILogger<BaseController> _logger;
        private readonly SS13StatsDBContext _dbc;

        public BaseController(ILogger<BaseController> logger, SS13StatsDBContext dbc) {
            _logger = logger;
            _dbc = dbc;
        }

        [Route("")]
        public IActionResult Index() {
            ServerListModel slm = new ServerListModel();
            List<Server> servers = _dbc.Servers.Where(s => s.LastSeen > DateTime.Now.AddDays(-7)).ToList();
            // Now we do ordering stuff
            List<Snapshot> latest_snaps = new List<Snapshot>();
            foreach(Server S in servers) {
                latest_snaps.Add(_dbc.Snapshots.Where(s => s.Server == S).OrderByDescending(s => s.SnapshotTime).First());
            }
            latest_snaps = latest_snaps.OrderByDescending(s => s.PlayerCount).ToList();
            slm.servers = latest_snaps;
            return View("Index", slm);
        }

        [Route("server/{serverid?}")]
        public IActionResult GetServer(int? serverid) {
            if(serverid == null) {
                return BadRequest("Please supply a server ID");
            }
            ServerViewModel svm = new ServerViewModel();
            if(_dbc.Servers.Where(s => s.Id == serverid).Any()) {
                svm.server = _dbc.Servers.Where(s => s.Id == serverid).First();
            } else {
                return NotFound("A server with ID " + serverid + " could not be found");
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new DictionaryAsArrayResolver();

            Dictionary<DateTime, int> snaps_24h = _dbc.Snapshots.Where(s => s.Server == svm.server).Where(s => s.SnapshotTime > DateTime.Now.AddDays(-1)).OrderBy(s => s.SnapshotTime).ToDictionary(s => s.SnapshotTime, s => s.PlayerCount);
            
            svm.day_snapshots_json = JsonConvert.SerializeObject(snaps_24h, settings);

            Dictionary<DateTime, int> snaps_7d = _dbc.Snapshots.Where(s => s.Server == svm.server).Where(s => s.SnapshotTime > DateTime.Now.AddDays(-7)).OrderBy(s => s.SnapshotTime).ToDictionary(s => s.SnapshotTime, s => s.PlayerCount);
            svm.week_snapshots_json = JsonConvert.SerializeObject(snaps_7d, settings);

            return View("ServerView", svm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("error")]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
