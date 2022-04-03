# GameFramework VerifyInfo

资源更新器

## 1.ResourceUpdater ==> ResourceManager ==> GameFramework.Resource
	ResourceManager m_ResourceManager;
	Queue<ApplyInfo> m_ApplyWaitingInfo;
	List<UpdateInfo> m_UpdateWaitingInfo;
	HashSet<UpdateInfo> m_UpdateWaitingInfoWhilePlaying;
	Dictionary<ResourceName, UpdateInfo> m_UpdateCandidateInfo;
	SortedDictionary<string, List<int>> m_CachedFileSystemsForGenerateReadWriteVersionList;
	List<ResourceName> m_CachedResourceNames;
	
	IDownloadManager m_DownloadManager;
	FileStream m_ApplyingResourcePackStream;
	ResourceGroup m_UpdatingResourceGroup;
	
## 2.ApplyInfo	资源应用信息
	struct ApplyInfo
	
	public ApplyInfo(ResourceName resourceName, string fileSystemName, 
		LoadType loadType, long offset, int length, int hashCode, 
		int compressedLength, int compressedHashCode, string resourcePath)
	
	ResourceName m_ResourceName;	//资源名称
	string m_FileSystemName;		//资源所在的文件系统名称
	LoadType m_LoadType;			//资源加载方式
	long m_Offset;					//资源偏移
	int m_Length;					//资源大小
	int m_HashCode;					//资源哈希值
	int m_CompressedLength;			//压缩后大小
	int m_CompressedHashCode;		//压缩后哈希值
	string m_ResourcePath;			//资源路径
	
## 3.UpdateInfo 资源更新信息
	class UpdateInfo
	
	public UpdateInfo(ResourceName resourceName, string fileSystemName, 
		LoadType loadType, int length, int hashCode, int compressedLength, 
		int compressedHashCode, string resourcePath)
		
	ResourceName m_ResourceName;	//资源名称
	string m_FileSystemName;		//资源所在的文件系统名称
	LoadType m_LoadType;			//资源加载方式
	int m_Length;					//资源大小
	int m_HashCode;					//资源哈希值
	int m_CompressedLength;			//压缩后大小
	int m_CompressedHashCode;		//压缩后哈希值
	string m_ResourcePath;			//资源路径
	
	bool m_Downloading;
	int m_RetryCount;

	...
	bool Downloading;				//获取或设置下载状态
	int RetryCount;					//获取或设置已重试次数











