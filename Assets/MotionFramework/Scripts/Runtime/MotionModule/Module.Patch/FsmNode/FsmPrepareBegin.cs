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
	internal class FsmPrepareBegin : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmPrepareBegin(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.PrepareBegin.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.PrepareBegin);
		}
		void IFsmNode.OnUpdate()
		{
			_system.SwitchNext();
		}
		void IFsmNode.OnExit()
		{
		}
		void IFsmNode.OnHandleMessage(object msg)
		{
		}
	}
}