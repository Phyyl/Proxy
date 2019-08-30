using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Phyyl.Proxy
{
    internal class DefaultILGenerator : IILGenerator
    {
        private readonly ILGenerator generator;

        public DefaultILGenerator(ILGenerator generator)
        {
            this.generator = generator;
        }

        public LocalBuilder DeclareLocal(Type localType, bool pinned)
        {
            return generator.DeclareLocal(localType, pinned);
        }

        public LocalBuilder DeclareLocal(Type localType)
        {
            return generator.DeclareLocal(localType);
        }

        public void Emit(OpCode opcode, string str)
        {
            generator.Emit(opcode, str);
        }

        public void Emit(OpCode opcode, FieldInfo field)
        {
            generator.Emit(opcode, field);
        }

        public void Emit(OpCode opcode, Label[] labels)
        {
            generator.Emit(opcode, labels);
        }

        public void Emit(OpCode opcode, Label label)
        {
            generator.Emit(opcode, label);
        }

        public void Emit(OpCode opcode, LocalBuilder local)
        {
            generator.Emit(opcode, local);
        }

        public void Emit(OpCode opcode, float arg)
        {
            generator.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode, byte arg)
        {
            generator.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode, short arg)
        {
            generator.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode, double arg)
        {
            generator.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode, MethodInfo meth)
        {
            generator.Emit(opcode, meth);
        }

        public void Emit(OpCode opcode, int arg)
        {
            generator.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode)
        {
            generator.Emit(opcode);
        }

        public void Emit(OpCode opcode, long arg)
        {
            generator.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode, Type cls)
        {
            generator.Emit(opcode, cls);
        }

        public void Emit(OpCode opcode, ConstructorInfo con)
        {
            generator.Emit(opcode, con);
        }

        public void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
        {
            generator.EmitCall(opcode, methodInfo, optionalParameterTypes);
        }
    }
}
