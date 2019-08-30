using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Phyyl.Proxy
{
    public interface IProxyHandler
    {
        object HandleMethod(MethodBase methodBase, object[] parameters);
    }
}
