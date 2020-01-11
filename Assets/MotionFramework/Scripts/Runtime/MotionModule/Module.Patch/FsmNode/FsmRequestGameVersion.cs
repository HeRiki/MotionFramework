﻿//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.AI;
using MotionFramework.Network;

namespace MotionFramework.Patch
{
	internal class FsmRequestGameVersion : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmRequestGameVersion(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.RequestGameVersion.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.RequestGameVersion);
			AppEngine.Instance.StartCoroutine(Download());
		}
		void IFsmNode.OnUpdate()
		{
		}
		void IFsmNode.OnExit()
		{
		}
		void IFsmNode.OnHandleMessage(object msg)
		{
		}

		public IEnumerator Download()
		{
			// 获取最新的游戏版本号
			{
				string url = PatchSystem.Instance.GetWebServerIP();
				string post = PatchSystem.Instance.GetWebPostData();
				PatchHelper.Log(ELogType.Log, $"Request game version : {url} : {post}");
				WebPostRequest download = new WebPostRequest(url, post);
				yield return download.DownLoad();

				//Check fatal
				if (download.States != EWebRequestStates.Success)
				{
					download.Dispose();
					PatchEventDispatcher.SendGameVersionRequestFailedMsg();
					yield break;
				}

				string responseData = download.GetResponse();
				PatchSystem.Instance.ParseResponseData(responseData);
				download.Dispose();
			}

			int newResourceVersion = PatchSystem.Instance.RequestedResourceVersion;
			int oldResourceVersion = PatchSystem.Instance.SandboxPatchManifest.Version;

			// 检测强更安装包
			string appInstallURL = PatchSystem.Instance.GetForceInstallAppURL();
			if(string.IsNullOrEmpty(appInstallURL) == false)
			{
				PatchHelper.Log(ELogType.Log, $"Found new APP can be install : {PatchSystem.Instance.GameVersion.ToString()}");
				PatchEventDispatcher.SendFoundForceInstallAPPMsg(PatchSystem.Instance.GameVersion.ToString(), appInstallURL);
				yield break;
			}

			// 检测资源版本是否变化
			if (newResourceVersion == oldResourceVersion)
			{
				PatchHelper.Log(ELogType.Log, $"Resource version is not change.");
				_system.Switch(EPatchStates.DownloadOver.ToString());
			}
			else
			{
				PatchHelper.Log(ELogType.Log, $"Resource version is change : {oldResourceVersion} -> {newResourceVersion}");
				_system.SwitchNext();
			}
		}
	}
}