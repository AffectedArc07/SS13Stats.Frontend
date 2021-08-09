using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SS13Stats.Frontend.Database;
using SS13Stats.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                latest_snaps.Add(_dbc.Snapshots.Where(s => s.Server == S).OrderByDescending(s => s.PlayerCount).First());
            }
            latest_snaps = latest_snaps.OrderByDescending(s => s.PlayerCount).ToList();
            slm.servers = latest_snaps;
            return View("Index", slm);
        }

        [Route("server/{serverid}")]
        public IActionResult GetServer(int serverid) {
            return Ok("TOOD");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("error")]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
