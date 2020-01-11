//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.FSM;

namespace MotionFramework.Patch
{
	internal class FsmCheckSandboxDirty : IFiniteStateNode
	{
		private PatchCenter _center;
		public string Name { private set; get; }
		
		public FsmCheckSandboxDirty(PatchCenter center)
		{
			_center = center;
			Name = EPatchStates.CheckSandboxDirty.ToString();
		}
		void IFiniteStateNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.CheckSandboxDirty);

			string appVersion = PatchManager.Instance.GetAPPVersion();
			string filePath = PatchHelper.GetSandboxStaticFilePath();

			// 记录APP版本信息到静态文件
			if (PatchHelper.CheckSandboxStaticFileExist() == false)
			{
				PatchHelper.Log(ELogType.Log, $"Create sandbox static file : {filePath}");
				PatchHelper.CreateFile(filePath, appVersion);
				_center.SwitchNext();
				return;
			}

			// 每次启动时比对APP版本号是否一致		
			string recordVersion = PatchHelper.ReadFile(filePath);

			// 如果记录的版本号不一致		
			if (recordVersion != appVersion)
			{
				PatchHelper.Log(ELogType.Warning, $"Sandbox is dirty, Record version is {recordVersion}, APP version is {appVersion}");
				PatchHelper.Log(ELogType.Warning, "Clear all sandbox files.");
				PatchHelper.ClearSandbox();
				_center.SwitchLast();
			}
			else
			{
				_center.SwitchNext();
			}
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