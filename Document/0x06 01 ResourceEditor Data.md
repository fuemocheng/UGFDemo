# GameFramework DataStructure

## 1.Editor.ResourceTools
	//加载方式
	public enum LoadType : byte
        LoadFromFile = 0,				//使用文件方式加载。
        LoadFromMemory,					//使用内存方式加载。
        LoadFromMemoryAndQuickDecrypt,	//使用内存快速解密方式加载。
        LoadFromMemoryAndDecrypt,		//使用内存解密方式加载。
        LoadFromBinary,					//使用二进制方式加载。
        LoadFromBinaryAndQuickDecrypt,	//使用二进制快速解密方式加载。
        LoadFromBinaryAndDecrypt		//使用二进制解密方式加载。
	
    //资源类型。
    public enum AssetType : byte
        Unknown = 0,	//未知。
        Asset,			//存放资源。
        Scene,			//存放场景。
		
## 2.Asset ==> UnityGameFramework.Editor.ResourceTools 		Asset.cs
	//指单个资源
	private Asset(string guid, Resource resource){}
	
	public static Asset Create(string guid){}
	public static Asset Create(string guid, Resource resource){}
	
	string Guid;
	string Name;		return AssetDatabase.GUIDToAssetPath(Guid);
	Resource Resource;

## 3.Resource ==> UnityGameFramework.Editor.ResourceTools   	Resource.cs
	//指bundle资源
	private Resource(string name, string variant, string fileSystem, 
		LoadType loadType, bool packed, string[] resourceGroups)
	
	public static Resource Create(string name, string variant, string fileSystem, 
		LoadType loadType, bool packed, string[] resourceGroups)
	
	private readonly List<Asset> m_Assets;
    private readonly List<string> m_ResourceGroups;
	
	string Name;  
	string Variant;
	string FullName;		// Name.Variant or Name  
	AssetType AssetType;
	bool IsLoadFromBinary;
	string FileSystem;
	LoadType LoadType;
	bool Packed;
	
	Asset[] GetAssets();
	public Asset GetFirstAsset();
	public void Rename(string name, string variant);
	public void AssignAsset(Asset asset, bool isScene);
	public void UnassignAsset(Asset asset);
	public string[] GetResourceGroups();
	public bool HasResourceGroup(string resourceGroup);
	public void AddResourceGroup(string resourceGroup);
	public bool RemoveResourceGroup(string resourceGroup);
	public void Clear();
	
## 4.ResourceItem ==> ResourceEditor ==> UnityGameFramework.Editor.ResourceTools  	
	ResourceEditor.ResourceItem.cs
	//ResourceList列表中的资源项
	
	public ResourceItem(string name, Resource resource, ResourceFolder folder){}
	
	string Name;
	Resource Resource;
	ResourceFolder Folder;
	string FromRootPath;
	int Depth;
	Texture Icon;
	
	private static Texture CachedUnknownIcon;
	private static Texture CachedAssetIcon;
	private static Texture CachedSceneIcon;
	private static Texture GetIcon(string iconName);
	
## 5.ResourceFolder ==> ResourceEditor ==> UnityGameFramework.Editor.ResourceTools
	ResourceEditor.ResourceFolder.cs
	//ResourceList列表中的文件夹项
	
	private readonly List<ResourceFolder> m_Folders;
    private readonly List<ResourceItem> m_Items;
	
	public ResourceFolder(string name, ResourceFolder folder){}
	
	string Name;
	ResourceFolder Folder;
	string FromRootPath;
	int Depth;
	static Texture Icon;
	
	ResourceFolder[] GetFolders();
	ResourceFolder GetFolder(string name);
	ResourceFolder AddFolder(string name)
	ResourceItem[] GetItems();
	ResourceItem GetItem(string name);
	void AddItem(string name, Resource resource);

## 6.SourceAsset ==> UnityGameFramework.Editor.ResourceTools
	SourceAsset.cs
	//显示在AssetList列表里的资源项

	public SourceAsset(string guid, string path, string name, SourceFolder folder){}
	
	string Guid;
	string Path;
	string Name;
	SourceFolder Folder;
	string FromRootPath;
	int Depth;
	Texture Icon;
	
## 7.SourceFolder ==> UnityGameFramework.Editor.ResourceTools
	SourceFolder.cs
	//显示在AssetList列表里的文件夹项
	
	private readonly List<SourceFolder> m_Folders;
    private readonly List<SourceAsset> m_Assets;
	
	public SourceFolder(string name, SourceFolder folder){}
	
	string Name;
	SourceFolder Folder;
	string FromRootPath;
	int Depth;
	static Texture Icon;
	
	SourceFolder[] GetFolders();
	SourceFolder GetFolder(string name);
	SourceFolder AddFolder(string name);
	SourceAsset[] GetAssets();
	SourceAsset GetAsset(string name);
	SourceAsset AddAsset(string guid, string path, string name);
	
	
## 8.AssetData ==> ResourceBuilderController ==> UnityGameFramework.Editor.ResourceTools
	ResourceBuilderController.AssetData.cs
	
	public AssetData(string guid, string name, int length, int hashCode, 
		string[] dependencyAssetNames){}
	
	string m_Guid;
	string m_Name;
	int m_Length;
	int m_HashCode;
	string[] m_DependencyAssetNames;
	
## 9.ResourceCode ==> ResourceBuilderController ==> nityGameFramework.Editor.ResourceTools
	ResourceBuilderController.ResourceCode.cs
	
	public ResourceCode(Platform platform, int length, int hashCode, 
		int compressedLength, int compressedHashCode)
		
	Platform m_Platform;
	int m_Length;
	int m_HashCode;
	int m_CompressedLength;
	int m_CompressedHashCode;
	
## 8.ResourceData => ResourceBuilderController ==> UnityGameFramework.Editor.ResourceTools
	ResourceBuilderController.ResourceData.cs  (class)
	
	public ResourceData(string name, string variant, string fileSystem, 
		LoadType loadType, bool packed, string[] resourceGroups)
	
	string m_Name;
	string m_Variant;
	string m_FileSystem;
	LoadType m_LoadType;
	bool m_Packed;
	string[] m_ResourceGroups;
	List<AssetData> m_AssetDatas;
	List<ResourceCode> m_Codes;





	
	
	
	
	
	
