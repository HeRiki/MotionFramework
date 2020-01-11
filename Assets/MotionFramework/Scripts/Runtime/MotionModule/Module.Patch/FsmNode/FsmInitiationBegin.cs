//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.FSM;

namespace MotionFramework.Patch
{
	internal class FsmInitiationBegin : IFiniteStateNode
	{
		private PatchCenter _center;
		public string Name { private set; get; }

		public FsmInitiationBegin(PatchCenter center)
		{
			_center = center;
			Name = EPatchStates.InitiationBegin.ToString();
		}
		void IFiniteStateNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.InitiationBegin);
		}
		void IFiniteStateNode.OnUpdate()
		{
			_center.SwitchNext();
		}
		void IFiniteStateNode.OnExit()
		{
		}
		void IFiniteStateNode.OnHandleMessage(object msg)
		{
		}
	}
}