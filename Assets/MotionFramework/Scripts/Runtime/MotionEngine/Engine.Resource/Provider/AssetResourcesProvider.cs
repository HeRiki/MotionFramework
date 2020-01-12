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
	internal class AssetResourcesProvider : AssetProvider
	{
		private ResourceRequest _cacheRequest;
		public override float Progress
		{
			get
			{
				if (_cacheRequest == null)
					return 0;
				return _cacheRequest.progress;
			}
		}

		public AssetResourcesProvider(AssetFileLoader owner, string assetName, System.Type assetType)
			: base(owner, assetName, assetType)
		{
		}
		public override void Update()
		{
			if (IsDone)
				return;

			if (States == EAssetStates.None)
			{
				States = EAssetStates.Loading;
			}

			// 1. 加载资源对象
			if (States == EAssetStates.Loading)
			{
				_cacheRequest = Resources.LoadAsync(_owner.LoadPath, AssetType);
				States = EAssetStates.Checking;
			}

			// 2. 检测加载结果
			if (States == EAssetStates.Checking)
			{
				if (_cacheRequest.isDone == false)
					return;
				AssetObject = _cacheRequest.asset;
				States = AssetObject == null ? EAssetStates.Fail : EAssetStates.Success;
				if (States == EAssetStates.Fail)
					MotionLog.Log(ELogType.Warning, $"Failed to load asset object : {_owner.LoadPath} : {AssetName}");
				InvokeCompletion();
			}
		}
	}
}