### 引用池系统 (ReferenceSystem)

定义引用类
```C#
using MotionFramework.Reference;

public class ReferClass : IReference
{
	public int Value = 0;

	// 在回收的时候该方法会被执行
	public void OnRelease()
	{
		Value = 0;
	}
}
```

单个回收范例
```C#
using MotionFramework.Reference;

public class Test
{
	public void Start()
	{
		// 获取对象方式1
		{
			ReferClass refer = ReferenceSystem.Instance.Spawn(typeof(ReferClass)) as ReferClass;
			ReferenceSystem.Instance.Release(refer)
		}

		// 获取对象方式2
		{
			ReferClass refer = ReferenceSystem.Instance.Spawn<ReferClass>();
			ReferenceSystem.Instance.Release(refer)
		}
	}
}
```

批量回收范例
```C#
using System.Collections.Generic;
using MotionFramework.Reference;

public class Test
{
	private List<ReferClass> _referList = new List<ReferClass>();
	private ReferClass[] _referArray = new ReferClass[10];

	public void Start()
	{
		// 回收列表对象集合
		ReferenceSystem.Instance.Release<ReferClass>(_referList)

		// 回收数组对象集合
		ReferenceSystem.Instance.Release<ReferClass>(_referArray)
	}
}
```