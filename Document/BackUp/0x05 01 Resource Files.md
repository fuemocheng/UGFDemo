# GameFramework Resource Files


## 1.Asset->Resource
Asset: 用于描述Unity中的一个具体资产，如一个预制体、一个材质、一张图片等;

Resource:用于描述Unity中的一个 AssetBundle（一些 Asset 的集合），
		或者一个GameFramework定义下的二进制文件（一个Asset的二进制形式，能够用于脱离Unity直接加载）

## 2.VersionInfo ==> GameMain
	bool ForceUpdateGame;				//是否需要强制更新游戏应用
	string LatestGameVersion;			//最新的游戏版本号
	int InternalGameVersion;			//最新的游戏内部版本号
	string UpdatePrefixUri;				//资源更新下载地址
	int VersionListLength;				//资源版本列表长度
	int VersionListHashCode;			//资源版本列表哈希值
	int VersionListCompressedLength;	//资源版本列表压缩后长度
	int VersionListCompressedHashCode;	//资源版本列表压缩后哈希值

## 3.ReadOnlyPath
	ResourceComponent.Start()->
	m_ResourceManager.SetReadOnlyPath(Application.streamingAssetsPath);
	m_ResourceManager.SetReadWritePath(Application.persistentDataPath);
	
	//资源只读区路径
	ResourceManager.m_ReadOnlyPath == Application.streamingAssetsPath
	e.g. 	F:/ProjectName/Assets/StreamingAssets
	
	//资源读写区路径
	ResourceManager.m_ReadWritePath == Application.persistentDataPath
	e.g. 	C:/Users/username/AppData/LocalLow/CompanyName/ProductName
	
## 4.GameFramework.Resource.ResourceManager    UnityGameFramework.Editor.ResourceTools.ResourceBuilderController
	const string RemoteVersionListFileName = "GameFrameworkVersion.dat";
	const string LocalVersionListFileName = "GameFrameworkList.dat";
	

## 5.GameFrameworkVersion.dat
	UpdatableVersionList

	
## 6.GameFrameworkList.dat
	LocalVersionList
	
## 7.


















	
	
	
	
