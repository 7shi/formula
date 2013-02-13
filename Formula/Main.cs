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
		public List<Expr> list = new List<Expr> ();

		public Add (params Expr[] args)
		{
			list.AddRange (args);
		}

		public override string ToString ()
		{
			return string.Join ("+", list);
		}

		public override int Eval ()
		{
			return (from x in list select x.Eval ()).Sum ();
		}
	}

	class Mul : Expr
	{
		public List<Expr> list = new List<Expr> ();
		
		public Mul (params Expr[] args)
		{
			list.AddRange (args);
		}
		
		public override string ToString ()
		{
			return string.Join (
				"*",
				from x in list select x is Add ? "(" + x + ")" : x.ToString ());
		}
		
		public override int Eval ()
		{
			return (from x in list select x.Eval ()).Aggregate ((x, y) => x * y);
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
