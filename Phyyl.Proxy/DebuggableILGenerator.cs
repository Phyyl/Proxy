using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Phyyl.Proxy
{
    internal class DebuggableILGenerator : IILGenerator
    {
        private readonly ILGenerator generator;

        public DebuggableILGenerator(ILGenerator generator)
        {
            this.generator = generator;
        }

        public LocalBuilder DeclareLocal(Type localType, bool pinned)
        {
            Console.WriteLine($"LocalBuilder DeclareLocal({localType}, {pinned})");
            return generator.DeclareLocal(localType, pinned);
        }

        public LocalBuilder DeclareLocal(Type localType)
        {
            Console.WriteLine($"LocalBuilder DeclareLocal({localType})");
            return generator.DeclareLocal(localType);
        }

        public void Emit(OpCode opcode, string str)
        {
            Console.WriteLine($"void Emit({opcode}, {str})");
            generator.Emit(opcode, str);
        }

        public void Emit(OpCode opcode, FieldInfo field)
        {
            Console.WriteLine($"void Emit({opcode}, {field})");
            generator.Emit(opcode, field);
        }

        public void Emit(OpCode opcode, Label[] labels)
        {
            Console.WriteLine($"void Emit({opcode}, {labels})");
            generator.Emit(opcode, labels);
        }

        public void Emit(OpCode opcode, Label label)
        {
            Console.WriteLine($"void Emit({opcode}, {label})");
            generator.Emit(opcode, label);
        }

        public void Emit(OpCode opcode, LocalBuilder local)
        {
            Console.WriteLine($"void Emit({opcode}, {local})");
            generator.Emit(opcode, local);
        }

        public void Emit(OpCode opcode, float arg)
        {
            Console.WriteLine($"void Emit({opcode}, {arg})");
            generator.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode, byte arg)
        {
            Console.WriteLine($"void Emit({opcode}, {arg})");
            generator.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode, short arg)
        {
            Console.WriteLine($"void Emit({opcode}, {arg})");
            generator.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode, double arg)
        {
            Console.WriteLine($"void Emit({opcode}, {arg})");
            generator.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode, MethodInfo meth)
        {
            Console.WriteLine($"void Emit({opcode}, {meth})");
            generator.Emit(opcode, meth);
        }

        public void Emit(OpCode opcode, int arg)
        {
            Console.WriteLine($"void Emit({opcode}, {arg})");
            generator.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode)
        {
            Console.WriteLine($"void Emit({opcode})");
            generator.Emit(opcode);
        }

        public void Emit(OpCode opcode, long arg)
        {
            Console.WriteLine($"void Emit({opcode}, {arg})");
            generator.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode, Type cls)
        {
            Console.WriteLine($"void Emit({opcode}, {cls})");
            generator.Emit(opcode, cls);
        }

        public void Emit(OpCode opcode, ConstructorInfo con)
        {
            Console.WriteLine($"void Emit({opcode}, {con})");
            generator.Emit(opcode, con);
        }

        public void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
        {
            Console.WriteLine($"void EmitCall({opcode}, {methodInfo}, {optionalParameterTypes})");
            generator.EmitCall(opcode, methodInfo, optionalParameterTypes);
        }
    }
}
