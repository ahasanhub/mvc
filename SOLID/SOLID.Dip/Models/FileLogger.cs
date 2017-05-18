using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOLID.Dip.Models
{
    public class FileLogger : ILogger
    {
        public bool LogMessage(string message)
        {
            return true;
        }

        public bool LogMessage(string message, string callStack)
        {
            return true;

        }
    }
}