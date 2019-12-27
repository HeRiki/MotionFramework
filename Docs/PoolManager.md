### 对象池管理器 (PoolManager)

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

1. [MotionGame/Runtime/Game.Pool/PoolManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/MotionGame/Runtime/Game.Pool/PoolManager.cs)