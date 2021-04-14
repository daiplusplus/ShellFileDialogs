using System;

namespace ShellFileDialogs
{
	internal enum SICHINTF
	{
		SICHINT_DISPLAY = 0x00000000,
		SICHINT_CANONICAL = 0x10000000,
		SICHINT_TEST_FILESYSPATH_IF_NOT_EQUAL = 0x20000000,
		SICHINT_ALLFIELDS = unchecked((int)0x80000000)
	}

	/// <summary>Note: these are *not* HResult codes. But Win32 errors can be converted to HResults!</summary>
	/// <remarks>See https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-erref/18d8fbe8-a967-4f1c-ae50-99ca8e491d2d
	/// &quot;All Win32 error codes MUST be in the range 0x0000 to 0xFFFF, although Win32 error codes can be used both in 16-bit fields (such as within the HRESULT type specified in section 2.1) as well as 32-bit fields.&quot;
	/// </remarks>
	internal enum Win32ErrorCodes : UInt16
	{
		/// <summary><c>ERROR_SUCCESS = 0x0000 == 0</c></summary>
		Success = 0,
		
		/// <summary><c>ERROR_CANCELLED = 0x000004C7 == 1223</c></summary>
		ErrorCancelled = 1223,
	}

	/// <summary>Remember that HRESULT values are actually 32-bit packed structures which *encapsulate* 16-bit Win32 error codes - and other error codes. Use the methods in <see cref="HResults"/> to correctly inspect a <see cref="HResult"/> value.</summary>
	internal enum HResult : UInt32
	{
		/// <summary>S_OK</summary>    
		Ok = 0x0000,

//		/// <summary>S_FALSE</summary> 
//		/// <remarks>Why on earth is <c>1</c> being used to represent <c>false</c>?!?</remarks>
//		False = 0x0001,

		/// <summary>E_INVALIDARG</summary>
		InvalidArguments = 0x80070057,

		/// <summary>E_OUTOFMEMORY</summary>
		OutOfMemory = 0x8007000E,

		/// <summary>E_NOINTERFACE</summary>
		NoInterface = 0x80004002,

		/// <summary>E_FAIL</summary>
		Fail = 0x80004005,

		/// <summary>E_ELEMENTNOTFOUND</summary>
		ElementNotFound =0x80070490,

		/// <summary>TYPE_E_ELEMENTNOTFOUND</summary>
		TypeElementNotFound = 0x8002802B,

		/// <summary>NO_OBJECT</summary>
		NoObject = 0x800401E5,

		/// <summary>ERROR_CANCELLED</summary>
		Canceled = 0x800704C7,

		/// <summary>The requested resource is in use</summary>
		ResourceInUse = 0x800700AA,

		/// <summary>The requested resources is read-only.</summary>
		AccessDenied = 0x80030005
	}

	[Flags]
	internal enum FileOpenOptions
	{
		None               = 0,
		OverwritePrompt    = 0x00000002,
		StrictFileTypes    = 0x00000004,
		NoChangeDirectory  = 0x00000008,
		PickFolders        = 0x00000020,
		// Ensure that items returned are filesystem items.
		ForceFilesystem    = 0x00000040,
		// Allow choosing items that have no storage.
		AllNonStorageItems = 0x00000080,
		NoValidate         = 0x00000100,
		AllowMultiSelect   = 0x00000200,
		PathMustExist      = 0x00000800,
		FileMustExist      = 0x00001000,
		CreatePrompt       = 0x00002000,
		ShareAware         = 0x00004000,
		NoReadOnlyReturn   = 0x00008000,
		NoTestFileCreate   = 0x00010000,
		HideMruPlaces      = 0x00020000,
		HidePinnedPlaces   = 0x00040000,
		NoDereferenceLinks = 0x00100000,
		DontAddToRecent    = 0x02000000,
		ForceShowHidden    = 0x10000000,
		DefaultNoMiniMode  = 0x20000000
	}

	/// <summary>
	/// Indicate flags that modify the property store object retrieved by methods 
	/// that create a property store, such as IShellItem2::GetPropertyStore or 
	/// IPropertyStoreFactory::GetPropertyStore.
	/// </summary>
	[Flags]
	internal enum GetPropertyStoreOptions
	{
		/// <summary>
		/// Meaning to a calling process: Return a read-only property store that contains all 
		/// properties. Slow items (offline files) are not opened. 
		/// Combination with other flags: Can be overridden by other flags.
		/// </summary>
		Default = 0,

