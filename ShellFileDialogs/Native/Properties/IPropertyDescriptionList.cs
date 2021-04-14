#if PROPERTIES

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ShellFileDialogs
{
	[ComImport]
	[Guid(ShellIIDGuid.IPropertyDescriptionList)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPropertyDescriptionList
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetCount(out uint pcElem);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetAt([In] uint iElem, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyDescription ppv);
	}
}
#endif
