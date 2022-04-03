# GameFramework ResourceBuilder

## 1.ResourceBuilder : EditorWindow ==> UnityGameFramework.Editor.ResourceTools
	void BuildResources()
	-> m_Controller.BuildResources()
	
## 2.ResourceBuilderController ==> UnityGameFramework.Editor.ResourceTools
	bool BuildResources()
	-> m_ResourceCollection.Load()
	-> this.PrepareBuildData(out assetBundleBuildDatas, out assetBundleResourceDatas, out binaryResourceDatas)
		PrepareBuildData()
		遍历 m_ResourceCollection.m_Resources, 即 Resource[]，和 m_ResourceCollection.m_Assets, 即Asset[];
		然后构造数据 SortedDictionary<string, ResourceData> m_ResourceDatas;
		遍历m_ResourceDatas,
		通过二进制文件类型加载的ResourceData会被放入binaryResourceDatas中；
		通过文件或内存方式加载的ResourceData则会被放入assetBundleResourceDatas中，
		并且还会被转换成AssetBundleBuild类型的数据再放入assetBundleBuildDatas中；
	
	-> BuildResources(Platform platform, 
			AssetBundleBuild[] assetBundleBuildDatas, 
			BuildAssetBundleOptions buildAssetBundleOptions, 
			ResourceData[] assetBundleResourceDatas, 
			ResourceData[] binaryResourceDatas)
		先构建输出路径；
		然后又对工作路径进行判断：如果已经存在工作路径，对路径下文件进行遍历，删掉已经存在的，但是本次打包时不包含的资源的文件；
		
	-> BuildPipeline.BuildAssetBundles	
		//UnityEditor.BuildPipeline(class)
		public static AssetBundleManifest BuildAssetBundles(string outputPath, 
				AssetBundleBuild[] builds, 
				BuildAssetBundleOptions assetBundleOptions, 
				BuildTarget targetPlatform);
		
	-> Start create file system
		如果在构建窗口中，在各个输出路径后面的"Generate"打上钩，接下来就会对应生成filesystem。
		然后，又把打好的包读了进来，生成hash值。
		再然后，把处理过的资源写回到硬盘上并把hash值保存到ResourceData中。
		根据生成类型不同生成不同的VersionList写到对应的路径下。
	
		1.ProcessPackageVersionList() 单机包
			GameFrameworkVersion.dat
		2.ProcessUpdatableVersionList()	更新版本数据包
			GameFrameworkVersion.dat
		3.ProcessReadOnlyVersionList()
			GameFrameworkList.dat
## 3.	





## 4.









	
	
	
	
	
	
