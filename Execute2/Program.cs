


using Core2.Core;
using Core2.Enums;
using Core2.Utils;

Chunk chunk = new();

for (int i = 0; i < 4; i++)
{
    var offset = chunk.AddConstant(i);
    chunk.WriteChunk((byte)OpCode.OP_CONSTANT, 123);
    chunk.WriteChunk((byte)offset, 123);
}

chunk.WriteChunk((byte)OpCode.OP_RETURN, 123);

ChunkUtils.DisassembleChunk(chunk, "test chunk");

var vm = new VM();
vm.Chunk = chunk;
var result = vm.Interpret();



MemoryUtils.FreeChunk(chunk);

var G = 1;