using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStructures.Types
{
    public class PQueue<T> where T : IComparable<T>
    {
        // The number of elements currently inside the heap
        private int _heapSize = 0; 

        // The internal capacity of the heap
        private int _heapCapacity = 0; 

        // A dynamic list to track the elements inside the heap
        private List<T>? _heap = null; 

        /* This map keeps track of the possible indexes a particular
        node value is found in the heap. Having this map lets
        us have O(log(n)) removals and O(1) element containment check
        at the cost of some additional space and minor overhead */
        private Dictionary<T, SortedSet<int>> _map = new Dictionary<T, SortedSet<int>>(); 

        // Construct an initially empty priority Queue
        public PQueue(){
            _heapCapacity = 1; 
        }

        // Construct a priority queue with an initial capacity
        public PQueue(int size)
        {
            _heap = new List<T>(size); 
        } 

        // Construct a priority queue using heapify in O(n) time, a great explanation can be found at:
        // http://www.cs.umd.edu/~meesh/351/mount/lectures/lect14-heapsort-analysis-part.pdf
        public PQueue(T[] elems) {

            int _heapSize = elems.Count();
            _heap = new List<T>(_heapSize);

            // Place all element in heap
            for (int i = 0; i < _heapSize; i++) _heap.Add(elems[i]);

            // Heapify process, O(n)
            for (int i = Math.Max(0, (_heapSize / 2) - 1); i >= 0; i--) _sink(i);
        }

        // Priority Queue construction, O(nlog(n))
        public PQueue(ICollection<T> elems)
        {
            _heapSize = elems.Count(); 
            foreach (T item in elems)
            {
                _heap.Add(item); 
            }
        }

        /// <summary>
        /// Returns true if priority queue is empy. Otherwise, returns false
        /// </summary>
        public bool IsEmpty(){
            return _heapSize == 0; 
        }

        /// <summary>
        /// Clears everything inside the heap. O(n). 
        /// </summary>
        public void _clear(){
            for (int i = 0; i < _heapCapacity; i++)
            {
                _heap.Clear(); 
                _heapSize = 0; 
                _map.Clear(); 
            }
        }

        public int Size(){
            return _heapSize; 
        }

        /// <summary>
        /// Returns the value of the element with the lowest priority in this priority queue. 
        /// If the priority queue is empty, null is returned. 
        /// </summary>
        public T Peek(){
            if(IsEmpty()) return default(T);
            return _heap.ElementAt(0); 
        }

        /// <summary>
        /// Removes the root of the heap. O(log(n)). 
        /// </summary>
        public T Poll(){
            return _removeAt(0); 
        }

        /// <summary>
        /// Test if an element is in heap. O(n). 
        /// </summary>
        public bool Contains(T element){
            if(element == null) return false; 
            return _map.ContainsKey(element); 
        }

        /// <summary>
        /// Adds an element to the priority queue. It must not be null. O(log(n))
        /// </summary>
        public void Add(T element){
            if(element is null) throw new ArgumentNullException(); 

             _heap.Add(element);

            int indexOfLastElem = size() - 1;
            _swim(indexOfLastElem);
        }

        /// <summary>
        /// Tests if value of node i <= node j.
        /// This method assumes i and j are valid indexes. O(1)
        /// </summary>
        private bool _lessOrEqualThan(int i, int j){
            T nodeI = _heap.ElementAt(i); 
            T nodeJ = _heap.ElementAt(j); 
            return nodeI.CompareTo(nodeJ) <= 0; 
        }
        
        /// <summary>
        /// Perform bottom up node swim, O(log(n))
        /// </summary>
        private void _swim(int k) {

            // Grab the index of the next parent node WRT to k
            int parent = (k - 1) / 2;

            // Keep swimming while we have not reached the
            // root and while we're less than our parent.
            while (k > 0 && _lessOrEqualThan(k, parent)) {
            // Exchange k with the parent
            _swap(parent, k);
            k = parent;

            // Grab the index of the next parent node WRT to k
            parent = (k - 1) / 2;
            }
        }

        // Top down node sink, O(log(n))
        private void _sink(int k) {
            int heapSize = size();
            while (true) {
                int left = 2 * k + 1; // Left  node
                int right = 2 * k + 2; // Right node
                int smallest = left; // Assume left is the smallest node of the two children

                // Find which is smaller left or right
                // If right is smaller set smallest to be right
                if (right < heapSize && _lessOrEqualThan(right, left)) smallest = right;

                // Stop if we're outside the bounds of the tree
                // or stop early if we cannot sink k anymore
                if (left >= heapSize || _lessOrEqualThan(k, smallest)) break;

                // Move down the tree following the smallest node
                _swap(smallest, k);
                k = smallest;
            }
        }

        // Return the size of the heap
        public int size() {
            return _heap.Count(); 
        }

        // Swap two nodes. Assumes i & j are valid, O(1)
        private void _swap(int i, int j) {
            T elem_i = _heap.ElementAt(i);
            T elem_j = _heap.ElementAt(j);

            _heap[i] = elem_j;
            _heap[j] = elem_i;
        }

        // Removes a particular element in the heap, O(n)
        public bool Remove(T element) {
            if (element == null) return false;
            // Linear removal via search, O(n)
            for (int i = 0; i < size(); i++) {
            if (element.Equals(_heap.ElementAt(i))) {
                _removeAt(i);
                return true;
            }
            }
            return false;
        }

        // Removes a node at particular index, O(log(n))
        private T _removeAt(int i) {
            if (IsEmpty()) return default(T);

            int indexOfLastElem = size() - 1;
            T removed_data = _heap.ElementAt(i);
            _swap(i, indexOfLastElem);

            // Obliterate the value
            _heap.RemoveAt(indexOfLastElem);

            // Check if the last element was removed
            if (i == indexOfLastElem) return removed_data;
            T elem = _heap.ElementAt(i);

            // Try sinking element
            _sink(i);

            // If sinking did not work try swimming
            if (_heap.ElementAt(i).Equals(elem)) _swim(i);
            return removed_data;
        }

        // TODO IsMinHeap() and override toString()

        // Recursively checks if this heap is a min heap
        // This method is just for testing purposes to make
        // sure the heap invariant is still being maintained
        // Called this method with k=0 to start at the root
        public bool isMinHeap(int k) {
            // If we are outside the bounds of the heap return true
            int heapSize = size();
            if (k >= heapSize) return true;

            int left = 2 * k + 1;
            int right = 2 * k + 2;

            // Make sure that the current node k is less than
            // both of its children left, and right if they exist
            // return false otherwise to indicate an invalid heap
            if (left < heapSize && !_lessOrEqualThan(k, left)) return false;
            if (right < heapSize && !_lessOrEqualThan(k, right)) return false;

            // Recurse on both children to make sure they're also valid heaps
            return isMinHeap(left) && isMinHeap(right);
        }

        public override string ToString()
        {
            return _heap.ToString(); 
        }

    }
}