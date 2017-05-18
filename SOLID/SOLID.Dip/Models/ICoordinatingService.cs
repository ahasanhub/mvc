using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID.Dip.Models
{
    public interface ICoordinatingService
    {
        void CordinateTransaction(IList<IWidget> widgets);
    }
}
