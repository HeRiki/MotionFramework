### 控制台 (AppConsole)

控制台可以帮助开发者在游戏运行时显示一些关键数据或者帮助调试游戏。

框架默认自带的控制台窗口截图
![image](https://github.com/gmhevinci/MotionFramework/raw/master/Docs/Image/img2.png)  

自定义控制台窗口  
```C#
using MotionFramework.Console;

namespace MotionGame
{
	[ConsoleAttribute("窗口标题", 201)]
	public class CustomDebugWindow : IConsoleWindow
	{
		public void OnInit()
		{
		}
		public void OnGUI()
		{
			AppConsole.GUILable("在这里编写GUI代码");
		}
	}
}
```

更详细的教程请参考示例代码
1. [MotionModule/Module.Console](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/Scripts/Runtime/MotionModule/Module.Console)