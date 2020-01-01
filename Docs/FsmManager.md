### 状态机管理器 (FsmManager)

定义节点
```C#
using MotionFramework.AI;

public class FsmStart : IFsmNode
{
	public string Name { private set; get; }

	public FsmStart()
	{
		Name = "Start";
	}

	void IFsmNode.OnEnter()
	{
	}
	void IFsmNode.OnUpdate()
	{
		// 转换状态
		FsmManager.Instance.Transition("Running");
	}
	void IFsmNode.OnExit()
	{
	}
	void IFsmNode.OnHandleMessage(object msg)
	{
	}
}

public class FsmRunning : IFsmNode
{
	public string Name { private set; get; }

	public FsmRunning()
	{
		Name = "Running";
	}

	void IFsmNode.OnEnter()
	{
	}
	void IFsmNode.OnUpdate()
	{
	}
	void IFsmNode.OnExit()
	{
	}
	void IFsmNode.OnHandleMessage(object msg)
	{
	}
}
```

```C#
using MotionFramework.AI;

public class Test
{
	public void Start()
	{
		// 创建模块
		FsmManager.CreateParameters param = new FsmManager.CreateParameters();
		param.Graph = null;
		param.RunNode = "Start";
		AppEngine.Instance.CreateModule<FsmManager>(param);

		// 添加节点
	 	FsmManager.Instance.AddNode(new FsmStart())
	 	FsmManager.Instance.AddNode(new FsmRunning());
	}
}
```

更详细的教程请参考示例代码
1. [MotionModule/Runtime/Module.AI/FsmManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/MotionModule/Runtime/Module.AI/FsmManager.cs)