using System;
namespace DynamicForms.Models
{
	public class Stack
	{
		private List<bool> array;
		private int top;
		
		public Stack()
		{
			array = new ();
			top = -1;
		}

        public Stack(bool[] stack)
        {
            array = stack.ToList();
			top = stack.Count();
        }

        public void Push(bool obj)
        {
			top++;
			array.Add(obj);
        }

        public bool Pop()
		{
			bool obj = array.ElementAt(top);
			array.RemoveAt(top);
			top--;
			return obj;
		}

		public int Count()
		{
			return top;
		}

		public bool[] GetArray()
		{
			return array.ToArray();
		}


    }
}