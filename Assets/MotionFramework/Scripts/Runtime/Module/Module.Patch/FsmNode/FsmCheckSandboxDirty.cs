﻿//--------------------------------------------------
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
		private readonly PatchUpdater _patcher;
		public string Name { private set; get; }
		
		public FsmCheckSandboxDirty(PatchUpdater patcher)
		{
			_patcher = patcher;
			Name = EPatchStates.CheckSandboxDirty.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.CheckSandboxDirty);

			string appVersion = PatchManager.Instance.GetAPPVersion();
			string filePath = PatchHelper.GetSandboxStaticFilePath();

			// 记录APP版本信息到静态文件
			if (PatchHelper.CheckSandboxStaticFileExist() == false)
			{
				PatchHelper.Log(ELogLevel.Log, $"Create sandbox static file : {filePath}");
				PatchHelper.CreateFile(filePath, appVersion);
				_patcher.SwitchNext();
				return;
			}

			// 每次启动时比对APP版本号是否一致		
			string recordVersion = PatchHelper.ReadFile(filePath);

			// 如果记录的版本号不一致		
			if (recordVersion != appVersion)
			{
				PatchHelper.Log(ELogLevel.Warning, $"Sandbox is dirty, Record version is {recordVersion}, APP version is {appVersion}");
				PatchHelper.Log(ELogLevel.Warning, "Clear all sandbox files.");
				PatchHelper.ClearSandbox();
				_patcher.SwitchLast();
			}
			else
			{
				_patcher.SwitchNext();
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