using System;

namespace ShellFileDialogs.Demo
{
	internal static class Program
	{
		public static void Main(string[] args)
		{
			// FolderBrowserDialog
			{
				String selectedDirectory = FolderBrowserDialog.ShowDialog( IntPtr.Zero, "Title", null );
				if( selectedDirectory != null )
				{
					Console.WriteLine( "Folder browser. Selected directory: \"{0}\".", selectedDirectory );
				}
				else
				{
					Console.WriteLine( "Folder browser. Cancelled." );
				}
			}

			// FileOpenDialog
			{
				Filter[] filters = new Filter[]
				{
					new Filter( "Images", "gif", "png", "jpg", "jpeg", "heic", "webp" ),
					new Filter( "Videos", "mov", "wmv", "mp4", "mpeg", "mpg", "avi", "webm" ),
					new Filter( "Audio" , "mp3", "wma", "wav", "aac" ),
					new Filter( "All files" , "*" ),
				};

				String[] fileNames = FileOpenDialog.ShowMultiSelectDialog( IntPtr.Zero, "Title", @"C:\Users\David\Music", defaultFileName: null, filters: filters, selectedFilterIndex: 2 );
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

				String fileName = FileSaveDialog.ShowDialog( IntPtr.Zero, "Title", initialDirectory, defaultFileName, filters, selectedFilterIndex: 2 );
				if( fileName != null )
				{
					Console.WriteLine( "Save file dialog. Selected file: \"{0}\".", fileName );
				}
				else
				{
					Console.WriteLine( "Save file dialog. Cancelled." );
				}
			}

			
		}

		
	}
}
