using System;
using System.Runtime.InteropServices;

namespace ShellFileDialogs
{
	public static class FolderBrowserDialog
	{
		/// <summary>Shows the folder browser dialog. Returns null if the dialog cancelled. Otherwise returns the selected path.</summary>
		public static String ShowDialog(IntPtr parentHWnd, String title, String initialDirectory)
		{
			NativeFileOpenDialog nfod = new NativeFileOpenDialog();
			try
			{
				return ShowDialogInner( nfod, parentHWnd, title, initialDirectory );
			}
			finally
			{
				Marshal.ReleaseComObject( nfod );
			}
		}

		private static String ShowDialogInner(IFileOpenDialog dialog, IntPtr parentHWnd, String title, String initialDirectory)
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
				IShellItem2 initialDirectoryShellItem = Utility.ParseShellItem2Name( initialDirectory );
				if( initialDirectoryShellItem != null )
				{
					dialog.SetFolder( initialDirectoryShellItem );
				}
			}

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
				return fileNames.Length == 0 ? null : fileNames[0];
			}
		}
	}
}
