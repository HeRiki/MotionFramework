### 控制台 (Engine.Console)

控制台可以帮助开发者在游戏运行时显示一些关键数据或者帮助调试游戏。  

框架默认自带的控制台窗口截图
![image](https://github.com/gmhevinci/MotionFramework/raw/master/Docs/Image/img2.png)  

初始化控制台  
```C#
public class GameLauncher : MonoBehaviour
{
	void Awake()
	{
		// 初始化控制台
		if (Application.isEditor || Debug.isDebugBuild)
			DeveloperConsole.Initialize();
	}

	void OnGUI()
	{
		// 绘制控制台窗口
		if (Application.isEditor || Debug.isDebugBuild)
			DeveloperConsole.DrawGUI();
	}
}
```

自定义控制台窗口  
```C#
using MotionFramework.Console;

[ConsoleAttribute("窗口标题", 201)]
public class CustomDebugWindow : IConsoleWindow
{
	public void OnCreate()
	{
	}

	public void OnGUI()
	{
		ConsoleGUI.Lable("在这里编写GUI代码");
	}
}
```

更详细的教程请参考示例代码
1. [Module.Console](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/Scripts/Runtime/Module/Module.Console)