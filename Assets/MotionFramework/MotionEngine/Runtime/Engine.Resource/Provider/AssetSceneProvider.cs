//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MotionFramework.Resource
{
	internal class AssetSceneProvider : AssetProvider
	{
		private SceneInstanceParam _param;
		private AsyncOperation _asyncOp;	

		public override float Progress
		{
			get
			{
				if (_asyncOp == null)
					return 0;
				return _asyncOp.progress;
			}
		}

		public AssetSceneProvider(AssetFileLoader owner, string assetName, System.Type assetType, SceneInstanceParam param)
			: base(owner, assetName, assetType)
		{
			_param = param;
		}
		public override void Update()
		{
			if (IsDone)
				return;

			if (States == EAssetProviderStates.None)
			{
				States = EAssetProviderStates.Loading;
			}

			// 1. 加载资源对象
			if (States == EAssetProviderStates.Loading)
			{
				var mode = _param.IsAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
				_asyncOp = SceneManager.LoadSceneAsync(AssetName, mode);
				if (_asyncOp != null)
				{
					_asyncOp.allowSceneActivation = _param.ActivateOnLoad;
					States = EAssetProviderStates.Checking;
				}
				else
				{
					LogHelper.Log(ELogType.Warning, $"Failed to load scene : {AssetName}");
					States = EAssetProviderStates.Failed;
					InvokeCompletion();
				}
			}

			// 2. 检测加载结果
			if (States == EAssetProviderStates.Checking)
			{
				if (_asyncOp.isDone || (_param.ActivateOnLoad == false && _asyncOp.progress == 0.9f))
				{
					SceneInstance instance = new SceneInstance(_asyncOp);
					instance.Scene = SceneManager.GetSceneByName(AssetName);
					AssetObject = instance;
					States = EAssetProviderStates.Succeed;
					InvokeCompletion();
				}
			}
		}
	}
}