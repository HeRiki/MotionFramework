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
	internal class FsmInitiationOver : IFiniteStateNode
	{
		private PatchCenter _center;
		public string Name { private set; get; }

		public FsmInitiationOver(PatchCenter center)
		{
			_center = center;
			Name = EPatchStates.InitiationOver.ToString();
		}
		void IFiniteStateNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.InitiationOver);
		}
		void IFiniteStateNode.OnUpdate()
		{
			// 初始化阶段结束之后，挂起流程系统
		}
		void IFiniteStateNode.OnExit()
		{
		}
		void IFiniteStateNode.OnHandleMessage(object msg)
		{
		}
	}
}