﻿//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.AI;
using MotionFramework.Resource;
using MotionFramework.Network;

namespace MotionFramework.Patch
{
	internal class FsmParseAppPatchFile : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmParseAppPatchFile(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.ParseAppPatchFile.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.ParseAppPatchFile);
			AppEngine.Instance.StartCoroutine(DownLoad());
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

		private IEnumerator DownLoad()
		{
			// 解析APP里的补丁文件
			string filePath = AssetPathHelper.MakeStreamingLoadPath(PatchDefine.PatchFileName);
			string url = AssetPathHelper.ConvertToWWWPath(filePath);
			WebDataRequest downloader = new WebDataRequest(url);
			yield return downloader.DownLoad();

			if (downloader.States == EWebRequestStates.Succeed)
			{
				PatchHelper.Log(ELogType.Log, "Parse app patch file.");
				PatchSystem.Instance.ParseAppPatchFile(downloader.GetText());
				downloader.Dispose();
				_system.SwitchNext();
			}
			else
			{
				throw new System.Exception($"Fatal error : Failed download file : {url}");
			}
		}
	}
}