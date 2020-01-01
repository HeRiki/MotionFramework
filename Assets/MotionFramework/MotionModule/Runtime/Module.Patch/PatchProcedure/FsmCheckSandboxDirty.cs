//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.AI;

namespace MotionFramework.Patch
{
	public class FsmCheckSandboxDirty : FsmNode
	{
		private ProcedureSystem _system;

		public FsmCheckSandboxDirty(ProcedureSystem system) : base((int)EPatchStates.CheckSandboxDirty)
		{
			_system = system;
		}
		public override void OnEnter()
		{
			PatchManager.SendPatchStatesChangeMsg((EPatchStates)_system.Current());

			string appVersion = PatchManager.Instance.GetAPPVersion();
			string filePath = PatchManager.GetSandboxStaticFilePath();

			// 记录APP版本信息到静态文件
			if (PatchManager.CheckSandboxStaticFileExist() == false)
			{
				PatchManager.Log(ELogType.Log, $"Create sandbox static file : {filePath}");
				PatchManager.CreateFile(filePath, appVersion);
				_system.SwitchNext();
				return;
			}

			// 每次启动时比对APP版本号是否一致		
			string recordVersion = PatchManager.ReadFile(filePath);

			//如果记录的版本号不一致		
			if (recordVersion != appVersion)
			{
				PatchManager.Log(ELogType.Log, $"Sandbox is dirty, Record version is {recordVersion}, APP version is {appVersion}");
				PatchManager.Log(ELogType.Log, "Clear all cached sandbox files.");
				PatchManager.ClearSandbox();
				_system.SwitchLast();
			}
			else
			{
				PatchManager.Log(ELogType.Log, $"Sandbox is not dirty, Record version is {recordVersion}, APP version is {appVersion}");
				_system.SwitchNext();
			}
		}
		public override void OnUpdate()
		{
		}
		public override void OnExit()
		{
		}
	}
}