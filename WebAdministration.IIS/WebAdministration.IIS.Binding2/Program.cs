using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace WebAdministration.IIS.Binding2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServerManager serverManager=new ServerManager())
            {
                Configuration config = serverManager.GetApplicationHostConfiguration();
                ConfigurationSection sitesSection = config.GetSection("system.applicationHost/sites");
                ConfigurationElementCollection sitesCollection = sitesSection.GetCollection();

                ConfigurationElement siteElement = FindElement(sitesCollection, "site", "name", @"testsite");

                if (siteElement == null) throw new InvalidOperationException("Element not found!");

                ConfigurationElementCollection bindingsCollection = siteElement.GetCollection("bindings");
                ConfigurationElement bindingElement = bindingsCollection.CreateElement("binding");
                bindingElement["protocol"] = @"http";
                bindingElement["bindingInformation"] = @"*:80:test1.site";
                bindingsCollection.Add(bindingElement);

                //ConfigurationElement bindingElement1 = bindingsCollection.CreateElement("binding");
                //bindingElement1["protocol"] = @"https";
                //bindingElement1["bindingInformation"] = @"*:443:";
                //bindingsCollection.Add(bindingElement1);

                serverManager.CommitChanges();
            }
        }
        private static ConfigurationElement FindElement(ConfigurationElementCollection collection, string elementTagName, params string[] keyValues)
        {
            foreach (ConfigurationElement element in collection)
            {
                if (String.Equals(element.ElementTagName, elementTagName, StringComparison.OrdinalIgnoreCase))
                {
                    bool matches = true;
                    for (int i = 0; i < keyValues.Length; i += 2)
                    {
                        object o = element.GetAttributeValue(keyValues[i]);
                        string value = null;
                        if (o != null)
                        {
                            value = o.ToString();
                        }
                        if (!String.Equals(value, keyValues[i + 1], StringComparison.OrdinalIgnoreCase))
                        {
                            matches = false;
                            break;
                        }
                    }
                    if (matches)
                    {
                        return element;
                    }
                }
            }
            return null;
        }
    }
}
