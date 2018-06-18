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
	[ComImport,
    ClassInterface(ClassInterfaceType.None),
    TypeLibType(TypeLibTypeFlags.FCanCreate),
    Guid(ShellCLSIDGuid.FileOpenDialog)]
    internal class FileOpenDialogRCW
    {
    }
}
