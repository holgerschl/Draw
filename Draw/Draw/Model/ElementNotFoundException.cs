using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw.Model
{
    class ElementNotFoundException:Exception
    {
        public ElementNotFoundException() : base() { }
        public ElementNotFoundException(string message) : base(message) { }
        public ElementNotFoundException(string message, System.Exception inner) : base(message, inner) { }
    }
}
