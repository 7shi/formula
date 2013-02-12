using System;
using System.Collections.Generic;
using System.Linq;

namespace Formula
{
	abstract class Expr
	{
		public abstract int Eval ();
	}

	class Value : Expr
	{
		public int n;

		public Value (int n)
		{
			this.n = n;
		}

		public override string ToString ()
		{
			return n.ToString ();
		}

		public override int Eval ()
		{
			return n;
		}
	}

	class Add : Expr
	{
		public Expr x, y;

		public Add (Expr x, Expr y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString ()
		{
			return x + "+" + y;
		}

		public override int Eval ()
		{
			return x.Eval () + y.Eval ();
		}
	}

	class Mul : Expr
	{
		public Expr x, y;
		
		public Mul (Expr x, Expr y)
		{
			this.x = x;
			this.y = y;
		}
		
		public override string ToString ()
		{
			string sx, sy;
			if (x is Add)
				sx = "(" + x + ")";
			else
				sx = x.ToString ();
			if (y is Add)
				sy = "(" + y + ")";
			else
				sy = y.ToString ();
			return sx + "*" + sy;
		}
		
		public override int Eval ()
		{
			return x.Eval () * y.Eval ();
		}
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

			var a2 = new Value (1);
			var b2 = new Value (2);
			var c2 = new Add (a2, b2);
			Console.WriteLine ("{0} + {1} = {2}", a2, b2, c2);
			Console.WriteLine ("c2: {0} = {1}", c2, c2.Eval ());

			var f1 = new Add (
				new Value (1),
				new Value (2));
			Console.WriteLine ("f1: {0} = {1}", f1, f1.Eval ());

			var f2 = new Add (
				new Add (new Value (1), new Value (2)),
				new Value (3));
			Console.WriteLine ("f2: {0} = {1}", f2, f2.Eval ());

			var f3a = new Add (
				new Value (1),
				new Mul (new Value (2), new Value (3)));
			var f3b = new Mul (
				new Add (new Value (1), new Value (2)),
				new Value (3));
			Console.WriteLine ("f3a: {0} = {1}", f3a, f3a.Eval ());
			Console.WriteLine ("f3b: {0} = {1}", f3b, f3b.Eval ());
		}
	}
}
