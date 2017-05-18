using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOLID.Dip.Models
{
    public class Widget : IWidget
    {
        private readonly ILogger _logger;

        public Widget(ILogger logger)
        {
            _logger = logger;
        }
        public int Length { get; set; }
        public int Width { get; set; }
        public string OtherStuffPersisted { get; set; }
        public bool DoWork()
        {
            _logger.LogMessage("Log Message");
            return true;
        }
    }
}