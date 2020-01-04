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
	internal class FsmParseSandboxPatchFile : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmParseSandboxPatchFile(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.ParseSandboxPatchFile.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.ParseSandboxPatchFile);

			// 读取并解析沙盒内的补丁文件
			if (PatchHelper.CheckSandboxPatchFileExist())
			{
				string filePath = AssetPathHelper.MakePersistentLoadPath(PatchDefine.PatchFileName);
				string fileContent = PatchHelper.ReadFile(filePath);

				// 解析补丁文件
				PatchHelper.Log(ELogType.Log, $"Parse sandbox patch file.");
				PatchSystem.Instance.ParseSandboxPatchFile(fileContent);
			}
			else
			{
				PatchSystem.Instance.ParseSandboxPatchFile(PatchSystem.Instance.AppPatchFile);
			}

			_system.SwitchNext();
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
	}
}