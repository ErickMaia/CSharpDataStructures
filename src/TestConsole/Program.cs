using DataStructures.Types;

StaticArrayQueue<string> names = new StaticArrayQueue<string>(); 

names.Enqueue("Charles"); 
names.Enqueue("Erick"); 
names.Dequeue(); 
names.Dequeue(); 
names.Dequeue(); 
names.Enqueue("Alexander"); 
names.Enqueue("Kane"); 


System.Console.WriteLine(names.Peek());

