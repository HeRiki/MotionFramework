//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using UnityEngine;
using MotionFramework.Console;
using MotionFramework.Utility;

namespace MotionFramework.Resource
{
	/// <summary>
	/// 资源管理器
	/// </summary>
	public sealed class ResourceManager : ModuleSingleton<ResourceManager>, IModule
	{
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

			/// <summary>
			/// AssetBundle服务接口
			/// </summary>
			public IBundleServices BundleServices;

			/// <summary>
			/// 资源系统自动释放零引用资源的间隔秒数
			/// 注意：如果小于等于零代表不自动释放，可以使用ResourceManager.Release接口主动释放
			/// </summary>
			public float AutoReleaseInterval;
		}

		private RepeatTimer _releaseTimer;


		void IModule.OnCreate(System.Object param)
		{
			CreateParameters createParam = param as CreateParameters;
			if (createParam == null)
				throw new Exception($"{nameof(ResourceManager)} create param is invalid.");

			AssetSystem.Initialize(createParam.AssetRootPath, createParam.AssetSystemMode, createParam.BundleServices);

			// 创建间隔计时器
			if(createParam.AutoReleaseInterval > 0)
				_releaseTimer = new RepeatTimer(0, createParam.AutoReleaseInterval);
		}
		void IModule.OnUpdate()
		{
			// 轮询更新资源系统
			AssetSystem.UpdatePoll();

			// 自动释放零引用资源
			if(_releaseTimer != null && _releaseTimer.Update(Time.unscaledDeltaTime))
			{
				AssetSystem.Release();
			}			
		}
		void IModule.OnGUI()
		{
			int totalCount = AssetSystem.GetFileLoaderCount();
			int failedCount = AssetSystem.GetFileLoaderFailedCount();
			ConsoleGUI.Lable($"[{nameof(ResourceManager)}] AssetSystemMode : {AssetSystem.AssetSystemMode}");
			ConsoleGUI.Lable($"[{nameof(ResourceManager)}] Loader total count : {totalCount}");
			ConsoleGUI.Lable($"[{nameof(ResourceManager)}] Loader failed count : {failedCount}");
		}

		/// <summary>
		/// 资源回收
		/// 卸载引用计数为零的资源
		/// </summary>
		public static void Release()
		{
			AssetSystem.Release();
		}

		/// <summary>
		/// 强制回收所有资源
		/// </summary>
		public void ForceReleaseAll()
		{
			AssetSystem.ForceReleaseAll();
		}

		/// <summary>
		/// 同步加载接口
		/// 注意：仅支持无依赖关系的资源
		/// </summary>
		public T SyncLoad<T>(string location) where T : UnityEngine.Object
		{
			UnityEngine.Object result = null;

			if (AssetSystem.AssetSystemMode == EAssetSystemMode.AssetDatabase)
			{
#if UNITY_EDITOR
				string loadPath = AssetPathHelper.FindDatabaseAssetPath(location);
				result = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(loadPath);
				if (result == null)
					MotionLog.Log(ELogLevel.Error, $"Failed to load {loadPath}");
#else
				throw new Exception("EAssetSystemMode.EditorMode only support unity editor.");
#endif
			}
			else if (AssetSystem.AssetSystemMode == EAssetSystemMode.Resources)
			{
				result = Resources.Load<T>(location);
				if (result == null)
					MotionLog.Log(ELogLevel.Error, $"Failed to load {location}");
			}
			else if (AssetSystem.AssetSystemMode == EAssetSystemMode.AssetBundle)
			{
				if (AssetSystem.BundleServices == null)
					throw new Exception($"{nameof(AssetSystem.BundleServices)} is null.");

				string fileName = System.IO.Path.GetFileNameWithoutExtension(location);
				string manifestPath = AssetPathHelper.ConvertLocationToManifestPath(location);
				string loadPath = AssetSystem.BundleServices.GetAssetBundleLoadPath(manifestPath);
				AssetBundle bundle = AssetBundle.LoadFromFile(loadPath);
				if(bundle != null)
					result = bundle.LoadAsset<T>(fileName);
				if (result == null)
					MotionLog.Log(ELogLevel.Error, $"Failed to load {loadPath}");
				if(bundle != null)
					bundle.Unload(false);
			}
			else
			{
				throw new NotImplementedException($"{AssetSystem.AssetSystemMode}");
			}

			return result as T;
		}
	}
}