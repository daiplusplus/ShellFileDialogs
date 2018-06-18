using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ShellFileDialogs
{
	[ComImport]
	[Guid( ShellIIDGuid.IShellItemArray )]
	[InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
	internal interface IShellItemArray
	{
		// Not supported: IBindCtx.
		[PreserveSig]
		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		HResult BindToHandler(
			[In, MarshalAs( UnmanagedType.Interface )] IntPtr pbc,
			[In] ref Guid rbhid,
			[In] ref Guid riid,
			out IntPtr ppvOut);

		[PreserveSig]
		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		HResult GetPropertyStore(
			[In] int Flags,
			[In] ref Guid riid,
			out IntPtr ppv);

#if PROPERTIES
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetPropertyDescriptionList(
			[In] ref PropertyKey keyType,
			[In] ref Guid riid,
			out IntPtr ppv);
#else
		[PreserveSig]
		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		HResult GetPropertyDescriptionList(
			[In] IntPtr keyType,
			[In] ref Guid riid,
			out IntPtr ppv);
#endif

		[PreserveSig]
		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		HResult GetAttributes(
			[In] ShellItemAttributeOptions dwAttribFlags,
			[In] ShellFileGetAttributesOptions sfgaoMask,
			out ShellFileGetAttributesOptions psfgaoAttribs);

		[PreserveSig]
		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		HResult GetCount(out uint pdwNumItems);

		[PreserveSig]
		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		HResult GetItemAt(
			[In] uint dwIndex,
			[MarshalAs( UnmanagedType.Interface )] out IShellItem ppsi);

		// Not supported: IEnumShellItems (will use GetCount and GetItemAt instead).
		[PreserveSig]
		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		HResult EnumItems([MarshalAs( UnmanagedType.Interface )] out IntPtr ppenumShellItems);
	}
}
