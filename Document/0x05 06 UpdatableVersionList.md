# GameFramework UpdatableVersionList

可更新模式版本资源列表

## 1.UpdatableVersionList  ==> GameFramework.Resource  
	[StructLayout(LayoutKind.Auto)]  
    public struct PackageVersionList {}
	
	//适配的游戏版本号,内部资源版本号,包含的资源集合,包含的资源集合,包含的文件系统集合,包含的资源组集合
	public UpdatableVersionList(string applicableGameVersion, int internalResourceVersion, 
		Asset[] assets, Resource[] resources, FileSystem[] fileSystems, ResourceGroup[] resourceGroups)
    
	bool IsValid;					//可更新模式版本资源列表是否有效
	string ApplicableGameVersion;	//适配的游戏版本号
	int InternalResourceVersion;	//内部资源版本号
	
	Asset[] m_Assets;
	Resource[] m_Resources;
	FileSystem[] m_FileSystems;
	ResourceGroup[] m_ResourceGroups;
	
## 2.Asset ==> 资源 UpdatableVersionList.Asset.cs  
	[StructLayout(LayoutKind.Auto)]    
	struct Asset {}
	
	//初始化资源的新实例  
	public Asset(string name, int[] dependencyAssetIndexes){}  
	
	string Name;							//资源名称  
	int[] GetDependencyAssetIndexes();		//资源包含的依赖资源索引集  
	
## 3.Resource ==> 资源 UpdatableVersionList.Resource.cs  
	[StructLayout(LayoutKind.Auto)]  
	struct Resource {}  
	
	//初始化资源的新实例
	public Resource(string name, string variant, string extension, byte loadType, int length, int hashCode, 
		int compressedLength, int compressedHashCode, int[] assetIndexes){}
	
	string Name;				//资源名称
	string Variant;				//资源变体名称
	string Extension;			//资源扩展名称
	byte LoadType;				//资源加载方式
	int Length;					//资源长度
	int HashCode;				//资源哈希值
	int CompressedLength;		//资源压缩后长度
	int CompressedHashCode;		//资源压缩后哈希值
	int[] GetAssetIndexes();	//资源包含的资源索引集合 private readonly int[] m_AssetIndexes;

## 4.FileSystem ==> 文件系统 UpdatableVersionList.FileSystem.cs
	[StructLayout(LayoutKind.Auto)]  
	struct FileSystem {}  
	
	//文件系统名称,文件系统包含的资源索引集合
	public FileSystem(string name, int[] resourceIndexes){}
	
	string Name;					//文件系统名称
	int[] GetResourceIndexes()		//文件系统包含的资源索引集合

## 5.ResourceGroup => 资源组 UpdatableVersionList.ResourceGroup.cs
	[StructLayout(LayoutKind.Auto)]  
	struct ResourceGroup {}  
	
	//资源组名称,资源组包含的资源索引集合
	public ResourceGroup(string name, int[] resourceIndexes){}
	
	string Name;					//资源组名称
	int[] GetResourceIndexes();		//资源组包含的资源索引集合



















