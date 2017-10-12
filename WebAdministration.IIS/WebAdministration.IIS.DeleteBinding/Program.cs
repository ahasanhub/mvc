using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace WebAdministration.IIS.DeleteBinding
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                Configuration config = serverManager.GetApplicationHostConfiguration();
                ConfigurationSection sitesSection = config.GetSection("system.applicationHost/sites");
                ConfigurationElementCollection sitesCollection = sitesSection.GetCollection();

                ConfigurationElement siteElement = FindElement(sitesCollection, "site", "name", @"EHR");

                if (siteElement == null) throw new InvalidOperationException("Element not found!");

                ConfigurationElementCollection bindingsCollection = siteElement.GetCollection("bindings");
                for (int i = 0; i < bindingsCollection.Count; i++)
                {
                    var bindingInfo = bindingsCollection[i].Attributes["bindingInformation"].Value;
                    if (bindingInfo.ToString()== @"*:80:ss.text")
                    {
                        bindingsCollection.RemoveAt(i);
                    }
                    
                }
                //ConfigurationElement bindingElement = bindingsCollection.CreateElement("binding");
                //bindingElement["protocol"] = @"http";
                //bindingElement["bindingInformation"] = @"*:80:ss.text";
                //bindingsCollection.Remove(bindingElement);
                serverManager.CommitChanges();
            }
            Console.WriteLine("Success");
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
