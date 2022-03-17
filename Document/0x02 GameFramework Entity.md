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

## 4.EntityLoader












