# GameFramework DataTable

## 1.DataRow 数据表行基类
DataRow在Excel数据表中，对应的是一行数据；在代码中，对应的是一条数据对象；

IDataRow 接口，在GF中定义
DataRowBase 抽象类，数据表行基类，在UGF中实现

DataRowBase : IDataRow 	数据表行基类
	public abstract int Id;														获取数据表行的编号
	public virtual bool ParseDataRow(string dataRowString, object userData);	解析数据表行
	public virtual bool ParseDataRow(byte[] dataRowBytes, ...);					解析数据表行

Tips：
DRxxx类是工具自动生成，不可修改，工具菜单: StarFroece/Generate DataTables

e.g. 场景配置表
DRScene : DataRowBase
	public override int Id;			获取场景编号
	public int AssetId;				获取场景编号
	public int BackgroundMusicId;	获取背景音乐编号
	public override bool ParseDataRow(string dataRowString, object userData);	解析数据表行具体逻辑
	...

## 2.IDataTable 数据表接口
在GF中定义
数据表接口结构，限定了数据表具有增删查的功能，配置表是不需要修改的，一般也不需要删除;
这实际就是对于一个数据集合的访问查询接口;
提供了按condition检查的条件，和comparison排序的条件;

interface IDataTable<T> : IEnumerable<T> where T : IDataRow
	string Name;
	string FullName;
	Type Type;
	int Count;
	bool HasDataRow(int id);
	bool HasDataRow(Predicate<T> condition);						//要检查的条件
	T GetDataRow(int id);
	T GetDataRow(Predicate<T> condition);
	T[] GetDataRows(Predicate<T> condition);
	void GetDataRows(Predicate<T> condition, List<T> results);
	T[] GetDataRows(Comparison<T> comparison);						//要排序的条件
	void GetDataRows(Comparison<T> comparison, List<T> results);
	T[] GetDataRows(Predicate<T> condition, Comparison<T> comparison);
	void GetDataRows(Predicate<T> condition, Comparison<T> comparison, List<T> results);
	T[] GetAllDataRows();
	void GetAllDataRows(List<T> results);
	bool AddDataRow(byte[] dataRowBytes, int startIndex, int length, object userData);
	bool AddDataRow(string dataRowString, object userData);
	bool RemoveDataRow(int id);
	void RemoveAllDataRows();
	...
	
## 3.DataTableBase
数据表基类，在GF中，定义了加载数据需要的DataProvider类; 以及读数据表成功和失败的事件

DataTableBase : IDataProvider<DataTableBase>
	DataProvider<DataTableBase> m_DataProvider;			
	string Name;			获取数据表名称			
	string FullName；		获取数据表完整名称	
	Type Type;				获取数据表行的类型
	int Count;				获取数据表行数
	
	public event EventHandler<ReadDataSuccessEventArgs> ReadDataSuccess
	public event EventHandler<ReadDataFailureEventArgs> ReadDataFailure
	public event EventHandler<ReadDataUpdateEventArgs> ReadDataUpdate
	public event EventHandler<ReadDataDependencyAssetEventArgs> ReadDataDependencyAsset
	
	读取数据表
	public void ReadData(string dataTableAssetName)
		m_DataProvider.ReadData(dataTableAssetName);
	
	解析数据表
	bool ParseData(string dataTableString);
		m_DataProvider.ParseData(dataTableString);
	
	public abstract bool HasDataRow(int id);
	public abstract bool AddDataRow(string dataRowString, object userData);
	public abstract bool AddDataRow(byte[] dataRowBytes, int startIndex, int length, object userData);
	public abstract bool RemoveDataRow(int id);
	public abstract void RemoveAllDataRows();
	...
	void Shutdown();	 	关闭并清理数据表
	...

## 4.DataProvider
数据提供者接口
interface IDataProvider<T>	数据提供者接口，在GF框架中
	void ReadData(...);				读取数据
	bool ParseData(...);			解析内容
	event EventHandler<ReadDataSuccessEventArgs> ReadDataSuccess;		读取数据成功事件
	event EventHandler<ReadDataFailureEventArgs> ReadDataFailure;		读取数据失败事件
	event EventHandler<ReadDataUpdateEventArgs> ReadDataUpdate;			读取数据更新事件
	event EventHandler<ReadDataDependencyAssetEventArgs> ReadDataDependencyAsset;	读取数据时加载依赖资源事件

