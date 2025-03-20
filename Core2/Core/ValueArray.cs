using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core2.Core
{
    using Value = double;
    public class ValueArray
    {
        public int Capacity = 0;
        public int Count = 0;
        public Value[] Values = null;

        public void WriteValueArray(Value value)
        {
            if (this.Capacity < this.Count + 1)
            {
                int oldCapacity = this.Capacity;
                this.Capacity = GrowCapacity(oldCapacity);

                Value[] newValues = new Value[this.Capacity];
                if (this.Values != null)
                    Array.Copy(this.Values, newValues, this.Count);
                this.Values = newValues;
            }
            this.Values[this.Count] = value;
            this.Count++;
        }
        private int GrowCapacity(int oldCapacity) => oldCapacity > 0 ? oldCapacity * 2 : 8;
    }
}
