//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.AI;
using MotionFramework.Resource;
using MotionFramework.Network;

namespace MotionFramework.Patch
{
	internal class FsmDownloadWebPatchManifest : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmDownloadWebPatchManifest(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.DownloadWebPatchManifest.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.DownloadWebPatchManifest);
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
			// 注意：等所有文件下载完毕后，下载并替换补丁清单
			int newResourceVersion = PatchSystem.Instance.RequestedResourceVersion;
			string url = PatchSystem.Instance.GetWebDownloadURL(newResourceVersion.ToString(), PatchDefine.PatchManifestFileName);
			string savePath = AssetPathHelper.MakePersistentLoadPath(PatchDefine.PatchManifestFileName);
			WebFileRequest download = new WebFileRequest(url, savePath);
			yield return download.DownLoad();

			if (download.States != EWebRequestStates.Succeed)
			{
				download.Dispose();
				PatchEventDispatcher.SendWebPatchManifestDownloadFailedMsg();
				yield break;
			}
			else
			{
				PatchHelper.Log(ELogType.Log, "Web patch manifest is download.");
				download.Dispose();
				_system.SwitchNext();
			}
		}
	}
}