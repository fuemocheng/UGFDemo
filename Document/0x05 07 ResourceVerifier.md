# GameFramework ResourceVerifier

资源校验器

## 1.ResourceVerifier ==> ResourceManager ==> GameFramework.Resource
	ResourceManager m_ResourceManager;
	List<VerifyInfo> m_VerifyInfos;
	
## 2.VerifyInfo 资源校验信息
	private struct VerifyInfo()
	
	//资源名称,资源所在的文件系统名称,资源加载方式,资源大小,资源哈希值
	public VerifyInfo(ResourceName resourceName, string fileSystemName, 
			LoadType loadType, int length, int hashCode)
    
	ResourceName m_ResourceName;	//资源名称
	string m_FileSystemName;		//资源所在的文件系统名称
	LoadType m_LoadType;			//资源加载方式
	int m_Length;					//资源大小
	int m_HashCode;					//资源哈希值
	
	public ResourceName ResourceName;	//资源名称
	public bool UseFileSystem;			//资源是否使用文件系统
	public string FileSystemName;		//资源所在的文件系统名称
	public LoadType LoadType;			//资源加载方式
	public int Length;					//资源大小
	public int HashCode;				//资源哈希值
	


















