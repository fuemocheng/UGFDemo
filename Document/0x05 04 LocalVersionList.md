# GameFramework LocalVersionList

本地版本资源列表

## 1.LocalVersionList  ==> GameFramework.Resource  
	[StructLayout(LayoutKind.Auto)]  
    public struct LocalVersionList {}
	
	//包含的资源集合,包含的文件系统集合
	public LocalVersionList(Resource[] resources, FileSystem[] fileSystems)
    
	bool IsValid;					//本地版本资源列表是否有效
	
	Resource[] m_Resources;			//包含的资源集合
	FileSystem[] m_FileSystems;		//包含的文件系统集合
	
## 2.Resource ==> 资源 LocalVersionList.Resource.cs  
	[StructLayout(LayoutKind.Auto)]  
	struct Resource {}  
	
	//资源名称,资源变体名称,资源扩展名称,资源加载方式,资源长度,资源哈希值
	public Resource(string name, string variant, string extension, 
		byte loadType, int length, int hashCode)
	
	string Name;				//资源名称
	string Variant;				//资源变体名称
	string Extension;			//资源扩展名称
	byte LoadType;				//资源加载方式
	int Length;					//资源长度
	int HashCode;				//资源哈希值

## 3.FileSystem ==> 文件系统 LocalVersionList.FileSystem.cs
	[StructLayout(LayoutKind.Auto)]  
	struct FileSystem {}  
	
	//文件系统名称,文件系统包含的资源索引集合
	public FileSystem(string name, int[] resourceIndexes){}
	
	string Name;					//文件系统名称
	int[] m_ResourceIndexes()		//文件系统包含的资源索引集合















