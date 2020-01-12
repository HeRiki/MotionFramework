### 状态机管理器 (FsmManager)

定义节点
```C#
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
		// 转换节点
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

创建有限状态机管理器
```C#
using MotionFramework.AI;

public class Test
{
	public void Start()
	{
		// 设置参数
		FsmManager.CreateParameters createParam = new FsmManager.CreateParameters();
		createParam.Graph = null;
		createParam.EntryNode = "Start";
		createParam.Nodes = new List<IFsmNode>() { new FsmStart(), new FsmRunning() };

		// 创建模块
		// 在模块被创建之后，会自动运行我们指定的"Start"节点
		MotionEngine.CreateModule<FsmManager>(createParam);
	}
}
```

更详细的教程请参考示例代码
1. [Module.AI/FsmManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/Scripts/Runtime/Module/Module.AI/FSM/FsmManager.cs)