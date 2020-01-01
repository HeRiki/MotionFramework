//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using UnityEngine;
using MotionFramework.Debug;

namespace MotionFramework.Resource
{
	/// <summary>
	/// 资源管理器
	/// </summary>
	public sealed class ResourceManager : IModule
	{
		/// <summary>
		/// 游戏模块全局实例
		/// </summary>
		public static ResourceManager Instance { private set; get; }

		/// <summary>
		/// 游戏模块创建参数
		/// </summary>
		public class CreateParameters
		{
			/// <summary>
			/// 资源系统根路径
			/// </summary>
			public string AssetRootPath;

			/// <summary>
			/// 资源系统模式
			/// </summary>
			public EAssetSystemMode AssetSystemMode;	
		}


		void IModule.OnCreate(System.Object param)
		{
			CreateParameters createParam = param as CreateParameters;
			if (createParam == null)
				throw new Exception($"{nameof(ResourceManager)} create param is invalid.");

			AssetSystem.Instance.Initialize(createParam.AssetRootPath, createParam.AssetSystemMode);

			// 全局实例赋值
			Instance = this;
		}
		void IModule.OnStart()
		{
		}
		void IModule.OnUpdate()
		{
			AssetSystem.Instance.UpdatePoll();
		}
		void IModule.OnGUI()
		{
			int totalCount = AssetSystem.Instance.DebugGetFileLoaderCount();
			int failedCount = AssetSystem.Instance.DebugGetFileLoaderFailedCount();
			DebugConsole.GUILable($"[{nameof(ResourceManager)}] AssetSystemMode : {AssetSystem.Instance.AssetSystemMode}");
			DebugConsole.GUILable($"[{nameof(ResourceManager)}] Loader total count : {totalCount}");
			DebugConsole.GUILable($"[{nameof(ResourceManager)}] Loader failed count : {failedCount}");
		}

		/// <summary>
		/// 同步加载接口
		/// 注意：仅支持无依赖关系的资源
		/// </summary>
		public T SyncLoad<T>(string location) where T : UnityEngine.Object
		{
			UnityEngine.Object result = null;

			if (AssetSystem.Instance.AssetSystemMode == EAssetSystemMode.EditorMode)
			{
#if UNITY_EDITOR
				string loadPath = AssetPathHelper.FindDatabaseAssetPath(location);
				result = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(loadPath);
				if (result == null)
					Logger.Log(ELogType.Error, $"Failed to load {loadPath}");
#else
				throw new Exception("AssetDatabaseLoader only support unity editor.");
#endif
			}
			else if (AssetSystem.Instance.AssetSystemMode == EAssetSystemMode.ResourcesMode)
			{
				result = Resources.Load<T>(location);
				if (result == null)
					Logger.Log(ELogType.Error, $"Failed to load {location}");
			}
			else if (AssetSystem.Instance.AssetSystemMode == EAssetSystemMode.BundleMode)
			{
				if (AssetSystem.Instance.BundleServices == null)
					throw new Exception($"{nameof(AssetSystem.BundleServices)} is null.");

				string fileName = System.IO.Path.GetFileNameWithoutExtension(location);
				string manifestPath = AssetPathHelper.ConvertLocationToManifestPath(location);
				string loadPath = AssetSystem.Instance.BundleServices.GetAssetBundleLoadPath(manifestPath);
				AssetBundle bundle = AssetBundle.LoadFromFile(loadPath);
				if(bundle != null)
					result = bundle.LoadAsset<T>(fileName);
				if (result == null)
					Logger.Log(ELogType.Error, $"Failed to load {loadPath}");
				if(bundle != null)
					bundle.Unload(false);
			}
			else
			{
				throw new NotImplementedException($"{AssetSystem.Instance.AssetSystemMode}");
			}

			return result as T;
		}
	}
}