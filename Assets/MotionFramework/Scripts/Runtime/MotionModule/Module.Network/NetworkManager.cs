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
using MotionFramework.Console;

namespace MotionFramework.Network
{
	/// <summary>
	/// 网络管理器
	/// </summary>
	public sealed class NetworkManager : ModuleSingleton<NetworkManager>, IMotionModule
	{
		/// <summary>
		/// 游戏模块创建参数
		/// </summary>
		public class CreateParameters
		{
			/// <summary>
			/// 网络包编码解码器
			/// </summary>
			public System.Type PackageCoderType;
		}


		private TcpServer _server;
		private TcpChannel _channel;
		private System.Type _packageCoderType;

		// GUI显示数据
		private string _host;
		private int _port;
		private AddressFamily _family = AddressFamily.Unknown;

		/// <summary>
		/// 当前的网络状态
		/// </summary>
		public ENetworkStates State { private set; get; } = ENetworkStates.Disconnect;

		/// <summary>
		/// Mono层网络消息接收回调
		/// </summary>
		public Action<INetPackage> MonoPackageCallback;

		/// <summary>
		/// 热更层网络消息接收回调
		/// </summary>
		public Action<INetPackage> HotfixPackageCallback;


		void IMotionModule.OnCreate(System.Object param)
		{
			CreateParameters createParam = param as CreateParameters;
			if (createParam == null)
				throw new Exception($"{nameof(NetworkManager)} create param is invalid.");

			_packageCoderType = createParam.PackageCoderType;
			_server = new TcpServer();
			_server.Start(false, null);
		}
		void IMotionModule.OnUpdate()
		{
			_server.Update();

			if (_channel != null)
			{
				// 拉取网络消息
				// 注意：如果服务器意外断开，未拉取的消息将不会处理
				INetPackage package = (INetPackage)_channel.PickMsg();
				if (package != null)
				{
					if (package.IsHotfixPackage)
						HotfixPackageCallback.Invoke(package);
					else
						MonoPackageCallback.Invoke(package);
				}

				// 侦测服务器主动断开的连接
				if (State == ENetworkStates.Connected)
				{
					if (_channel.IsConnected() == false)
					{
						State = ENetworkStates.Disconnect;
						NetworkEventDispatcher.SendDisconnectMsg();
						CloseChannel();
						AppLog.Log(ELogType.Warning, "Server disconnect.");
					}
				}
			}
		}
		void IMotionModule.OnGUI()
		{
			ConsoleSystem.GUILable($"[{nameof(NetworkManager)}] State : {State}");
			ConsoleSystem.GUILable($"[{nameof(NetworkManager)}] IP Host : {_host}");
			ConsoleSystem.GUILable($"[{nameof(NetworkManager)}] IP Port : {_port}");
			ConsoleSystem.GUILable($"[{nameof(NetworkManager)}] IP Type : {_family}");
		}

		/// <summary>
		/// 连接服务器
		/// </summary>
		/// <param name="host">地址</param>
		/// <param name="port">端口</param>
		public void ConnectServer(string host, int port)
		{
			if (State == ENetworkStates.Disconnect)
			{
				State = ENetworkStates.Connecting;
				NetworkEventDispatcher.SendBeginConnectMsg();
				IPEndPoint remote = new IPEndPoint(IPAddress.Parse(host), port);
				_server.ConnectAsync(remote, OnConnectServer, _packageCoderType);

				// 记录数据
				_host = host;
				_port = port;
				_family = remote.AddressFamily;
			}
		}
		private void OnConnectServer(TcpChannel channel, SocketError error)
		{
			AppLog.Log(ELogType.Log, $"Server connect result : {error}");
			if (error == SocketError.Success)
			{
				_channel = channel;
				State = ENetworkStates.Connected;
				NetworkEventDispatcher.SendConnectSuccessMsg();
			}
			else
			{
				State = ENetworkStates.Disconnect;
				NetworkEventDispatcher.SendConnectFailMsg(error.ToString());
			}
		}

		/// <summary>
		/// 断开连接
		/// </summary>
		public void DisconnectServer()
		{
			if (State == ENetworkStates.Connected)
			{
				State = ENetworkStates.Disconnect;
				NetworkEventDispatcher.SendDisconnectMsg();
				CloseChannel();
			}
		}

		/// <summary>
		/// 发送网络消息
		/// </summary>
		public void SendMessage(INetPackage package)
		{
			if (State != ENetworkStates.Connected)
			{
				AppLog.Log(ELogType.Warning, "Network is not connected.");
				return;
			}

			if (_channel != null)
				_channel.SendMsg(package);
		}

		private void CloseChannel()
		{
			if (_channel != null)
			{
				_server.CloseChannel(_channel);
				_channel = null;
			}
		}
	}
}