数据提供者
主要作用通过ResourceManager去合适的位置加载泛型T类型的数据表
DataProvider<T> : IDataProvider<T>	数据提供者，在GF框架中
	T m_Owner;
	IResourceManager m_ResourceManager;
	IDataProviderHelper<T> m_DataProviderHelper;		--> 					DefaultDataTableHelper
	public void ReadData(string dataAssetName, int priority, object userData);	读取数据，具体实现
		ResourceManager.LoadAsset(...)
	public bool ParseData(string dataString, object userData);					解析内容，具体实现
		m_DataProviderHelper.ParseData(...)
	
## 5.DataTable : DataTableBase, IDataTable<T> where T : class, IDataRow, new()
DataTable 是一个表的数据集合，可以理解为DataRow的集合Dictionary<int, T> m_DataSet;

在 DataTableManager : GameFrameworkModule, IDataTableManager 中定义，
包含了按条件查询，按id查询，全部查询，排序查询等；

DataTable<T> : DataTableBase, IDataTable<T> where T : class, IDataRow, new()
	Dictionary<int, T> m_DataSet;			//数据表
	Type Type;
	int Count;
	bool HasDataRow(int id);
	bool HasDataRow(Predicate<T> condition);
	T GetDataRow(int id);
	T GetDataRow(Predicate<T> condition);
	T[] GetDataRows(Predicate<T> condition);
	void GetDataRows(Predicate<T> condition, List<T> results);
	T[] GetDataRows(Comparison<T> comparison);
	void GetDataRows(Comparison<T> comparison, List<T> results);
	T[] GetDataRows(Predicate<T> condition, Comparison<T> comparison);
	void GetDataRows(Predicate<T> condition, Comparison<T> comparison, List<T> results);
	T[] GetAllDataRows();
	void GetAllDataRows(List<T> results);
	bool AddDataRow(string dataRowString, object userData);
	bool AddDataRow(byte[] dataRowBytes, int startIndex, int length, object userData);
	bool RemoveDataRow(int id);
	void RemoveAllDataRows();
	void Shutdown();
	...
	public IEnumerator<T> GetEnumerator();	//获取迭代器

## 6.DataTableExtension 
DataTableExtension -> DataTable的扩展类
	DataTableComponent.LoadDataTable()
	
	Color32 ParseColor32(string value)
	Color ParseColor(string value)
	Quaternion ParseQuaternion(string value)
	Vector2 ParseVector2(string value)
	Vector3 ParseVector3(string value)
	Vector4 ParseVector4(string value)
	ParseInt32Array(string value)
	Rect ParseRect(string value)

## 7.DataTableManager
m_DataTables 就是所有的数据表的合集

通过DataTableManager进行加载数据文本，
IDataProviderHelper->DefaultDataTableHelper进行数据解析，
最后把配置表数据填充到m_DataTables集合中;

DataTableManager : GameFrameworkModule, IDataTableManager
	Dictionary<TypeNamePair, DataTableBase> m_DataTables;		配置表数据集的总集和
	IResourceManager m_ResourceManager;
	IDataProviderHelper<DataTableBase> m_DataProviderHelper;
	IDataTableHelper m_DataTableHelper;
	int Count;
	bool HasDataTable<T>() where T : IDataRow					是否存在数据表
	bool HasDataTable(Type dataRowType)
	bool HasDataTable<T>(string name) where T : IDataRow		
	IDataTable<T> GetDataTable<T>() where T : IDataRow			获取数据表
	DataTableBase GetDataTable(Type dataRowType)				
	IDataTable<T> GetDataTable<T>(string name) where T : IDataRow
	DataTableBase GetDataTable(Type dataRowType, string name)
	DataTableBase[] GetAllDataTables()							获取所有数据表
	void GetAllDataTables(List<DataTableBase> results)
	
	IDataTable<T> CreateDataTable<T>() where T : class, IDataRow, new()
	DataTableBase CreateDataTable(Type dataRowType)
	IDataTable<T> CreateDataTable<T>(string name) where T : class, IDataRow, new()
	DataTableBase CreateDataTable(Type dataRowType, string name)
	
	void SetResourceManager(IResourceManager resourceManager);
	void SetDataProviderHelper(IDataProviderHelper<DataTableBase> dataProviderHelper);
	void SetDataTableHelper(IDataTableHelper dataTableHelper);
	
	Destory Functions ...
	Internal Functions ...
	
