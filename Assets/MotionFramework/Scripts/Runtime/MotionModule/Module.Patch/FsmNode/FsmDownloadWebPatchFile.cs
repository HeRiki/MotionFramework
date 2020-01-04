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
	internal class FsmDownloadWebPatchFile : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmDownloadWebPatchFile(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.DownloadWebPatchFile.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.DownloadWebPatchFile);
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
			// 注意：等所有文件下载完毕后，再替换补丁文件
			int newResourceVersion = PatchSystem.Instance.GameVersion.Revision;
			string url = PatchSystem.Instance.GetWebDownloadURL(newResourceVersion.ToString(), PatchDefine.PatchFileName);
			string savePath = AssetPathHelper.MakePersistentLoadPath(PatchDefine.PatchFileName);
			WebFileRequest download = new WebFileRequest(url, savePath);
			yield return download.DownLoad();

			if (download.States != EWebRequestStates.Succeed)
			{
				download.Dispose();
				PatchEventDispatcher.SendPatchFileDownloadFailedMsg();
				yield break;
			}
			else
			{
				PatchHelper.Log(ELogType.Log, "Web patch file is download.");
				download.Dispose();
				_system.SwitchNext();
			}
		}
	}
}