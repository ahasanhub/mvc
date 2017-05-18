using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID.Dip.Models
{
    public interface IWidget
    {
        int Length { get; set; }
        int Width { get; set; }
        bool DoWork();
    }
}
