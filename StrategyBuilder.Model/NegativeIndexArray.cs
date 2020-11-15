using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyBuilder.Model
{
    public class NegativeIndexArray<T>
    {
        private int _size;
        private T[] _positives;
        private T[] _negatives;

        public NegativeIndexArray(int size)
        {
            _size = size;
            _positives = new T[_size + 1]; // explicitly count 0 index
            _negatives = new T[_size];
        }

        public T this[int index]
        {
            get { return index >= 0 ? _positives[index] : _negatives[-1 - index]; }
            set 
            {
                if (index >= 0)
                    _positives[index] = value;
                else
                    _negatives[-1 - index] = value; 
            }
        }
    }
}
