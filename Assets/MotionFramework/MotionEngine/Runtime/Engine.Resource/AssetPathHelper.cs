//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.IO;
using MotionFramework.Patch;
using MotionFramework.IO;
using UnityEngine;

namespace MotionFramework.Resource
{
	public static class AssetPathHelper
	{
		/// <summary>
		/// 获取规范的路径
		/// </summary>
		public static string GetRegularPath(string path)
		{
			return path.Replace('\\', '/').Replace("\\", "/"); //替换为Linux路径格式
		}

		/// <summary>
		/// 获取基于流文件夹的加载路径
		/// </summary>
		public static string MakeStreamingLoadPath(string path)
		{
			return StringFormat.Format("{0}/{1}", Application.streamingAssetsPath, path);
		}

		/// <summary>
		/// 获取基于沙盒文件夹的加载路径
		/// </summary>
		public static string MakePersistentLoadPath(string path)
		{
#if UNITY_EDITOR
			// 注意：为了方便调试查看，编辑器下把存储目录放到项目里
			string projectPath = Path.GetDirectoryName(Application.dataPath);
			projectPath = GetRegularPath(projectPath);
			return StringFormat.Format("{0}/Sandbox/{1}", projectPath, path);
#else
		return StringFormat.Format("{0}/Sandbox/{1}", Application.persistentDataPath, path);
#endif
		}

		/// <summary>
		/// 获取网络资源加载路径
		/// </summary>
		public static string ConvertToWWWPath(string path)
		{
			// 注意：WWW加载方式，必须要在路径前面加file://
#if UNITY_EDITOR
			return StringFormat.Format("file:///{0}", path);
#elif UNITY_IPHONE
			return StringFormat.Format("file://{0}", path);
#elif UNITY_ANDROID
			return path;
#elif UNITY_STANDALONE
			return StringFormat.Format("file:///{0}", path);
#endif
		}

		/// <summary>
		/// 把资源定位地址转换为Manifest路径
		/// </summary>
		private static string CachedManifestRootPath = null;
		public static string ConvertLocationToManifestPath(string location)
		{
			if (CachedManifestRootPath == null)
			{
				if (string.IsNullOrEmpty(AssetSystem.AssetRootPath))
					throw new System.Exception("Asset system root path is null or empty.");
				CachedManifestRootPath = AssetSystem.AssetRootPath.ToLower();
			}

			location = location.ToLower(); //转换为小写形式
			return StringFormat.Format("{0}/{1}{2}", CachedManifestRootPath, location, PatchDefine.StrBundleSuffixName);
		}
	}
}