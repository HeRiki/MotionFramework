### 日志 (MotionLog)

监听框架日志
```C#
using MotionFramework;

public void Start()
{
	// 监听框架日志
	MotionLog.RegisterCallback(LogCallback);
}

private void LogCallback(ELogLevel logLevel, string log)
{
	if (logLevel == ELogLevel.Log)
	{
		UnityEngine.Debug.Log(log);
	}
	else if (logLevel == ELogLevel.Error)
	{
		UnityEngine.Debug.LogError(log);
	}
	else if (logLevel == ELogLevel.Warning)
	{
		UnityEngine.Debug.LogWarning(log);
	}
	else if (logLevel == ELogLevel.Exception)
	{
		UnityEngine.Debug.LogError(log);
	}
	else
	{
		throw new NotImplementedException($"{logLevel}");
	}
}
```