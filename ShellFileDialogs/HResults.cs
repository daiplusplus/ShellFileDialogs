using System;

namespace ShellFileDialogs
{
	internal static class HResults
	{
		public static readonly HResult Cancelled = CreateWin32( code: Win32ErrorCodes.ErrorCancelled );

		/// <summary>Creates a Win32 <see cref="HResult"/> value.</summary>
		public static HResult CreateWin32( Win32ErrorCodes code )
		{
			return Create(
				isFailure : code != Win32ErrorCodes.Success,
				isCustomer: false,
				facility  : HResultFacility.Win32,
				code      : (UInt16)Win32ErrorCodes.ErrorCancelled
			);
		}

		// Bytes:                             333333333    222222222    111111111    000000000
		const UInt32 _codeBitMask     = 0b____0000_0000____0000_0000____1111_1111____1111_1111; // Lower 16 bits
		const UInt32 _facilityBitMask = 0b____0000_0111____1111_1111____0000_0000____0000_0000; // Next 11 bits
		const UInt32 _reserveXBitMask = 0b____0000_1000____0000_0000____0000_0000____0000_0000; // Next 1 bit
		const UInt32 _ntStatusBitMask = 0b____0001_0000____0000_0000____0000_0000____0000_0000; // Next 1 bit
		const UInt32 _customerBitMask = 0b____0010_0000____0000_0000____0000_0000____0000_0000; // Next 1 bit
		const UInt32 _reservedBitMask = 0b____0100_0000____0000_0000____0000_0000____0000_0000; // Highest bit
		const UInt32 _severityBitMask = 0b____1000_0000____0000_0000____0000_0000____0000_0000; // Highest bit

		/// <summary>Creates a non-NTSTATUS <see cref="HResult"/> value.</summary>
		public static HResult Create(
			Boolean         isFailure,
			Boolean         isCustomer,
			HResultFacility facility,
			UInt16          code
		)
		{
			if( facility >= HResultFacility.MAX_VALUE_EXCLUSIVE )
			{
				throw new ArgumentOutOfRangeException( paramName: nameof(facility), actualValue: facility, message: "Value must be in the range 0 through 2047 (inclusive). The HRESULT Facility code is an 11-bit number." );
			}

			UInt32 value = 0;

			if( isFailure )
			{
				value |= _severityBitMask;
			}

			if( isCustomer )
			{
				value |= _customerBitMask;
			}

			//

			{
				UInt32 facilityBits = (UInt32)facility;
				facilityBits = facilityBits << 16;

				value |= facilityBits;
			}

			//

			value |= code;

			//

			return (HResult)value;
		}

		public static HResultSeverity GetSeverity( this HResult hr )
		{
			UInt32 severityBits = (UInt32)hr & _severityBitMask;
			return ( severityBits == 0 ) ? HResultSeverity.Success : HResultSeverity.Failure;
		}

		public static HResultCustomer GetCustomer( this HResult hr )
		{
			UInt32 customerBits = (UInt32)hr & _customerBitMask;
			return ( customerBits == 0 ) ? HResultCustomer.MicrosoftDefined : HResultCustomer.CustomerDefined;
		}

		public static HResultFacility GetFacility( this HResult hr )
		{
			UInt32 facilityBits = (UInt32)hr & _facilityBitMask;

			UInt32 facilityBitsOnly = facilityBits >> 16;

			UInt16 facilityBitsOnlyAsU16 = (UInt16)facilityBitsOnly;

			return (HResultFacility)facilityBitsOnlyAsU16;
		}

		public static UInt16 GetCode( this HResult hr )
		{
			UInt32 codeBits = (UInt32)hr & _codeBitMask;
			return (UInt16)codeBits;
		}

		/// <summary>Indicates if the required zeros for reserved bits are indeed zero - otherwise <paramref name="hr"/> may be an <c>NTSTATUS</c> value or some other 32-bit value.</summary>
		public static Boolean IsValidHResult( this HResult hr )
		{
			UInt32 ntstatusBits = (UInt32)hr & _ntStatusBitMask;
			if( ntstatusBits != 0 )
			{
				// The NTSTATUS bit is set, so this is not a HRESULT.
				return false;
			}

			// If the NTSTATUS (`N`) bit is clear, then the Reserved (`R`) bit must also be clear:
			// > Reserved. If the N bit is clear, this bit MUST be set to 0. If the N bit is set, this bit is defined by the NTSTATUS numbering space (as specified in section 2.3).

			UInt32 reservedBits = (UInt32)hr & _reservedBitMask;
			if( reservedBits != 0 )
			{
				// Invalid HRESULT: `R` must be 0 if `N` is 0.
				return false;
			}

			UInt32 reservedXBits = (UInt32)hr & _reserveXBitMask;
			if( reservedXBits != 0 )
			{
				// The `X` bit should always be false. (Though "should" - implying it *COULD* be non-zero... but how should this function interpret that language in the spec?)
				return false;
			}
			
			return true;
		}

		public static Boolean TryGetWin32ErrorCode( this HResult hr, out Win32ErrorCodes win32Code )
		{
			// Set `win32Code` anyway, just in case the HRESULT's Customer and Facility codes are wrong:
			UInt16 codeBits = GetCode( hr );
			win32Code = (Win32ErrorCodes)codeBits;

			if( IsValidHResult( hr ) )
			{
				// But only return true or false if the flag bits are correct:
				if( GetCustomer( hr ) == HResultCustomer.MicrosoftDefined )
				{
					if( GetFacility( hr ) == HResultFacility.Win32 )
					{
						Boolean hresultSeverityMatchesWin32Code = 
							( GetSeverity( hr ) == HResultSeverity.Success && win32Code == Win32ErrorCodes.Success )
							||
							( GetSeverity( hr ) == HResultSeverity.Failure && win32Code != Win32ErrorCodes.Success );

						if( hresultSeverityMatchesWin32Code )
						{
							return true;
						}
					}
				}
			}

			return false;
		}
	}

	internal enum HResultSeverity
	{
		Success = 0,
		Failure = 1
	}

	internal enum HResultCustomer
	{
		MicrosoftDefined = 0,
		CustomerDefined  = 1
	}

	internal enum HResultFacility : UInt16
	{
		/// <summary>FACILITY_NULL - The default facility code.</summary>
		Null = 0,
		
		/// <summary>FACILITY_RPC - The source of the error code is an RPC subsystem.</summary>
		Rpc = 1,
		
		/// <summary>FACILITY_WIN32 - This region is reserved to map undecorated error codes into HRESULTs.</summary>
		Win32 = 7,
		
		/// <summary>FACILITY_WINDOWS - The source of the error code is the Windows subsystem.</summary>
		Windows = 8,

		MAX_VALUE_INCLUSIVE = 2047,
		MAX_VALUE_EXCLUSIVE = 2048,
	}
}
