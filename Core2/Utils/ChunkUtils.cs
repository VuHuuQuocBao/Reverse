using Core2.Core;
using Core2.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core2.Utils
{
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
            byte instruction = chunk.Code[offset];

            switch (instruction)
            {
                case (byte)OpCode.OP_RETURN:
                    return SimpleInstruction("OP_RETURN", offset);
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
    }
}
