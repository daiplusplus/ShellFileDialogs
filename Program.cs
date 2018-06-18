using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

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
