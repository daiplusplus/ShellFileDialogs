using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShellFileDialogs
{
	public class Filter
	{
		private static readonly Char[] _semiColon = new Char[] { ';' };
		private static readonly Char[] _pipe      = new Char[] { '|' };

		/// <summary></summary>
		/// <param name="displayName">Required. Cannot be <see langword="null"/>, empty, nor whitespace.</param>
		/// <param name="extensions">Required. Cannot be <see langword="null"/> or empty (i.e. at least one extension filter must be specified).</param>
		public Filter( String displayName, params String[] extensions )
			: this( displayName, (IEnumerable<String>)extensions )
		{
		}

		/// <summary></summary>
		/// <param name="displayName">Required. Cannot be <see langword="null"/>, empty, nor whitespace.</param>
		/// <param name="extensions">Required. The collection cannot be <see langword="null"/> or empty - and it cannot contain null, empty, or whitespace values - nor duplicate values.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="displayName"/> or <paramref name="extensions"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="extensions"/> is empty or contains only empty file extensions.</exception>
		public Filter( String displayName, IEnumerable<String> extensions )
		{
			if( String.IsNullOrWhiteSpace(displayName) ) throw new ArgumentNullException(nameof(displayName));
			if( extensions is null ) throw new ArgumentNullException(nameof(extensions));

			this.DisplayName = displayName.Trim();

			this.Extensions = extensions
				.Select( s => s.Trim() ) // Trim whitespace
				.Where( s => !String.IsNullOrWhiteSpace( s ) )
				.ToList(); // make a copy to prevent possible changes

			if( this.Extensions.Count == 0 )
			{
				throw new ArgumentException( message: "Extensions collection must not be empty, nor can it contain only null, empty or whitespace extensions.", paramName: nameof(extensions));
			}
		}

		public String DisplayName { get; }

		/// <summary>All extension values have their leading dot and any leading asterisk filter trimmed, so if &quot;<c>*.wav</c>&quot; is passed as an extension string to <see cref="Filter.Filter(string, string[])"/> then it will appear in this list as &quot;<c>wav</c>&quot;.</summary>
		public IReadOnlyList<String> Extensions { get; }

		/// <summary>Returns <see langword="null"/> if the string couldn't be parsed.</summary>
#if NETCOREAPP3_1_OR_GREATER
		public static IReadOnlyList<Filter>? ParseWindowsFormsFilter(String filter)
#else
		public static IReadOnlyList<Filter> ParseWindowsFormsFilter(String filter)
#endif
		{
			// https://msdn.microsoft.com/en-us/library/system.windows.forms.filedialog.filter(v=vs.110).aspx
			if( String.IsNullOrWhiteSpace( filter ) ) return null;

			String[] components = filter.Split( _pipe, StringSplitOptions.RemoveEmptyEntries );
			if( components.Length % 2 != 0 ) return null;

			Filter[] filters = new Filter[ components.Length / 2 ];
			Int32 fi = 0;
			for( Int32 i = 0; i < components.Length; i += 2 )
			{
				String displayName   = components[i];
				String extensionsCat = components[i+1];

				String[] extensions = extensionsCat.Split( _semiColon, StringSplitOptions.RemoveEmptyEntries );

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
				if( !first ) _ = sb.Append( ';' );
				first = false;

				_ = sb.Append( "*." );
				_ = sb.Append( extension );
			}

			return sb.ToString();
		}

		internal void ToExtensionList(StringBuilder sb)
		{
			Boolean first = true;
			foreach( String extension in this.Extensions )
			{
				if( !first ) _ = sb.Append( ", " );
				first = false;

				_ = sb.Append( "*." );
				_ = sb.Append( extension );
			}
		}

		public override String ToString()
		{
			StringBuilder sb = new StringBuilder();
			_ = sb.Append( this.DisplayName );
			
			_ = sb.Append( " (" );
			this.ToExtensionList( sb );
			_ = sb.Append( ')' );

			return sb.ToString();
		}

		internal FilterSpec ToFilterSpec()
		{
			String filter = this.ToFilterSpecString();
			return new FilterSpec( this.DisplayName, filter );
		}
	}
}
