# GameFramework AssetInfo


## 1.ResourceName ==> GameFramework.Resource 资源名称 ResourceManager.ResourceName.cs

	public ResourceName(string name, string variant, string extension)
	
	string m_Name;				//资源名称
	string m_Variant;			//变体名称
	string m_Extension;			//扩展名称
	
	string FullName;			//全名 Name.Variant.Extension 或者 Name.Extension
	
	string ToString();			// return FullName;
	
## 2.AssetInfo ==> GameFramework.Resource 资源信息 ResourceManager.AssetInfo.cs
	
	//资源名称，所在资源名称，依赖资源名称
	public AssetInfo(string assetName, ResourceName resourceName, string[] dependencyAssetNames)
	
	string m_AssetName;					//资源名称
	ResourceName m_ResourceName;		//所在资源名称
	string[] m_DependencyAssetNames;	//依赖资源名称
	
## 3.ResourceInfo ==> GameFramework.Resource 资源信息 ResourceManager.ResourceInfo.cs 
	
	public ResourceInfo(ResourceName resourceName, string fileSystemName, LoadType loadType, 
		int length, int hashCode, int compressedLength, bool storageInReadOnly, bool ready)
            
	ResourceName m_ResourceName;	//资源名称
	string m_FileSystemName;		//文件系统名称
	LoadType m_LoadType;			//资源加载方式
	int m_Length;					//资源大小
	int m_HashCode;					//资源哈希值
	int m_CompressedLength;			//压缩后资源大小
	bool m_StorageInReadOnly;		//资源是否在只读区
	bool m_Ready;					//资源是否准备完毕
	
## 4.ResourceGroup ==> GameFramework.Resource 资源组 ResourceManager.ResourceGroup.cs
	
	//资源组名称，资源信息引用
	public ResourceGroup(string name, Dictionary<ResourceName, ResourceInfo> resourceInfos)
	
	string m_Name;												//资源组名称
	Dictionary<ResourceName, ResourceInfo> m_ResourceInfos;		//资源信息引用	
	HashSet<ResourceName> m_ResourceNames;						//
	long m_TotalLength;											//资源组包含资源的总大
	long m_TotalCompressedLength;								//资源组包含资源压缩后的总大小

	bool Ready;													//资源组是否准备完毕
	int TotalCount;												//资源组包含资源数量		
	int ReadyCount;												//资源组中已准备完成资源数量
	long TotalLength;											//资源组包含资源的总大小
	long TotalCompressedLength;									//资源组包含资源压缩后的总大小
	long ReadyLength;											//资源组中已准备完成资源的总大小
	long ReadyCompressedLength;									//资源组中已准备完成资源压缩后的总大小
	float Progress;												//资源组的完成进度
	string[] GetResourceNames();								//获取资源组包含的资源名称列表
	void GetResourceNames(List<string> results);				//获取资源组包含的资源名称列表
	bool HasResource(ResourceName resourceName);				//检查指定资源是否属于资源组
	void AddResource(ResourceName resourceName, int length, int compressedLength);		//向资源组中增加资源


















