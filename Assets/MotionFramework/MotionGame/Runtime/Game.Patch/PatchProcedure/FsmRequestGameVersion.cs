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
	public class FsmRequestGameVersion : FsmState
	{
		private ProcedureSystem _system;

		public FsmRequestGameVersion(ProcedureSystem system) : base((int)EPatchStates.RequestGameVersion)
		{
			_system = system;
		}
		public override void Enter()
		{
			PatchManager.SendPatchStatesChangeMsg((EPatchStates)_system.Current());
			AppEngine.Instance.StartCoroutine(Download(_system));
		}
		public override void Execute()
		{
		}
		public override void Exit()
		{
		}

		public IEnumerator Download(ProcedureSystem system)
		{
			// 如果跳过CDN服务器
			if (PatchManager.Instance.SkipCDN)
			{
				PatchManager.Log(ELogType.Log, $"Skip CDN server !");
				system.Switch((int)EPatchStates.PatchOver);
				yield break;
			}

			// 获取最新的游戏版本号
			{
				string url = $"{PatchManager.Instance.StrWebServerIP}/GameVersion.php";
				PatchManager.Log(ELogType.Log, $"Request game version : {url}");
				WebPostRequest download = new WebPostRequest(url);
				download.PostContent = PatchManager.Instance.AppVersion.ToString();
				yield return download.DownLoad();

				//Check fatal
				if (download.States != EWebLoadStates.Succeed)
				{
					download.Dispose();
					system.Switch((int)EPatchStates.PatchError);
					yield break;
				}

				string version = download.GetResponse();
				PatchManager.Instance.InitGameVesion(version);
				download.Dispose();
			}

			int newResourceVersion = PatchManager.Instance.GameVersion.Revision;
			int oldResourceVersion = PatchManager.Instance.SandboxPatchFile.Version;

			// 检测是否需要重新下载安装包
			if (PatchManager.Instance.GameVersion.Major != PatchManager.Instance.AppVersion.Major || PatchManager.Instance.GameVersion.Minor != PatchManager.Instance.AppVersion.Minor)
			{
				PatchManager.Log(ELogType.Log, $"Found new APP can be install : {PatchManager.Instance.GameVersion.ToString()}");
				PatchManager.SendFoundNewAPPMsg(PatchManager.Instance.GameVersion.ToString());
				yield break;
			}

			// 检测是否需要下载热更文件
			if (newResourceVersion == oldResourceVersion)
			{
				PatchManager.Log(ELogType.Log, $"Not found file to download.");
				system.Switch((int)EPatchStates.PatchOver);
			}
			else
			{
				PatchManager.Log(ELogType.Log, $"Found new file to download : {newResourceVersion.ToString()}");
				system.SwitchNext();
			}
		}
	}
}