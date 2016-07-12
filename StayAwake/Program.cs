using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayAwake
{
	class Program
	{
		#region Main

		static void Main(string[] args)
		{
			Console.WriteLine();
			Console.WriteLine("StayAwake");
			Console.WriteLine();

			try
			{
				// TODO: do stuff.

			}
			catch (Exception ex)
			{
				OutputExceptionInfo(ex);
			}

			Console.WriteLine();
			Console.Write("\nPress ENTER to EXIT...");
			Console.ReadLine();
		}

		#endregion
		#region Private Exception Output Methods

		private static void OutputExceptionInfo(Exception ex)
		{
			Console.WriteLine("");
			Console.WriteLine("Exception Caught.");
			Console.WriteLine(ex.GetType().ToString());
			Console.WriteLine(ex.Message);
			if (ex.InnerException != null)
			{
				OutputInnerExceptionInfoRecursive(ex.InnerException);
			}
		}

		private static void OutputInnerExceptionInfoRecursive(Exception ex)
		{
			Console.WriteLine("");
			Console.WriteLine("Inner");
			Console.WriteLine(ex.GetType().ToString());
			Console.WriteLine(ex.Message);

			if (ex.InnerException != null)
			{
				// Recurse.
				OutputInnerExceptionInfoRecursive(ex.InnerException);
			}
		}

		#endregion
	}
}
