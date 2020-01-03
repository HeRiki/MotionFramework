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
			PatchEventDispatcher.SendPatchStatesChangeMsg(_system.Current());
			AppEngine.Instance.StartCoroutine(Download(_system));
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

		public IEnumerator Download(ProcedureSystem system)
		{
			// 如果跳过CDN服务器
			if (PatchSystem.Instance.SkipCDN)
			{
				PatchHelper.Log(ELogType.Warning, $"Skip CDN server !");
				system.Switch(EPatchStates.PatchOver.ToString());
				yield break;
			}

			// 获取最新的游戏版本号
			{
				string url = $"{PatchSystem.Instance.WebServerIP}/GameVersion.php";
				string post = PatchSystem.Instance.AppVersion.ToString();
				PatchHelper.Log(ELogType.Log, $"Request game version : {url}");
				WebPostRequest download = new WebPostRequest(url, post);
				yield return download.DownLoad();

				//Check fatal
				if (download.States != EWebRequestStates.Succeed)
				{
					download.Dispose();
					system.Switch(EPatchStates.PatchError.ToString());
					yield break;
				}

				string version = download.GetResponse();
				PatchSystem.Instance.CreateGameVesion(version);
				download.Dispose();
			}

			int newResourceVersion = PatchSystem.Instance.GameVersion.Revision;
			int oldResourceVersion = PatchSystem.Instance.SandboxPatchFile.Version;

			// 检测是否需要重新下载安装包
			if (PatchSystem.Instance.GameVersion.Major != PatchSystem.Instance.AppVersion.Major || PatchSystem.Instance.GameVersion.Minor != PatchSystem.Instance.AppVersion.Minor)
			{
				PatchHelper.Log(ELogType.Log, $"Found new APP can be install : {PatchSystem.Instance.GameVersion.ToString()}");
				PatchEventDispatcher.SendFoundNewAPPMsg(PatchSystem.Instance.GameVersion.ToString());
				yield break;
			}

			// 检测是否需要下载热更文件
			if (newResourceVersion == oldResourceVersion)
			{
				PatchHelper.Log(ELogType.Log, $"Not found file to download.");
				system.Switch(EPatchStates.PatchOver.ToString());
			}
			else
			{
				PatchHelper.Log(ELogType.Log, $"Found new file to download : {newResourceVersion.ToString()}");
				system.SwitchNext();
			}
		}
	}
}