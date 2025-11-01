using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Exceptions
{
    /// <summary>
    /// Domain exception.
    /// </summary>
    public abstract class DomainException(string message) : Exception(message);
}
