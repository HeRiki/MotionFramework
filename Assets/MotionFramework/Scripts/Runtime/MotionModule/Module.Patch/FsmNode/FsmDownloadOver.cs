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
	internal class FsmDownloadOver : IFiniteStateNode
	{
		private PatchCenter _center;
		public string Name { private set; get; }

		public FsmDownloadOver(PatchCenter center)
		{
			_center = center;
			Name = EPatchStates.DownloadOver.ToString();
		}
		void IFiniteStateNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.DownloadOver);
		}
		void IFiniteStateNode.OnUpdate()
		{
		}
		void IFiniteStateNode.OnExit()
		{
		}
		void IFiniteStateNode.OnHandleMessage(object msg)
		{
		}
	}
}