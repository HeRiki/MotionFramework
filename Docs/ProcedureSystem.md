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
public class CheckResourceVersion : FsmState
{
	private ProcedureSystem _system;
	public CheckResourceVersion(ProcedureSystem system, int type) : base(type)
	{
		_system = system;
	}
	public override void Enter()
	{
	}
	public override void Execute()
	{
		// 切换到下一个流程
		_system.SwitchNextProcedure();
	}
	public override void Exit()
	{
	}
}

// 流程2
public class DownloadVersionFile : FsmState
{
	private ProcedureSystem _system;
	public DownloadVersionFile(ProcedureSystem system, int type) : base(type)
	{
		_system = system;
	}
	public override void Enter()
	{
	}
	public override void Execute()
	{
		// 切换到下一个流程
		_system.SwitchNextProcedure();
	}
	public override void Exit()
	{
	}
}

// 流程3
public class DownloadPatchFiles : FsmState
{
	private ProcedureSystem _system;
	public DownloadPatchFiles(ProcedureSystem system, int type) : base(type)
	{
		_system = system;
	}
	public override void Enter()
	{
	}
	public override void Execute()
	{
		// 已经是最后一个流程
	}
	public override void Exit()
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
	 	CheckResourceVersion node1 = new CheckResourceVersion(_system, (int)EProcedureType.Procedure1);
	 	DownloadVersionFile node2 = new DownloadVersionFile(_system, (int)EProcedureType.Procedure2);
	 	DownloadPatchFiles node3 = new DownloadPatchFiles(_system, (int)EProcedureType.Procedure3);

	 	// 按顺序添加流程
	 	_system.AddProcedure(node1);
	 	_system.AddProcedure(node2);
	 	_system.AddProcedure(node3);

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

更详细的教程请参考示例代码
1. [MotionEngine/Runtime/Engine.AI/Procedure](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/MotionEngine/Runtime/Engine.AI/Procedure)