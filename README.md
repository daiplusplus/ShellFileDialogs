# ShellFileDialogs

## What is this?

If you want to use the post-Windows Vista modern File Open, File Save and Folder Browser dialogs then you need to use the Shell COM API, and deal with all the Shell COM interfaces and types.

This is handled for you by the Microsoft Windows API Code Pack - you'll need both Microsoft.WindowsAPICodePack.dll and Microsoft.WindowsAPICodePack.Shell.dll - combined are **632KB**, they also haven't been kept up-to-date by Microsoft, so the latest packages on NuGet are not by Microsoft. It also provides more functionality than you probably need.

Whereas this assembly is only 32KB and will only be about the shell dialogs - not any of the new shell and platform features. You can also copy + paste the raw files into your own projects.

## NuGet

This library is available in two forms on NuGet:

### `Jehoel.ShellFileDialogs`

This is a normal NuGet package that includes separate assembly DLLs (with PDBs and XML documentation) for:
 * .NET Framework 4.8
 * .NET Standard 2.0
 * .NET Core 3.1 (including C# 8.0 nullable reference type annotations)

### <del>`Jehoel.ShellFileDialogs.Content`</del>

(This package (`Jehoel.ShellFileDialogs.Content`) is not currently published as I'm having trouble getting NuGet to work as-intended)

<del>This NuGet package includes `ShellFileDialogs` as C# source files that are added to your project as content files. However the behaviour is very different depending on your project configuration:</del>

* <del>If you're using SDK-style projects or `<PackageReference>` with non-SDK-style projects...</del>
    * <del>...then the `ShellFileDialogs` C# source content files are never added to your project directly, instead they're added during build-time. Note that you cannot (easily) modify the files directly yourself. This is how NuGet now handles package content files (i.e. as immutable content).</del>
* <del>If you're using old-style `package.config`...</del>
    * <del>...then the C# source files are added to a new subdirectory of your project called `ShellFileDialogs` where you can edit the files as you wish, though re-installing this NuGet package will overwrite your changes.</del>


## Usage

The API is intentionally simple. It can be safely used from any context: Console, WPF, or WinForms. The `ShowDialog` methods all accept an `IntPtr hWnd` of the parent window. You can also specify `IntPtr.Zero` when there is no parent window.

### Folder Browser Dialog

	String selection = FolderBrowserDialog.ShowDialog( IntPtr.Zero, "Title", null );
	if( selection == null )
	{
		// Dialog cancelled
	}
	else
	{
		Console.WriteLine( selection );
	}
	
### File Open Dialog

#### Single files

	Filter[] filters = new Filter[]
	{
		new Filter( "Images", "gif", "png", "jpg", "jpeg", "heic", "webp" ),
		new Filter( "Videos", "mov", "wmv", "mp4", "mpeg", "mpg", "avi", "webm" ),
		new Filter( "Audio" , "mp3", "wma", "wav", "aac" ),
		new Filter( "All files" , "*" ),
	};

	String selection = FileOpenDialog.ShowSingleSelectDialog( IntPtr.Zero, "Title", initialDirectory: null, defaultFileName: null, filters: filters );
	if( selection == null )
	{
		// Dialog cancelled
	}
	else
	{
		Console.WriteLine( selection );
	}

#### Multiple files

	// The Filter class can parse WinForms filters:
	Filter[] filters = Filter.ParseWindowsFormsFilter( @"Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*" );
	
	String[] fileNames = FileOpenDialog.ShowMultiSelectDialog( IntPtr.Zero, "Title", initialDirectory: null, defaultFileName: null, filters: filters );
	if( selection == null )
	{
		// Dialog cancelled
	}
	else
	{
		foreach( String fileName in fileNames ) Console.WriteLine( fileName );
	}

### File Save Dialog

	String fileName = FileSaveDialog.ShowDialog( IntPtr.Zero, "Title", initialDirectory: null, defaultFileName: null, filters: filters );
	if( fileName == null )
	{
		// Dialog cancelled
	}
	else
	{
		Console.WriteLine( "Save file dialog. Selected file: \"{0}\".", fileName );
	}

## Feedback and Suggestions

Please post to the GitHub project issues page: https://github.com/Jehoel/ShellFileDialogs/issues
