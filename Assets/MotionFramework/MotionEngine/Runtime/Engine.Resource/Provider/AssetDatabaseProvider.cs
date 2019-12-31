//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionFramework.Resource
{
	internal class AssetDatabaseProvider : AssetProvider
	{
		public override float Progress
		{
			get
			{
				if (IsDone)
					return 100f;
				else
					return 0;
			}
		}

		public AssetDatabaseProvider(AssetFileLoader owner, string assetName, System.Type assetType)
			: base(owner, assetName, assetType)
		{
		}
		public override void Update()
		{
#if UNITY_EDITOR
			if (IsDone)
				return;

			if (States == EAssetProviderStates.None)
			{
				States = EAssetProviderStates.Loading;
			}

			// 1. 加载资源对象
			if (States == EAssetProviderStates.Loading)
			{
				string loadPath = _owner.LoadPath;

				// 注意：如果加载路径指向的是文件夹
				if (UnityEditor.AssetDatabase.IsValidFolder(loadPath))
				{
					string folderPath = loadPath;
					string fileName = AssetName;
					loadPath = AssetSystem.FindDatabaseAssetPath(folderPath, fileName);
				}

				AssetObject = UnityEditor.AssetDatabase.LoadAssetAtPath(loadPath, AssetType);
				States = EAssetProviderStates.Checking;
			}

			// 2. 检测加载结果
			if (States == EAssetProviderStates.Checking)
			{
				States = AssetObject == null ? EAssetProviderStates.Failed : EAssetProviderStates.Succeed;
				if (States == EAssetProviderStates.Failed)
					LogSystem.Log(ELogType.Warning, $"Failed to load asset object : {_owner.LoadPath} : {AssetName}");
				InvokeCompletion();
			}
#endif
		}
	}
}