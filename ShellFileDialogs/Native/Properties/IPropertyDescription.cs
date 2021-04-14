#if PROPERTIES

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ShellFileDialogs
{
	[ComImport]
	[Guid(ShellIIDGuid.IPropertyDescription)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPropertyDescription
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetPropertyKey(out PropertyKey pkey);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetCanonicalName([MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetPropertyType(out VarEnum pvartype);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
		PreserveSig]
		HResult GetDisplayName(out IntPtr ppszName);
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetEditInvitation(out IntPtr ppszInvite);
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetTypeFlags([In] PropertyTypeOptions mask, out PropertyTypeOptions ppdtFlags);
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetViewFlags(out PropertyViewOptions ppdvFlags);
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetDefaultColumnWidth(out uint pcxChars);
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetDisplayType(out PropertyDisplayType pdisplaytype);
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetColumnState(out PropertyColumnStateOptions pcsFlags);
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetGroupingRange(out PropertyGroupingRange pgr);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetRelativeDescriptionType(out RelativeDescriptionType prdt);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetRelativeDescription([In] PropVariant propvar1, [In] PropVariant propvar2, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc1, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc2);
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetSortDescription(out PropertySortDescription psd);
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetSortDescriptionLabel([In] bool fDescending, out IntPtr ppszDescription);
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetAggregationType(out PropertyAggregationType paggtype);
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetConditionType(out PropertyConditionType pcontype, out PropertyConditionOperation popDefault);
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetEnumTypeList([In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IPropertyEnumTypeList ppv);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void CoerceToCanonicalValue([In, Out] PropVariant propvar);
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] // Note: this method signature may be wrong, but it is not used.
		HResult FormatForDisplay([In] PropVariant propvar, [In] ref PropertyDescriptionFormatOptions pdfFlags, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult IsValueCanonical([In] PropVariant propvar);
	}
}
#endif
