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

资源的加载教程请参考[资源系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/AssetSystem.md)

更详细的教程请参考示例代码
1. [MotionModule/Module.Resource/ResourceManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/Scripts/Runtime/MotionModule/Module.Resource/ResourceManager.cs)
