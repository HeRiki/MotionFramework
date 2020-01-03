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
	internal class FsmDownloadWebFilesFinish : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmDownloadWebFilesFinish(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.DownloadWebFilesFinish.ToString();
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
			// 注意：等所有文件下载完毕后，再替换版本文件
			int newResourceVersion = PatchSystem.Instance.GameVersion.Revision;
			string url = PatchSystem.Instance.GetWebDownloadURL(newResourceVersion.ToString(), PatchDefine.PatchFileName);
			string savePath = AssetPathHelper.MakePersistentLoadPath(PatchDefine.PatchFileName);
			WebFileRequest download = new WebFileRequest(url, savePath);
			yield return download.DownLoad();

			if (download.States != EWebRequestStates.Succeed)
			{
				download.Dispose();
				system.Switch(EPatchStates.PatchError.ToString());
				yield break;
			}
			else
			{
				PatchHelper.Log(ELogType.Log, "Download web files is finish.");
				download.Dispose();
				system.SwitchNext();
			}
		}
	}
}