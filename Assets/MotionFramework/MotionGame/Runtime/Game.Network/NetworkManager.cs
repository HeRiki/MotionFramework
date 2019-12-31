//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using MotionFramework.Debug;

namespace MotionFramework.Network
{
	/// <summary>
	/// 网络状态
	/// </summary>
	public enum ENetworkState
	{
		Disconnect,
		Connecting,
		Connected,
	}

	/// <summary>
	/// 网络管理器
	/// </summary>
	public sealed class NetworkManager : IGameModule
	{
		public static readonly NetworkManager Instance = new NetworkManager();

		// 服务端
		private TServer _server;

		// 通信频道
		private TChannel _channel;

		// GUI显示数据
		private string _host = string.Empty;
		private int _port = 0;
		private AddressFamily _family = AddressFamily.Unknown;

		/// <summary>
		/// 当前的网络状态
		/// </summary>
		public ENetworkState State { private set; get; } = ENetworkState.Disconnect;

		/// <summary>
		/// Mono层网络消息接收回调
		/// </summary>
		public Action<INetPackage> MonoPackageCallback;

		/// <summary>
		/// 热更层网络消息接收回调
		/// </summary>
		public Action<INetPackage> HotfixPackageCallback;


		private NetworkManager()
		{
		}
		public void Awake()
		{
		}
		public void Start()
		{
			_server = new TServer();
			_server.Start(false, null);
		}
		public void Update()
		{
			if (_server != null)
				_server.Update();

			UpdatePickMsg();
			UpdateNetworkState();
		}
		public void LateUpdate()
		{
		}
		public void OnGUI()
		{
			DebugConsole.GUILable($"[{nameof(NetworkManager)}] State : {State}");
			DebugConsole.GUILable($"[{nameof(NetworkManager)}] IP Host : {_host}");
			DebugConsole.GUILable($"[{nameof(NetworkManager)}] IP Port : {_port}");
			DebugConsole.GUILable($"[{nameof(NetworkManager)}] IP Type : {_family}");
		}

		private void UpdatePickMsg()
		{
			if (_channel != null)
			{
				INetPackage package = (INetPackage)_channel.PickMsg();
				if (package != null)
				{
					if (package.IsHotfixPackage)
						HotfixPackageCallback.Invoke(package);
					else
						MonoPackageCallback.Invoke(package);		
				}
			}
		}
		private void UpdateNetworkState()
		{
			if (State == ENetworkState.Connected)
			{
				if (_channel != null && _channel.IsConnected() == false)
				{
					State = ENetworkState.Disconnect;
					LogSystem.Log(ELogType.Warning, "Server disconnect.");
				}
			}
		}

		/// <summary>
		/// 连接服务器
		/// </summary>
		public void ConnectServer(string host, int port, Type packageParseType)
		{
			if (State == ENetworkState.Disconnect)
			{
				State = ENetworkState.Connecting;
				IPEndPoint remote = new IPEndPoint(IPAddress.Parse(host), port);
				_server.ConnectAsync(remote, OnConnectServer, packageParseType);

				// 记录数据
				_host = host;
				_port = port;
				_family = remote.AddressFamily;
			}
		}
		private void OnConnectServer(TChannel channel, SocketError error)
		{
			LogSystem.Log(ELogType.Log, $"Server connect result : {error}");
			if (error == SocketError.Success)
			{
				_channel = channel;
				State = ENetworkState.Connected;
			}
			else
			{
				State = ENetworkState.Disconnect;
			}
		}

		/// <summary>
		/// 断开连接
		/// </summary>
		public void DisconnectServer()
		{
			State = ENetworkState.Disconnect;
			if (_channel != null)
			{
				_server.ReleaseChannel(_channel);
				_channel = null;
			}
		}

		/// <summary>
		/// 发送网络消息
		/// </summary>
		public void SendMessage(INetPackage package)
		{
			if (State != ENetworkState.Connected)
			{
				LogSystem.Log(ELogType.Warning, "Network is not connected.");
				return;
			}

			if (_channel != null)
				_channel.SendMsg(package);
		}
	}
}