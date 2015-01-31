using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSiteManagment.Models {
    public class Site {
        public string Name { get; set; }
        public List<Application> Applications { get; set; }
        public override string ToString() {
            return string.Format("Name: {0}, Applications: [{1}]", Name, string.Join(", ", Applications));
        }
    }

    public class Application {
        public string Name { get; set; }
        public string PoolName { get; set; }
        public override string ToString() {
            return string.Format("Name: {0}, Pool: {1}", Name, PoolName);
        }
    }

    public class AppPool {
        public string Name { get; set; }
    }
}
