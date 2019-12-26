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
	internal class AssetSceneProvider : IAssetProvider
	{
		private AsyncOperation _asyncOp;
		private SceneInstanceParam _param;

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
				if (_asyncOp == null)
					return 0;
				return _asyncOp.progress;
			}
		}
		public bool IsDone
		{
			get
			{
				return States == EAssetProviderStates.Succeed || States == EAssetProviderStates.Failed;
			}
		}

		public AssetSceneProvider(string assetName, System.Type assetType, SceneInstanceParam param)
		{
			_param = param;
			AssetName = assetName;
			AssetType = assetType;
			States = EAssetProviderStates.None;
			Handle = new AssetOperationHandle(this);
		}
		public void Update()
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
					LogSystem.Log(ELogType.Warning, $"Failed to load scene : {AssetName}");
					States = EAssetProviderStates.Failed;
					Callback?.Invoke(Handle);
				}
			}

			// 2. 检测加载结果
			if (States == EAssetProviderStates.Checking)
			{
				if (_asyncOp.isDone || (_param.ActivateOnLoad == false && _asyncOp.progress == 0.9f))
				{
					SceneInstance instance = new SceneInstance(_asyncOp);
					instance.Scene = SceneManager.GetSceneByName(AssetName);
					Result = instance;
					States = EAssetProviderStates.Succeed;
					Callback?.Invoke(Handle);
				}
			}
		}
	}
}