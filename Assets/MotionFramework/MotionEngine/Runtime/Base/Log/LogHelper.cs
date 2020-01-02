//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework
{
	public static class LogHelper
	{
		private static System.Action<ELogType, string> _callback;

		/// <summary>
		/// 监听日志
		/// </summary>
		public static void RegisterCallback(System.Action<ELogType, string> callback)
		{
			_callback += callback;
		}

		/// <summary>
		/// 输出日志
		/// </summary>
		public static void Log(ELogType logType, string format, params object[] args)
		{
			if (_callback != null)
			{
				string log = string.Format(format, args);
				_callback.Invoke(logType, log);
			}
		}

		/// <summary>
		/// 输出日志
		/// </summary>
		public static void Log(ELogType logType, string log)
		{
			if (_callback != null)
			{
				_callback.Invoke(logType, log);
			}
		}
	}
}