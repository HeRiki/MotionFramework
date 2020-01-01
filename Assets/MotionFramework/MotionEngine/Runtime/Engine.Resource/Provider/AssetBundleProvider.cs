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
	internal class AssetBundleProvider : AssetProvider
	{
		private AssetBundleFileLoader _loader;
		private AssetBundleRequest _cacheRequest;

		public override float Progress
		{
			get
			{
				if (_cacheRequest == null)
					return 0;
				return _cacheRequest.progress;
			}
		}

		public AssetBundleProvider(AssetFileLoader owner, string assetName, System.Type assetType)
			: base(owner, assetName, assetType)
		{
			_loader = owner as AssetBundleFileLoader;
		}
		public override void Update()
		{
			if (IsDone)
				return;

			if (_loader.CacheBundle == null)
			{
				States = EAssetProviderStates.Failed;
				InvokeCompletion();
			}

			if (States == EAssetProviderStates.None)
			{
				States = EAssetProviderStates.Loading;
			}

			// 1. 加载资源对象
			if (States == EAssetProviderStates.Loading)
			{
				if (AssetType == null)
					_cacheRequest = _loader.CacheBundle.LoadAssetAsync(AssetName);
				else
					_cacheRequest = _loader.CacheBundle.LoadAssetAsync(AssetName, AssetType);
				States = EAssetProviderStates.Checking;
			}

			// 2. 检测加载结果
			if (States == EAssetProviderStates.Checking)
			{
				if (_cacheRequest.isDone == false)
					return;
				AssetObject = _cacheRequest.asset;
				States = AssetObject == null ? EAssetProviderStates.Failed : EAssetProviderStates.Succeed;
				if (States == EAssetProviderStates.Failed)
					Logger.Log(ELogType.Warning, $"Failed to load asset object : {_loader.LoadPath} : {AssetName}");
				InvokeCompletion();
			}
		}
	}
}