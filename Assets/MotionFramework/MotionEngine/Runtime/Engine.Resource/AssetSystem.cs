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
	public static class AssetSystem
	{
		/// <summary>
		/// 加载器集合
		/// </summary>
		private static readonly List<AssetFileLoader> _fileLoaders = new List<AssetFileLoader>(1000);

		/// <summary>
		/// 资源卸载辅助集合
		/// </summary>
		private static readonly List<string> _removeKeys = new List<string>(100);


		/// <summary>
		/// 资源系统根路径
		/// </summary>
		public static string AssetRootPath { set; get; }

		/// <summary>
		/// 资源系统模式
		/// </summary>
		public static EAssetSystemMode SystemMode { set; get; }

		/// <summary>
		/// AssetBundle服务接口（BundleMode模式下需要设置该接口）
		/// </summary>
		public static IBundleServices BundleServices { set; get; }


		/// <summary>
		/// 轮询更新
		/// </summary>
		public static void UpdatePoll()
		{
			for (int i = 0; i < _fileLoaders.Count; i++)
			{
				_fileLoaders[i].Update();
			}
		}

		/// <summary>
		/// 创建资源文件加载器
		/// </summary>
		public static AssetFileLoader CreateFileLoader(string location)
		{
			AssetFileLoader loader;
			if (SystemMode == EAssetSystemMode.EditorMode)
			{
#if UNITY_EDITOR
				string loadPath = FindDatabaseAssetPath(location);
				loader = CreateFileLoaderInternal(loadPath, null);
#else
				throw new Exception("EAssetSystemMode.EditorMode only support unity editor.");
#endif
			}
			else if (SystemMode == EAssetSystemMode.ResourcesMode)
			{
				string loadPath = location;
				loader = CreateFileLoaderInternal(loadPath, null);
			}
			else if (SystemMode == EAssetSystemMode.BundleMode)
			{
				if (BundleServices == null)
					throw new Exception($"{nameof(IBundleServices)} is null.");

				string manifestPath = AssetPathHelper.ConvertLocationToManifestPath(location);
				string loadPath = BundleServices.GetAssetBundleLoadPath(manifestPath);
				loader = CreateFileLoaderInternal(loadPath, manifestPath);
			}
			else
			{
				throw new NotImplementedException($"{SystemMode}");
			}
			return loader;
		}
		internal static AssetFileLoader CreateFileLoaderInternal(string loadPath, string manifestPath)
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
			if (SystemMode == EAssetSystemMode.EditorMode)
				newLoader = new AssetDatabaseFileLoader(loadPath);
			else if (SystemMode == EAssetSystemMode.ResourcesMode)
				newLoader = new AssetResourcesFileLoader(loadPath);
			else if (SystemMode == EAssetSystemMode.BundleMode)
				newLoader = new AssetBundleFileLoader(loadPath, manifestPath);
			else
				throw new NotImplementedException($"{SystemMode}");

			// 新增下载需求
			_fileLoaders.Add(newLoader);
			newLoader.Reference(); //引用计数
			return newLoader;
		}

		/// <summary>
		/// 资源回收
		/// 卸载引用计数为零的资源
		/// </summary>
		public static void Release()
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
		public static void ForceReleaseAll()
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
		/// 获取AssetDatabase的加载路径
		/// </summary>
		public static string FindDatabaseAssetPath(string location)
		{
#if UNITY_EDITOR
			// 如果定位地址的资源是一个文件夹
			string path = $"{AssetSystem.AssetRootPath}/{location}";
			if (UnityEditor.AssetDatabase.IsValidFolder(path))
				return path;

			string fileName = Path.GetFileName(path);
			string folderPath = Path.GetDirectoryName(path);
			string assetPath = FindDatabaseAssetPath(folderPath, fileName);
			if (string.IsNullOrEmpty(assetPath))
				return path;
			return assetPath;
#else
			throw new Exception($"AssetSystem.FindDatabaseAssetPath method only support unity editor.");
#endif
		}

		/// <summary>
		/// 获取AssetDatabase的加载路径
		/// </summary>
		public static string FindDatabaseAssetPath(string folderPath, string fileName)
		{
#if UNITY_EDITOR
			// AssetDatabase加载资源需要提供文件后缀格式，然而资源定位地址并没有文件格式信息。
			// 所以我们通过查找该文件所在文件夹内同名的首个文件来确定AssetDatabase的加载路径。
			string[] guids = UnityEditor.AssetDatabase.FindAssets(string.Empty, new[] { folderPath });
			for (int i = 0; i < guids.Length; i++)
			{
				string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
				string assetName = Path.GetFileNameWithoutExtension(assetPath);
				if (assetName == fileName)
					return assetPath;
			}
#endif
			// 没有找到同名的资源文件
			return string.Empty;
		}

		// 从列表里获取加载器
		private static AssetFileLoader TryGetFileLoader(string loadPath)
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
		public static int DebugGetFileLoaderCount()
		{
			return _fileLoaders.Count;
		}
		public static int DebugGetFileLoaderFailedCount()
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
		public static List<AssetFileLoader> DebugAllLoaders()
		{
			return _fileLoaders;
		}
		#endregion
	}
}