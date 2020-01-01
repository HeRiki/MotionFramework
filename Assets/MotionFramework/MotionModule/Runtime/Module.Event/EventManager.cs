//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using MotionFramework.Debug;

namespace MotionFramework.Event
{
	/// <summary>
	/// 事件管理器
	/// </summary>
	public sealed class EventManager : IModule
	{
		/// <summary>
		/// 游戏模块全局实例
		/// </summary>
		public static EventManager Instance { private set; get; }

		private readonly EventSystem _system = new EventSystem();


		void IModule.OnCreate(System.Object param)
		{
			// 全局实例赋值
			Instance = this;
		}
		void IModule.OnStart()
		{
		}
		void IModule.OnUpdate()
		{
		}
		void IModule.OnGUI()
		{
			DebugConsole.GUILable($"[{nameof(EventManager)}] Listener total count : {_system.GetAllListenerCount()}");
		}

		/// <summary>
		/// 添加监听
		/// </summary>
		public void AddListener(string eventTag, System.Action<IEventMessage> listener)
		{
			_system.AddListener(eventTag, listener);
		}

		/// <summary>
		/// 移除监听
		/// </summary>
		public void RemoveListener(string eventTag, System.Action<IEventMessage> listener)
		{
			_system.RemoveListener(eventTag, listener);
		}

		/// <summary>
		/// 发送事件消息
		/// </summary>
		public void SendMessage(string eventTag, IEventMessage message)
		{
			_system.Broadcast(eventTag, message);
		}

		/// <summary>
		/// 清空所有监听
		/// </summary>
		public void ClearListeners()
		{
			_system.ClearListeners();
		}
	}
}