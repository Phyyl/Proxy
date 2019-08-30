using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Phyyl.Proxy
{
    internal interface IILGenerator
    {
        LocalBuilder DeclareLocal(Type localType);
        LocalBuilder DeclareLocal(Type localType, bool pinned);
        void Emit(OpCode opcode);
        void Emit(OpCode opcode, byte arg);
        void Emit(OpCode opcode, ConstructorInfo con);
        void Emit(OpCode opcode, double arg);
        void Emit(OpCode opcode, FieldInfo field);
        void Emit(OpCode opcode, float arg);
        void Emit(OpCode opcode, int arg);
        void Emit(OpCode opcode, Label label);
        void Emit(OpCode opcode, Label[] labels);
        void Emit(OpCode opcode, LocalBuilder local);
        void Emit(OpCode opcode, long arg);
        void Emit(OpCode opcode, MethodInfo meth);
        void Emit(OpCode opcode, short arg);
        void Emit(OpCode opcode, string str);
        void Emit(OpCode opcode, Type cls);
        void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes);
    }
}