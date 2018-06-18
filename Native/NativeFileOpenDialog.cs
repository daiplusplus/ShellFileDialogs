using System;
using System.Runtime.InteropServices;

namespace ShellFileDialogs
{
	[ComImport]
	[Guid( ShellIIDGuid.IFileOpenDialog )]
	[CoClass( typeof( FileOpenDialogRCW ) )]
	internal interface NativeFileOpenDialog : IFileOpenDialog
	{
	}
}
