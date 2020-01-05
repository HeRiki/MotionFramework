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
	internal class FsmParseSandboxPatchManifest : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmParseSandboxPatchManifest(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.ParseSandboxPatchManifest.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.ParseSandboxPatchManifest);

			// 读取并解析沙盒内的补丁清单
			if (PatchHelper.CheckSandboxPatchManifestFileExist())
			{
				string filePath = AssetPathHelper.MakePersistentLoadPath(PatchDefine.PatchManifestFileName);
				string fileContent = PatchHelper.ReadFile(filePath);

				PatchHelper.Log(ELogType.Log, $"Parse sandbox patch file.");
				PatchSystem.Instance.ParseSandboxPatchManifest(fileContent);
			}
			else
			{
				PatchSystem.Instance.ParseSandboxPatchManifest(PatchSystem.Instance.AppPatchManifest);
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