		/// <summary>
		/// Meaning to a calling process: Include only properties directly from the property
		/// handler, which opens the file on the disk, network, or device. Meaning to a file 
		/// folder: Only include properties directly from the handler.
		/// 
		/// Meaning to other folders: When delegating to a file folder, pass this flag on 
		/// to the file folder; do not do any multiplexing (MUX). When not delegating to a 
		/// file folder, ignore this flag instead of returning a failure code.
		/// 
		/// Combination with other flags: Cannot be combined with GPS_TEMPORARY, 
		/// GPS_FASTPROPERTIESONLY, or GPS_BESTEFFORT.
		/// </summary>
		HandlePropertiesOnly = 0x1,

		/// <summary>
		/// Meaning to a calling process: Can write properties to the item. 
		/// Note: The store may contain fewer properties than a read-only store. 
		/// 
		/// Meaning to a file folder: ReadWrite.
		/// 
		/// Meaning to other folders: ReadWrite. Note: When using default MUX, 
		/// return a single unmultiplexed store because the default MUX does not support ReadWrite.
		/// 
		/// Combination with other flags: Cannot be combined with GPS_TEMPORARY, GPS_FASTPROPERTIESONLY, 
		/// GPS_BESTEFFORT, or GPS_DELAYCREATION. Implies GPS_HANDLERPROPERTIESONLY.
		/// </summary>
		ReadWrite = 0x2,

		/// <summary>
		/// Meaning to a calling process: Provides a writable store, with no initial properties, 
		/// that exists for the lifetime of the Shell item instance; basically, a property bag 
		/// attached to the item instance. 
		/// 
		/// Meaning to a file folder: Not applicable. Handled by the Shell item.
		/// 
		/// Meaning to other folders: Not applicable. Handled by the Shell item.
		/// 
		/// Combination with other flags: Cannot be combined with any other flag. Implies GPS_READWRITE
		/// </summary>
		Temporary = 0x4,

		/// <summary>
		/// Meaning to a calling process: Provides a store that does not involve reading from the 
		/// disk or network. Note: Some values may be different, or missing, compared to a store 
		/// without this flag. 
		/// 
		/// Meaning to a file folder: Include the "innate" and "fallback" stores only. Do not load the handler.
		/// 
		/// Meaning to other folders: Include only properties that are available in memory or can 
		/// be computed very quickly (no properties from disk, network, or peripheral IO devices). 
		/// This is normally only data sources from the IDLIST. When delegating to other folders, pass this flag on to them.
		/// 
		/// Combination with other flags: Cannot be combined with GPS_TEMPORARY, GPS_READWRITE, 
		/// GPS_HANDLERPROPERTIESONLY, or GPS_DELAYCREATION.
		/// </summary>
		FastPropertiesOnly = 0x8,

		/// <summary>
		/// Meaning to a calling process: Open a slow item (offline file) if necessary. 
		/// Meaning to a file folder: Retrieve a file from offline storage, if necessary. 
		/// Note: Without this flag, the handler is not created for offline files.
		/// 
		/// Meaning to other folders: Do not return any properties that are very slow.
		/// 
		/// Combination with other flags: Cannot be combined with GPS_TEMPORARY or GPS_FASTPROPERTIESONLY.
		/// </summary>
		OpensLowItem = 0x10,

		/// <summary>
		/// Meaning to a calling process: Delay memory-intensive operations, such as file access, until 
		/// a property is requested that requires such access. 
		/// 
		/// Meaning to a file folder: Do not create the handler until needed; for example, either 
		/// GetCount/GetAt or GetValue, where the innate store does not satisfy the request. 
		/// Note: GetValue might fail due to file access problems.
		/// 
		/// Meaning to other folders: If the folder has memory-intensive properties, such as 
		/// delegating to a file folder or network access, it can optimize performance by 
		/// supporting IDelayedPropertyStoreFactory and splitting up its properties into a 
		/// fast and a slow store. It can then use delayed MUX to recombine them.
		/// 
		/// Combination with other flags: Cannot be combined with GPS_TEMPORARY or 
		/// GPS_READWRITE
		/// </summary>
		DelayCreation = 0x20,

		/// <summary>
		/// Meaning to a calling process: Succeed at getting the store, even if some 
		/// properties are not returned. Note: Some values may be different, or missing,
		/// compared to a store without this flag. 
		/// 
		/// Meaning to a file folder: Succeed and return a store, even if the handler or 
		/// innate store has an error during creation. Only fail if substores fail.
		/// 
		/// Meaning to other folders: Succeed on getting the store, even if some properties 
		/// are not returned.
		/// 
		/// Combination with other flags: Cannot be combined with GPS_TEMPORARY, 
		/// GPS_READWRITE, or GPS_HANDLERPROPERTIESONLY.
		/// </summary>
		BestEffort = 0x40,

