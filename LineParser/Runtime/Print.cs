using System;

namespace Runtime
{
	public class Print
	{
		public static Action<string> printFunction;

		internal static void print(string message){
			printFunction.Invoke (message);
		}
	}
}

