//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.FSM;
using MotionFramework.Network;

namespace MotionFramework.Patch
{
	internal class FsmRequestGameVersion : IFiniteStateNode
	{
		private PatchCenter _center;
		public string Name { private set; get; }

		public FsmRequestGameVersion(PatchCenter center)
		{
			_center = center;
			Name = EPatchStates.RequestGameVersion.ToString();
		}
		void IFiniteStateNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.RequestGameVersion);
			MotionEngine.StartCoroutine(Download());
		}
		void IFiniteStateNode.OnUpdate()
		{
		}
		void IFiniteStateNode.OnExit()
		{
		}
		void IFiniteStateNode.OnHandleMessage(object msg)
		{
		}

		public IEnumerator Download()
		{
			// 获取最新的游戏版本号
			{
				string url = _center.GetWebServerIP();
				string post = _center.GetWebPostData();
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
				_center.ParseResponseData(responseData);
				download.Dispose();
			}

			int newResourceVersion = _center.RequestedResourceVersion;
			int oldResourceVersion = _center.SandboxPatchManifest.Version;

			// 检测强更安装包
			string appInstallURL = _center.GetForceInstallAppURL();
			if(string.IsNullOrEmpty(appInstallURL) == false)
			{
				PatchHelper.Log(ELogType.Log, $"Found new APP can be install : {_center.GameVersion.ToString()}");
				PatchEventDispatcher.SendFoundForceInstallAPPMsg(_center.GameVersion.ToString(), appInstallURL);
				yield break;
			}

			// 检测资源版本是否变化
			if (newResourceVersion == oldResourceVersion)
			{
				PatchHelper.Log(ELogType.Log, $"Resource version is not change.");
				_center.Switch(EPatchStates.DownloadOver.ToString());
			}
			else
			{
				PatchHelper.Log(ELogType.Log, $"Resource version is change : {oldResourceVersion} -> {newResourceVersion}");
				_center.SwitchNext();
			}
		}
	}
}