using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Phyyl.Proxy
{
    public static partial class ProxyFactory
    {
        private static void AddEventImplementation(TypeBuilder typeBuilder, Type interfaceType, EventInfo eventInfo)
        {
            AddEventAdderImplementation(typeBuilder, interfaceType, eventInfo, eventInfo.GetAddMethod());
            AddEventRemoverImplementation(typeBuilder, interfaceType, eventInfo, eventInfo.GetRemoveMethod());
        }

        //TODO: Add event support
        private static void AddEventAdderImplementation(TypeBuilder typeBuilder, Type interfaceType, EventInfo eventInfo, MethodInfo adderMethod)
        {
            throw new NotImplementedException();
        }

        //TODO: Add event support
        private static void AddEventRemoverImplementation(TypeBuilder typeBuilder, Type interfaceType, EventInfo eventInfo, MethodInfo removerMethod)
        {
            throw new NotImplementedException();
        }
    }
}
