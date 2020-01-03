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
	internal class FsmParseWebPatchFile : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmParseWebPatchFile(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.ParseWebPatchFile.ToString();
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

		private IEnumerator Download(ProcedureSystem system)
		{
			// 从网络上解析最新的补丁文件
			int newResourceVersion = PatchSystem.Instance.GameVersion.Revision;
			string url = PatchSystem.Instance.GetWebDownloadURL(newResourceVersion.ToString(), PatchDefine.PatchFileName);
			WebDataRequest download = new WebDataRequest(url);
			yield return download.DownLoad();

			// Check fatal
			if (download.States != EWebRequestStates.Succeed)
			{
				download.Dispose();
				system.Switch(EPatchStates.PatchError.ToString());
				yield break;
			}

			// 解析补丁文件
			PatchHelper.Log(ELogType.Log, $"Parse web patch file.");
			PatchSystem.Instance.ParseWebPatchFile(download.GetText());
			download.Dispose();
			system.SwitchNext();
		}
	}
}