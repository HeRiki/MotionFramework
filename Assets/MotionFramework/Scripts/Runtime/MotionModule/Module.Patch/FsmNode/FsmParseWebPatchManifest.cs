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
	internal class FsmParseWebPatchManifest : IFiniteStateNode
	{
		private PatchCenter _center;
		public string Name { private set; get; }

		public FsmParseWebPatchManifest(PatchCenter center)
		{
			_center = center;
			Name = EPatchStates.ParseWebPatchManifest.ToString();
		}
		void IFiniteStateNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.ParseWebPatchManifest);
			AppEngine.Instance.StartCoroutine(Download());
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

		private IEnumerator Download()
		{
			// 从网络上解析最新的补丁清单
			int newResourceVersion = _center.RequestedResourceVersion;
			string url = _center.GetWebDownloadURL(newResourceVersion.ToString(), PatchDefine.PatchManifestFileName);
			WebDataRequest download = new WebDataRequest(url);
			yield return download.DownLoad();

			// Check fatal
			if (download.States != EWebRequestStates.Success)
			{
				download.Dispose();
				PatchEventDispatcher.SendWebPatchManifestDownloadFailedMsg();
				yield break;
			}

			PatchHelper.Log(ELogType.Log, $"Parse web patch manifest.");
			_center.ParseWebPatchManifest(download.GetText());
			download.Dispose();
			_center.SwitchNext();
		}
	}
}