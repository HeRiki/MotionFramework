### 游戏模块 (IModule)  

创建自定义模块代码示例
```C#
using MotionFramework;

public class CustomManager : IModule
{
  /// <summary>
  /// 游戏模块全局实例
  /// </summary>
  public static CustomManager Instance { private set; get; }

  void IModule.OnCreate(System.Object param)
  {
    //当模块被创建的时候
    Instance = this;
  }
  void IModule.OnStart()
  {
    //在首次Update之前被调用，仅被执行一次
  }
  void IModule.OnUpdate()
  {
    //轮询模块
  }
  void IModule.OnGUI()
  {
    //GUI绘制
    //可以显示模块的一些关键信息
  }

  public void Print()
  {
    Debug.Log("Hello world");
  }
}
```

在合适的地方注册我们的模块
```C#
using MotionFramework;

public class Test
{
  public void Start()
  {
    // 创建模块
    AppEngine.Instance.CreateModule<CustomManager>();

    // 带优先级的创建方式
    // 说明：运行时的优先级，优先级越大越早轮询。如果没有设置优先级，那么会按照添加顺序执行
    int priority = 1000;
    AppEngine.Instance.CreateModule<CustomManager>(priority);

    // 获取模块
    CustomManager mgr = AppEngine.Instance.GetModule<CustomManager>();
    mgr.Print();

    // 模块的全局实例
    CustomManager.Instance.Print();
  }
}
```
