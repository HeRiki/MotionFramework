### 流程系统 (ProcedureSystem)

定义流程步骤
```C#
using MotionFramework.AI;

public enum EProcedureType
{
	Procedure1,
	Procedure2,
	Procedure3,
}

// 流程1
public class CheckResourceVersion : IFsmNode
{
	private ProcedureSystem _system;
	public string Name { private set; get; }

	public CheckResourceVersion(ProcedureSystem system)
	{
		_system = system;
		Name = EProcedureType.Procedure1.ToString();
	}
	public override void OnEnter()
	{
	}
	public override void OnUpdate()
	{
		// 切换到下一个节点
		_system.SwitchNext();
	}
	public override void OnExit()
	{
	}
}

// 流程2
public class DownloadVersionFile : IFsmNode
{
	private ProcedureSystem _system;
	public string Name { private set; get; }

	public DownloadVersionFile(ProcedureSystem system)
	{
		_system = system;
		Name = EProcedureType.Procedure2.ToString();
	}
	public override void OnEnter()
	{
	}
	public override void OnUpdate()
	{
		// 切换到下一个节点
		_system.SwitchNext();
	}
	public override void OnExit()
	{
	}
}

// 流程3
public class DownloadPatchFiles : IFsmNode
{
	private ProcedureSystem _system;
	public string Name { private set; get; }

	public DownloadPatchFiles(ProcedureSystem system)
	{
		_system = system;
		Name = EProcedureType.Procedure3.ToString();
	}
	public override void OnEnter()
	{
	}
	public override void OnUpdate()
	{
		// 已经是最后一个节点
	}
	public override void OnExit()
	{
	}
}
```

创建流程系统
```C#
using MotionFramework.AI;

public class Test
{
	private ProcedureSystem _system = new ProcedureSystem();

	public void Start()
	{
		// 创建流程
	 	CheckResourceVersion node1 = new CheckResourceVersion(_system);
	 	DownloadVersionFile node2 = new DownloadVersionFile(_system);
	 	DownloadPatchFiles node3 = new DownloadPatchFiles(_system);

	 	// 按顺序添加流程
	 	_system.AddNode(node1);
	 	_system.AddNode(node2);
	 	_system.AddNode(node3);

	 	// 运行流程系统
	 	_system.Run();
	}

	public void Update()
	{
		// 更新流程系统
		_system.Update()
	}
}
```