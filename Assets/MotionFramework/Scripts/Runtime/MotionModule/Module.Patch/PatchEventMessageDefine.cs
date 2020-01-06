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
		PatchWindowDispatchEvents,
		PatchSystemDispatchEvents,
	}

	public class PatchEventMessageDefine
	{
		#region PatchWindowDispatchEvent
		/// <summary>
		/// 开始请求游戏版本号
		/// </summary>
		public class OperationEvent : IEventMessage
		{
			public EPatchOperation operation;
		}
		#endregion

		#region PatchSystemDispatchEvent
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
		/// 发现更新文件
		/// </summary>
		public class FoundUpdateFiles : IEventMessage
		{
			public int TotalCount;
			public long TotalSizeKB;
		}

		/// <summary>
		/// 下载文件列表进度
		/// </summary>
		public class DownloadFilesProgress : IEventMessage
		{
			public int TotalDownloadCount;
			public int CurrentDownloadCount;	
			public long TotalDownloadSizeKB;
			public long CurrentDownloadSizeKB;
		}

		/// <summary>
		/// 游戏版本号请求失败
		/// </summary>
		public class GameVersionRequestFailed : IEventMessage
		{
		}

		/// <summary>
		/// 网络上补丁清单下载失败
		/// </summary>
		public class WebPatchManifestDownloadFailed : IEventMessage
		{
		}

		/// <summary>
		/// 网络文件下载失败
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
		#endregion
	}
}