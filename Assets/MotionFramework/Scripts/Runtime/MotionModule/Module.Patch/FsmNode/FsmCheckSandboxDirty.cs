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
	internal class FsmCheckSandboxDirty : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }
		
		public FsmCheckSandboxDirty(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.CheckSandboxDirty.ToString();
		}

		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(_system.Current());

			string appVersion = PatchManager.Instance.GetAPPVersion();
			string filePath = PatchHelper.GetSandboxStaticFilePath();

			// 记录APP版本信息到静态文件
			if (PatchHelper.CheckSandboxStaticFileExist() == false)
			{
				PatchHelper.Log(ELogType.Log, $"Create sandbox static file : {filePath}");
				PatchHelper.CreateFile(filePath, appVersion);
				_system.SwitchNext();
				return;
			}

			// 每次启动时比对APP版本号是否一致		
			string recordVersion = PatchHelper.ReadFile(filePath);

			//如果记录的版本号不一致		
			if (recordVersion != appVersion)
			{
				PatchHelper.Log(ELogType.Log, $"Sandbox is dirty, Record version is {recordVersion}, APP version is {appVersion}");
				PatchHelper.Log(ELogType.Log, "Clear all cached sandbox files.");
				PatchHelper.ClearSandbox();
				_system.SwitchLast();
			}
			else
			{
				PatchHelper.Log(ELogType.Log, $"Sandbox is not dirty, Record version is {recordVersion}, APP version is {appVersion}");
				_system.SwitchNext();
			}
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