#if PROPERTIES

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ShellFileDialogs
{
	[ComImport]
	[Guid(ShellIIDGuid.IPropertyEnumTypeList)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPropertyEnumTypeList
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetCount([Out] out uint pctypes);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetAt(
		[In] uint itype,
		[In] ref Guid riid,   // riid may be IID_IPropertyEnumType
		[Out, MarshalAs(UnmanagedType.Interface)] out IPropertyEnumType ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetConditionAt(
		[In] uint index,
		[In] ref Guid riid,
		out IntPtr ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void FindMatchingIndex(
		[In] PropVariant propvarCmp,
		[Out] out uint pnIndex);
	}
}
#endif
