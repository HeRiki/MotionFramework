### 事件管理器 (EventManager)

定义事件类
```C#
using MotionFramework.Event;

public class TestEventMsg : IEventMessage
{
  public string Value;
}
```

监听事件
```C#
using UnityEngine;
using MotionFramework.Event;

public class Test
{
  public void Start()
  {
    EventManager.Instance.AddListener("customEventTag", OnHandleEventMsg);
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
    EventManager.Instance.SendMessage("customEventTag", msg);
  }
}
```

1. [MotionModule/Runtime/Module.Event/EventManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/MotionModule/Runtime/Module.Event/EventManager.cs)