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
	internal class FsmPatchPrepare : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmPatchPrepare(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.PatchPrepare.ToString();
		}

		void IFsmNode.OnEnter()
		{
			PatchManager.SendPatchStatesChangeMsg(_system.Current());
		}
		void IFsmNode.OnUpdate()
		{
			if (AssetSystem.Instance.AssetSystemMode == EAssetSystemMode.BundleMode)
				_system.SwitchNext();
			else
				_system.Switch(EPatchStates.PatchOver.ToString());
		}
		void IFsmNode.OnExit()
		{
		}
		void IFsmNode.OnHandleMessage(object msg)
		{
		}
	}
}