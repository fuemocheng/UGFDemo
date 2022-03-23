# GameFramework Data
在DataTable 上再封装一层

## 1.IData 	在GF框架中
	string Name { get; }
	void Init();
	void Preload();
	void Load();
	void Unload();
	void Shutdown();

## 2.Data : IData 抽象类	在GF框架中
Data : IData
	public virtual string Name;
	public abstract void Init();
	public abstract void Preload();
	public abstract void Load();
	public abstract void Unload();
	public abstract void Shutdown();
	
## 3.DataBase : GameFramework.Data.Data 抽象类，在GameMain中
	Dictionary<string, bool> loadedFlag;	添加了加载标记，表示是否加载完，全部是异步加载
	EventSubscriber eventSubscriber;		加载Config,DataTable,Dictionary的成功和失败事件
	bool IsPreloadReady;
	void Init();
	void Preload();
	void Load();
	void Unload();
	void Shutdown();
	virtual void OnInit();
	virtual void OnPreload();
	virtual void OnLoad();
	virtual void OnUnload();
	virtual void OnShutdown();
	...
	LoadConfig(string configName);
	LoadDataTable(string dataTableName);
	LoadDictionary(string dictionaryName);
	...

## 4. e.g. DataScene : DataBase 场景表
DataScene : DataBase
	IDataTable<DRScene> dtScene;
    Dictionary<int, SceneData> dicSceneData;
	
	override void OnInit();
	override void OnPreload();
		LoadDataTable("Scene");
		DataBase->LoadDataTable(); -> DataTableExtension.LoadDataTable(); 见 0x03;
	override void OnLoad();
	override void OnUnload();
	override void OnShutdown();
	
	public SceneData GetSceneData(int id);
	SceneData[] GetAllSceneData();
	
SceneData
	DRScene dRScene;	
	DRAssetsPath dRAssetsPath;
	int Id;
	string AssetPath;
	string Procedure;

获取SceneData
SceneData sceneData = GameEntry.Data.GetData<DataScene>().GetSceneData(sceneId);
	
## 5.DataInfo 定义在 DataManager 中
	对Data进行了一层封装，添加了加载的状态和优先级
	Data m_Data;
	DataStatus m_Status;
	int m_Priority;
	DataInfo Create(Data data);
	void Clear();

## 5.DataManager : GameFrameworkModule, IDataManager 用来添加删除Data, 管理DataInfo ，
	Dictionary<Type, DataInfo> m_dicDataInfos;
    private GameFrameworkLinkedList<DataInfo> m_linkedListDataInfos;
	int DataCount；
	
	void AddData<T>() where T : Data
	void AddData<T>(int priority) where T : Data
	void AddData(Data data)
	void AddData(Data data, int priority)
	void AddData(Data data, Type dataType, int priority)
	
	void RemoveData<T>() where T : Data
	void RemoveData(Data data)
	
	Data[] GetAllData()
	void GetAllData(List<Data> result)
	T GetData<T>() where T : Data
	Data GetData(string name)
	bool HasData<T>() where T : Data
	bool HasData(string name)
	
	void InitAllData()
	void PreLoadAllData() -> 每个Data Preload()
	void LoadAllData()
	void UnLoadAllData()
	void Shutdown()

## 6.DataComponent : GameFrameworkComponent
	IDataManager m_DataManager;
	DataItem[] dataItems;
	int DataCount;
	
	T GetData<T>() where T : Data
	Data GetData(string name)
	bool HasData<T>() where T : Data
	bool HasData(string name)
	Data[] GetAllData()
	void GetAllData(List<Data> result)
	void AddData<T>() where T : Data
	void AddData(Data Data)
	void RemoveData<T>() where T : Data
	void RemoveData(Data Data)
	void InitAllData()
	void PreLoadAllData()
	void LoadAllData()
	void UnLoadAllData()
	
	每个函数的具体实现都是在DataManager中执行;

## 7.总结
1.ProcedurePreload->OnEnter()->PreloadResources()->GameEntry.Data.PreLoadAllData()
	GameEntry.Data == DataComponent : GameFrameworkComponent
	
2.DataComponent->PreLoadAllData->m_DataManager.PreLoadAllData()
	DataManager : GameFrameworkModule, IDataManager
	
3.DataManager.PreLoadAllData->DataInfo.Data.Preload()

4.DataBase : GameFramework.Data.Data
	DataBase->Preload()
		添加加载监听事件
	DataBase->OnPreload()
	
5.e.g. 每个DR类的单独OnPreload()
	DataScene : DataBase
	DataScene->OnPreload()-> LoadDataTable("Scene")
	DataBase.LoadDataTable()->
	GameEntry.DataTable.LoadDataTable(dataTableName, dataTableAssetName, this);

6.GameEntry.DataTable
	GameEntry.DataTable == DataTableComponent : GameFrameworkComponent

7.DataTableExtension
	DataTableExtension->LoadDataTable()->
	
	DataTableBase dataTable = dataTableComponent.CreateDataTable(dataRowType, name)
	dataTable.ReadData()->
	
	m_DataProvider.ReadData(dataTableAssetName, priority, userData)->

8.DataProvider
	DataProvider->ReadData(string dataAssetName, int priority, object userData)
	配置表属于AssetOnFileSystem,调用ResourceManager资源管理器，执行读取数据;
	case HasAssetResult.AssetOnFileSystem:
        m_ResourceManager.LoadAsset(dataAssetName, priority, m_LoadAssetCallbacks, userData);
	
	加载成功回调
	LoadAssetSuccessCallback->m_DataProviderHelper.ReadData()
	
9.DataProviderHelper.ReadData()
	DefaultDataTableHelper->ReadData()->
	dataTable.ParseData()->DataTableBase.ParseData()->
	m_DataProvider.ParseData()->
	DefaultDataTableHelper->ParseData()->
	dataTable.AddDataRow()

10.DataTableBase.AddDataRow()
	DataTableManager.DataTable.AddDataRow()
	再接下去就是每个单独DR类的ParseDataRow的方法了，然后添加到集合中
	DataTableManager.DataTable.InternalAddDataRow(T dataRow)

11.ProcedurePreload 所有的预加载都加载完，执行SetComponents()
	SetComponents->GameEntry.Data.LoadAllData()->m_DataManager.LoadAllData()
	->每个Data.Load()
	->DataBase.Load()
	->DataBase.OnLoad()
	->每个e.g.DataScene类数据转换成需要的e.g.SceneData类，更方便获取

# 8.Custom
	可以自定义的模块
	
	
	
	
	
	
