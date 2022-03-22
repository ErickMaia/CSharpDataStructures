using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.DataStructures.AbstractTypes;

namespace src.DataStructures.Types
{
    public class StaticArrayQueue<T> : IQueue<T>
    {
        private int _arrayCapacity = 1;
        private T[] _array; 
        private int _arraySize = 0; 

        public StaticArrayQueue()
        {
            _array = new T[_arrayCapacity]; 
        }

        public StaticArrayQueue(T initialValue)
        {
            _array = new T[_arrayCapacity]; 
            Enqueue(initialValue); 
        }

        public void Enqueue(T value){
            _growArrayIfNeeded(); 
            _array[_arraySize] = value; 
            _arraySize += 1; 
        }

        public T Dequeue(){
            
            if(_arraySize <= 0) return default(T); 

            T elementDequeued = _array[0]; 

            for(int i = 0; i < _arraySize - 1; i++){
                _array[i] = _array[i + 1]; 
            }

            _array[_arraySize - 1] = default(T); 

            _arraySize--; 

            _shrinkArrayIfPossible(); 

            return elementDequeued; 
        }

        //internal methods
        private void _growArrayIfNeeded(){
            if(_arraySize + 1 > _arrayCapacity){
                _arrayCapacity *= 2;  
                T[] newArray = new T[_arrayCapacity];

                for(int i = 0; i < _arraySize; i++){
                    newArray[i] = _array[i]; 
                }

                _array = newArray; 

            }
        }

        private void _shrinkArrayIfPossible(){
            if(_arrayCapacity > _arraySize && _arraySize > 0){
                T[] newArray = new T[_arraySize];

                for(int i = 0; i < _arraySize; i++){
                    newArray[i] = _array[i]; 
                }

                _array = newArray; 
                _arrayCapacity = _arraySize; 
            }
        }

        public bool IsEmpty()
        {
            return _arraySize == 0; 
        }

        public T Peek()
        {
            if(_arraySize == 0)
                return default(T); 
            return _array[0]; 
        }

        public int Size()
        {
            return _arraySize;
        }
    }
}