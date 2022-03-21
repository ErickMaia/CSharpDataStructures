using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.DataStructures.AbstractTypes
{
    public interface IQueue<T>
    {

        public void Enqueue(T elem);

        public T Dequeue();

        public T Peek();

        public int Size();

        public bool IsEmpty();
    }
}