		/// <summary>
		/// Mask for valid GETPROPERTYSTOREFLAGS values.
		/// </summary>
		MaskValid = 0xff,
	}


	internal enum FileDialogEventShareViolationResponse
	{
		Default = 0x00000000,
		Accept = 0x00000001,
		Refuse = 0x00000002
	}
	internal enum FileDialogEventOverwriteResponse
	{
		Default = 0x00000000,
		Accept = 0x00000001,
		Refuse = 0x00000002
	}
	internal enum FileDialogAddPlacement
	{
		Bottom = 0x00000000,
		Top = 0x00000001,
	}

	internal enum ShellItemDesignNameOptions
	{
		Normal = 0x00000000,           // SIGDN_NORMAL
		ParentRelativeParsing = unchecked((int)0x80018001),   // SIGDN_INFOLDER | SIGDN_FORPARSING
		DesktopAbsoluteParsing = unchecked((int)0x80028000),  // SIGDN_FORPARSING
		ParentRelativeEditing = unchecked((int)0x80031001),   // SIGDN_INFOLDER | SIGDN_FOREDITING
		DesktopAbsoluteEditing = unchecked((int)0x8004c000),  // SIGDN_FORPARSING | SIGDN_FORADDRESSBAR
		FileSystemPath = unchecked((int)0x80058000),             // SIGDN_FORPARSING
		Url = unchecked((int)0x80068000),                     // SIGDN_FORPARSING
		ParentRelativeForAddressBar = unchecked((int)0x8007c001),     // SIGDN_INFOLDER | SIGDN_FORPARSING | SIGDN_FORADDRESSBAR
		ParentRelative = unchecked((int)0x80080001)           // SIGDN_INFOLDER
	}

	internal enum ShellItemAttributeOptions
	{
		// if multiple items and the attirbutes together.
		And = 0x00000001,
		// if multiple items or the attributes together.
		Or = 0x00000002,
		// Call GetAttributes directly on the 
		// ShellFolder for multiple attributes.
		AppCompat = 0x00000003,

		// A mask for SIATTRIBFLAGS_AND, SIATTRIBFLAGS_OR, and SIATTRIBFLAGS_APPCOMPAT. Callers normally do not use this value.
		Mask = 0x00000003,

		// Windows 7 and later. Examine all items in the array to compute the attributes. 
		// Note that this can result in poor performance over large arrays and therefore it 
		// should be used only when needed. Cases in which you pass this flag should be extremely rare.
		AllItems = 0x00004000
	}

	[Flags]
	internal enum ShellFileGetAttributesOptions
	{
		/// <summary>
		/// The specified items can be copied.
		/// </summary>
		CanCopy = 0x00000001,

		/// <summary>
		/// The specified items can be moved.
		/// </summary>
		CanMove = 0x00000002,

		/// <summary>
		/// Shortcuts can be created for the specified items. This flag has the same value as DROPEFFECT. 
		/// The normal use of this flag is to add a Create Shortcut item to the shortcut menu that is displayed 
		/// during drag-and-drop operations. However, SFGAO_CANLINK also adds a Create Shortcut item to the Microsoft 
		/// Windows Explorer's File menu and to normal shortcut menus. 
		/// If this item is selected, your application's IContextMenu::InvokeCommand is invoked with the lpVerb 
		/// member of the CMINVOKECOMMANDINFO structure set to "link." Your application is responsible for creating the link.
		/// </summary>
		CanLink = 0x00000004,

		/// <summary>
		/// The specified items can be bound to an IStorage interface through IShellFolder::BindToObject.
		/// </summary>
		Storage = 0x00000008,

		/// <summary>
		/// The specified items can be renamed.
		/// </summary>
		CanRename = 0x00000010,

		/// <summary>
		/// The specified items can be deleted.
		/// </summary>
		CanDelete = 0x00000020,

		/// <summary>
		/// The specified items have property sheets.
		/// </summary>
		HasPropertySheet = 0x00000040,

		/// <summary>
		/// The specified items are drop targets.
		/// </summary>
		DropTarget = 0x00000100,

		/// <summary>
		/// This flag is a mask for the capability flags.
		/// </summary>
		CapabilityMask = 0x00000177,

		/// <summary>
		/// Windows 7 and later. The specified items are system items.
		/// </summary>
		System = 0x00001000,

		/// <summary>
		/// The specified items are encrypted.
		/// </summary>
		Encrypted = 0x00002000,

