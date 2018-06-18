using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ShellFileDialogs
{
	internal static class Utility
	{
		public static String[] GetFileNames(IShellItemArray items)
		{
			UInt32 count;
			HResult hresult = items.GetCount( out count );
			if( hresult != HResult.Ok ) throw new Exception( "IShellItemArray.GetCount failed. HResult: " + hresult ); // TODO: Will this ever happen?

			String[] fileNames = new String[ count ];

			for( int i = 0; i < count; i++ )
			{
				IShellItem shellItem = Utility.GetShellItemAt( items, i );
				String fileName = Utility.GetFileNameFromShellItem( shellItem );

				fileNames[i] = fileName;
			}

			return fileNames;
		}

		private static readonly Guid _ishellItem2Guid = new Guid(ShellIIDGuid.IShellItem2);

		public static IShellItem2 ParseShellItem2Name( String value )
		{
			Guid ishellItem2GuidCopy = _ishellItem2Guid;

			IShellItem2 shellItem;
			HResult hresult = ShellNativeMethods.SHCreateItemFromParsingName( value, IntPtr.Zero, ref ishellItem2GuidCopy, out shellItem );
			if( hresult == HResult.Ok )
			{
				return shellItem;
			}
			else
			{
				// TODO: Handle HRESULT error codes?
				return null;
			}
		}

		public static HResult HResultFromWin32(int win32ErrorCode)
		{
			const int FacilityWin32 = 7;

			if( win32ErrorCode > 0 )
			{
				win32ErrorCode = (int)( ( (uint)win32ErrorCode & 0x0000FFFF ) | ( FacilityWin32 << 16 ) | 0x80000000 );
			}
			return (HResult)win32ErrorCode;

		}

		public static String GetFileNameFromShellItem(IShellItem item)
		{
			string filename = null;
			IntPtr pszString = IntPtr.Zero;
			HResult hr = item.GetDisplayName( ShellItemDesignNameOptions.DesktopAbsoluteParsing, out pszString );
			if( hr == HResult.Ok && pszString != IntPtr.Zero )
			{
				filename = Marshal.PtrToStringAuto( pszString );
				Marshal.FreeCoTaskMem( pszString );
			}
			return filename;
		}

		public static IShellItem GetShellItemAt(IShellItemArray array, int i)
		{
			IShellItem result;
			uint index = (uint)i;
			array.GetItemAt( index, out result );
			return result;
		}

		public static FilterSpec[] CreateFilterSpec(ICollection<Filter> filters)
		{
			FilterSpec[] specs = new FilterSpec[ filters.Count ];
			Int32 i = 0;
			foreach( Filter filter in filters )
			{
				specs[i] = filter.ToFilterSpec();
				i++;
			}
			return specs;
		}
	}

	public class Filter
	{
		private static readonly Char[] _extensionTrim = new Char[] { ' ', '.', ';', '*', '\\', '/', '?' };

		public Filter(String displayName, params String[] extensions)
			: this( displayName, (IEnumerable<String>)extensions )
		{
		}

		public Filter(String displayName, IEnumerable<String> extensions)
		{
			if( String.IsNullOrWhiteSpace(displayName) ) throw new ArgumentNullException(nameof(displayName));
			if( extensions == null ) throw new ArgumentNullException(nameof(extensions));

			this.DisplayName = displayName.Trim();
			this.Extensions  = extensions
				.Select( s => s == "*" ? s : s.TrimStart( _extensionTrim ) ) // TrimStart, not Trim or TrimEnd, e.g. dots in extensions, asterisks in extension patterns, etc
				.Where( s => !String.IsNullOrWhiteSpace( s ) )
				.ToList(); // make a copy to prevent possible changes
		}

		public String DisplayName { get; }
		public IReadOnlyList<String> Extensions { get; }

		internal String ToFilterSpecString()
		{
			StringBuilder sb = new StringBuilder();
			Boolean first = true;
			foreach( String extension in this.Extensions )
			{
				if( !first ) sb.Append( ';' );
				first = false;

				sb.Append( "*." );
				sb.Append( extension );
			}

			return sb.ToString();
		}

		internal void ToExtensionList(StringBuilder sb)
		{
			Boolean first = true;
			foreach( String extension in this.Extensions )
			{
				if( !first ) sb.Append( ", " );
				first = false;

				sb.Append( "*." );
				sb.Append( extension );
			}
		}

		public override String ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append( this.DisplayName );
			sb.Append( " (" );
			this.ToExtensionList( sb );
			sb.Append( ")" );
			return sb.ToString();
		}

		internal FilterSpec ToFilterSpec()
		{
			String filter = this.ToFilterSpecString();
			return new FilterSpec( this.DisplayName, filter );
		}
	}
}
