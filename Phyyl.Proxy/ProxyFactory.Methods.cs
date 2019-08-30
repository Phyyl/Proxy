using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using static System.Reflection.MethodAttributes;
using static System.Reflection.Emit.OpCodes;
using System.Linq;

namespace Phyyl.Proxy
{
    public static partial class ProxyFactory
    {
        private static void AddMethodImplementation(TypeBuilder typeBuilder, Type interfaceType, MethodInfo methodInfo, FieldBuilder handlerFieldBuiler)
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

            IILGenerator il = new DefaultILGenerator(methodBuilder.GetILGenerator());

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
                    else
                    {
                        switch (elementType.FullName)
                        {
                            case "System.Boolean":
                            case "System.Byte":
                                il.Emit(Ldind_U1);
                                break;
                            case "System.SByte":
                                il.Emit(Ldind_I1);
                                break;
                            case "System.Char":
                                il.Emit(Ldind_U2);
                                break;
                            case "System.Int16":
                                il.Emit(Ldind_I2);
                                break;
                            case "System.UInt16":
                                il.Emit(Ldind_U2);
                                break;
                            case "System.Int32":
                                il.Emit(Ldind_I4);
                                break;
                            case "System.UInt32":
                                il.Emit(Ldind_U4);
                                break;
                            case "System.Int64":
                            case "System.UInt64":
                                il.Emit(Ldind_I8);
                                break;
                            case "System.Double":
                                il.Emit(Ldind_R8);
                                break;
                            case "System.Single":
                                il.Emit(Ldind_R4);
                                break;
                            case "System.UIntPtr":
                            case "System.IntPtr":
                                il.Emit(Ldind_I);
                                break;
                            default:
                                il.Emit(Ldobj, elementType);
                                break;
                        }

                        il.Emit(Box, elementType);
                    }
                }
                else if (!parameterTypeInfo.IsClass)
                {
                    il.Emit(Box, parameterType);
                }

                il.Emit(Stelem_Ref);
            }

            il.Emit(Ldarg_0);
            il.Emit(Ldfld, handlerFieldBuiler);
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

                        switch (elementType.FullName)
                        {
                            case "System.Boolean":
                            case "System.Byte":
                            case "System.SByte":
                                il.Emit(Stind_I1);
                                break;
                            case "System.Char":
                            case "System.Int16":
                            case "System.UInt16":
                                il.Emit(Stind_I2);
                                break;
                            case "System.Int32":
                            case "System.UInt32":
                                il.Emit(Stind_I4);
                                break;
                            case "System.Int64":
                            case "System.UInt64":
                                il.Emit(Stind_I8);
                                break;
                            case "System.Double":
                                il.Emit(Stind_R8);
                                break;
                            case "System.Single":
                                il.Emit(Stind_R4);
                                break;
                            case "System.UIntPtr":
                            case "System.IntPtr":
                                il.Emit(Stind_I);
                                break;
                            default:
                                il.Emit(Stobj, elementType);
                                break;
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
    }
}
