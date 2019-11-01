创建自定义模块代码示例
```C#
using MotionEngine;

public class CustomModule : IModule
{
  public static readonly CustomModule Instance = new CustomModule();
  
  private CustomModule()
  {
  }
  public void Awake()
  {
    //当模块被注册的时候被调用，仅被执行一次
  }
  public void Start()
  {
    //当第一次Update之前被调用，仅被执行一次
  }
  public void Update()
  {
    //Update方法
  }
  public void LateUpdate()
  {
    //在所有Update执行完毕后被调用
  }
  public void OnGUI()
  {
    //GUI绘制
    //可以在这里写一些Unity GUI相关的代码
  }
}
```

在合适的地方注册我们的模块