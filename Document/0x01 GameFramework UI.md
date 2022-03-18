# GameFramework UI

## UI创建步骤

1.建立UIPrefab

2.创建相应代码

3.UIForm表中添加相应数据

4.AssetPath表中添加相应数据

Tips：
	表格格式：UTF8-BOM
	
## UI Item 逻辑管理层级回收利用等

### Item 主要逻辑在下面的两个脚本中
ItemTowerBuildButton : ItemLogicEx
ItemLevelSelectionButton : ItemLogicEx

ItemLogicEx : ItemLogic ;
ItemLogic : MonoBehaviour ; Item逻辑基类,包含了Name, Available是否可用，Visible是否可见等;

以上只负责逻辑功能，不掺杂一点加载，回收利用等；


Item : MonoBehaviour, IItem
包含了 ID，AssetName资源名称, GameObject的索引，ItemLogic逻辑，initRoot父节点，IItemGroup物品所属的物品组

Item 对 ItemLogic 进行初始化，显示隐藏，更新，回收等；


### 界面如何控制Item的
UITowerListForm 	: UGuiFormEx : UGuiForm : UIFormLogic : MonoBehaviour
UILevelSelectForm 	: UGuiFormEx : UGuiForm : UIFormLogic : MonoBehaviour

UIFormLogic 包含界面 UIForm, Name, Available, Visible 等，是界面逻辑，如何回收利用是 UIForm 控制；
UGuiForm 	添加了界面层级，界面之间切换的过度效果，播放音效，设置字体；
UGuiFormEx	添加了事件的订阅，ItemLoader，EntityLoader等；
UITowerListForm/UILevelSelectForm ->ShowItem()->回调设置在ScrollView 或者设置在GridLayout中；


### ShowItem() 是由 ItemLoader 来完成的

ItemLoader : IReference  包装了回调, 根据ID寻找Item, 与Item是一对多的关系；可以说是Item的管理器
ItemLoader -> ShowItem() -> GameEntry.Item.ShowItem(serialId, itemId, userData);

GameEntry.Item 即 ItemComponent
ItemComponent : GameFrameworkComponent : MonoBehaviour

ItemComponentExtension->ShowItem() 期间获取 ItemData 表数据信息
-> ItemComponent.ShowItem
-> ItemManager.ShowItem
-> ItemGroup.SpawnItemInstanceObject(itemAssetName) 
	-> m_InstancePool.Spawn(name) -> ObjectPoolManager.ObjectPool.Spawn<T>() 
	=>> ItemInstanceObject
	ItemInstanceObject == null，则 ResourceManager.LoadAsset(itemAssetName...),
		加载成功 LoadAssetSuccessCallback -> InternalShowItem()
	ItemInstanceObject != null, 则 InternalShowItem()
	
### ItemInstanceObject : ObjectBase
	object m_ItemAsset 			GameObject的索引
	IItemHelper m_ItemHelper	

DefaultItemHelper : ItemHelperBase(abstract) : MonoBehaviour, IItemHelper
a.实例化物体
b.创建物体 	添加 Item 组件，设置父物体为 itemGroup.Helper.Transform
c.释放物体	

### ItemGroup 为 ItemManager.ItemGroup
IItemGroupHelper m_ItemGroupHelper 				Item实体组辅助器
												用来辅助创建ItemGroup GameObject
												对象回收在此GameObject下
IObjectPool<ItemInstanceObject> m_InstancePool 	对象池

ItemGroup 提前创建
ProcedurePreload->SetComponents()->SetItemComponent()->
    读取ItemGroup.txt表信息，GetAllItemGroupData()
    创建GameEntry.Item.AddItemGroup()
	Helper.CreateHelper -> (T)new GameObject().AddComponent(helperType); helperType = "UnityGameFramework.Runtime.DefaultItemGroupHelper";
	ItemManager->AddItemGroup()
	m_ItemGroups.Add(); [Dictionary<string, ItemGroup> m_ItemGroups]












