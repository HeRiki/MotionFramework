//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.FSM;
using MotionFramework.Resource;

namespace MotionFramework.Patch
{
	internal class FsmParseSandboxPatchManifest : IFiniteStateNode
	{
		private PatchCenter _center;
		public string Name { private set; get; }

		public FsmParseSandboxPatchManifest(PatchCenter center)
		{
			_center = center;
			Name = EPatchStates.ParseSandboxPatchManifest.ToString();
		}
		void IFiniteStateNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.ParseSandboxPatchManifest);

			// 读取并解析沙盒内的补丁清单
			if (PatchHelper.CheckSandboxPatchManifestFileExist())
			{
				string filePath = AssetPathHelper.MakePersistentLoadPath(PatchDefine.PatchManifestFileName);
				string fileContent = PatchHelper.ReadFile(filePath);

				PatchHelper.Log(ELogType.Log, $"Parse sandbox patch file.");
				_center.ParseSandboxPatchManifest(fileContent);
			}
			else
			{
				_center.ParseSandboxPatchManifest(_center.AppPatchManifest);
			}

			_center.SwitchNext();
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
	}
}