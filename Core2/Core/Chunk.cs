using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core2.Core
{
    using Value = double;
    public class Chunk
    {
        public int Count { get; set; } = 0;
        public int Capacity { get; set; } = 0;
        public byte[] Code { get; set; } = null;
        public int[] Lines { get; set; }
        public ValueArray Constants { get; set; } = new();
        public Chunk() { }
        public Chunk(int count, int capacity, byte[] code)
        {
            Count = count;
            Capacity = capacity;
            Code = code;
        }

        public void WriteChunk(byte newByte, int Line)
        {
            if (this.Capacity < this.Count + 1)
            {
                int oldCapacity = this.Capacity;
                this.Capacity = GrowCapacity(oldCapacity);

                this.Code = GrowArray(this.Code, oldCapacity, this.Capacity);
                this.Lines = GrowArray(this.Lines, oldCapacity, this.Capacity);

                var newCode = new byte[this.Capacity];
                Array.Copy(this.Code, newCode, oldCapacity);
                this.Code = newCode;
            }
            this.Code[this.Count] = newByte;
            this.Lines[this.Count] = Line;
            this.Count++;
        }
        public int AddConstant(Value value)
        {
            this.Constants.WriteValueArray(value);
            return this.Constants.Count - 1;
        }

        public static T[] GrowArray<T>(T[] array, int oldCount, int newCount)
        {
            T[] newArray = new T[newCount]; 
            if (array != null)
                Array.Copy(array, newArray, oldCount); 
            return newArray;
        }

        private int GrowCapacity(int oldCapacity) => oldCapacity > 0 ? oldCapacity * 2 : 8;
    }

}
