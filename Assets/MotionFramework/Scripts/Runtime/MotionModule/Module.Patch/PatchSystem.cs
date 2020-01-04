//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.AI;
using MotionFramework.Event;

namespace MotionFramework.Patch
{
	internal class PatchSystem
	{
		public readonly static PatchSystem Instance = new PatchSystem();

		private readonly ProcedureSystem _system = new ProcedureSystem();

		// 版本号
		public Version AppVersion { private set; get; }
		public Version GameVersion { private set; get; }

		// 补丁文件
		public PatchFile AppPatchFile { private set; get; }
		public PatchFile SandboxPatchFile { private set; get; }
		public PatchFile WebPatchFile { private set; get; }

		/// <summary>
		/// 下载列表
		/// </summary>
		public readonly List<PatchElement> DownloadList = new List<PatchElement>(1000);

		/// <summary>
		/// CDN服务器IP地址
		/// </summary>
		public string CDNServerIP { private set; get; }

		/// <summary>
		/// WEB服务器IP地址
		/// </summary>
		public string WebServerIP { private set; get; }


		public void Initialize(string cdnServerIP, string webServerIP)
		{
			CDNServerIP = cdnServerIP;
			WebServerIP = webServerIP;
			AppVersion = new Version(Application.version);
		}
		public void Start()
		{
			// 注意：按照先后顺序添加流程节点
			_system.AddNode(new FsmPrepareBegin(_system));
			_system.AddNode(new FsmCheckSandboxDirty(_system));
			_system.AddNode(new FsmParseAppPatchFile(_system));
			_system.AddNode(new FsmParseSandboxPatchFile(_system));
			_system.AddNode(new FsmPrepareEnd(_system));
			_system.AddNode(new FsmRequestGameVersion(_system));
			_system.AddNode(new FsmParseWebPatchFile(_system));
			_system.AddNode(new FsmGetDonwloadList(_system));
			_system.AddNode(new FsmDownloadWebFiles(_system));
			_system.AddNode(new FsmDownloadWebPatchFile(_system));
			_system.AddNode(new FsmPatchOver(_system));
			_system.Run();
		}
		public void Update()
		{
			_system.Update();
		}

		/// <summary>
		/// 创建游戏版本号
		/// </summary>
		public void CreateGameVesion(string version)
		{
			if (GameVersion != null)
				throw new Exception("Should never get here.");
			GameVersion = new Version(version);
		}

		/// <summary>
		/// 修复客户端
		/// </summary>
		public void FixClient()
		{
			// 清空缓存
			PatchHelper.ClearSandbox();

			// 重启游戏
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}

		/// <summary>
		/// 获取网络文件下载地址
		/// </summary>
		public string GetWebDownloadURL(string version, string fileName)
		{
			if (Application.platform == RuntimePlatform.Android)
				return $"{CDNServerIP}/Android/{version}/{fileName}";
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
				return $"{CDNServerIP}/IPhone/{version}/{fileName}";
			else
				return $"{CDNServerIP}/Standalone/{version}/{fileName}";
		}

		/// <summary>
		/// 接收事件
		/// </summary>
		public void HandleEventMessage(IEventMessage msg)
		{
			if (msg is PatchEventMessageDefine.OperationEvent)
			{
				var message = msg as PatchEventMessageDefine.OperationEvent;
				if (message.operationType == EOperationType.BeginingRequestGameVersion)
				{
					if (_system.Current == EPatchStates.PrepareEnd.ToString())
						_system.SwitchNext();
					else
						AppLog.Log(ELogType.Error, $"Patch system is not prepare : {_system.Current}");
				}
				else if (message.operationType == EOperationType.BeginingDownloadWebFiles)
				{
					if (_system.Current == EPatchStates.GetDonwloadList.ToString())
						_system.SwitchNext();
					else
						AppLog.Log(ELogType.Error, $"Patch system is not prepare : {_system.Current}");
				}
				else if (message.operationType == EOperationType.TryRequestGameVersion)
				{
					if (_system.Current == EPatchStates.RequestGameVersion.ToString())
						_system.Switch(_system.Current);
					else
						AppLog.Log(ELogType.Error, $"Patch states is incorrect : {_system.Current}");
				}
				else if (message.operationType == EOperationType.TryDownloadPatchFile)
				{
					if (_system.Current == EPatchStates.DownloadWebPatchFile.ToString() || _system.Current == EPatchStates.ParseWebPatchFile.ToString())
						_system.Switch(_system.Current);
					else
						AppLog.Log(ELogType.Error, $"Patch states is incorrect : {_system.Current}");
				}
				else if (message.operationType == EOperationType.TryDownloadWebFile)
				{
					if (_system.Current == EPatchStates.DownloadWebFiles.ToString())
						_system.Switch(EPatchStates.GetDonwloadList.ToString());
					else
						AppLog.Log(ELogType.Error, $"Patch states is incorrect : {_system.Current}");
				}
				else
				{
					throw new NotImplementedException($"{message.operationType}");
				}
			}
		}

		// 解析补丁文件相关接口
		public void ParseAppPatchFile(string fileContent)
		{
			if (AppPatchFile != null)
				throw new Exception("Should never get here.");
			AppPatchFile = new PatchFile();
			AppPatchFile.Parse(fileContent);
		}
		public void ParseSandboxPatchFile(string fileContent)
		{
			if (SandboxPatchFile != null)
				throw new Exception("Should never get here.");
			SandboxPatchFile = new PatchFile();
			SandboxPatchFile.Parse(fileContent);
		}
		public void ParseSandboxPatchFile(PatchFile patchFile)
		{
			if (SandboxPatchFile != null)
				throw new Exception("Should never get here.");
			SandboxPatchFile = patchFile;
		}
		public void ParseWebPatchFile(string fileContent)
		{
			if (WebPatchFile != null)
				throw new Exception("Should never get here.");
			WebPatchFile = new PatchFile();
			WebPatchFile.Parse(fileContent);
		}
	}
}