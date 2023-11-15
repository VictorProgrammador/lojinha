using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.InfraStructure
{
    public interface ILogWritter
    {
        void Writer(string Message);
    }
}