		/// <summary>
		/// Indicates that accessing the object = through IStream or other storage interfaces, 
		/// is a slow operation. 
		/// Applications should avoid accessing items flagged with SFGAO_ISSLOW.
		/// </summary>
		IsSlow = 0x00004000,

		/// <summary>
		/// The specified items are ghosted icons.
		/// </summary>
		Ghosted = 0x00008000,

		/// <summary>
		/// The specified items are shortcuts.
		/// </summary>
		Link = 0x00010000,

		/// <summary>
		/// The specified folder objects are shared.
		/// </summary>    
		Share = 0x00020000,

		/// <summary>
		/// The specified items are read-only. In the case of folders, this means 
		/// that new items cannot be created in those folders.
		/// </summary>
		ReadOnly = 0x00040000,

		/// <summary>
		/// The item is hidden and should not be displayed unless the 
		/// Show hidden files and folders option is enabled in Folder Settings.
		/// </summary>
		Hidden = 0x00080000,

		/// <summary>
		/// This flag is a mask for the display attributes.
		/// </summary>
		DisplayAttributeMask = 0x000FC000,

		/// <summary>
		/// The specified folders contain one or more file system folders.
		/// </summary>
		FileSystemAncestor = 0x10000000,

		/// <summary>
		/// The specified items are folders.
		/// </summary>
		Folder = 0x20000000,

		/// <summary>
		/// The specified folders or file objects are part of the file system 
		/// that is, they are files, directories, or root directories).
		/// </summary>
		FileSystem = 0x40000000,

		/// <summary>
		/// The specified folders have subfolders = and are, therefore, 
		/// expandable in the left pane of Windows Explorer).
		/// </summary>
		HasSubFolder = unchecked((int)0x80000000),

		/// <summary>
		/// This flag is a mask for the contents attributes.
		/// </summary>
		ContentsMask = unchecked((int)0x80000000),

		/// <summary>
		/// When specified as input, SFGAO_VALIDATE instructs the folder to validate that the items 
		/// pointed to by the contents of apidl exist. If one or more of those items do not exist, 
		/// IShellFolder::GetAttributesOf returns a failure code. 
		/// When used with the file system folder, SFGAO_VALIDATE instructs the folder to discard cached 
		/// properties retrieved by clients of IShellFolder2::GetDetailsEx that may 
		/// have accumulated for the specified items.
		/// </summary>
		Validate = 0x01000000,

		/// <summary>
		/// The specified items are on removable media or are themselves removable devices.
		/// </summary>
		Removable = 0x02000000,

		/// <summary>
		/// The specified items are compressed.
		/// </summary>
		Compressed = 0x04000000,

		/// <summary>
		/// The specified items can be browsed in place.
		/// </summary>
		Browsable = 0x08000000,

		/// <summary>
		/// The items are nonenumerated items.
		/// </summary>
		Nonenumerated = 0x00100000,

		/// <summary>
		/// The objects contain new content.
		/// </summary>
		NewContent = 0x00200000,

		/// <summary>
		/// It is possible to create monikers for the specified file objects or folders.
		/// </summary>
		CanMoniker = 0x00400000,

		/// <summary>
		/// Not supported.
		/// </summary>
		HasStorage = 0x00400000,

		/// <summary>
		/// Indicates that the item has a stream associated with it that can be accessed 
		/// by a call to IShellFolder::BindToObject with IID_IStream in the riid parameter.
		/// </summary>
		Stream = 0x00400000,

		/// <summary>
		/// Children of this item are accessible through IStream or IStorage. 
		/// Those children are flagged with SFGAO_STORAGE or SFGAO_STREAM.
		/// </summary>
		StorageAncestor = 0x00800000,

		/// <summary>
		/// This flag is a mask for the storage capability attributes.
		/// </summary>
		StorageCapabilityMask = 0x70C50008,

		/// <summary>
		/// Mask used by PKEY_SFGAOFlags to remove certain values that are considered 
		/// to cause slow calculations or lack context. 
		/// Equal to SFGAO_VALIDATE | SFGAO_ISSLOW | SFGAO_HASSUBFOLDER.
		/// </summary>
		PkeyMask = unchecked((int)0x81044000),
	}

	[Flags]
	internal enum ShellFolderEnumerationOptions : ushort
	{
		CheckingForChildren = 0x0010,
		Folders = 0x0020,
		NonFolders = 0x0040,
		IncludeHidden = 0x0080,
		InitializeOnFirstNext = 0x0100,
		NetPrinterSearch = 0x0200,
		Shareable = 0x0400,
		Storage = 0x0800,
		NavigationEnum = 0x1000,
		FastItems = 0x2000,
		FlatList = 0x4000,
		EnableAsync = 0x8000
	}
}
