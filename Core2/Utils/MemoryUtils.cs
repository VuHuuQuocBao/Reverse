using Core2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core2.Utils
{
    public static class MemoryUtils
    {
        public static int GrowCapacity(int capacity) => capacity < 8 ? 8 : capacity * 2;

        public static T[] GrowArray<T>(T[] array, int oldCount, int newCount)
        {
            T[] newArray = new T[newCount];
            Array.Copy(array, newArray, oldCount);
            return newArray;
        }

        public static byte[] Reallocate(byte[] array, int oldSize, int newSize)
        {
            if (newSize == 0)
                return null;

            var newArray = new byte[newSize];

            if (array != null)
                Array.Copy(array, newArray, Math.Min(oldSize, newSize));

            return newArray;
        }

        public static void FreeChunk(Chunk chunk)
        {
            chunk.Code = null;
            InitChunk(chunk);
        }

        public static void InitChunk(Chunk chunk)
        {
            chunk.Count = 0;
            chunk.Capacity = 0;
            chunk.Code = new byte[0];
        }

        // TODO: free array 
    }

}
