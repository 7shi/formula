using System;
using System.Collections.Generic;
using System.Linq;

namespace Formula
{
	class Value
	{
		public int n;
	}

	class Add
	{
		public Value x, y;
	}

	class MainClass
	{
		public static void Main (string[] args)
		{
			int a = 1;
			int b = 2;
			int c = a + b;
			Console.WriteLine (c);
			Console.WriteLine ("{0} + {1} = {2}", a, b, c);

			var a2 = new Value { n = 1 };
			var b2 = new Value { n = 2 };
			var c2 = new Add { x = a2, y = b2 };
			Console.WriteLine ("{0} + {1} = {2}", a2, b2, c2);
		}
	}
}
