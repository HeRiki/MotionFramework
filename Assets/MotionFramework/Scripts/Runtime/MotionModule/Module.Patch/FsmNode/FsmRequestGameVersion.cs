//--------------------------------------------------
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
				string post = PatchSystem.Instance.AppVersion.ToString();
				PatchHelper.Log(ELogType.Log, $"Request game version : {url} : {post}");
				WebPostRequest download = new WebPostRequest(url, post);
				yield return download.DownLoad();

				//Check fatal
				if (download.States != EWebRequestStates.Succeed)
				{
					download.Dispose();
					PatchEventDispatcher.SendGameVersionRequestFailedMsg();
					yield break;
				}

				string version = download.GetResponse();
				PatchSystem.Instance.CreateGameVesion(version);
				download.Dispose();
			}

			int newResourceVersion = PatchSystem.Instance.RequestedResourceVersion;
			int oldResourceVersion = PatchSystem.Instance.SandboxPatchManifest.Version;

			// 检测是否需要重新下载安装包
			if (PatchSystem.Instance.GameVersion.Major != PatchSystem.Instance.AppVersion.Major || PatchSystem.Instance.GameVersion.Minor != PatchSystem.Instance.AppVersion.Minor)
			{
				PatchHelper.Log(ELogType.Log, $"Found new APP can be install : {PatchSystem.Instance.GameVersion.ToString()}");
				PatchEventDispatcher.SendFoundNewAPPMsg(PatchSystem.Instance.GameVersion.ToString());
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