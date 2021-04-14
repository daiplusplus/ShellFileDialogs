using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ShellFileDialogs
{
	[ComImport]
	[Guid( ShellIIDGuid.IFileDialogEvents )]
	[InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
	internal interface IFileDialogEvents
	{
		// NOTE: some of these callbacks are cancelable - returning S_FALSE means that 
		// the dialog should not proceed (e.g. with closing, changing folder); to 
		// support this, we need to use the PreserveSig attribute to enable us to return
		// the proper HRESULT.

		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime ),
		PreserveSig]
		HResult OnFileOk([In, MarshalAs( UnmanagedType.Interface )] IFileDialog pfd);

		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime ),
		PreserveSig]
		HResult OnFolderChanging(
			[In, MarshalAs( UnmanagedType.Interface )] IFileDialog pfd,
			[In, MarshalAs( UnmanagedType.Interface )] IShellItem psiFolder);

		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		void OnFolderChange([In, MarshalAs( UnmanagedType.Interface )] IFileDialog pfd);

		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		void OnSelectionChange([In, MarshalAs( UnmanagedType.Interface )] IFileDialog pfd);

		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		void OnShareViolation(
			[In, MarshalAs( UnmanagedType.Interface )] IFileDialog pfd,
			[In, MarshalAs( UnmanagedType.Interface )] IShellItem psi,
			out FileDialogEventShareViolationResponse pResponse);

		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		void OnTypeChange([In, MarshalAs( UnmanagedType.Interface )] IFileDialog pfd);

		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		void OnOverwrite([In, MarshalAs( UnmanagedType.Interface )] IFileDialog pfd,
			[In, MarshalAs( UnmanagedType.Interface )] IShellItem psi,
			out FileDialogEventOverwriteResponse pResponse);
	}
}
