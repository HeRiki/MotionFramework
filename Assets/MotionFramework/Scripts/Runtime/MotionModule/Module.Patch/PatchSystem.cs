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
		private Dictionary<RuntimePlatform, string> _webServers;
		private Dictionary<RuntimePlatform, string> _cdnServers;
		private string _defaultWebServer;
		private string _defaultCDNServer;
		private int _serverID;
		private int _channelID;
		private long _deviceID;
		private int _testFlag;
		private string _forceInstallAppURL;

		// 版本号
		public Version AppVersion { private set; get; }
		public Version GameVersion { private set; get; }

		// 补丁清单
		public PatchManifest AppPatchManifest { private set; get; }
		public PatchManifest SandboxPatchManifest { private set; get; }
		public PatchManifest WebPatchManifest { private set; get; }

		/// <summary>
		/// 下载列表
		/// </summary>
		public readonly List<PatchElement> DownloadList = new List<PatchElement>(1000);

		/// <summary>
		/// 当前运行的状态
		/// </summary>
		public string CurrentStates
		{
			get
			{
				return _system.Current;
			}
		}

		/// <summary>
		/// 向WEB服务器请求的资源版本号
		/// </summary>
		public int RequestedResourceVersion
		{
			get
			{
				if (GameVersion.Revision < 0)
					return 0;
				return GameVersion.Revision;
			}
		}


		public void Initialize(PatchManager.CreateParameters createParam)
		{
			_webServers = createParam.WebServers;
			_cdnServers = createParam.CDNServers;
			_defaultWebServer = createParam.DefaultWebServerIP;
			_defaultCDNServer = createParam.DefaultCDNServerIP;
			_serverID = createParam.ServerID;
			_channelID = createParam.ChannelID;
			_deviceID = createParam.DeviceID;
			_testFlag = createParam.TestFlag;
			AppVersion = new Version(Application.version);
		}
		public void Start()
		{
			// 注意：按照先后顺序添加流程节点
			_system.AddNode(new FsmInitiationBegin(_system));
			_system.AddNode(new FsmCheckSandboxDirty(_system));
			_system.AddNode(new FsmParseAppPatchManifest(_system));
			_system.AddNode(new FsmParseSandboxPatchManifest(_system));
			_system.AddNode(new FsmInitiationOver(_system));
			_system.AddNode(new FsmRequestGameVersion(_system));
			_system.AddNode(new FsmParseWebPatchManifest(_system));
			_system.AddNode(new FsmGetDonwloadList(_system));
			_system.AddNode(new FsmDownloadWebFiles(_system));
			_system.AddNode(new FsmDownloadWebPatchManifest(_system));
			_system.AddNode(new FsmDownloadOver(_system));
			_system.Run();
		}
		public void Update()
		{
			_system.Update();
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
		/// 接收事件
		/// </summary>
		public void HandleEventMessage(IEventMessage msg)
		{
			if (msg is PatchEventMessageDefine.OperationEvent)
			{
				var message = msg as PatchEventMessageDefine.OperationEvent;
				if (message.operation == EPatchOperation.BeginingRequestGameVersion)
				{
					// 从挂起的地方继续
					if (_system.Current == EPatchStates.InitiationOver.ToString())
						_system.SwitchNext();
					else
						AppLog.Log(ELogType.Error, $"Patch system is not prepare : {_system.Current}");
				}
				else if (message.operation == EPatchOperation.BeginingDownloadWebFiles)
				{
					// 从挂起的地方继续
					if (_system.Current == EPatchStates.GetDonwloadList.ToString())
						_system.SwitchNext();
					else
						AppLog.Log(ELogType.Error, $"Patch states is incorrect : {_system.Current}");
				}
				else if (message.operation == EPatchOperation.TryRequestGameVersion)
				{
					// 修复当前节点错误
					if (_system.Current == EPatchStates.RequestGameVersion.ToString())
						_system.Switch(_system.Current);
					else
						AppLog.Log(ELogType.Error, $"Patch states is incorrect : {_system.Current}");
				}
				else if (message.operation == EPatchOperation.TryDownloadWebPatchManifest)
				{
					// 修复当前节点错误
					if (_system.Current == EPatchStates.DownloadWebPatchManifest.ToString() || _system.Current == EPatchStates.ParseWebPatchManifest.ToString())
						_system.Switch(_system.Current);
					else
						AppLog.Log(ELogType.Error, $"Patch states is incorrect : {_system.Current}");
				}
				else if (message.operation == EPatchOperation.TryDownloadWebFiles)
				{
					// 修复当前节点错误
					if (_system.Current == EPatchStates.DownloadWebFiles.ToString())
						_system.Switch(EPatchStates.GetDonwloadList.ToString());
					else
						AppLog.Log(ELogType.Error, $"Patch states is incorrect : {_system.Current}");
				}
				else
				{
					throw new NotImplementedException($"{message.operation}");
				}
			}
		}

		// 解析补丁清单文件相关接口
		public void ParseAppPatchManifest(string fileContent)
		{
			if (AppPatchManifest != null)
				throw new Exception("Should never get here.");
			AppPatchManifest = new PatchManifest();
			AppPatchManifest.Parse(fileContent);
		}
		public void ParseSandboxPatchManifest(string fileContent)
		{
			if (SandboxPatchManifest != null)
				throw new Exception("Should never get here.");
			SandboxPatchManifest = new PatchManifest();
			SandboxPatchManifest.Parse(fileContent);
		}
		public void ParseSandboxPatchManifest(PatchManifest patchFile)
		{
			if (SandboxPatchManifest != null)
				throw new Exception("Should never get here.");
			SandboxPatchManifest = patchFile;
		}
		public void ParseWebPatchManifest(string fileContent)
		{
			if (WebPatchManifest != null)
				throw new Exception("Should never get here.");
			WebPatchManifest = new PatchManifest();
			WebPatchManifest.Parse(fileContent);
		}

		// 服务器IP相关
		public string GetWebServerIP()
		{
			RuntimePlatform runtimePlatform = Application.platform;
			if (_webServers != null && _webServers.ContainsKey(runtimePlatform))
				return _webServers[runtimePlatform];
			else
				return _defaultWebServer;
		}
		public string GetCDNServerIP()
		{
			RuntimePlatform runtimePlatform = Application.platform;
			if (_cdnServers != null && _cdnServers.ContainsKey(runtimePlatform))
				return _cdnServers[runtimePlatform];
			else
				return _defaultCDNServer;
		}

		// WEB相关
		public string GetWebDownloadURL(string resourceVersion, string fileName)
		{
			return $"{GetCDNServerIP()}/{resourceVersion}/{fileName}";
		}
		public string GetWebPostData()
		{
			return $"{AppVersion}&{_serverID}&{_channelID}&{_deviceID}&{_testFlag}";
		}
		public void ParseResponseData(string data)
		{
			if (string.IsNullOrEmpty(data))
				throw new Exception("Web server response data is null or empty.");
			if (GameVersion != null)
				throw new Exception("Should never get here.");
		
			// $"{GameVersion}&{AppInstallURL}"
			string[] splits = data.Split('&');
			string gameVersionContent = splits[0];
			GameVersion = new Version(data);
			_forceInstallAppURL = splits[1];
		}
		public string GetForceInstallAppURL()
		{
			// 注意：如果不需要强更安装包，返回的路径会为空
			return _forceInstallAppURL;
		}
	}
}