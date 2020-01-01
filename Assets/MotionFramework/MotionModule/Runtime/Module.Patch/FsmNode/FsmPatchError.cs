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
	internal class FsmPatchError : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmPatchError(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.PatchError.ToString();
		}

		void IFsmNode.OnEnter()
		{
			PatchManager.SendPatchStatesChangeMsg(_system.Current());
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