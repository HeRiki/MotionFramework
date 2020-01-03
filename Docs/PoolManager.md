### 对象池管理器 (PoolManager)

创建对象池管理器
```C#
public void Start()
{
	// 创建模块
	AppEngine.Instance.CreateModule<PoolManager>();
}
```

对象池使用异步使用范例
```C#
using MotionFramework.Pool;

public class Test
{
	private string _npcName = "Model/Npc001";
	private GameObject _npc;

	public void Awake()
	{
		// 创建NPC的对象池
		int capacity = 0;
		PoolManager.Instance.CreatePool(_npcName, capacity);
	}
	public void Start()
	{
		// 异步方法
		PoolManager.Instance.Spawn(_npcName, SpawnCallback);
	}
	private void SpawnCallback(GameObject go)
	{
		_npc = go;
	}
}
```

对象池使用同步使用范例
```C#
using MotionFramework.Pool;

public class Test
{
	private string _npcName = "Model/Npc001";
	private GameObject _npc;
	private bool _isSpawn = false;

	public void Awake()
	{
		// 创建NPC的对象池
		int capacity = 0;
		PoolManager.Instance.CreatePool(_npcName, capacity);
	}
	public void Update()
	{
		if(_isSpawn == false && PoolManager.Instance.IsAllPrepare())
		{
			_isSpawn = true;
			_npc = PoolManager.Instance.Spawn(_npcName);
		}
	}
}
```

更详细的教程请参考示例代码
1. [MotionModule/Module.Pool/PoolManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/Scripts/Runtime/MotionModule/Module.Pool/PoolManager.cs)