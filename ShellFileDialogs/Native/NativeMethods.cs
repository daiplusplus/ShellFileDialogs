using System;
using System.Runtime.InteropServices;

namespace ShellFileDialogs
{
	internal static class ShellNativeMethods
	{
		[DllImport( "shell32.dll", CharSet = CharSet.Unicode, SetLastError = true )]
		internal static extern HResult SHCreateItemFromParsingName(
			[MarshalAs( UnmanagedType.LPWStr )] string path,
			// The following parameter is not used - binding context.
			IntPtr pbc,
			ref Guid riid,
			[MarshalAs( UnmanagedType.Interface )] out IShellItem2 shellItem);


	}
}
