# ShellFileDialogs

## What is this?

If you want to use the post-Windows Vista modern File Open, File Save and Folder Browser dialogs then you need to use the Shell COM API, and deal with all the Shell COM interfaces and types.

This is handled for you by the Microsoft Windows API Code Pack - you'll need both Microsoft.WindowsAPICodePack.dll and Microsoft.WindowsAPICodePack.Shell.dll - combined are 632KB, they also haven't been kept up-to-date by Microsoft, so the latest packages on NuGet are not by Microsoft. It also provides more functionality than you probably need.

Whereas this assembly is only 32KB and will only be about the shell dialogs - not any of the new shell and platform features.

## Usage

The API is intentionally simple:

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
	
