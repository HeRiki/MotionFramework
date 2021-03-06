﻿//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionFramework.Resource
{
	internal class AssetDatabaseFileLoader : AssetFileLoader
	{
		public AssetDatabaseFileLoader(string loadPath)
			: base(loadPath)
		{
		}
		public override void Update()
		{
#if UNITY_EDITOR
			// 如果资源文件加载完毕
			if (States == EFileStates.Fail || States == EFileStates.Success)
			{
				UpdateAllProvider();
				return;
			}

			// 检测资源文件是否存在
			string guid = UnityEditor.AssetDatabase.AssetPathToGUID(LoadPath);
			if (string.IsNullOrEmpty(guid))
				States = EFileStates.Fail;
			else
				States = EFileStates.Success;
#endif
		}
	}
}
