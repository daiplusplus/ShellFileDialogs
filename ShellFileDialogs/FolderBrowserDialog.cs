using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ShellFileDialogs
{
	public static class FolderBrowserDialog
	{
		/// <summary>Shows the folder browser dialog. Returns <see langword="null"/> if the user cancelled the dialog. Otherwise returns the selected path.</summary>
		public static String? ShowDialog(IntPtr parentHWnd, String? title, String? initialDirectory)
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

		private static String? ShowDialogInner(IFileOpenDialog dialog, IntPtr parentHWnd, String? title, String? initialDirectory)
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
				IShellItem2? initialDirectoryShellItem = Utility.ParseShellItem2Name( initialDirectory );
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

				IReadOnlyList<String?> fileNames = Utility.GetFileNames( resultsArray );
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
