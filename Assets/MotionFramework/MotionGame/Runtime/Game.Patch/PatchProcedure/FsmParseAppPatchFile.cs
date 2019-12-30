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
	public class FsmParseAppPatchFile : FsmState
	{
		private ProcedureSystem _system;

		public FsmParseAppPatchFile(ProcedureSystem system) : base((int)EPatchStates.ParseAppPatchFile)
		{
			_system = system;
		}
		public override void Enter()
		{
			PatchManager.SendPatchStatesChangeMsg((EPatchStates)_system.Current());
			AppEngine.Instance.StartCoroutine(DownLoad(_system));
		}
		public override void Execute()
		{
		}
		public override void Exit()
		{
		}

		private IEnumerator DownLoad(ProcedureSystem system)
		{
			// 解析APP里的补丁文件
			string filePath = AssetPathHelper.MakeStreamingLoadPath(PatchDefine.StrPatchFileName);
			string url = AssetPathHelper.ConvertToWWWPath(filePath);
			WebDataRequest downloader = new WebDataRequest(url);
			yield return downloader.DownLoad();

			if (downloader.States == EWebLoadStates.Succeed)
			{
				PatchManager.Log(ELogType.Log, "Parse app patch file.");
				PatchManager.Instance.ParseAppPatchFile(downloader.GetText());
				downloader.Dispose();
				system.SwitchNext();
			}
			else
			{
				throw new System.Exception($"Fatal error : Failed download file : {url}");
			}
		}
	}
}