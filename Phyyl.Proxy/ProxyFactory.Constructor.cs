using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using static System.Reflection.MethodAttributes;
using static System.Reflection.Emit.OpCodes;

namespace Phyyl.Proxy
{
    public static partial class ProxyFactory
    {
        private static void AddConstructorImplementation(TypeBuilder typeBuilder, FieldBuilder handlerFieldBuilder)
        {
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(Public | HideBySig | SpecialName | RTSpecialName, CallingConventions.Standard, new[]
            {
                typeof(IProxyHandler)
            });

            ILGenerator il = constructorBuilder.GetILGenerator();

            il.Emit(Ldarg_0);
            il.Emit(Call, typeof(object).GetConstructor(Type.EmptyTypes));
            il.Emit(Ldarg_0);
            il.Emit(Ldarg_1);
            il.Emit(Stfld, handlerFieldBuilder);
            il.Emit(Ret);
        }
    }
}
