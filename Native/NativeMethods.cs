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
	internal static class ShellNativeMethods
	{
		[DllImport( "shell32.dll", CharSet = CharSet.Unicode, SetLastError = true )]
		internal static extern int SHCreateItemFromParsingName(
			[MarshalAs( UnmanagedType.LPWStr )] string path,
			// The following parameter is not used - binding context.
			IntPtr pbc,
			ref Guid riid,
			[MarshalAs( UnmanagedType.Interface )] out IShellItem2 shellItem);


	}
}
