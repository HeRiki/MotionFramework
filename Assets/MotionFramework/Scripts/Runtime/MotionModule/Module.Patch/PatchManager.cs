//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using MotionFramework.Resource;
using MotionFramework.AI;
using MotionFramework.Event;

namespace MotionFramework.Patch
{
	public sealed class PatchManager : ModuleSingleton<PatchManager>, IMotionModule, IBundleServices
	{
		/// <summary>
		/// 游戏模块创建参数
		/// </summary>
		public class CreateParameters
		{
			/// <summary>
			/// CDN服务器IP地址
			/// </summary>
			public string CDNServerIP;

			/// <summary>
			/// Web服务器IP地址
			/// </summary>
			public string WebServerIP;

			/// <summary>
			/// 是否跳过CDN服务器
			/// </summary>
			public bool SkipCDN;
		}


		void IMotionModule.OnCreate(System.Object param)
		{
			CreateParameters createParam = param as CreateParameters;
			if (createParam == null)
				throw new Exception($"{nameof(PatchManager)} create param is invalid.");

			PatchSystem.Instance.Initialize(createParam.CDNServerIP, createParam.WebServerIP, createParam.SkipCDN);
		}
		void IMotionModule.OnStart()
		{
			PatchSystem.Instance.Start();
		}
		void IMotionModule.OnUpdate()
		{
			PatchSystem.Instance.Update();
		}
		void IMotionModule.OnGUI()
		{
		}

		/// <summary>
		/// 获取APP版本号
		/// </summary>
		public string GetAPPVersion()
		{
			if (PatchSystem.Instance.AppVersion == null)
				return "0.0.0.0";
			return PatchSystem.Instance.AppVersion.ToString();
		}

		/// <summary>
		/// 获取游戏版本号
		/// </summary>
		public string GetGameVersion()
		{
			if (PatchSystem.Instance.GameVersion == null)
				return "0.0.0.0";
			return PatchSystem.Instance.GameVersion.ToString();
		}

		/// <summary>
		/// 修复客户端
		/// </summary>
		public void FixClient()
		{
			PatchSystem.Instance.FixClient();
		}

		#region IBundleServices接口
		private AssetBundleManifest _manifest;
		private AssetBundleManifest LoadManifest()
		{
			IBundleServices bundleServices = this as IBundleServices;
			string loadPath = bundleServices.GetAssetBundleLoadPath(PatchDefine.ManifestFileName);
			AssetBundle bundle = AssetBundle.LoadFromFile(loadPath);
			if (bundle == null)
				return null;

			AssetBundleManifest result = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
			bundle.Unload(false);
			return result;
		}
		string IBundleServices.GetAssetBundleLoadPath(string manifestPath)
		{
			PatchFile patchFile;
			if (PatchSystem.Instance.WebPatchFile != null)
				patchFile = PatchSystem.Instance.WebPatchFile;
			else
				patchFile = PatchSystem.Instance.SandboxPatchFile;

			// 注意：可能从APP内加载，也可能从沙盒内加载
			PatchElement element;
			if (patchFile.Elements.TryGetValue(manifestPath, out element))
			{
				// 先查询APP内的资源
				PatchElement appElement;
				if (PatchSystem.Instance.AppPatchFile.Elements.TryGetValue(manifestPath, out appElement))
				{
					if (appElement.MD5 == element.MD5)
						return AssetPathHelper.MakeStreamingLoadPath(manifestPath);
				}

				// 如果APP里不存在或者MD5不匹配，则从沙盒里加载
				return AssetPathHelper.MakePersistentLoadPath(manifestPath);
			}
			else
			{
				PatchHelper.Log(ELogType.Warning, $"Not found bundle in package : {manifestPath}");
				return AssetPathHelper.MakeStreamingLoadPath(manifestPath);
			}
		}
		string[] IBundleServices.GetDirectDependencies(string assetBundleName)
		{
			if (_manifest == null)
				_manifest = LoadManifest();
			return _manifest.GetDirectDependencies(assetBundleName);
		}
		string[] IBundleServices.GetAllDependencies(string assetBundleName)
		{
			if (_manifest == null)
				_manifest = LoadManifest();
			return _manifest.GetAllDependencies(assetBundleName);
		}
		#endregion
	}
}