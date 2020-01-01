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
	public class FsmParseWebPatchFile : FsmNode
	{
		private ProcedureSystem _system;

		public FsmParseWebPatchFile(ProcedureSystem system) : base((int)EPatchStates.ParseWebPatchFile)
		{
			_system = system;
		}
		public override void OnEnter()
		{
			PatchManager.SendPatchStatesChangeMsg((EPatchStates)_system.Current());
			AppEngine.Instance.StartCoroutine(Download(_system));
		}
		public override void OnUpdate()
		{
		}
		public override void OnExit()
		{
		}

		/// <summary>
		/// 第五阶段
		/// </summary>
		public IEnumerator Download(ProcedureSystem system)
		{
			// 从网络上解析最新的补丁文件
			int newResourceVersion = PatchManager.Instance.GameVersion.Revision;
			string url = PatchManager.Instance.MakeWebDownloadURL(newResourceVersion.ToString(), PatchDefine.StrPatchFileName);
			WebDataRequest download = new WebDataRequest(url);
			yield return download.DownLoad();

			// Check fatal
			if (download.States != EWebRequestStates.Succeed)
			{
				download.Dispose();
				system.Switch((int)EPatchStates.PatchError);
				yield break;
			}

			// 解析补丁文件
			PatchManager.Log(ELogType.Log, $"Parse web patch file.");
			PatchManager.Instance.ParseWebPatchFile(download.GetText());
			download.Dispose();
			system.SwitchNext();
		}
	}
}