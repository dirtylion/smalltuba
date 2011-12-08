namespace SmallTuba
{

	/// <author>Henrik Haugbølle (hhau@itu.dk)</author>
	/// <version>2011-12-08</version>
	/// <summary>
	/// Static debug class used to determine whether
	/// the project is currenctly in debug-mode.
	/// 
	/// Used to modify the calls to external data
	/// sources for correct unit-test response
	/// information.
	/// </summary>
	public class Debug {
		// Used to determine whether to use test versions of
		// the external data sources.
		public static bool ExternalDataSources = false;

		// Used to determine whether console output should be
		// turned on or not.
		public static bool ConsoleOutput = false;
	}
}
