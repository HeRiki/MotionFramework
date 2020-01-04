//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework.Patch
{
	/// <summary>
	/// 补丁更新状态
	/// </summary>
	public enum EPatchStates
	{
		/// <summary>
		/// 准备阶段开始
		/// </summary>
		PrepareBegin,

		/// <summary>
		/// 检测沙盒是否变脏
		/// 注意：在覆盖安装的时候，会保留沙盒目录里的文件，所以需要强制清空。
		/// </summary>
		CheckSandboxDirty,

		/// <summary>
		/// 分析APP内的补丁文件
		/// </summary>
		ParseAppPatchFile,

		/// <summary>
		/// 分析沙盒内的补丁文件
		/// </summary>
		ParseSandboxPatchFile,

		/// <summary>
		/// 准备阶段结束
		/// </summary>
		PrepareEnd,


		/// <summary>
		/// 请求最新的游戏版本
		/// </summary>
		RequestGameVersion,

		/// <summary>
		/// 分析网络上的补丁文件
		/// </summary>
		ParseWebPatchFile,

		/// <summary>
		/// 获取下载列表
		/// </summary>
		GetDonwloadList,

		/// <summary>
		/// 下载网络文件到沙盒
		/// </summary>
		DownloadWebFiles,

		/// <summary>
		/// 下载网络补丁文件到沙盒
		/// </summary>
		DownloadWebPatchFile,

		/// <summary>
		/// 补丁更新结束
		/// </summary>
		PatchOver,
	}

	/// <summary>
	/// 操作类型
	/// </summary>
	public enum EOperationType
	{
		BeginingRequestGameVersion,
		BeginingDownloadWebFiles,
		TryRequestGameVersion,
		TryDownloadPatchFile,
		TryDownloadWebFile,
	}
}