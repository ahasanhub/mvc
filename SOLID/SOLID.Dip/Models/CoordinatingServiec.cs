using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOLID.Dip.Models
{
    public class CoordinatingServiec : ICoordinatingService
    {
        private readonly ILogger _logger;

        public CoordinatingServiec(ILogger logger)
        {
            _logger = logger;
        }
        public void CordinateTransaction(IList<IWidget> widgets)
        {
            //Begin transaction
            foreach (var widget in widgets)
            {
                widget.DoWork();
            }
            //Commit...
        }
    }
}