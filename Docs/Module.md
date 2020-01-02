### 游戏模块 (IModule)  

AppEngine是MotionFramework框架的核心，所有的游戏模块都是通过AppEngine创建和管理。  

自定义模块代码示例
```C#
using MotionFramework;

public class CustomManager : ModuleSingleton<CustomManager>, IModule
{
  void IModule.OnCreate(System.Object param)
  {
    //当模块被创建的时候
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

游戏内创建模块
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

    // 通过AppEngine调用模块方法
    AppEngine.Instance.GetModule<CustomManager>().Print();

    // 通过全局实例调用模块方法
    CustomManager.Instance.Print();
  }
}
```
