using System;
using System.Collections.Generic;

namespace ShellFileDialogs.Demo
{
	internal static class Program
	{
		// Not using [STAThread] can make COM behave unexpectedly.
		[STAThread] // <-- " If the attribute is not present, the application uses the multithreaded apartment model, which is not supported for Windows Forms."
		public static Int32 Main( String[] args )
		{
			Console.WriteLine( "Now showing a folder browser dialog. Press [Enter] to continue." );
			_ = Console.ReadLine();

			// FolderBrowserDialog
			{
				String selectedDirectory = FolderBrowserDialog.ShowDialog( parentHWnd: IntPtr.Zero, title: "Select a folder...", initialDirectory: null );
				if( selectedDirectory != null )
				{
					Console.WriteLine( "Folder browser. Selected directory: \"{0}\".", selectedDirectory );
				}
				else
				{
					Console.WriteLine( "Folder browser. Cancelled." );
				}
			}

			Console.WriteLine( "Now showing an open-file dialog to select multiple files (with multiple extension filters). Press [Enter] to continue." );
			_ = Console.ReadLine();

			// FileOpenDialog
			{
				Filter[] filters = new Filter[]
				{
					new Filter( "Images", "gif", "png", "jpg", "jpeg", "heic", "webp" ),
					new Filter( "Videos", "mov", "wmv", "mp4", "mpeg", "mpg", "avi", "webm" ),
					new Filter( "Audio" , "mp3", "wma", "wav", "aac" ),
					new Filter( "All files" , "*" ),
				};

				IReadOnlyList<String> fileNames = FileOpenDialog.ShowMultiSelectDialog( IntPtr.Zero, title: "Open multiple files...", initialDirectory: @"C:\Users\David\Music", defaultFileName: null, filters: filters, selectedFilterZeroBasedIndex: 2 );
				if( fileNames != null )
				{
					Console.WriteLine( "Open file dialog. Selected files:" );
					foreach( String fileName in fileNames )
					{
						Console.WriteLine( fileName );
					}
				}
				else
				{
					Console.WriteLine( "Open file dialog. Cancelled." );
				}
			}

			Console.WriteLine( "Now showing an open-file dialog to select a single file (with a single extension filter). Press [Enter] to continue." );
			_ = Console.ReadLine();

			// FileOpenDialog
			{
				const String windowsFormsFilter = @"Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"; // from https://msdn.microsoft.com/en-us/library/system.windows.forms.filedialog.filter(v=vs.110).aspx
				IReadOnlyList<Filter> filters = Filter.ParseWindowsFormsFilter( windowsFormsFilter );

				IReadOnlyList<String> fileNames = FileOpenDialog.ShowMultiSelectDialog( IntPtr.Zero, title: "Open a single file...", initialDirectory: @"C:\Users\David\Music", defaultFileName: null, filters: filters, selectedFilterZeroBasedIndex: 2 );
				if( fileNames != null )
				{
					Console.WriteLine( "Open file dialog. Selected files:" );
					foreach( String fileName in fileNames )
					{
						Console.WriteLine( fileName );
					}
				}
				else
				{
					Console.WriteLine( "Open file dialog. Cancelled." );
				}
			}

			Console.WriteLine( "Now showing an save-file dialog to save a single file (with multiple extension filters). Press [Enter] to continue." );
			_ = Console.ReadLine();

			// FileSaveDialog
			{
				Filter[] filters = new Filter[]
				{
					new Filter( "Images", "gif", "png", "jpg", "jpeg", "heic", "webp" ),
					new Filter( "Videos", "mov", "wmv", "mp4", "mpeg", "mpg", "avi", "webm" ),
					new Filter( "Audio" , "mp3", "wma", "wav", "aac" ),
					new Filter( "All files" , "*" ),
				};

				String initialDirectory = @"C:\Users\David\Music\Aerosmith\2006 - The Very Best Of\";
				String defaultFileName  = /*initialDirectory +*/ @"12 - Aerosmith - Dream On.mp3";

				String fileName = FileSaveDialog.ShowDialog( IntPtr.Zero, "Save a file...", initialDirectory, defaultFileName, filters, selectedFilterZeroBasedIndex: 2 );
				if( fileName != null )
				{
					Console.WriteLine( "Save file dialog. Selected file: \"{0}\".", fileName );
				}
				else
				{
					Console.WriteLine( "Save file dialog. Cancelled." );
				}
			}

			Console.WriteLine( "Shell file dialogs demo completed. Press [Enter] to exit." );
			_ = Console.ReadLine();

			return 0;
		}
	}
}
