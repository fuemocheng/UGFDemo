# GameFramework Resource

## 1.ResourceComponent : GameFrameworkComponent		==> UFG
	private IResourceManager m_ResourceManager;		//资源管理器
    private EventComponent m_EventComponent;		//事件管理器
	
	//默认资源辅助器，加载AssetBundle资源辅助器
	private ResourceHelperBase m_CustomResourceHelper;						//DefaultResourceHelper
	//默认加载资源代理辅助器，加载Asset资源代理辅助器
	private LoadResourceAgentHelperBase m_CustomLoadResourceAgentHelper;	//DefaultLoadResourceAgentHelper

	...
	//异步加载资源
	public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks)
	//卸载资源
	public void UnloadAsset(object asset)


	ResourceHelper和LoadResourceAgentHelper这两个类是框架留给开发者的后门，用于扩展资源加载的方式;
	
## 2.ResourceManager : GameFrameworkModule, IResourceManager		==> GF
	private const string RemoteVersionListFileName = "GameFrameworkVersion.dat";
    private const string LocalVersionListFileName = "GameFrameworkList.dat";
	
	//资源列表序列化器
	private PackageVersionListSerializer m_PackageVersionListSerializer;
    private UpdatableVersionListSerializer m_UpdatableVersionListSerializer;
    private ReadOnlyVersionListSerializer m_ReadOnlyVersionListSerializer;
    private ReadWriteVersionListSerializer m_ReadWriteVersionListSerializer;
    private ResourcePackVersionListSerializer m_ResourcePackVersionListSerializer;
	
	private IFileSystemManager m_FileSystemManager;			//文件管理系统
    private ResourceIniter m_ResourceIniter;				//资源初始化
    private VersionListProcessor m_VersionListProcessor;	//版本资源列表处理器
    private ResourceVerifier m_ResourceVerifier;			//资源验证器
    private ResourceChecker m_ResourceChecker;				//资源更新检查器
    private ResourceUpdater m_ResourceUpdater;				//资源更新器
    private ResourceLoader m_ResourceLoader;				//资源加载器
    private IResourceHelper m_ResourceHelper;				//资源辅助器

	void Update()
		m_ResourceUpdater.Update();
		m_ResourceLoader.Update();

## 3.





## 4.









	
	
	
	
	
	
