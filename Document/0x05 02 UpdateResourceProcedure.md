# GameFramework UpdateResourceProcedure

## 1.检查更新模式
	ProcedureSplash->OnUpdate()
	a.编辑器模式 -> ProcedurePreload -> 加载预加载资源，进入游戏;
	b.单机模式 -> ProcedureInitResources
	c.可更新模式 -> ProcedureCheckVersion
	
## 2.单机模式
	使用单机模式并初始化资源前，需要先构建 AssetBundle 并复制到 StreamingAssets 中;
	ProcedureInitResources.OnEnter()  
		GameEntry.Resource.InitResources()->  
		ResourceManager.InitResources()->  
		ResourceIniter.InitResources->  
		ResourceManager.m_ResourceHelper.LoadBytes(path, callBack)  
			加载本地资源版本数据 
			path == Application.streamingAssetsPath + "GameFrameworkVersion.dat"
		
		OnLoadPackageVersionListSuccess()->
			1.遍历PackageVersionList的 fileSystems 文件系统集合，填充到ResourceIniter的 Dictionary<ResourceName, string> m_CachedFileSystemNames;
			2.遍历PackageVersionList的 m_Resources 资源集合
				遍历m_Resources的 m_AssetIndexes 资源包含的资源索引集合
					添加到ResourceManager的 m_AssetInfos 中;
				根据PackageVersionList的 Resource 信息，构造ResourceManager的 ResourceInfo 信息，并添加进ResourceManager.m_ResourceInfos
				向ResourceManager.m_ResourceGroups资源组中添加此 resource 资源
			3.遍历PackageVersionList的 m_ResourceGroups 资源组信息，
		
## 3.可更新模式
### 3.1 ProcedureCheckVersion.OnEnter
	ProcedureCheckVersion.OnEnter()
		1.订阅WebRequest成功失败事件
		2.向服务器请求版本信息
		GameEntry.WebRequest.AddWebRequest(GameEntry.BuiltinData.BuildInfo.CheckVersionUrl);
		->GameEntry.BuiltinData.BuildInfo 为 Assets/GameMain/Configs/BuildInfo.txt
		->BuildInfo.CheckVersionUrl ==> "https://starforce.gameframework.cn/Resources/{0}Version.txt"
		3.OnWebRequestSuccess()->
		解析版本信息
		Utility.Json.ToObject<VersionInfo>(versionInfoString);
		设置资源更新下载地址
		GameEntry.Resource.UpdatePrefixUri = Utility.Path.GetRegularPath(m_VersionInfo.UpdatePrefixUri);
		检查是否需要更新
		GameEntry.Resource.CheckVersionList(m_VersionInfo.InternalResourceVersion)

#### 3.1.1 VersionListProcessor.CheckVersionList
	使用可更新模式并检查版本资源列表
	//最新的内部资源版本号
	ResourceManager.VersionListProcessor.CheckVersionList(int latestInternalResourceVersion)
		1.可读写路径(ReadWritePath，即persistentDataPath)是否存在, 不存在，抛出异常
		2.Application.persistentDataPath + "GameFrameworkVersion.dat" 是否存在
			不存在，则需要更新
		3.读取 "GameFrameworkVersion.dat" ，并序列化出 InternalResourceVersion 内部资源版本号字段
			不存在此字段，则需要更新
			此字段不等于下载的version.txt中记录的字段，则需要更新
		
		4.否则返回已经是最新的			
	
	回到3.1 ProcedureCheckVersion.OnWebRequestSuccess()
	已经得到m_NeedUpdateVersion是否需要更新
		需要更新，则 -> ProcedureUpdateVersion
		否则 -> ProcedureVerifyResources
		
### 3.2 ProcedureUpdateVersion 更新版本列表
	主要是下载服务器端 GameFrameworkVersion.9fa944c3.dat 文件，并保存在本地

	ProcedureUpdateVersion->OnEnter()
	->GameEntry.Resource.UpdateVersionList()

	使用可更新模式并检查版本资源列表
	//版本资源列表大小,版本资源列表哈希值,
	//版本资源列表压缩后大小,版本资源列表压缩后哈希值,
	//版本资源列表更新回调函数集
	->ResourceManager.VersionListProcessor.UpdateVersionList(int versionListLength, int versionListHashCode, 
		int versionListCompressedLength, int versionListCompressedHashCode, 
		UpdateVersionListCallbacks updateVersionListCallbacks)
	
	->VersionListProcessor.UpdateVersionList(int versionListLength, int versionListHashCode, 
		int versionListCompressedLength, int versionListCompressedHashCode)
	
