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
	internal class FsmParseWebPatchManifest : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmParseWebPatchManifest(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.ParseWebPatchManifest.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.ParseWebPatchManifest);
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

		private IEnumerator Download()
		{
			// 从网络上解析最新的补丁清单
			int newResourceVersion = PatchSystem.Instance.RequestedResourceVersion;
			string url = PatchSystem.Instance.GetWebDownloadURL(newResourceVersion.ToString(), PatchDefine.PatchManifestFileName);
			WebDataRequest download = new WebDataRequest(url);
			yield return download.DownLoad();

			// Check fatal
			if (download.States != EWebRequestStates.Succeed)
			{
				download.Dispose();
				PatchEventDispatcher.SendWebPatchManifestDownloadFailedMsg();
				yield break;
			}

			PatchHelper.Log(ELogType.Log, $"Parse web patch manifest.");
			PatchSystem.Instance.ParseWebPatchManifest(download.GetText());
			download.Dispose();
			_system.SwitchNext();
		}
	}
}