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
	public class FsmPatchPrepare : FsmState
	{
		private ProcedureSystem _system;

		public FsmPatchPrepare(ProcedureSystem system) : base((int)EPatchStates.PatchPrepare)
		{
			_system = system;
		}

		public override void Enter()
		{
			PatchManager.SendPatchStatesChangeMsg((EPatchStates)_system.Current());
		}
		public override void Execute()
		{
			_system.SwitchNext();
		}
		public override void Exit()
		{
		}
	}
}