﻿//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections.Generic;
using System;

namespace MotionFramework.Resource
{
	/// <summary>
	/// 资源加载器基类
	/// </summary>
	public abstract class AssetFileLoader
	{
		/// <summary>
		/// 加载路径
		/// </summary>
		public string LoadPath { get; private set; }

		/// <summary>
		/// 引用计数
		/// </summary>
		public int RefCount { get; private set; }

		/// <summary>
		/// 加载状态
		/// </summary>
		public EAssetFileLoaderStates States { get; protected set; }


		public AssetFileLoader(string loadPath)
		{
			LoadPath = loadPath;
			RefCount = 0;
			States = EAssetFileLoaderStates.None;
		}

		/// <summary>
		/// 轮询更新方法
		/// </summary>
		public abstract void Update();

		/// <summary>
		/// 引用方法（引用计数递加）
		/// </summary>
		public virtual void Reference()
		{
			RefCount++;
		}

		/// <summary>
		/// 释放方法（引用计数递减）
		/// </summary>
		public virtual void Release()
		{
			RefCount--;
		}

		/// <summary>
		/// 销毁方法
		/// </summary>
		public virtual void Destroy(bool force)
		{
			IsDestroy = true;
		}

		/// <summary>
		/// 是否完毕（无论成功失败）
		/// </summary>
		public virtual bool IsDone()
		{
			return States == EAssetFileLoaderStates.LoadAssetFileOK || States == EAssetFileLoaderStates.LoadAssetFileFailed;
		}

		/// <summary>
		/// 是否已经销毁
		/// </summary>
		public bool IsDestroy { private set; get; }

		#region Asset Provider
		internal readonly List<IAssetProvider> _providers = new List<IAssetProvider>();

		/// <summary>
		/// 加载资源对象
		/// </summary>
		public AssetOperationHandle LoadAssetAsync(string assetName, System.Type assetType, IAssetParam param)
		{
			IAssetProvider provider = TryGetProvider(assetName);
			if (provider == null)
			{
				if (assetType == typeof(SceneInstance))
				{
					SceneInstanceParam sceneParam = param as SceneInstanceParam;
					provider = new AssetSceneProvider(this, assetName, assetType, sceneParam);
				}
				else if(assetType == typeof(PackageInstance))
				{
					throw new NotImplementedException(nameof(PackageInstance)); // TODO
				}
				else
				{
					if (this is AssetBundleLoader)
						provider = new AssetBundleProvider(this, assetName, assetType);
					else if (this is AssetDatabaseLoader)
						provider = new AssetDatabaseProvider(this, assetName, assetType);
					else if (this is AssetResourceLoader)
						provider = new AssetResourceProvider(this, assetName, assetType);
					else
						throw new NotImplementedException($"{this.GetType()}");
				}
				_providers.Add(provider);
			}
			return provider.Handle;
		}

		/// <summary>
		/// 获取失败的资源提供者总数
		/// </summary>
		public int GetFailedProviderCount()
		{
			int failedCount = 0;
			for (int i = 0; i < _providers.Count; i++)
			{
				var provider = _providers[i];
				if (provider.States == EAssetProviderStates.Failed)
					failedCount++;
			}
			return failedCount;
		}

		/// <summary>
		/// 检测所有资源提供者是否完毕
		/// </summary>
		protected bool CheckAllProviderIsDone()
		{
			bool isAllLoadDone = true;
			for (int i = 0; i < _providers.Count; i++)
			{
				var provider = _providers[i];
				if (provider.IsDone == false)
				{
					isAllLoadDone = false;
					break;
				}
			}
			return isAllLoadDone;
		}

		/// <summary>
		/// 轮询更新所有资源提供者
		/// </summary>
		protected void UpdateAllProvider()
		{
			for (int i = 0; i < _providers.Count; i++)
			{
				_providers[i].Update();
			}
		}

		// 获取一个资源提供者
		private IAssetProvider TryGetProvider(string assetName)
		{
			IAssetProvider provider = null;
			for (int i = 0; i < _providers.Count; i++)
			{
				IAssetProvider temp = _providers[i];
				if (temp.AssetName.Equals(assetName))
				{
					provider = temp;
					break;
				}
			}
			return provider;
		}
		#endregion
	}
}