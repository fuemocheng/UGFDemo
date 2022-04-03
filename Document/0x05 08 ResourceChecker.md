# GameFramework ResourceChecker

资源检查器

## 1.ResourceChecker ==> ResourceManager ==> GameFramework.Resource
	ResourceManager m_ResourceManager;
	Dictionary<ResourceName, CheckInfo> m_CheckInfos;

## 2.CheckInfo  资源检查信息
	class CheckInfo{}
	
	//资源名称
	public CheckInfo(ResourceName resourceName)
    
	ResourceName m_ResourceName;		//资源名称
	CheckStatus m_Status;				//资源检查状态
	bool m_NeedRemove;					//是否需要移除读写区的资源。
	bool m_NeedMoveToDisk;				//是否需要将读写区的资源移动到磁盘
	bool m_NeedMoveToFileSystem;		//是否需要将读写区的资源移动到文件系统
	RemoteVersionInfo m_VersionInfo;	//
	LocalVersionInfo m_ReadOnlyInfo;	//
	LocalVersionInfo m_ReadWriteInfo;	//
	string m_CachedFileSystemName;		//
	
	ResourceName ResourceName;			//资源名称
	CheckStatus Status;					//资源检查状态
	bool NeedRemove;					//是否需要移除读写区的资源	
	bool NeedMoveToDisk;				//是否需要将读写区的资源移动到磁盘
	bool NeedMoveToFileSystem;			//是否需要将读写区的资源移动到文件系统
	string FileSystemName;				//资源所在的文件系统名称
	bool ReadWriteUseFileSystem;		//资源是否使用文件系统
	string ReadWriteFileSystemName;		//读写资源所在的文件系统名称
	LoadType LoadType;					//资源加载方式
	int Length;							//资源大小
	int HashCode;						//资源哈希值
	int CompressedLength;				//压缩后大小
	int CompressedHashCode;				//压缩后哈希值
	
	void SetCachedFileSystemName(string fileSystemName);			//临时缓存资源所在的文件系统名称
	void SetVersionInfo(LoadType loadType, int length, int hashCode, 
		int compressedLength, int compressedHashCode)					//设置资源在版本中的信息
	void SetReadOnlyInfo(LoadType loadType, int length, int hashCode)	//设置资源在只读区中的信息
	void SetReadWriteInfo(LoadType loadType, int length, int hashCode)	//设置资源在读写区中的信息
	void RefreshStatus(string currentVariant, bool ignoreOtherVariant)	//刷新资源信息状态

## 3.CheckStatus : byte	资源检查状态
    Unknown = 0,		//资源状态未知。
    StorageInReadOnly,	//资源存在且已存放于只读区中。
    StorageInReadWrite,	//资源存在且已存放于读写区中。
    Unavailable,		//资源不适用于当前变体。
    Update,				//资源需要更新。
    Disuse				//资源已废弃。
	
## 4.RemoteVersionInfo ==> ResourceManager.ResourceChecker
	远程资源状态信息
	struct RemoteVersionInfo{}
	
	public RemoteVersionInfo(string fileSystemName, LoadType loadType, 
		int length, int hashCode, int compressedLength, int compressedHashCode)

	bool m_Exist;
    string m_FileSystemName;
    LoadType m_LoadType;
    int m_Length;
    int m_HashCode;
    int m_CompressedLength;
    int m_CompressedHashCode;
	
	bool Exist;
	bool UseFileSystem;
	string FileSystemName;
	LoadType LoadType;
	int Length;
	int HashCode;
	int CompressedLength;
	int CompressedHashCode;
	
## 5.LocalVersionInfo ==> ResourceManager.ResourceChecker
	本地资源状态信息
	struct LocalVersionInfo{}
	
	public LocalVersionInfo(string fileSystemName, LoadType loadType, 
											int length, int hashCode)
	
	bool m_Exist;
	string m_FileSystemName;
	LoadType m_LoadType;
	int m_Length;
	int m_HashCode;
	
	bool Exist;
	bool UseFileSystem;
	string FileSystemName;
	LoadType LoadType;
	int Length;
	int HashCode;
	














