using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace WebAdministration.IIS.App
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                Configuration configuration = serverManager.GetApplicationHostConfiguration();
                ConfigurationSection sitesSection = configuration.GetSection("system.applicationHost/sites");
                ConfigurationElementCollection sitesCollection = sitesSection.GetCollection();

                ConfigurationElement siteElement = sitesCollection.CreateElement("site");
                siteElement["name"] = @"testsite";
                siteElement["id"] = 14;
                siteElement["serverAutoStart"] = true;

                ConfigurationElementCollection bindingsCollection = siteElement.GetCollection("bindings");
                ConfigurationElement bindingElement = bindingsCollection.CreateElement("binding");
                bindingElement["protocol"] = @"http";
                bindingElement["bindingInformation"] = @"*:80:test.site";
                bindingsCollection.Add(bindingElement);

                ConfigurationElementCollection siteCollection = siteElement.GetCollection();
                ConfigurationElement applicationElement = siteCollection.CreateElement("application");
                applicationElement["path"] = @"/";
                ConfigurationElementCollection applicationCollection = applicationElement.GetCollection();
                ConfigurationElement virtualDirectoryElement = applicationCollection.CreateElement("virtualDirectory");
                virtualDirectoryElement["path"] = @"/";
                virtualDirectoryElement["physicalPath"] = @"C:\site\web";
                applicationCollection.Add(virtualDirectoryElement);
                siteCollection.Add(applicationElement);
                sitesCollection.Add(siteElement);

                serverManager.CommitChanges();

            }
        }
    }
}
