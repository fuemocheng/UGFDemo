# GameFramework Entity

## 1.创建武器预览
点击触发事件分发
UITowerListForm->ButtonClick->ShowBuildInfo(TowerData towerData)-> GameEntry.Event.Fire(ShowPreviewTowerEventArgs)

ProcedureLevel订阅事件被触发，展示武器Tower展示信息
ProcedureLevel.Subscribe(ShowPreviewTowerEventArgs.EventId)->OnShowPreviewTower()

ProcedureLevel调用到LevelControl
LevelControl->ShowPreviewTower()->entityLoader.ShowEntity()

## 2.LevelControl : IReference

ProcedureLevel -> OnEnter() -> LevelControl.Create()

## 3.EntityLoader 负责加载 Entity
ShowEntity() -> GameEntry.Entity.ShowEntity(serialId ...)
GameEntry.Entity 即 EntityComponent
EntityComponent : GameFrameworkComponent : MonoBehaviour

EntityComponentExtension -> ShowEntity(serialId ...) 期间获取 DataEntity 表的数据
-> EntityManager.ShowEntity()
-> EntityGroup.SpawnEntityInstanceObject(entityAssetName)
	-> m_InstancePool.Spawn(name) -> ObjectPoolManager.ObjectPool.Spawn<T>() 
	==>> EntityInstanceObject
	EntityInstanceObject == null，则 ResourceManager.LoadAsset(entityAssetName...),
		加载成功LoadAssetSuccessCallback -> InternalShowEntity()
	EntityInstanceObject != null, 则 InternalShowEntity()

## 4.EntityInstanceObject 实体实例对象又包装了一层
object m_EntityAsset;			GameObject引用
IEntityHelper m_EntityHelper;	

DefaultEntityHelper : EntityHelperBase(abstract) : MonoBehaviour, IEntityHelper
a.实例化物体
b.创建物体 	添加 Entity 组件，设置父物体为 entityGroup.Helper.Transform
c.释放物体

## 5.EntityGroup 为 EntityManager.ItemGroup : IEntityGroup
IEntityGroupHelper m_EntityGroupHelper 				Entity实体组辅助器
													用来辅助创建ItemGroup GameObject
													对象回收在此GameObject下
IObjectPool<EntityInstanceObject> m_InstancePool; 	对象池

EntityGroup 是在加载Eitity期间创建
EntityComponentExtension -> ShowEntity(serialId ...)	期间获取 DataEntity 表的数据
如果没有EntityGroup 则创建
if (!entityComponent.HasEntityGroup(entityData.EntityGroupData.Name))
	GameEntry.Entity.AddEntityGroup
	Helper.CreateHelper -> (T)new GameObject().AddComponent(helperType); helperType = "UnityGameFramework.Runtime.DefaultEntityGroupHelper";
	EntityManager.AddEntityGroup()
	m_EntityGroups.Add() [Dictionary<string, EntityGroup> m_EntityGroups]

## 6.Entity : MonoBehaviour, IEntity
Id 				实体编号
EntityAssetName	实体资源名称
Handle			实体实例 GameObject
EntityGroup		实体所属的实体组
EntityLogic		实体逻辑

初始化 EntityLogic ，显示隐藏 EntityLogic，更新 EntityLogic;

## 7.EntityLogic 
EntityLogic : MonoBehaviour
	Name : 		实体名称
	Available :	实体是否可用
	Visible : 	实体是否可见
	OnInit()初始化, OnRecycle()实体回收, OnShow()/OnHide()实体显示/隐藏, InternalSetVisible()设置实体的可见性
	OnAttached()/OnDetached()/OnAttachTo()/OnDetachFrom() 等
	OnUpdate()	更新
	
EntityLogicWithData : EntityLogic
	Id	-> Entity.Id
	EntityData -> Position,Rotation,UserData
	Animation -> private set;
	
EntityLogicEx : EntityLogicWithData
	EventSubscriber
	EntityLoader
	ItemLoader


EntityTowerPreview : EntityLogicEx

EntityRadiusVisualizer : EntityLogicEx

EntityPlayer : EntityLogicEx

EntityTargetable : EntityLogicEx




