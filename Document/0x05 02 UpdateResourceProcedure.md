# GameFramework UpdateResourceProcedure

## 1.检查更新模式
	ProcedureSplash->OnUpdate()
	a.编辑器模式 -> ProcedurePreload -> 加载预加载资源，进入游戏;
	b.单机模式 -> ProcedureInitResources
	c.更新模式 -> ProcedureCheckVersion
	
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
		
## 3.















