//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.FSM;
using MotionFramework.Resource;
using MotionFramework.Network;

namespace MotionFramework.Patch
{
	internal class FsmDownloadWebPatchManifest : IFiniteStateNode
	{
		private PatchCenter _center;
		public string Name { private set; get; }

		public FsmDownloadWebPatchManifest(PatchCenter center)
		{
			_center = center;
			Name = EPatchStates.DownloadWebPatchManifest.ToString();
		}
		void IFiniteStateNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.DownloadWebPatchManifest);
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
			// 注意：等所有文件下载完毕后，下载并替换补丁清单
			int newResourceVersion = _center.RequestedResourceVersion;
			string url = _center.GetWebDownloadURL(newResourceVersion.ToString(), PatchDefine.PatchManifestFileName);
			string savePath = AssetPathHelper.MakePersistentLoadPath(PatchDefine.PatchManifestFileName);
			WebFileRequest download = new WebFileRequest(url, savePath);
			yield return download.DownLoad();

			if (download.States != EWebRequestStates.Success)
			{
				download.Dispose();
				PatchEventDispatcher.SendWebPatchManifestDownloadFailedMsg();
				yield break;
			}
			else
			{
				PatchHelper.Log(ELogType.Log, "Web patch manifest is download.");
				download.Dispose();
				_center.SwitchNext();
			}
		}
	}
}