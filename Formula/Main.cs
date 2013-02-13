using System;
using System.Collections.Generic;
using System.Linq;

namespace Formula
{
	abstract class Expr
	{
		public abstract int Eval ();

		public static Add operator+ (Expr x, int n)
		{
			return new Add (x, new Value (n));
		}
		
		public static Add operator+ (int n, Expr x)
		{
			return new Add (new Value (n), x);
		}
		
		public static Add operator+ (Expr x, Expr y)
		{
			return new Add (x, y);
		}
		
		public static Mul operator* (Expr x, int n)
		{
			return new Mul (x, new Value (n));
		}
		
		public static Mul operator* (int n, Expr x)
		{
			return new Mul (new Value (n), x);
		}
		
		public static Mul operator* (Expr x, Expr y)
		{
			return new Mul (x, y);
		}
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
			foreach (var x in args) {
				if (x is Add)
					list.AddRange ((x as Add).list);
				else
					list.Add (x);
			}
		}

		public override string ToString ()
		{
			return string.Join ("+", list);
		}

		public override int Eval ()
		{
			return (from x in list select x.Eval ()).Sum ();
		}

		public void Sort ()
		{
			list.Sort ((x, y) => (y is Var ? (y as Var).n : 0) -
				(x is Var ? (x as Var).n : 0));
		}

		public void Simplify ()
		{
			Sort ();
			var newlist = new List<Expr> ();
			Expr last = null;
			foreach (var x in list) {
				if (last == null) {
					last = x;
				} else if (last is Value && x is Value) {
					last = new Value ((last as Value).n + (x as Value).n);
				} else {
					var v1 = last as Var;
					var v2 = x as Var;
					if (v1 != null && v2 != null && v1.n == v2.n) {
						last = new Var (v1.a + v2.a, v1.n);
					} else {
						if (last != null)
							newlist.Add (last);
						last = x;
					}
				}
			}
			if (last != null)
				newlist.Add (last);
			list = newlist;
		}
	}

	class Mul : Expr
	{
		public List<Expr> list = new List<Expr> ();
		
		public Mul (params Expr[] args)
		{
			foreach (var x in args) {
				if (x is Mul)
					list.AddRange ((x as Mul).list);
				else
					list.Add (x);
			}
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

	class Var : Expr
	{
		public int a, n;

		public Var (int a = 1, int n = 1)
		{
			this.a = a;
			this.n = n;
		}

		public override string ToString ()
		{
			return (a == 1 ? "" : a.ToString ()) +
				"x" + (n == 1 ? "" : "^" + n);
		}

		public override int Eval ()
		{
			throw new NotImplementedException ();
		}

		public static Var operator* (int a, Var x)
		{
			return new Var (a * x.a, x.n);
		}

		public Var this [int n] {
			get { return new Var (a, this.n * n); }
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

			var f4a = new Add (new Var (), new Value (1));
			Console.WriteLine ("f4a: {0}", f4a);
			var x = new Var ();
			var f4b = x + 1;
			Console.WriteLine ("f4b: {0}", f4b);

			var f5 = x + 2 + 3 * x + 4;
			Console.WriteLine ("f5: {0}", f5);

			var f6 = x [2] + 2 * x + 3 + 4 * x + 5 * x [2] + 6;
			Console.WriteLine ("f6: {0}", f6);

			f5.Sort ();
			f6.Sort ();
			Console.WriteLine ("f5: {0}", f5);
			Console.WriteLine ("f6: {0}", f6);

			f5.Simplify ();
			f6.Simplify ();
			Console.WriteLine ("f5: {0}", f5);
			Console.WriteLine ("f6: {0}", f6);
		}
	}
}
