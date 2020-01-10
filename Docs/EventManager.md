### 事件管理器 (EventManager)

创建事件管理器
```C#
public void Start()
{
  // 创建模块
  AppEngine.Instance.CreateModule<EventManager>();
}
```

定义事件类
```C#
using MotionFramework.Event;

public class TestEventMsg : IEventMessage
{
  public string Value;
}
```

订阅事件
```C#
using UnityEngine;
using MotionFramework.Event;

public class Test
{
  public void Start()
  {
    EventManager.Instance.AddListener<TestEventMsg>(OnHandleEventMsg);
  }

  public void Destroy()
  {
    EventManager.Instance.RemoveListener<TestEventMsg>(OnHandleEventMsg);
  }

  private void OnHandleEventMsg(IEventMessage msg)
  {
    if(msg is TestEventMsg)
    {
      TestEventMsg temp = msg as TestEventMsg;
      Debug.Log($"{temp.Value}");
    }
  }
}
```

发送事件
```C#
using UnityEngine;
using MotionFramework.Event;

public class Test
{
  public void Start()
  {
    TestEventMsg msg = new TestEventMsg()
    {
      Value = $"hello world",
    };
    EventManager.Instance.SendMessage(msg);
  }
}
```

1. [MotionModule/Module.Event/EventManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/Scripts/Runtime/MotionModule/Module.Event/EventManager.cs)