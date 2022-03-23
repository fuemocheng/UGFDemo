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

//下面是具体塔防建筑逻辑

塔防建筑预览
EntityTowerPreview : EntityLogicEx
	Update中根据 Physics.Raycast() 实时改变位置;

预览建筑的攻击半径特效
EntityRadiusVisualizer : EntityLogicEx
	附在 EntityTowerPreview 中;
	
最终敌人攻击的目标
EntityPlayer : EntityLogicEx

可被命中的目标
EntityTargetable : EntityLogicEx
	Transform hpBarRoot;
	Vector3 m_CurrentPosition, m_PreviousPosition;
	RandomSound randomSound;
	EnumAlignment Alignment;	结盟，阵营
	float HP;
	bool IsDead;
	void Damage(float value);
	void Dead();
	...
	
普通敌人
EntityEnemy : EntityTargetable, IPause
	Transform turret;				旋转枪架
	Transform[] projectilePoints;	发射点
	Transform epicenter;			中心
	Launcher launcher;				发射器
	...
	IFsm<EntityEnemy> fsm;						状态机单个状态
	List<FsmState<EntityEnemy>> stateList;		状态机列表
	...
	NavMeshAgent Agent;				寻路Agent
	...
	Targetter
	Attacker
	...
	寻路逻辑，状态转换，暂停与恢复，死亡隐藏，特效，等等
	
	
飞行敌人
EntityFlyingEnemy : EntityEnemy
	void AddFsmState()
		FlyingEnemyMoveState
		FlyingEnemyPushingThroughState
		EnemyAttackHomeBaseState
	void StartFsm()
	
	只是状态机与基类不同，其他全部相同

塔防基类
EntityTowerBase : EntityTargetable, IPause
	EntityDataTower entityDataTower;
	Entity entityTowerLevel;
	EntityTowerLevel entityLogicTowerLevel;
	EnumAlignment Alignment;
	float MaxHP;
	OnUpgradeTower(object sender, GameEventArgs e);
	ShowTowerLevelEntity(int level);
	void Dead();
	ShowControlForm();
	显示模型，升级模型，点击菜单，死亡，等；

塔防攻击类
EntityTowerAttacker : EntityTowerBase
	Targetter targetter;
	Attacker attacker;

突击加农炮	(与基类相同)
EntityAssaultCannon : EntityTowerAttacker

火箭炮		(与基类相同)
EntityRocketPlatform : EntityTowerAttacker

等离子炮	(与基类相同)
EntityPlasmaLance : EntityTowerAttacker

导弹阵列
EntityMissileArray : EntityTowerAttacker
	发射完即消失
	
能量塔 生产能量
EntityEnergyPylon : EntityTowerBase
	float timer;
    DataPlayer dataPlayer;
    DataLevel dataLevel;
	RaiseEnergy();

电磁脉冲发生器 使敌人减速
EntityEMPGenerator : EntityTowerBase
	Targetter targetter;
	List<EntityEnemy> slowList;


发射物
EntityProjectile : EntityLogicEx, IPause
	EntityDataProjectile entityDataProjectile;
		EntityTargetable EntityTargetable;
		ProjectileData ProjectileData;
		Vector3 Origin;
		Transform FiringPoint;
	int collisionParticlesEntityId;		碰撞粒子实体ID
	SpawnCollisionParticles();			生成碰撞粒子
	
自动隐藏发射物
EntityHideSelfProjectile : EntityProjectile
	float selfDestroyTime = 10;			自我隐藏时间
	OnUpdate()							Update中计时隐藏
	
弹道发射物 --火箭投射物
EntityProjectileBallistic : EntityHideSelfProjectile
	Rigidbody m_Rigidbody;
	Collider[] m_Colliders;
	Collider[] s_Enemies;					AOE伤害
	OnTriggerEnter(Collider other);			与敌人碰撞;
	
摆动归位投射物 -- 超级塔投射物 SuperTowerProjectile 
EntityProjectileWobblingHoming : EntityHideSelfProjectile
	Rigidbody m_Rigidbody;
	EntityTargetable enemy;
	Collider[] s_Enemies;
	Fire(Vector3 firingVector);				随机产生射出角度(一定范围内)，和射入角度;
	OnTriggerEnter(Collider other);			与敌人碰撞;
	
点射投射物 --激光投射物
EntityProjectileHitscan : EntityProjectile
	ParticleSystem projectileParticles;	粒子
	AttackTarget();						攻击目标->产生伤害，生成粒子

粒子
EntityParticle : EntityLogicEx, IPause
	ParticleSystem ps;
	EntityDataFollower entityDataFollower;

自动隐藏粒子 -- 加农炮炮弹
EntityParticleAutoHide : EntityParticle


