//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionFramework.Resource
{
	/// <summary>
	/// Resources接口加载器
	/// </summary>
	public class AssetResourceLoader : AssetFileLoader
	{
		public AssetResourceLoader(EAssetFileType assetFileType, string loadPath)
			: base(assetFileType, loadPath)
		{
		}
		public override void Update()
		{
			// 如果资源文件加载完毕
			if (States == EAssetFileLoaderStates.LoadAssetFileOK || States == EAssetFileLoaderStates.LoadAssetFileFailed)
			{
				UpdateAllProvider();
				return;
			}

			States = EAssetFileLoaderStates.LoadAssetFileOK;
		}
		public override bool IsDone()
		{
			if (base.IsDone() == false)
				return false;

			return CheckAllProviderIsDone();
		}
	}
}