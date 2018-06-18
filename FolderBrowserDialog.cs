using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace ShellFileDialogs
{
	public static class FolderBrowserDialog
	{
		/// <summary>Shows the new folder browser dialog. Returns null if the dialog cancelled. Otherwise returns the selected path.</summary>
		public static String ShowDialog(IntPtr parentHWnd, String title, String initialDirectory)
		{
			NativeFileOpenDialog nfod = new NativeFileOpenDialog();

			IFileDialog ifd = nfod;

			Guid guid = new Guid(ShellIIDGuid.IShellItem2);

			FileOpenOptions flags =
				FileOpenOptions.NoTestFileCreate |
				FileOpenOptions.PathMustExist |
				FileOpenOptions.PickFolders |
				FileOpenOptions.ForceFilesystem;

			ifd.SetOptions( flags );
			if( title != null )
			{
				ifd.SetTitle( title );
			}

			if( initialDirectory != null )
			{
				IShellItem2 initialDirectoryShellItem;
				ShellNativeMethods.SHCreateItemFromParsingName( initialDirectory, IntPtr.Zero, ref guid, out initialDirectoryShellItem );
				if( initialDirectoryShellItem != null )
				{
					ifd.SetFolder( initialDirectoryShellItem );
				}
			}

			HResult result = ifd.Show( parentHWnd );

			HResult cancelledAsHResult = HResultFromWin32( (int)HResult.Win32ErrorCanceled );
			if( result == cancelledAsHResult )
			{
				// Cancelled
				return null;
			}
			else
			{
				// OK

				List<String> fileNames = new List<String>();

				IShellItemArray resultsArray;
				uint count;

				nfod.GetResults( out resultsArray );
				HResult hresult = resultsArray.GetCount(out count);
				if( hresult != HResult.Ok ) throw new Exception( "resultsArray.GetCount failed:" + hresult );

				for( int i = 0; i < count; i++ )
				{
					IShellItem shellItem = GetShellItemAt( resultsArray, i );
					String fileName = GetFileNameFromShellItem( shellItem );

					fileNames.Add( fileName );
				}

				return fileNames.FirstOrDefault();
			}
		}

		private static HResult HResultFromWin32(int win32ErrorCode)
		{
			const int FacilityWin32 = 7;

			if( win32ErrorCode > 0 )
			{
				win32ErrorCode = (int)( ( (uint)win32ErrorCode & 0x0000FFFF ) | ( FacilityWin32 << 16 ) | 0x80000000 );
			}
			return (HResult)win32ErrorCode;

		}

		private static string GetFileNameFromShellItem(IShellItem item)
		{
			string filename = null;
			IntPtr pszString = IntPtr.Zero;
			HResult hr = item.GetDisplayName(ShellItemDesignNameOptions.DesktopAbsoluteParsing, out pszString);
			if( hr == HResult.Ok && pszString != IntPtr.Zero )
			{
				filename = Marshal.PtrToStringAuto( pszString );
				Marshal.FreeCoTaskMem( pszString );
			}
			return filename;
		}

		private static IShellItem GetShellItemAt(IShellItemArray array, int i)
		{
			IShellItem result;
			uint index = (uint)i;
			array.GetItemAt( index, out result );
			return result;
		}
	}
}
