//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.AI;

namespace MotionFramework.Patch
{
	public class FsmPatchOver : FsmNode
	{
		private ProcedureSystem _system;

		public FsmPatchOver(ProcedureSystem system) : base((int)EPatchStates.PatchOver)
		{
			_system = system;
		}
		public override void OnEnter()
		{
			PatchManager.SendPatchStatesChangeMsg((EPatchStates)_system.Current());
			PatchManager.SendPatchOverMsg();
		}
		public override void OnUpdate()
		{
		}
		public override void OnExit()
		{
		}
	}
}