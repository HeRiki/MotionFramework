### 调试控制台 (DebugConsole)

控制台系统可以帮助开发者在游戏运行时显示一些关键数据或帮助调试游戏。

框架默认自带几个控制台节点：系统，模块，日志，资源列表，引用池，实体池等
![image](https://github.com/gmhevinci/MotionFramework/raw/master/Docs/Image/img2.png)  

定义自己的控制台节点  
```C#
using MotionFramework.Debug;

namespace MotionGame
{
	[DebugAttribute("控制台节点显示名称", 201)]
	public class CustomDebugWindow : IDebugWindow
	{
		public void OnInit()
		{
		}
		public void OnGUI()
		{
			DebugConsole.GUILable("在这里编写GUI代码");
		}
	}
}
```

注意：定义完成后，运行游戏即可查看我们自定义的节点

更详细的教程请参考示例代码
1. [MotionGame/Runtime/Game.Debug](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/MotionGame/Runtime/Game.Debug)