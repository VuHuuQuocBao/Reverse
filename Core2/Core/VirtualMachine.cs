using Core2.Enums;
using Core2.Utils;

namespace Core2.Core
{
    using Value = double;
    public class VM
    {
        public Chunk Chunk { get; set; }
        public byte[] Ip { get; set; }
        public int IpIndex { get; set; } = 0;
        public Value[] Stack { get; set; } = new Value[256]; // set max 256, if exceeds => stack overflow
        public int StackTop { get; set; }
        public InterpretResult Interpret()
        {
            this.Ip = this.Chunk.Code;
            return Run();
        }

        public InterpretResult Run()
        {
            while (true)
            {
                byte instruction;

                try
                {
                    instruction = ReadByte();
                }
                catch (Exception ex)
                {
                    return InterpretResult.INTERPRET_OK;
                }

                switch (instruction)
                {
                    case (byte)OpCode.OP_CONSTANT:
                        {
                            Value constant = ReadConstant();
                            Push(constant);
                            break;
                        }
                    case (byte)OpCode.OP_RETURN:
                        {
                            ChunkUtils.PrintValue(Pop());
                            Console.WriteLine();
                            return InterpretResult.INTERPRET_OK;
                        }
                    case (byte)OpCode.OP_NEGATE:
                        Push(-Pop()); break;
                    default:
                        break;
                }
            }
        }
        private Value ReadConstant() => this.Chunk.Constants.Values[ReadByte()];
        byte ReadByte() => this.Ip[this.IpIndex++];
        private void ResetStack() => this.StackTop = 0;
        public void Push(Value value)
        {
            this.Stack[this.StackTop] = value;
            this.StackTop++;
        }

        public Value Pop()
        {
            this.StackTop--;
            return this.Stack[this.StackTop];
        }

    }

}