#### 3.2.1 VersionListProcessor.UpdateVersionList
	更新版本资源列表。
	1.拼接本地下载地址
	localVersionListFilePath = Utility.Path.GetRegularPath
		(Path.Combine(m_ResourceManager.m_ReadWritePath, RemoteVersionListFileName));
		资源读写区路径
		即：Application.persistentDataPath + "GameFrameworkVersion.dat"
		即：C:/Users/username/AppData/LocalLow/CompanyName/ProductName/GameFrameworkVersion.dat
	2.计算服务器数据地址
		e.g.
		http://192.168.92.51/0_1_0_8/Windows/GameFrameworkVersion.9fa944c3.dat
	3.添加下载任务
		m_DownloadManager.AddDownload(localVersionListFilePath, Utility.Path.GetRemotePath
			(Path.Combine(m_ResourceManager.m_UpdatePrefixUri, latestVersionListFullNameWithCrc32)), this);
	4.OnDownloadSuccess()
		下载下来二进制数据流
		1.计算下载的数据流长度是否等于VersionListCompressedLength
		2.计算数据流hashcode是否等于VersionListCompressedHashCode
		3.解压数据流
		4.计算解压后的数据流是否等于VersionListLength
	5.回调
		成功下载GameFrameworkVersion.dat并校验
		回调VersionListUpdateSuccess()
	6.回到ProcedureUpdateVersion.OnUpdateVersionListSuccess()
		m_UpdateVersionComplete = true;
		Update()中完成
		ChangeState<ProcedureVerifyResources>(procedureOwner);
	
### 3.3 ProcedureVerifyResources 校验资源，验证资源
	主要是确保可读写路径下的"GameFrameworkList.dat"记录的数据，
	与可读写路径下的本地数据比如"GameData.dat","UI.dat","Resources.dat","GameFrameworkSetting.dat"中记录的数据保持一致；
	
	如果数据出错，则删除相应的"GameFrameworkList.dat"中的记录，和"GameData.dat"等中的文件;

	ProcedureVerifyResources.OnEnter()
	->GameEntry.Resource.VerifyResources()
	
	使用可更新模式并校验资源
	->ResourceManager.VerifyResources
	->m_ResourceVerifier.VerifyResources(verifyResourceLengthPerFrame)
	
