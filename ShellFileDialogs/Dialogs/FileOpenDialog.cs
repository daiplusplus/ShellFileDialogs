using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ShellFileDialogs
{
	public static class FileOpenDialog
	{
		/// <summary>Shows the file open dialog for multiple filename selections. Returns <see langword="null"/> if the user cancelled the dialog, otherwise returns all selected paths.</summary>
		/// <param name="selectedFilterZeroBasedIndex">0-based index of the filter to select.</param>
#if NETCOREAPP3_1_OR_GREATER
		public static IReadOnlyList<String>? ShowMultiSelectDialog(IntPtr parentHWnd, String? title, String? initialDirectory, String? defaultFileName, IReadOnlyCollection<Filter> filters, Int32? selectedFilterZeroBasedIndex)
#else
		public static IReadOnlyList<String> ShowMultiSelectDialog(IntPtr parentHWnd, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32? selectedFilterZeroBasedIndex)
#endif
		{
			return ShowDialog( parentHWnd, title, initialDirectory, defaultFileName, filters, selectedFilterZeroBasedIndex, FileOpenOptions.AllowMultiSelect );
		}

		/// <summary>Shows the file open dialog for a single filename selection. Returns <see langword="null"/> if the user cancelled the dialog, otherwise returns the selected path.</summary>
		/// <param name="selectedFilterZeroBasedIndex">0-based index of the filter to select.</param>
#if NETCOREAPP3_1_OR_GREATER
		public static String? ShowSingleSelectDialog(IntPtr parentHWnd, String? title, String? initialDirectory, String? defaultFileName, IReadOnlyCollection<Filter>? filters, Int32? selectedFilterZeroBasedIndex)
#else
		public static String ShowSingleSelectDialog(IntPtr parentHWnd, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32? selectedFilterZeroBasedIndex)
#endif
		{
#if NETCOREAPP3_1_OR_GREATER
			IReadOnlyList<String>? fileNames = ShowDialog( parentHWnd, title, initialDirectory, defaultFileName, filters, selectedFilterZeroBasedIndex, FileOpenOptions.None );
#else
			IReadOnlyList<String> fileNames = ShowDialog( parentHWnd, title, initialDirectory, defaultFileName, filters, selectedFilterZeroBasedIndex, FileOpenOptions.None );
#endif
			if( fileNames != null && fileNames.Count > 0 )
			{
				return fileNames[0];
			}
			else
			{
				return null;
			}
		}

#if NETCOREAPP3_1_OR_GREATER
		private static IReadOnlyList<String>? ShowDialog(IntPtr parentHWnd, String? title, String? initialDirectory, String? defaultFileName, IReadOnlyCollection<Filter>? filters, Int32? selectedFilterZeroBasedIndex, FileOpenOptions flags)
#else
		private static IReadOnlyList<String> ShowDialog(IntPtr parentHWnd, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32? selectedFilterZeroBasedIndex, FileOpenOptions flags)
#endif
		{
			NativeFileOpenDialog nfod = new NativeFileOpenDialog();
			try
			{
				return ShowDialogInner( nfod, parentHWnd, title, initialDirectory, defaultFileName, filters, selectedFilterZeroBasedIndex: selectedFilterZeroBasedIndex ?? (-1), flags );
			}
			finally
			{
				_ = Marshal.ReleaseComObject( nfod );
			}
		}

#if NETCOREAPP3_1_OR_GREATER
		private static IReadOnlyList<String>? ShowDialogInner(IFileOpenDialog dialog, IntPtr parentHWnd, String? title, String? initialDirectory, String? defaultFileName, IReadOnlyCollection<Filter>? filters, Int32 selectedFilterZeroBasedIndex, FileOpenOptions flags)
#else
		private static IReadOnlyList<String> ShowDialogInner(IFileOpenDialog dialog, IntPtr parentHWnd, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32 selectedFilterZeroBasedIndex, FileOpenOptions flags)
#endif
		{
			flags = flags |
				FileOpenOptions.NoTestFileCreate |
				FileOpenOptions.PathMustExist |
				FileOpenOptions.ForceFilesystem;

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

			if( defaultFileName != null )
			{
				dialog.SetFileName( defaultFileName );
			}

			Utility.SetFilters( dialog, filters, selectedFilterZeroBasedIndex );

			//

			HResult hr = dialog.Show( parentHWnd );
			if( hr.ValidateDialogShowHResult() )
			{
				dialog.GetResults( out IShellItemArray resultsArray );

				IReadOnlyList<String> fileNames = Utility.GetFileNames( resultsArray );
				return fileNames;
			}
			else
			{
				// User cancelled.
				return null;
			}
		}
	}
}
