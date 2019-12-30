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
	public class FsmDownloadWebFilesFinish : FsmState
	{
		private ProcedureSystem _system;

		public FsmDownloadWebFilesFinish(ProcedureSystem system) : base((int)EPatchStates.DownloadWebFilesFinish)
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
			// 注意：等所有文件下载完毕后，再替换版本文件
			int newResourceVersion = PatchManager.Instance.GameVersion.Revision;
			string url = PatchManager.MakeWebDownloadURL(newResourceVersion.ToString(), PatchDefine.StrPatchFileName);
			string savePath = AssetPathHelper.MakePersistentLoadPath(PatchDefine.StrPatchFileName);
			WebFileRequest download = new WebFileRequest(url, savePath);
			yield return download.DownLoad();
			
			if (download.States != EWebLoadStates.Succeed)
			{
				download.Dispose();
				system.Switch((int)EPatchStates.PatchError);
				yield break;
			}
			else
			{
				PatchManager.Log(ELogType.Log, "Download web files is finish.");
				download.Dispose();
				system.SwitchNext();
			}
		}
	}
}