#### 3.3.1 ResourceVerifier.VerifyResources(int verifyResourceLengthPerFrame) 校验资源
	1.加载本地资源列表数据 LocalVersionListFileName = "GameFrameworkList.dat"
	m_ResourceManager.m_ResourceHelper.LoadBytes(Path.Combine(m_ResourceManager.m_ReadWritePath, LocalVersionListFileName), 
		new LoadBytesCallbacks(...);
	
	2.加载成功
	OnLoadReadWriteVersionListSuccess()
	反序列化
	LocalVersionList versionList = m_ResourceManager.m_ReadWriteVersionListSerializer.Deserialize()
	
	3.构造 List<VerifyInfo> m_VerifyInfos，m_VerifyInfos是单个变体资源信息的集合
	构造字典ResourceName<->文件系统名		Dictionary<ResourceName, string> resourceInFileSystemNames; 		
	遍历 versionList.GetFileSystems() --> FileSystem[]
		...
		resourceInFileSystemNames.Add(new ResourceName(...))
	遍历 versionList.GetResources() -->  Resource[]
		m_VerifyInfos.Add(new VerifyInfo(...))
	
	m_LoadReadWriteVersionListComplete = true
	
	ResourceVerifyStart(m_VerifyInfos.Count, totalLength);
	->ResourceManger.OnVerifierResourceVerifyStart(int count, long totalLength)
	->ResourceComponent.OnResourceVerifyStart()

	4.Update()
	当读取完，构造完m_VerifyInfos，继续;
	当 m_VerifyResourceIndex < m_VerifyInfos.Count 不断循环
		取VerifyInfo verifyInfo = m_VerifyInfos[m_VerifyResourceIndex];
		校验VerifyResource(verifyInfo)
	
	4.1 校验单个资源 bool VerifyResource(VerifyInfo verifyInfo)
	当 verifyInfo.UseFileSystem 为 true
		根据verifyInfo.FileSystemName获取文件系统，比如"GameData";
		根据verifyInfo.ResourceName.FullName获取FileSystem.FileInfo fileInfo;
		若fileInfo有效，则继续，否则返回False
		fileSystem读取数据流，判断hashcode，字节流长度等是否和verifyInfo记录的一致
		一致则返回true
		否则删除有误的数据段；
		同时返回false, 且 m_VerifyInfos.RemoveAt(m_VerifyResourceIndex); 即m_VerifyInfos删除这一条
		
		这里是文件系统中，比如"GameData"，删除出错的数据段；
		
	当 verifyInfo.UseFileSystem 为 false
		获取resourcePath，读取文件流
		判断数据流长度和hashcode
		一致则返回true
		否则删除有误的数据段;
		同时返回false, 且 m_VerifyInfos.RemoveAt(m_VerifyResourceIndex); 即m_VerifyInfos删除这一条
		
	4.2 当VerifyResource校验成功，不断循环，校验下一个
		当VerifyResource校验失败，则记录m_FailureFlag为true;
		直到所有的资源校验完毕
		
		如果m_FailureFlag为false 则 GenerateReadWriteVersionList()
		
	5.GenerateReadWriteVersionList
		根据 m_VerifyInfos ，重新生成LocalVersionList
		
		先创建一个GameFrameworkList.tmp文件；
		遍历m_VerifyInfos，写入缓存字典中;
		根据缓存创建LocalVersionList,写入"GameFrameworkList.tmp"中;
		删除原有的"GameFrameworkList.dat"，将"GameFrameworkList.tmp"改为"GameFrameworkList.dat"
	
	6.Update()中校验完成，回调ResourceVerifyComplete()
	->ResourceManager.OnVerifierResourceVerifyComplete()
	->ProcedureVerifyResources.OnVerifyResourcesComplete()
		m_VerifyResourcesComplete = true;
	->Update()
		ChangeState<ProcedureCheckResources>(procedureOwner)
		
### 3.4 ProcedureCheckResources
	主要是检查资源并构造要更新的资源
	ReadWritePath -> GameFrameworkVersion.dat	总表，以此为准
	ReadOnlyPath -> GameFrameworkList.dat
	ReadWritePath -> GameFrameworkList.dat
	对比这三个表;
	
	ProcedureCheckResources.OnEnter()
	->GameEntry.Resource.CheckResources(OnCheckResourcesComplete);
	->ResourceManager.CheckResources(...)	//使用可更新模式并检查资源
	->m_ResourceChecker.CheckResources(m_CurrentVariant, ignoreOtherVariant);
		
#### 3.4.1 ResourceManager.ResourceChecker
	检查资源
	ResourceManager.ResourceChecker.CheckResources(string currentVariant, bool ignoreOtherVariant)
	-> 	LoadBytes(ReadWritePath + RemoteVersionListFileName)	"GameFrameworkVersion.dat"
		LoadBytes(ReadOnlyPath + LocalVersionListFileName)		"GameFrameworkList.dat"
		LoadBytes(ReadWritePath + LocalVersionListFileName)		"GameFrameworkList.dat"

	->	OnLoadUpdatableVersionListSuccess()
		OnLoadReadOnlyVersionListSuccess()
		OnLoadReadWriteVersionListSuccess()
		当这三者都完成，
		则RefreshCheckInfoStatus()

#### 3.4.2 OnLoad...Success ==> ResourceManager.ResourceChecker
	1.OnLoadUpdatableVersionListSuccess
	读取完成 ReadWritePath + RemoteVersionListFileName	"GameFrameworkVersion.dat"
	反序列换成 UpdatableVersionList versionList 实例;
	读取UpdatableVersionList.Asset[], UpdatableVersionList.Resource[], UpdatableVersionList.ResourceGroup[];
	
	遍历 UpdatableVersionList.FileSystem fileSystem;
		遍历 fileSystem.GetResourceIndexes() -> int[] resourceIndexes;
			构造ResourceName;
			设置临时缓存资源所在的文件系统名称;
			SetCachedFileSystemName();	创建并设置CheckInfo.m_VersionInfo;
	
	遍历 UpdatableVersionList.Resource resource;
		构造 ResourceName;
		获取 resource.GetAssetIndexes()，并遍历
			获取 UpdatableVersionList.Asset asset；
			获取 asset.GetDependencyAssetIndexes();
			根据以上数据构造AssetInfo并添加到ResourceManager的 m_AssetInfos 中,
			m_ResourceManager.m_AssetInfos.Add(...);
		
		设置CheckInfo.m_VersionInfo
		SetVersionInfo(...)
		defaultResourceGroup.AddResource();
	
	遍历 UpdatableVersionList.ResourceGroup resourceGroup;
		m_ResourceManager添加ResourceGroup
		ResourceGroup group = m_ResourceManager.GetOrAddResourceGroup(resourceGroup.Name)
		遍历 resourceGroup.GetResourceIndexes()；
			获取资源 UpdatableVersionList.Resource resource;
			向资源组中增加资源
			group.AddResource(...)
	
	m_UpdatableVersionListReady = true;
    RefreshCheckInfoStatus();
	
	总结
	重建ResourceManager中所有的资源信息，并设置当前资源版本号与本列表文件相同;
	m_ResourceManager.m_ApplicableGameVersion
	m_ResourceManager.m_InternalResourceVersion
	m_ResourceManager.m_AssetInfos
	m_ResourceManager.m_ResourceInfos
	创建默认空名的ResourceGroup加入到m_ResourceManager.m_ResourceGroups中;
	
	根据UpdatableVersionList.Resource[]数据构建ResourceName，创建并设置CheckInfo.m_VersionInfo;
	根据UpdatableVersionList.Asset[]构建AssetInfo添加到m_ResourceManager.m_AssetInfos中,
	为默认名为空的ResourceGroup加入所有的ResourceName及其文件长度和zip包长度信息;
	根据UpdatableVersionList.ResourceGroup[]建立ResourceGroup加入m_ResourceManager.m_ResourceGroups中，
	并为其加入所属的ReourceName及其文件长度和zip长度信息;
	
	2.OnLoadReadOnlyVersionListSuccess
	读取完成 ReadOnlyPath + LocalVersionListFileName	"GameFrameworkList.dat"
	反序列化成 LocalVersionList 实例;
	
	遍历 LocalVersionList.FileSystem fileSystem;
		遍历 fileSystem.GetResourceIndexes() -> int[] resourceIndexes;
			获取 LocalVersionList.Resource resource;
			构造ResourceName;
			设置临时缓存资源所在的文件系统名称;
			SetCachedFileSystemName()
	遍历 LocalVersionList.Resource resource;
		设置CheckInfo.m_ReadOnlyInfo
		SetReadOnlyInfo()
	
	m_ReadOnlyVersionListReady = true;
    RefreshCheckInfoStatus();
	
	3.OnLoadReadWriteVersionListSuccess
	读取完成 ReadWritePath + LocalVersionListFileName	"GameFrameworkList.dat"
	反序列化成 LocalVersionList 实例;

	遍历 LocalVersionList.FileSystem fileSystem;
		遍历 fileSystem.GetResourceIndexes() -> int[] resourceIndexes;
			获取 LocalVersionList.Resource resource;
			构造ResourceName;
			设置临时缓存资源所在的文件系统名称;
			SetCachedFileSystemName()
	遍历 LocalVersionList.Resource resource;
		设置CheckInfo.m_ReadWriteInfo
		SetReadOnlyInfo()
		--创建ReadWriteResourceInfo并和ResourceName一起添加到m_ResourceManager.m_ReadWriteResourceInfos中;

	m_ReadWriteVersionListReady = true;
    RefreshCheckInfoStatus();

#### 3.4.3 RefreshCheckInfoStatus 刷新 CheckInfo 的状态  ==> ResourceManager.ResourceChecker
	遍历 ResourceChecker.m_CheckInfos --> Dictionary<ResourceName, CheckInfo>;
		
		CheckInfo文件刷新资源信息状态
		checkInfo.RefreshStatus(...)
			如果 m_VersionInfo.Exist 为false，即远程资源信息不存在，如果存在则需要删除这一条;
				m_Status = CheckStatus.Disuse;
				m_NeedRemove = m_ReadWriteInfo.Exist;
				return;
			如果 资源变体名称不存在，或者变体名称等于当前变体，则继续，
				如果 m_ReadOnlyInfo.Exist 为true，即只读区资源存在，
					并且 m_ReadOnlyInfo 与 m_VersionInfo信息相同，则
					m_Status = CheckStatus.StorageInReadOnly;
                    m_NeedRemove = m_ReadWriteInfo.Exist;	即可读写区也存在这个资源的话，可读写区这个资源删掉
				如果 m_ReadWriteInfo.Exist 为true, 即读写区资源存在，
					并且 m_ReadWriteInfo 与 m_VersionInfo信息相同，则
					m_Status = CheckStatus.StorageInReadWrite;
					并且判断是否在同一文件系统中，
					是否需要将读写区的资源移动到磁盘，将指定文件存为物理文件	--*
					是否需要将读写区的资源移动到文件系统						--*
				否则 
					m_Status = CheckStatus.Update;
                    m_NeedRemove = m_ReadWriteInfo.Exist;
			否则
				m_Status = CheckStatus.Unavailable;
                m_NeedRemove = !ignoreOtherVariant && m_ReadWriteInfo.Exist;
		
		CheckInfo文件刷新完资源信息状态，继续
		如果 checkInfo.CheckStatus 是 StorageInReadOnly，
			则 m_ResourceManager.m_ResourceInfos，添加 ResourceInfo 实例;
		如果 checkInfo.CheckStatus 是 StorageInReadWrite，
			如果 checkInfo NeedMoveToDisk 或者 NeedMoveToFileSystem，则
			进行相应的资源移动删除等操作；
			然后，
			m_ResourceManager.m_ResourceInfos，添加 ResourceInfo 实例;
			m_ResourceManager.m_ReadWriteResourceInfos，添加 ReadWriteResourceInfo 实例，
		如果 checkInfo.CheckStatus 是 Update，
			m_ResourceManager.m_ResourceInfos，添加 ResourceInfo 实例;
			
			重点在这
			ResourceNeedUpdate.Invoke() 转到3.3.4
			
		如果 checkInfo.CheckStatus 是 Unavailable 或者 Disuse
			do nothing;
		
		如果 checkInfo.NeedRemove 则删除资源等等;
		
	RemoveEmptyFileSystems()
	资源检查完成
	ResourceCheckComplete() 
	->ResourceManager.OnCheckerResourceCheckComplete()
	->m_CheckResourcesCompleteCallback()
	->ProcedureCheckResources.OnCheckResourcesComplete()
		m_CheckResourcesComplete = true;
	
	ProcedureCheckResources.OnUpdate()
		需要更新 m_NeedUpdateResources
			ChangeState<ProcedureUpdateResources>(procedureOwner);
		不需要更新
			ChangeState<ProcedurePreload>(procedureOwner);
	
#### 3.3.4 ResourceNeedUpdate
	ResourceManager.ResourceChecker.RefreshCheckInfoStatus.ResourceNeedUpdate
	->ResourceManager.OnCheckerResourceNeedUpdate
	->m_ResourceUpdater.AddResourceUpdate(...)
	-> m_UpdateCandidateInfo.Add(ResourceName, UpdateInfo)
	
	接下来交由更新器

### 3.5 ProcedureUpdateResources
	ProcedureUpdateResources.OnEnter()
	-> 	GameEntry.UI.OpenDialog 是否更新
		更新则
		StartUpdateResources(null);
		-> GameEntry.Resource.UpdateResources(OnUpdateResourcesComplete);
		-> ResourceManager.UpdateResources();
		-> m_ResourceUpdater.UpdateResources(resourceGroup);
		
#### 3.5.1 ResourceUpdater
	1.更新指定资源组的资源, 初始资源组 string.Empty
	void UpdateResources(ResourceGroup resourceGroup)
		如果resourceGroup.Name 为 IsNullOrEmpty，则
		将所有候补更新信息m_UpdateCandidateInfo，添加到m_UpdateWaitingInfo；
		
	2.Update() 轮询
		如果 m_ApplyWaitingInfo 数量大于 0 ，则
			ApplyResource(applyInfo);
	
		如果m_UpdateWaitingInfo.Count 大于 0，则
			根据下载器的空余数量进行下载
			DownloadResource(m_UpdateWaitingInfo[i]);
	
	3.bool DownloadResource(UpdateInfo updateInfo)
		m_DownloadManager.AddDownload(ResourcePath, RemotePath, UpdateInfo)
		
	4.OnDownloadSuccess() 下载成功
		1.下载的数据已经下载并存储到指定目录
		2.回调参数是UpdateInfo
		UpdateInfo updateInfo = e.UserData as UpdateInfo;
		3.判断下载的资源是否是压缩文件
		读取下载地址的文件流;
		如果文件流的长度和updateInfo.CompressedLength
		
		
		
		
	5.bool ApplyResource(ApplyInfo applyInfo)
		
		
		
	
	





























