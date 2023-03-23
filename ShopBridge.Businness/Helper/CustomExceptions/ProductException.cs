using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Businness
{
    public class ProductException : Exception
    {
        public ProductException(string message) : base(message) { }
    }
}
