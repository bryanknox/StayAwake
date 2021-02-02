using System;
using System.Runtime.InteropServices;

namespace StayAwake
{
    class Program
	{
		#region kernel32 SetThreadExecutionState

		// The following is from the good folks at:
		// pinvoke.net
		// http://pinvoke.net/default.aspx/kernel32.SetThreadExecutionState

		/// <summary>
		/// The thread's execution requirements. 
		/// Can be one or more of the following values.
		/// See https://msdn.microsoft.com/en-us/library/aa373208.aspx
		/// </summary>
		/// <remarks>
		/// There is no need to store the state you set, Windows remembers it
		/// for you. Just set it back to ES_CONTINUOUS when you don't want it anymore.
		/// 
		/// Also note that this setting is per thread/application not global, 
		/// so if you go to ES_CONTINUOUS and another app/thread is still setting
		/// ES_DISPLAY the display will be kept on.
		/// </remarks>
		[FlagsAttribute]
		public enum EXECUTION_STATE : uint
		{
			/// <summary>
			/// Enables away mode. This value must be specified with ES_CONTINUOUS.
			/// Away mode should be used only by media-recording and media-distribution
			/// applications that must perform critical background processing on 
			/// desktop computers while the computer appears to be sleeping. 
			/// </summary>
			/// <remarks>
			/// Although Away Mode is supported on any Windows Vista PC, 
			/// the mode must be explicitly allowed by the current power policy.
			/// The Allow Away Mode power setting enables the user to selectively allow
			/// Away Mode on one or more power plans or individually for AC and DC
			/// (on battery) power states.
			/// (see more about this at http://msdn.microsoft.com/en-us/windows/hardware/gg463208.aspx )
			/// </remarks>
			ES_AWAYMODE_REQUIRED = 0x00000040,

			/// <summary>
			/// Informs the system that the state being set should remain in effect
			/// until the next call that uses ES_CONTINUOUS and one of the other 
			/// state flags is cleared.
			/// </summary>
			/// <remarks>
			/// Setting a flag without also using ES_CONTINUOUS merely resets the idle timer, 
			/// so it does not prevent sleep for an extended period.
			/// </remarks>
			ES_CONTINUOUS = 0x80000000,

			/// <summary>
			/// Prevent monitor powerdown.
			/// Forces the display to be on by resetting the display idle timer.
			/// Windows 8:  This flag can only keep a display turned on, it can't
			/// turn on a display that's currently off.
			/// </summary>
			ES_DISPLAY_REQUIRED = 0x00000002,

			/// <summary>
			/// Keep system awake.
			/// Forces the system to be in the working state by resetting the 
			/// system idle timer.
			/// </summary>
			ES_SYSTEM_REQUIRED = 0x00000001
		}

		/// <summary>
		/// Enables an application to inform the system that it is in use, 
		/// thereby preventing the system from entering sleep or turning 
		/// off the display while the application is running.
		/// </summary>
		/// <param name="esFlags"></param>
		/// <returns>
		/// The previous EXECUTION_STATE value.
		/// </returns>
		/// <remarks>
		/// See https://msdn.microsoft.com/en-us/library/aa373208.aspx
		/// </remarks>
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

		#endregion
		#region Main

		static void Main(string[] args)
		{
			Console.WriteLine();
			Console.WriteLine("StayAwake");
			Console.WriteLine();
			Console.WriteLine("While running, this app will:");
			Console.WriteLine(" - Keep the system awake.");
			Console.WriteLine(" - Prevent monitor powerdown.");
			Console.WriteLine();
			Console.WriteLine("Check it using the following command from an Administrator command prompt:");
			Console.WriteLine();
			Console.WriteLine("   powercfg -requests");

			try
			{
				SetThreadExecutionState(
					EXECUTION_STATE.ES_CONTINUOUS
					| EXECUTION_STATE.ES_DISPLAY_REQUIRED // Prevent monitor powerdown.
					| EXECUTION_STATE.ES_SYSTEM_REQUIRED // Keep system awake.
				);

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
