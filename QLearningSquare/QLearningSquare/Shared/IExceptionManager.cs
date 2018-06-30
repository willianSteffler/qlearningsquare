using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    interface IExceptionManager
    {
        void ResourceException(SafeThExc except,string resourceName);
    }
}
