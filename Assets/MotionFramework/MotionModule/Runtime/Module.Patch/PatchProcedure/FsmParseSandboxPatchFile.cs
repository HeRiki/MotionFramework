//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.AI;
using MotionFramework.Resource;

namespace MotionFramework.Patch
{
	public class FsmParseSandboxPatchFile : FsmNode
	{
		private ProcedureSystem _system;

		public FsmParseSandboxPatchFile(ProcedureSystem system) : base((int)EPatchStates.ParseSandboxPatchFile)
		{
			_system = system;
		}
		public override void OnEnter()
		{
			PatchManager.SendPatchStatesChangeMsg((EPatchStates)_system.Current());

			// 读取并解析沙盒内的补丁文件
			if (PatchManager.CheckSandboxPatchFileExist())
			{
				string filePath = AssetPathHelper.MakePersistentLoadPath(PatchDefine.StrPatchFileName);
				string fileContent = PatchManager.ReadFile(filePath);

				// 解析补丁文件
				PatchManager.Log(ELogType.Log, $"Parse sandbox patch file.");
				PatchManager.Instance.ParseSandboxPatchFile(fileContent);
			}
			else
			{
				PatchManager.Instance.ParseSandboxPatchFile(PatchManager.Instance.AppPatchFile);
			}

			_system.SwitchNext();
		}
		public override void OnUpdate()
		{
		}
		public override void OnExit()
		{
		}
	}
}