using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ShellFileDialogs
{
	public static class FolderBrowserDialog
	{
		/// <summary>Shows the folder browser dialog. Returns <see langword="null"/> if the user cancelled the dialog. Otherwise returns the selected path.</summary>
#if NETCOREAPP3_1_OR_GREATER
		public static String? ShowDialog(IntPtr parentHWnd, String? title, String? initialDirectory)
#else
		public static String ShowDialog(IntPtr parentHWnd, String title, String initialDirectory)
#endif
		{
			NativeFileOpenDialog nfod = new NativeFileOpenDialog();
			try
			{
				return ShowDialogInner( nfod, parentHWnd, title, initialDirectory );
			}
			finally
			{
				_ = Marshal.ReleaseComObject( nfod );
			}
		}

#if NETCOREAPP3_1_OR_GREATER
		private static String? ShowDialogInner(IFileOpenDialog dialog, IntPtr parentHWnd, String? title, String? initialDirectory)
#else
		private static String ShowDialogInner(IFileOpenDialog dialog, IntPtr parentHWnd, String title, String initialDirectory)
#endif
		{
			//IFileDialog ifd = dialog;
			FileOpenOptions flags =
				FileOpenOptions.NoTestFileCreate |
				FileOpenOptions.PathMustExist |
				FileOpenOptions.PickFolders |
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

			//

			HResult hr = dialog.Show( parentHWnd );
			if( hr.ValidateDialogShowHResult() )
			{
				dialog.GetResults( out IShellItemArray resultsArray );

#if NETCOREAPP3_1_OR_GREATER
				IReadOnlyList<String?> fileNames = Utility.GetFileNames( resultsArray );
#else
				IReadOnlyList<String> fileNames = Utility.GetFileNames( resultsArray );
#endif
				if( fileNames.Count == 0 )
				{
					return null;
				}
				else
				{
					return fileNames[0];
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
