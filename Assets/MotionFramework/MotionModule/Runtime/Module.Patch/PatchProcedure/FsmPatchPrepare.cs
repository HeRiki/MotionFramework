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
	public class FsmPatchPrepare : FsmNode
	{
		private ProcedureSystem _system;

		public FsmPatchPrepare(ProcedureSystem system) : base((int)EPatchStates.PatchPrepare)
		{
			_system = system;
		}

		public override void OnEnter()
		{
			PatchManager.SendPatchStatesChangeMsg((EPatchStates)_system.Current());
		}
		public override void OnUpdate()
		{
			if (AssetSystem.Instance.AssetSystemMode == EAssetSystemMode.BundleMode)
				_system.SwitchNext();
			else
				_system.Switch((int)EPatchStates.PatchOver);
		}
		public override void OnExit()
		{
		}
	}
}