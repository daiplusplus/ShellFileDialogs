using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ShellFileDialogs
{
	public static class FileSaveDialog
	{
		/// <summary>Shows the file-save dialog. Returns <see langword="null"/> if the user canceled the dialog - otherwise returns the user-selected path.</summary>
		/// <param name="selectedFilterZeroBasedIndex">0-based index of the filter to select. Use <c>-1</c> to indicate no default selection.</param>
#if NETCOREAPP3_1_OR_GREATER
		public static String? ShowDialog(IntPtr parentHWnd, String? title, String? initialDirectory, String? defaultFileName, IReadOnlyCollection<Filter>? filters, Int32 selectedFilterZeroBasedIndex = -1)
#else
		public static String ShowDialog(IntPtr parentHWnd, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32 selectedFilterZeroBasedIndex = -1)
#endif
		{
			NativeFileSaveDialog nfod = new NativeFileSaveDialog();
			try
			{
				return ShowDialogInner( nfod, parentHWnd, title, initialDirectory, defaultFileName, filters );
			}
			finally
			{
				_ = Marshal.ReleaseComObject( nfod );
			}
		}

#if NETCOREAPP3_1_OR_GREATER
		private static String? ShowDialogInner(IFileSaveDialog dialog, IntPtr parentHWnd, String? title, String? initialDirectory, String? defaultFileName, IReadOnlyCollection<Filter>? filters, Int32 selectedFilterZeroBasedIndex = -1)
#else
		private static String ShowDialogInner(IFileSaveDialog dialog, IntPtr parentHWnd, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32 selectedFilterZeroBasedIndex = -1)
#endif
		{
			FileOpenOptions flags =
				FileOpenOptions.NoTestFileCreate |
				FileOpenOptions.PathMustExist |
				FileOpenOptions.ForceFilesystem |
				FileOpenOptions.OverwritePrompt;

			dialog.SetOptions( flags );
			
			if( title != null )
			{
				dialog.SetTitle( title );
			}

			if( initialDirectory != null )
			{
#if NETCOREAPP3_1_OR_GREATER
				IShellItem2? initialDirectoryShellItem = Utility.ParseShellItem2Name( initialDirectory );
#else
				IShellItem2 initialDirectoryShellItem = Utility.ParseShellItem2Name( initialDirectory );
#endif
				if( initialDirectoryShellItem != null )
				{
					dialog.SetFolder( initialDirectoryShellItem );
				}
			}

//			if( initialSaveAsItem != null )
//			{
//				IShellItem2 initialSaveAsItemShellItem = Utility.ParseShellItem2Name( initialDirectory );
//				if( initialSaveAsItemShellItem != null )
//				{
//					dialog.SetSaveAsItem( initialSaveAsItemShellItem );
//				}
//			}

			if( defaultFileName != null )
			{
				dialog.SetFileName( defaultFileName );
			}

			Utility.SetFilters( dialog, filters, selectedFilterZeroBasedIndex );

			//

			HResult hr = dialog.Show( parentHWnd );
			if( hr.ValidateDialogShowHResult() )
			{
				dialog.GetResult( out IShellItem selectedItem );
				if( selectedItem != null )
				{
					return Utility.GetFileNameFromShellItem( selectedItem );
				}
				else
				{
					return null;
				}
			}
			else
			{
				// User cancelled.
				return null;
			}
		}
	}
}
