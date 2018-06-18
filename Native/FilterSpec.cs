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
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct FilterSpec
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        internal string Name;
        [MarshalAs(UnmanagedType.LPWStr)]
        internal string Spec;

        internal FilterSpec(string name, string spec)
        {
            Name = name;
            Spec = spec;
        }
    }
}
