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
	internal class FsmPatchOver : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmPatchOver(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.PatchOver.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchManager.SendPatchStatesChangeMsg(_system.Current());
			PatchManager.SendPatchOverMsg();
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