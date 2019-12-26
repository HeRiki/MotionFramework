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
	internal class AssetDatabaseProvider : IAssetProvider
	{
		private string _loadPath;

		public string AssetName { private set; get; }
		public System.Type AssetType { private set; get; }
		public System.Object Result { private set; get; }
		public EAssetProviderStates States { private set; get; }
		public AssetOperationHandle Handle { private set; get; }
		public System.Action<AssetOperationHandle> Callback { set; get; }
		public float Progress
		{
			get
			{
				if (IsDone)
					return 100f;
				else
					return 0;
			}
		}
		public bool IsDone
		{
			get
			{
				return States == EAssetProviderStates.Succeed || States == EAssetProviderStates.Failed;
			}
		}

		public AssetDatabaseProvider(string loadPath, string assetName, System.Type assetType)
		{
			_loadPath = loadPath;
			AssetName = assetName;
			AssetType = assetType;
			States = EAssetProviderStates.None;
			Handle = new AssetOperationHandle(this);
		}
		public void Update()
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
				string loadPath = _loadPath;

				// 注意：如果加载路径指向的是文件夹
				if (UnityEditor.AssetDatabase.IsValidFolder(loadPath))
				{
					string folderPath = loadPath;
					string fileName = AssetName;
					loadPath = AssetSystem.FindDatabaseAssetPath(folderPath, fileName);
				}

				Result = UnityEditor.AssetDatabase.LoadAssetAtPath(loadPath, AssetType);
				States = EAssetProviderStates.Checking;
			}

			// 2. 检测加载结果
			if (States == EAssetProviderStates.Checking)
			{
				States = Result == null ? EAssetProviderStates.Failed : EAssetProviderStates.Succeed;
				if (States == EAssetProviderStates.Failed)
					LogSystem.Log(ELogType.Warning, $"Failed to load asset object : {_loadPath} : {AssetName}");
				Callback?.Invoke(Handle);
			}
#endif
		}
	}
}