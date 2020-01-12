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
	internal class AssetResourcesFileLoader : AssetFileLoader
	{
		public AssetResourcesFileLoader(string loadPath)
			: base(loadPath)
		{
		}
		public override void Update()
		{
			// 如果资源文件加载完毕
			if (States == EFileStates.Success || States == EFileStates.Fail)
			{
				UpdateAllProvider();
				return;
			}

			States = EFileStates.Success;
		}
		public override bool IsDone()
		{
			if (base.IsDone() == false)
				return false;

			return CheckAllProviderIsDone();
		}
	}
}