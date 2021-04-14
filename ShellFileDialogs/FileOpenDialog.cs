using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ShellFileDialogs
{
	public static class FileOpenDialog
	{
		/// <summary>Shows the file open dialog for multiple filename selections. Returns null if the dialog cancelled. Otherwise returns all selected paths.</summary>
		/// <param name="selectedFilterIndex">0-based index of the filter to select.</param>
		public static String[] ShowMultiSelectDialog(IntPtr parentHWnd, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32 selectedFilterZeroBasedIndex)
		{
			return ShowDialog( parentHWnd, title, initialDirectory, defaultFileName, filters, selectedFilterZeroBasedIndex, FileOpenOptions.AllowMultiSelect );
		}

		/// <summary>Shows the file open dialog for a single filename selection. Returns null if the dialog cancelled. Otherwise returns the selected path.</summary>
		/// <param name="selectedFilterIndex">0-based index of the filter to select.</param>
		public static String ShowSingleSelectDialog(IntPtr parentHWnd, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32 selectedFilterZeroBasedIndex)
		{
			String[] fileNames = ShowDialog( parentHWnd, title, initialDirectory, defaultFileName, filters, selectedFilterZeroBasedIndex, FileOpenOptions.None );
			if( fileNames != null && fileNames.Length > 0 )
			{
				return fileNames[0];
			}
			else
			{
				return null;
			}
		}

		private static String[] ShowDialog(IntPtr parentHWnd, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32 selectedFilterZeroBasedIndex, FileOpenOptions flags)
		{
			NativeFileOpenDialog nfod = new NativeFileOpenDialog();
			try
			{
				return ShowDialogInner( nfod, parentHWnd, title, initialDirectory, defaultFileName, filters, selectedFilterZeroBasedIndex, flags );
			}
			finally
			{
				Marshal.ReleaseComObject( nfod );
			}
		}

		private static String[] ShowDialogInner(IFileOpenDialog dialog, IntPtr parentHWnd, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32 selectedFilterZeroBasedIndex, FileOpenOptions flags)
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
				IShellItem2 initialDirectoryShellItem = Utility.ParseShellItem2Name( initialDirectory );
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

			HResult result = dialog.Show( parentHWnd );

			HResult cancelledAsHResult = Utility.HResultFromWin32( (int)HResult.Win32ErrorCanceled );
			if( result == cancelledAsHResult )
			{
				// Cancelled
				return null;
			}
			else
			{
				// OK

				IShellItemArray resultsArray;
				dialog.GetResults( out resultsArray );

				String[] fileNames = Utility.GetFileNames( resultsArray );
				return fileNames;
			}
		}
	}
}
