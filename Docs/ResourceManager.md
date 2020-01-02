### 资源管理器 (ResourceManager)

创建资源管理器
```C#
public void Start()
{
    // 设置参数
    var createParam = new ResourceManager.CreateParameters();
    createParam.AssetRootPath = "Assets/Works/Resource";
    createParam.AssetSystemMode = AssetSystemMode;

    // 创建模块
    AppEngine.Instance.CreateModule<ResourceManager>(createParam);
}
```

资源使用范例
```C#
using MotionFramework.Resource;

public class Test
{
  private AssetReference _assetRef;

  public void Start()
  {
     // 加载NPC模型 
    _assetRef = new AssetReference("Model/npc001");
    _assetRef.LoadAssetAsync<GameObject>().Completed += Handle_Completed;
  }

  public void OnDestroy()
  {
    // 卸载模型
    if(_assetRef != null)
    {
      _assetRef.Release();
      _assetRef = null;
    }
  }

  private void Handle_Completed(AssetOperationHandle obj)
  {
    if (obj.AssetObject != null)
      return;
    
    // 模型已经加载完毕，我们可以在这里做任何处理
    GameObject go = obj.InstantiateObject;
    go.transform.position = Vector3.zero;
  }
}
```

更详细的教程请参考示例代码
1. [MotionModule/Runtime/Module.Resource/ResourceManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/MotionModule/Runtime/Module.Resource/ResourceManager.cs)
