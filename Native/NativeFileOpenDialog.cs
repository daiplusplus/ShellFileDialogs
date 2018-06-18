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
	[ComImport]
    [Guid(ShellIIDGuid.IFileOpenDialog)]
    [CoClass(typeof(FileOpenDialogRCW))]
    internal interface NativeFileOpenDialog : IFileOpenDialog
    {
    }
}
