using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Web.Administration;

namespace WebApplication1.Controllers
{
   
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            using (ServerManager serverManager = new ServerManager())
            {
                Configuration config = serverManager.GetApplicationHostConfiguration();
                ConfigurationSection sitesSection = config.GetSection("system.applicationHost/sites");
                ConfigurationElementCollection sitesCollection = sitesSection.GetCollection();

                ConfigurationElement siteElement = FindElement(sitesCollection, "site", "name", @"testsite");

                if (siteElement == null) throw new InvalidOperationException("Element not found!");

                ConfigurationElementCollection bindingsCollection = siteElement.GetCollection("bindings");
                ConfigurationElement bindingElement = bindingsCollection.CreateElement("binding");
                bindingElement["protocol"] = @"http";
                bindingElement["bindingInformation"] = @"*:80:test2.site";
                bindingsCollection.Add(bindingElement);

                //ConfigurationElement bindingElement1 = bindingsCollection.CreateElement("binding");
                //bindingElement1["protocol"] = @"https";
                //bindingElement1["bindingInformation"] = @"*:443:";
                //bindingsCollection.Add(bindingElement1);

                serverManager.CommitChanges();
            }
            return new string[] { "value1", "value2" };
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

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
