### 日志系统 (LogSystem)

MotionFramework内部使用了一套统一的日志系统。开发者需要注册才会显示内部的日志。

```C#
using MotionFramework;

public class Test
{
	public void Start()
	{
		// 监听日志
		Logger.RegisterCallback(LogCallback);
	}

	private void LogCallback(ELogType logType, string log)
	{
		if (logType == ELogType.Log)
		{
			UnityEngine.Debug.Log(log);
		}
		else if (logType == ELogType.Error)
		{
			UnityEngine.Debug.LogError(log);
		}
		else if (logType == ELogType.Warning)
		{
			UnityEngine.Debug.LogWarning(log);
		}
		else if (logType == ELogType.Exception)
		{
			UnityEngine.Debug.LogError(log);
		}
		else
		{
			throw new NotImplementedException($"{logType}");
		}
	}
}
```