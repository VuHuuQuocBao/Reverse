


using Core2.Core;
using Core2.Enums;
using Core2.Utils;

Chunk chunk = new();

chunk.WriteChunk((byte)OpCode.OP_RETURN);

ChunkUtils.DisassembleChunk(chunk, "test chunk");

MemoryUtils.FreeChunk(chunk);

var G = 1;