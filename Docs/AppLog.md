### 日志 (AppLog)

监听框架内部的日志系统
```C#
using MotionFramework;

public class Test
{
	public void Start()
	{
		// 监听日志
		AppLog.RegisterCallback(LogCallback);
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