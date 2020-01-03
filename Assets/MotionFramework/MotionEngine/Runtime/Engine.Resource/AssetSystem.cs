//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionFramework.Resource
{
	/// <summary>
	/// 资源系统
	/// </summary>
	public class AssetSystem
	{
		#region 全局实例
		private readonly static AssetSystem _instance = new AssetSystem();
		private static bool _isInitialize = false;

		/// <summary>
		/// 资源系统全局实例
		/// </summary>
		public static AssetSystem Instance
		{
			get
			{
				if (_isInitialize == false)
					throw new Exception($"{nameof(AssetSystem)} is not initialize.");
				return _instance;
			}
		}

		/// <summary>
		/// 初始化资源系统
		/// 注意：在使用AssetSystem之前需要初始化
		/// </summary>
		public static void Initialize(string assetRootPath, EAssetSystemMode assetSystemMode , IBundleServices bundleServices)
		{
			_instance.InitializeInternal(assetRootPath, assetSystemMode, bundleServices);
		}
		#endregion

		private readonly List<AssetFileLoader> _fileLoaders = new List<AssetFileLoader>(1000);
		private readonly List<string> _removeKeys = new List<string>(100);


		/// <summary>
		/// 资源系统根路径
		/// </summary>
		public string AssetRootPath { private set; get; }

		/// <summary>
		/// 资源系统模式
		/// </summary>
		public EAssetSystemMode AssetSystemMode { private set; get; }

		/// <summary>
		/// AssetBundle服务接口
		/// </summary>
		public IBundleServices BundleServices { private set; get; }


		private AssetSystem()
		{
		}

		/// <summary>
		/// 初始化资源系统
		/// </summary>
		internal void InitializeInternal(string assetRootPath, EAssetSystemMode assetSystemMode, IBundleServices bundleServices)
		{
			if (_isInitialize == false)
			{
				_isInitialize = true;
				AssetRootPath = assetRootPath;
				AssetSystemMode = assetSystemMode;
				BundleServices = bundleServices;
			}
			else
			{
				LogHelper.Log(ELogType.Error, $"{nameof(AssetSystem)} is initialized");
			}
		}

		/// <summary>
		/// 轮询更新
		/// </summary>
		public void UpdatePoll()
		{
			for (int i = 0; i < _fileLoaders.Count; i++)
			{
				_fileLoaders[i].Update();
			}
		}

		/// <summary>
		/// 创建资源文件加载器
		/// </summary>
		public AssetFileLoader CreateFileLoader(string location)
		{
			AssetFileLoader loader;
			if (AssetSystemMode == EAssetSystemMode.EditorMode)
			{
#if UNITY_EDITOR
				string loadPath = AssetPathHelper.FindDatabaseAssetPath(location);
				loader = CreateFileLoaderInternal(loadPath, null);
#else
				throw new Exception("EAssetSystemMode.EditorMode only support unity editor.");
#endif
			}
			else if (AssetSystemMode == EAssetSystemMode.ResourcesMode)
			{
				string loadPath = location;
				loader = CreateFileLoaderInternal(loadPath, null);
			}
			else if (AssetSystemMode == EAssetSystemMode.BundleMode)
			{
				if (BundleServices == null)
					throw new Exception($"{nameof(AssetSystem.BundleServices)} is null.");

				string manifestPath = AssetPathHelper.ConvertLocationToManifestPath(location);
				string loadPath = BundleServices.GetAssetBundleLoadPath(manifestPath);
				loader = CreateFileLoaderInternal(loadPath, manifestPath);
			}
			else
			{
				throw new NotImplementedException($"{AssetSystemMode}");
			}
			return loader;
		}
		internal AssetFileLoader CreateFileLoaderInternal(string loadPath, string manifestPath)
		{
			// 如果加载器已经存在
			AssetFileLoader loader = TryGetFileLoader(loadPath);
			if (loader != null)
			{
				loader.Reference(); //引用计数
				return loader;
			}

			// 创建加载器
			AssetFileLoader newLoader = null;
			if (AssetSystemMode == EAssetSystemMode.EditorMode)
				newLoader = new AssetDatabaseFileLoader(loadPath);
			else if (AssetSystemMode == EAssetSystemMode.ResourcesMode)
				newLoader = new AssetResourcesFileLoader(loadPath);
			else if (AssetSystemMode == EAssetSystemMode.BundleMode)
				newLoader = new AssetBundleFileLoader(loadPath, manifestPath);
			else
				throw new NotImplementedException($"{AssetSystemMode}");

			// 新增下载需求
			_fileLoaders.Add(newLoader);
			newLoader.Reference(); //引用计数
			return newLoader;
		}

		/// <summary>
		/// 资源回收
		/// 卸载引用计数为零的资源
		/// </summary>
		public void Release()
		{
			for (int i = _fileLoaders.Count - 1; i >= 0; i--)
			{
				AssetFileLoader loader = _fileLoaders[i];
				if (loader.IsDone() && loader.RefCount <= 0)
				{
					loader.Destroy(true);
					_fileLoaders.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// 强制回收所有资源
		/// </summary>
		public void ForceReleaseAll()
		{
			for (int i = 0; i < _fileLoaders.Count; i++)
			{
				AssetFileLoader loader = _fileLoaders[i];
				loader.Destroy(true);
			}
			_fileLoaders.Clear();

			// 释放所有资源
			Resources.UnloadUnusedAssets();
		}

		/// <summary>
		/// 从列表里获取加载器
		/// </summary>
		private AssetFileLoader TryGetFileLoader(string loadPath)
		{
			AssetFileLoader loader = null;
			for (int i = 0; i < _fileLoaders.Count; i++)
			{
				AssetFileLoader temp = _fileLoaders[i];
				if (temp.LoadPath.Equals(loadPath))
				{
					loader = temp;
					break;
				}
			}
			return loader;
		}

		#region 调试专属方法
		public int DebugGetFileLoaderCount()
		{
			return _fileLoaders.Count;
		}
		public int DebugGetFileLoaderFailedCount()
		{
			int count = 0;
			for (int i = 0; i < _fileLoaders.Count; i++)
			{
				AssetFileLoader temp = _fileLoaders[i];
				if (temp.States == EAssetFileLoaderStates.LoadAssetFileFailed || temp.GetFailedProviderCount() > 0)
					count++;
			}
			return count;
		}
		public List<AssetFileLoader> DebugGetAllLoaders()
		{
			return _fileLoaders;
		}
		#endregion
	}
}