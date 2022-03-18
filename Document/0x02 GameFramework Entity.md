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



## 3.EntityLogic 
EntityLogic : MonoBehaviour

EntityLogicWithData : EntityLogic

## 4.EntityLoader 负责加载 Entity
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

## 5.EntityInstanceObject 实体实例对象又包装了一层
object m_EntityAsset;			GameObject引用
IEntityHelper m_EntityHelper;	

DefaultEntityHelper : EntityHelperBase(abstract) : MonoBehaviour, IEntityHelper
a.实例化物体
b.创建物体 	添加 Entity 组件，设置父物体为 entityGroup.Helper.Transform
c.释放物体

## 6.EntityGroup 为 EntityManager.ItemGroup : IEntityGroup
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

## 7.











