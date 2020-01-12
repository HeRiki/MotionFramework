//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Resource;
using MotionFramework.Event;
using MotionFramework.Console;

namespace MotionFramework.Patch
{
	public sealed class PatchManager : ModuleSingleton<PatchManager>, IModule, IBundleServices
	{
		/// <summary>
		/// 游戏模块创建参数
		/// </summary>
		public class CreateParameters
		{
			/// <summary>
			/// 最近登录的服务器ID
			/// </summary>
			public int ServerID;
			
			/// <summary>
			/// 渠道ID
			/// </summary>
			public int ChannelID;

			/// <summary>
			/// 设备唯一ID
			/// </summary>
			public long DeviceID;

			/// <summary>
			/// 测试包标记
			/// </summary>
			public int TestFlag;

			/// <summary>
			/// WEB服务器地址
			/// </summary>
			public Dictionary<RuntimePlatform, string> WebServers;

			/// <summary>
			/// CDN服务器地址
			/// </summary>
			public Dictionary<RuntimePlatform, string> CDNServers;

			/// <summary>
			/// 默认的Web服务器地址
			/// </summary>
			public string DefaultWebServerIP;

			/// <summary>
			/// 默认的CDN服务器地址
			/// </summary>
			public string DefaultCDNServerIP;
		}

		private PatchUpdater _patchUpdater;
		private bool _isRun = false;


		void IModule.OnCreate(System.Object param)
		{
			CreateParameters createParam = param as CreateParameters;
			if (createParam == null)
				throw new Exception($"{nameof(PatchManager)} create param is invalid.");

			_patchUpdater = new PatchUpdater();
			_patchUpdater.Initialize(createParam);
		}
		void IModule.OnUpdate()
		{
			_patchUpdater.Update();
		}
		void IModule.OnGUI()
		{
			ConsoleGUI.Lable($"[{nameof(PatchManager)}] States : {_patchUpdater.CurrentStates}");
		}

		/// <summary>
		/// 运行补丁更新流程
		/// </summary>
		public void Run()
		{
			if(_isRun == false)
			{
				_isRun = true;
				_patchUpdater.Run();
			}
		}

		/// <summary>
		/// 修复客户端
		/// </summary>
		public void FixClient()
		{
			_patchUpdater.FixClient();
		}

		/// <summary>
		/// 获取APP版本号
		/// </summary>
		public string GetAPPVersion()
		{
			if (_patchUpdater.AppVersion == null)
				return "0.0.0.0";
			return _patchUpdater.AppVersion.ToString();
		}

		/// <summary>
		/// 获取游戏版本号
		/// </summary>
		public string GetGameVersion()
		{
			if (_patchUpdater.GameVersion == null)
				return "0.0.0.0";
			return _patchUpdater.GameVersion.ToString();
		}

		/// <summary>
		/// 接收事件
		/// </summary>
		public void HandleEventMessage(IEventMessage msg)
		{
			_patchUpdater.HandleEventMessage(msg);
		}

		/// <summary>
		/// 重新载入Unity清单
		/// 注意：在补丁更新结束之后，清单内容会发生变化。
		/// </summary>
		public void ReloadUnityManifest()
		{
			_unityManifest = LoadUnityManifest();
		}

		#region IBundleServices接口
		private AssetBundleManifest _unityManifest;
		private AssetBundleManifest LoadUnityManifest()
		{
			IBundleServices bundleServices = this as IBundleServices;
			string loadPath = bundleServices.GetAssetBundleLoadPath(PatchDefine.UnityManifestFileName);
			AssetBundle bundle = AssetBundle.LoadFromFile(loadPath);
			if (bundle == null)
				return null;

			AssetBundleManifest result = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
			bundle.Unload(false);
			return result;
		}
		string IBundleServices.GetAssetBundleLoadPath(string manifestPath)
		{
			PatchManifest patchManifest;
			if (_patchUpdater.WebPatchManifest != null)
				patchManifest = _patchUpdater.WebPatchManifest;
			else
				patchManifest = _patchUpdater.SandboxPatchManifest;

			// 注意：可能从APP内加载，也可能从沙盒内加载
			PatchElement element;
			if (patchManifest.Elements.TryGetValue(manifestPath, out element))
			{
				// 先查询APP内的资源
				PatchElement appElement;
				if (_patchUpdater.AppPatchManifest.Elements.TryGetValue(manifestPath, out appElement))
				{
					if (appElement.MD5 == element.MD5)
						return AssetPathHelper.MakeStreamingLoadPath(manifestPath);
				}

				// 如果APP里不存在或者MD5不匹配，则从沙盒里加载
				return AssetPathHelper.MakePersistentLoadPath(manifestPath);
			}
			else
			{
				PatchHelper.Log(ELogLevel.Warning, $"Not found element in patch manifest : {manifestPath}");
				return AssetPathHelper.MakeStreamingLoadPath(manifestPath);
			}
		}
		string[] IBundleServices.GetDirectDependencies(string assetBundleName)
		{
			if (_unityManifest == null)
				_unityManifest = LoadUnityManifest();
			return _unityManifest.GetDirectDependencies(assetBundleName);
		}
		string[] IBundleServices.GetAllDependencies(string assetBundleName)
		{
			if (_unityManifest == null)
				_unityManifest = LoadUnityManifest();
			return _unityManifest.GetAllDependencies(assetBundleName);
		}
		#endregion
	}
}