using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using Models = WebSiteManagment.Models;

namespace WebSiteManagment
{
    public class WebSiteManager {

        private const string AppHostConfigPath = @"%windir%\system32\inetsrv\config\applicationhost.config";

        public ServerManager _serverManager;

        public WebSiteManager() {
            _serverManager = new ServerManager(AppHostConfigPath);
        }

        public List<Models.Site> GetWebsites() {
            return _serverManager.Sites.ToList().ConvertAll(s => new  Models.Site {
                Name = s.Name, 
                Applications = s.Applications.ToList().ConvertAll(a=>new Models.Application {
                    Name = a.Path,
                    PoolName = a.ApplicationPoolName
                })
            });
        }
        public List<string> GetPools() {
            return _serverManager.ApplicationPools.ToList().ConvertAll(s => s.Name);
        }

        public void StopSite(string name) {
            var site = _serverManager.Sites.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (site != null) {
                site.Stop();
            }
        }

        public void StopPool(string name) {
            var pool = _serverManager.ApplicationPools.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (pool != null) {
                StopPool(pool);    
            }
        }
        public void StopPool(int index) {
            if (index < _serverManager.ApplicationPools.Count) {
                var pool = _serverManager.ApplicationPools[index];
                StopPool(pool);   
            }
        }

        private void StopPool(ApplicationPool pool) {
            if (pool.State == ObjectState.Started) {
                pool.Stop();
                var proc = pool.WorkerProcesses.FirstOrDefault();
                if (proc != null) {
                    var wProc = System.Diagnostics.Process.GetProcessById(proc.ProcessId);
                    wProc.Close();
                }
            }
        }

        public string GetSiteRoot(string siteName) {
            string res = string.Empty;
            var site =
                _serverManager.Sites.FirstOrDefault(s => s.Name.Equals(siteName, StringComparison.OrdinalIgnoreCase));
            if (site != null) {
                var app = site.Applications.FirstOrDefault();
                if (app != null) {
                    var dir = app.VirtualDirectories.FirstOrDefault();
                    if (dir != null) {
                        res = dir.PhysicalPath;
                    }
                }
            }
            return res;
        }
    }
}
