﻿//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework.Patch
{
	/// <summary>
	/// 操作方式
	/// </summary>
	public enum EPatchOperation
	{
		BeginingRequestGameVersion,
		BeginingDownloadWebFiles,
		TryRequestGameVersion,
		TryDownloadWebPatchManifest,
		TryDownloadWebFiles,
	}
}