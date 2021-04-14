using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShellFileDialogs
{
	public class Filter
	{
		private static readonly Char[] _extensionTrimStart = new Char[] { ' ', '.', ';' };

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
				.Select( s => s.Trim() ) // Trim whitespace
				.Select( s => // Trim leading
					s.StartsWith( "*.", StringComparison.OrdinalIgnoreCase ) ? s.Substring(2) :
					s.StartsWith( "." , StringComparison.OrdinalIgnoreCase ) ? s.Substring(1) : s
				)
				.Where( s => !String.IsNullOrWhiteSpace( s ) )
				.ToList(); // make a copy to prevent possible changes
		}

		public String DisplayName { get; }
		public IReadOnlyList<String> Extensions { get; }

		/// <summary>Returns null if the string couldn't be parsed.</summary>
		public static Filter[] ParseWindowsFormsFilter(String filter)
		{
			// https://msdn.microsoft.com/en-us/library/system.windows.forms.filedialog.filter(v=vs.110).aspx
			if( String.IsNullOrWhiteSpace( filter ) ) return null;

			String[] components = filter.Split( new Char[] { '|' }, StringSplitOptions.RemoveEmptyEntries );
			if( components.Length % 2 != 0 ) return null;

			Filter[] filters = new Filter[ components.Length / 2 ];
			Int32 fi = 0;
			for( Int32 i = 0; i < components.Length; i += 2 )
			{
				String displayName   = components[i];
				String extensionsCat = components[i+1];

				String[] extensions = extensionsCat.Split( new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries );

				filters[fi] = new Filter( displayName, extensions );
				fi++;
			}

			return filters;
		}

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
