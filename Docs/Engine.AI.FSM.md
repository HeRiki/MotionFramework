### 有限状态机 (Engine.AI.FSM)

定义有限状态机或流程的节点
```C#
public class CustomNode : IFsmNode
{
	public string Name { private set; get; }

	public CustomNode()
	{
		Name = "CustomNode";
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