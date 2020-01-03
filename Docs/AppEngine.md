### 引擎 (AppEngine)  

在开发游戏的过程中，我们常常需要定义自己的游戏模块或管理器。  

自定义模块代码示例
```C#
using MotionFramework;

public class BattleManager : ModuleSingleton<BattleManager>, IMotionModule
{
  void IMotionModule.OnCreate(System.Object param)
  {
    //当模块被创建的时候
  }
  void IMotionModule.OnStart()
  {
    //在首次Update之前被调用，仅被执行一次
  }
  void IMotionModule.OnUpdate()
  {
    //轮询模块
  }
  void IMotionModule.OnGUI()
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

创建和使用模块
```C#
using MotionFramework;

public class Test
{
  public void Start()
  {
    // 创建模块
    AppEngine.Instance.CreateModule<BattleManager>();

    // 带优先级的创建方式
    // 说明：运行时的优先级，优先级越大越早轮询。如果没有设置优先级，那么会按照添加顺序执行
    int priority = 1000;
    AppEngine.Instance.CreateModule<BattleManager>(priority);

    // 通过AppEngine调用模块方法
    AppEngine.Instance.GetModule<BattleManager>().Print();

    // 通过全局实例调用模块方法
    BattleManager.Instance.Print();
  }
}
```
