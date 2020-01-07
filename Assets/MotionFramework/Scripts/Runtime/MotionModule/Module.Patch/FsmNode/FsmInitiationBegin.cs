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
	internal class FsmInitiationBegin : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }
		private int _delayFrame = 1;

		public FsmInitiationBegin(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.InitiationBegin.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.InitiationBegin);
		}
		void IFsmNode.OnUpdate()
		{
			_delayFrame--;
			if (_delayFrame < 0)
			{
				_system.SwitchNext();
			}
		}
		void IFsmNode.OnExit()
		{
		}
		void IFsmNode.OnHandleMessage(object msg)
		{
		}
	}
}