## 8.DataTableComponent : GameFrameworkComponent 数据表组件
初始化设置DataTableManager的 DataProviderHelper，DataTableHelper，ResourceManager组件;
这里的 DataProviderHelper，DataTableHelper 都是 DefaultDataTableHelper.cs ;
之后再添加加载的监听事件;
	
	IDataTableManager m_DataTableManager;
	string m_DataTableHelperTypeName = "UnityGameFramework.Runtime.DefaultDataTableHelper";
	DataTableHelperBase m_CustomDataTableHelper;
	int Count;
	
	bool HasDataTable<T>() where T : IDataRow;
	bool HasDataTable(Type dataRowType);
	bool HasDataTable<T>(string name) where T : IDataRow;
	bool HasDataTable(Type dataRowType, string name);
	IDataTable<T> GetDataTable<T>() where T : IDataRow;
	DataTableBase GetDataTable(Type dataRowType);
	IDataTable<T> GetDataTable<T>(string name) where T : IDataRow;
	DataTableBase GetDataTable(Type dataRowType, string name);
	DataTableBase[] GetAllDataTables();
	void GetAllDataTables(List<DataTableBase> results)
	IDataTable<T> CreateDataTable<T>() where T : class, IDataRow, new();
	DataTableBase CreateDataTable(Type dataRowType);
	IDataTable<T> CreateDataTable<T>(string name) where T : class, IDataRow, new();
	DataTableBase CreateDataTable(Type dataRowType, string name);
	
	Destroy functions
	...
	
## 9.DefaultDataTableHelper 默认数据表辅助器 属于UGF
DefaultDataTableHelper : DataTableHelperBase
	bool ReadData(DataTableBase dataTable, string dataTableAssetName, object dataTableAsset, object userData)
	bool ReadData(DataTableBase dataTable, string dataTableAssetName, byte[] dataTableBytes, int startIndex, int length, object userData)
	bool ParseData(DataTableBase dataTable, string dataTableString, object userData)
	bool ParseData(DataTableBase dataTable, byte[] dataTableBytes, int startIndex, int length, object userData)
	
## 10.总结
1.ProcedurePreload->OnEnter()->PreloadResources()->LoadDataTable(dataTableName)->
	GameEntry.DataTable.LoadDataTable(dataTableName, dataTableAssetName, this)

2.DataTableExtension->LoadDataTable()
	DataTableBase dataTable = dataTableComponent.CreateDataTable(dataRowType, name);
    dataTable.ReadData(dataTableAssetName, Constant.AssetPriority.DataTableAsset, userData);
	
3.dataTableComponent.CreateDataTable
	DataTableComponent->CreateDataTable()->
	DataTableManager.CreateDataTable() -> m_DataTables.Add()

4.DataTable.ReadData
	dataTable.ReadData->DataTableBase.ReadData()->m_DataProvider.ReadData()->DataProvider.ReadData()->
	配置表属于AssetOnFileSystem,调用ResourceManager资源管理器，执行读取数据;
	case HasAssetResult.AssetOnFileSystem:
        m_ResourceManager.LoadAsset(dataAssetName, priority, m_LoadAssetCallbacks, userData);
	
	加载成功回调
	LoadAssetSuccessCallback->m_DataProviderHelper.ReadData()
	
5.DataProviderHelper.ReadData
	DefaultDataTableHelper->ReadData()->
	dataTable.ParseData()->DataTableBase.ParseData()->
	m_DataProvider.ParseData()->
	DefaultDataTableHelper->ParseData()->
	dataTable.AddDataRow()
	
6.DataTableBase.AddDataRow()
	DataTableManager.DataTable.AddDataRow()
	再接下去就是每个单独DR类的ParseDataRow的方法了，然后添加到集合中
	DataTableManager.DataTable.InternalAddDataRow(T dataRow)
	
