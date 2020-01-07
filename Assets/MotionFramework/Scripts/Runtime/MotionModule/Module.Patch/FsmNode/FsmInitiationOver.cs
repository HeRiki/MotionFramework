﻿//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.AI;

namespace MotionFramework.Patch
{
	internal class FsmInitiationOver : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmInitiationOver(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.InitiationOver.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.InitiationOver);
		}
		void IFsmNode.OnUpdate()
		{
			// 初始化阶段结束之后，挂起流程系统
		}
		void IFsmNode.OnExit()
		{
		}
		void IFsmNode.OnHandleMessage(object msg)
		{
		}
	}
}