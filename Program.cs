using System;

namespace ShellFileDialogs
{
	class Program
	{
		static void Main(string[] args)
		{
			String selection = FolderBrowserDialog.ShowDialog( IntPtr.Zero, "Title", null );
			Console.WriteLine( selection );
		}

		
	}
}
