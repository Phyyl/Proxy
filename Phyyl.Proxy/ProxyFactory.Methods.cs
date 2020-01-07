using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using static System.Reflection.MethodAttributes;
using static System.Reflection.Emit.OpCodes;

namespace Phyyl.Proxy
{
    public static partial class ProxyFactory
    {
        private static readonly Dictionary<Type, OpCode> ldindOpCodes = new Dictionary<Type, OpCode>
        {
            [typeof(bool)] = Ldind_U1,
            [typeof(byte)] = Ldind_U1,
            [typeof(sbyte)] = Ldind_I1,
            [typeof(char)] = Ldind_U2,
            [typeof(ushort)] = Ldind_U2,
            [typeof(short)] = Ldind_I2,
            [typeof(uint)] = Ldind_U4,
            [typeof(int)] = Ldind_I4,
            [typeof(ulong)] = Ldind_I8,
            [typeof(long)] = Ldind_I8,
            [typeof(float)] = Ldind_R4,
            [typeof(double)] = Ldind_R8,
            [typeof(IntPtr)] = Ldind_I,
            [typeof(UIntPtr)] = Ldind_I,
        };

        private static readonly Dictionary<Type, OpCode> stindOpCodes = new Dictionary<Type, OpCode>
        {
            [typeof(bool)] = Stind_I1,
            [typeof(byte)] = Stind_I1,
            [typeof(sbyte)] = Stind_I1,
            [typeof(char)] = Stind_I2,
            [typeof(ushort)] = Stind_I2,
            [typeof(short)] = Stind_I2,
            [typeof(uint)] = Stind_I4,
            [typeof(int)] = Stind_I4,
            [typeof(ulong)] = Stind_I8,
            [typeof(long)] = Stind_I8,
            [typeof(float)] = Stind_R4,
            [typeof(double)] = Stind_R8,
            [typeof(IntPtr)] = Stind_I,
            [typeof(UIntPtr)] = Stind_I,
        };

        private static void AddMethodImplementation(TypeBuilder typeBuilder, Type interfaceType, MethodInfo methodInfo, FieldBuilder handlerFieldBuilder)
        {
            string methodName = $"{interfaceType.Name}.{methodInfo.Name}";

            ParameterInfo[] parameters = methodInfo.GetParameters();
            Type[] ParameterTypes = parameters.Select(p => p.ParameterType).ToArray();
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodName, Private | Final | HideBySig | NewSlot | Virtual, methodInfo.ReturnType, ParameterTypes);

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo parameter = parameters[i];
                methodBuilder.DefineParameter(i + 1, parameter.Attributes, parameter.Name);
            }

            IILGenerator il = CreateILGenerator(methodBuilder.GetILGenerator());
            LocalBuilder parametersLocal = il.DeclareLocal(typeof(object[]));
            LocalBuilder resultLocal = il.DeclareLocal(typeof(object));
            bool hasReturnValue = methodInfo.ReturnType == typeof(void);
            int parametersLocalLength = parameters.Length;

            il.Emit(Ldc_I4, parametersLocalLength);
            il.Emit(Newarr, typeof(object));
            il.Emit(Stloc, parametersLocal);

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo parameterInfo = parameters[i];
                Type parameterType = parameterInfo.ParameterType;
                TypeInfo parameterTypeInfo = parameterType.GetTypeInfo();

                il.Emit(Ldloc, parametersLocal);
                il.Emit(Ldc_I4, i);
                il.Emit(Ldarg, i + 1);

                if (parameterType.IsByRef)
                {
                    Type elementType = parameterType.GetElementType();
                    TypeInfo elementTypeInfo = elementType.GetTypeInfo();

                    if (elementTypeInfo.IsClass)
                    {
                        il.Emit(Ldind_Ref);
                    }
                    else if (ldindOpCodes.TryGetValue(elementType, out var opCode))
                    {
                        il.Emit(opCode);
                    }
                    else
                    {
                        il.Emit(Ldobj, elementType);
                    }
                }
                else if (!parameterTypeInfo.IsClass)
                {
                    il.Emit(Box, parameterType);
                }

                il.Emit(Stelem_Ref);
            }

            il.Emit(Ldarg_0);
            il.Emit(Ldfld, handlerFieldBuilder);
            il.Emit(Call, typeof(MethodBase).GetMethod(nameof(MethodBase.GetCurrentMethod)));
            il.Emit(Ldloc, parametersLocal);
            il.Emit(Callvirt, typeof(IProxyHandler).GetMethod(nameof(IProxyHandler.HandleMethod)));
            il.Emit(Stloc, resultLocal);

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo parameterInfo = parameters[i];
                Type parameterType = parameterInfo.ParameterType;
                TypeInfo parameterTypeInfo = parameterType.GetTypeInfo();

                if (parameterType.IsByRef)
                {
                    Type elementType = parameterType.GetElementType();
                    TypeInfo elementTypeInfo = elementType.GetTypeInfo();

                    il.Emit(Ldarg, i + 1);
                    il.Emit(Ldloc, parametersLocal);
                    il.Emit(Ldc_I4, i);
                    il.Emit(Ldelem_Ref);

                    if (elementTypeInfo.IsClass)
                    {
                        il.Emit(Castclass, elementType);
                        il.Emit(Stind_Ref);
                    }
                    else
                    {
                        il.Emit(Unbox_Any, elementType);

                        if (stindOpCodes.TryGetValue(elementType, out var opCode))
                        {
                            il.Emit(opCode);
                        }
                        else
                        {
                            il.Emit(Stobj, elementType);
                        }
                    }
                }
            }

            if (methodInfo.ReturnType != typeof(void))
            {
                il.Emit(Ldloc, resultLocal);

                if (!methodInfo.ReturnType.GetTypeInfo().IsClass)
                {
                    il.Emit(Unbox_Any, methodInfo.ReturnType);
                }
            }

            il.Emit(Ret);

            typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
        }

        private static IILGenerator CreateILGenerator(ILGenerator ilGenerator)
        {
#if DEBUG
            return new DebuggableILGenerator(ilGenerator);
#else
            return new DefaultILGenerator(ilGenerator);
#endif
        }
    }
}
