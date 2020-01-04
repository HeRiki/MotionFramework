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
	internal class FsmPrepareEnd : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmPrepareEnd(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.PrepareEnd.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.PrepareEnd);
		}
		void IFsmNode.OnUpdate()
		{
			// 准备阶段结束之后，不再进行下面的流程
		}
		void IFsmNode.OnExit()
		{
		}
		void IFsmNode.OnHandleMessage(object msg)
		{
		}
	}
}