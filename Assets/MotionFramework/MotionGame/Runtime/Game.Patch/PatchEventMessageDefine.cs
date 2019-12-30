//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using MotionFramework.Event;

namespace MotionFramework.Patch
{
	/// <summary>
	/// 补丁事件标签
	/// </summary>
	public enum EPatchEventMessageTag
	{
		PatchManagerEvent,
		PatchWindowEvent,
	}

	public class PatchEventMessageDefine
	{
		/// <summary>
		/// 补丁更新状态改变
		/// </summary>
		public class PatchStatesChange : IEventMessage
		{
			public EPatchStates CurrentStates;
		}

		/// <summary>
		/// 发现新的APP安装包
		/// </summary>
		public class FoundNewAPP : IEventMessage
		{
			public string NewVersion;
		}

		/// <summary>
		/// 文件下载进度
		/// </summary>
		public class DownloadProgress : IEventMessage
		{
			public int TotalDownloadCount;
			public int CurrentDownloadCount;	
			public long TotalDownloadSizeKB;
			public long CurrentDownloadSizeKB;
		}

		/// <summary>
		/// 文件下载失败
		/// </summary>
		public class WebFileDownloadFailed : IEventMessage
		{
			public string FilePath;
		}

		/// <summary>
		/// 文件MD5验证失败
		/// </summary>
		public class WebFileMD5VerifyFailed : IEventMessage
		{
			public string FilePath;
		}
	}
}