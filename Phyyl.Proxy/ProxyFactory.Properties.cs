using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Phyyl.Proxy
{
    public static partial class ProxyFactory
    {
        private static void AddPropertyImplementation(TypeBuilder typeBuilder, Type interfaceType, PropertyInfo propertyInfo)
        {
            if (propertyInfo.CanRead)
            {
                AddGetterImplementation(typeBuilder, interfaceType, propertyInfo, propertyInfo.GetGetMethod());
            }

            if (propertyInfo.CanWrite)
            {
                AddSetterImplementation(typeBuilder, interfaceType, propertyInfo, propertyInfo.GetSetMethod());
            }
        }

        //TODO: Add property support
        private static void AddGetterImplementation(TypeBuilder typeBuilder, Type interfaceType, PropertyInfo propertyInfo, MethodInfo getterMethodInfo)
        {
            throw new NotImplementedException();
        }

        //TODO: Add property support
        private static void AddSetterImplementation(TypeBuilder typeBuilder, Type interfaceType, PropertyInfo propertyInfo, MethodInfo setterMethodInfo)
        {
            throw new NotImplementedException();
        }
    }
}
