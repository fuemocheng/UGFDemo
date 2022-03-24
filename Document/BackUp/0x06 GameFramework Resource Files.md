# GameFramework Resource

## 1.Asset->Resource
Asset: 用于描述 Unity 中的一个具体资产，如一个预制体、一个材质、一张图片等

Resource:用于描述 Unity 中的一个 AssetBundle（一些 Asset 的集合），
或者一个 GameFramework 定义下的二进制文件（一个 Asset 的二进制形式，能够用于脱离 Unity 直接加载）
	
	
## 2.ResourceName ==> ResourceManager ==> GF 资源名称 ResourceManager.ResourceName.cs

[StructLayout(LayoutKind.Auto)]  
struct ResourceName : IComparable, IComparable<ResourceName>, IEquatable<ResourceName>  
	//资源名称,变体名称,扩展名称  
	public ResourceName(string name, string variant, string extension){}  
	string Name;		//资源名称  
	string Variant;		//变体名称  
    string Extension;	//扩展名称  
	string FullName;	//Name.Variant.Extension 或者 Name.Extension  
	
## 3.Asset ==> PackageVersionList ==> GF 资源 PackageVersionList.Asset.cs  

[StructLayout(LayoutKind.Auto)]    
struct Asset  
	//初始化资源的新实例  
	public Asset(string name, int[] dependencyAssetIndexes){}  
	string Name;							//资源名称  
	int[] GetDependencyAssetIndexes();		//资源包含的依赖资源索引集  
	
## 4.Resource ==> PackageVersionList ==> GF 资源 PackageVersionList.Resource.cs  

[StructLayout(LayoutKind.Auto)]  
struct Resource  
	//初始化资源的新实例
	public Resource(string name, string variant, string extension, byte loadType, int length, int hashCode, int[] assetIndexes){}
	string Name;				//资源名称
	string Variant;				//资源变体名称
	string Extension;			//资源扩展名称
	byte LoadType;				//资源加载方式
	int Length;					//资源长度
	int HashCode;				//资源哈希值
	int[] GetAssetIndexes();	//资源包含的资源索引集合 private readonly int[] m_AssetIndexes;

## 5.FileSystem ==> PackageVersionList ==> GF 文件系统 PackageVersionList.FileSystem.cs

[StructLayout(LayoutKind.Auto)]  
struct FileSystem  
	//文件系统名称,文件系统包含的资源索引集合
	public FileSystem(string name, int[] resourceIndexes){}
	string Name;					//文件系统名称
	int[] GetResourceIndexes()		//文件系统包含的资源索引集合

## 6.ResourceGroup => PackageVersionList ==> GF 资源组 PackageVersionList.ResourceGroup.cs

[StructLayout(LayoutKind.Auto)]  
struct ResourceGroup	
	//资源组名称,资源组包含的资源索引集合
	public ResourceGroup(string name, int[] resourceIndexes){}
	string Name;					//资源组名称
	int[] GetResourceIndexes();		//资源组包含的资源索引集合
	
## 7.
	
	
	
	
