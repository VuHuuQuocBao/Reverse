using Core2.Core;
using Core2.Enums;

namespace Core2.Utils
{
    using Value = double;
    public static class ChunkUtils
    {
        public static void DisassembleChunk(Chunk chunk, string name)
        {
            Console.WriteLine($"== {name} ==");
            var offset = 0;
            while (offset < chunk.Count)
                offset = DisassembleInstruction(chunk, offset);
        }

        public static int DisassembleInstruction(Chunk chunk, int offset)
        {
            Console.Write($"{offset:D4} ");

            if (offset > 0 && chunk.Lines[offset] == chunk.Lines[offset - 1])
                Console.Write("   | ");
            else
                Console.Write($"{chunk.Lines[offset],4} ");


            byte instruction = chunk.Code[offset];

            switch (instruction)
            {
                case (byte)OpCode.OP_RETURN:
                    return SimpleInstruction("OP_RETURN", offset);
                case (byte)OpCode.OP_CONSTANT:
                    return ConstantInstruction("OP_CONSTANT", chunk, offset);
                default:
                    Console.WriteLine($"Unknown opcode {instruction}");
                    return offset + 1;
            }
        }

        public static int SimpleInstruction(string name, int offset)
        {
            Console.WriteLine(name);
            return offset + 1;
        }

        public static int ConstantInstruction(string name,Chunk chunk, int offset)
        {
            byte constant = chunk.Code[offset + 1];

            Console.Write($"{name,-16} {constant,4} '");

            PrintValue(chunk.Constants.Values[constant]);

            Console.WriteLine("'");

            return offset + 2;
        }
        public static void PrintValue(Value value) => Console.Write($"{value:G}");


    }
}
