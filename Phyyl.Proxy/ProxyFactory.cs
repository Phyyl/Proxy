using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Phyyl.Proxy
{
    public static partial class ProxyFactory
    {
        private const string handlerFieldName = "handler";

        public static T CreateProxy<T>(IProxyHandler handler)
        {
            Type instanceType = CreateType<T>();

            return (T)Activator.CreateInstance(instanceType, handler);
        }

        private static Type CreateType<T>()
        {
            Type interfaceType = typeof(T);

            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException($"Type {interfaceType.FullName} is not an interface");
            }

            string typeName = $"__Proxy_{interfaceType.Name}";
            string assemblyName = $"Proxy {Guid.NewGuid()}";
            string moduleName = $"Proxy {Guid.NewGuid()}";

            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.NotPublic);
            FieldBuilder handlerFieldBuilder = typeBuilder.DefineField(handlerFieldName, typeof(IProxyHandler), FieldAttributes.Private | FieldAttributes.InitOnly);

            AddConstructorImplementation(typeBuilder, handlerFieldBuilder);
            AddInterfaceImplementation(typeBuilder, interfaceType, handlerFieldBuilder);

            foreach (Type baseType in interfaceType.GetInterfaces())
            {
                AddInterfaceImplementation(typeBuilder, baseType, handlerFieldBuilder);
            }

            return typeBuilder.CreateTypeInfo().AsType();
        }

        //TODO: Add indexer support
        private static void AddInterfaceImplementation(TypeBuilder typeBuilder, Type interfaceType, FieldBuilder handlerFieldBuilder)
        {
            typeBuilder.AddInterfaceImplementation(interfaceType);

            foreach (PropertyInfo propertyInfo in interfaceType.GetProperties())
            {
                AddPropertyImplementation(typeBuilder, interfaceType, propertyInfo);
            }

            foreach (MethodInfo methodInfo in interfaceType.GetMethods())
            {
                AddMethodImplementation(typeBuilder, interfaceType, methodInfo, handlerFieldBuilder);
            }

            foreach (EventInfo eventInfo in interfaceType.GetEvents())
            {
                AddEventImplementation(typeBuilder, interfaceType, eventInfo);
            }
        }
    }
}
