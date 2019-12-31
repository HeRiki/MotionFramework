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
	public sealed class EventManager : IGameModule
	{
		public static readonly EventManager Instance = new EventManager();

		/// <summary>
		/// 事件系统
		/// </summary>
		public readonly EventSystem InternalSystem = new EventSystem();


		private EventManager()
		{
		}
		public void Awake()
		{
		}
		public void Start()
		{
		}
		public void Update()
		{
		}
		public void LateUpdate()
		{
		}
		public void OnGUI()
		{
			DebugConsole.GUILable($"[{nameof(EventManager)}] Listener total count : {InternalSystem.GetAllListenerCount()}");
		}

		/// <summary>
		/// 添加监听
		/// </summary>
		public void AddListener(string eventTag, System.Action<IEventMessage> listener)
		{
			InternalSystem.AddListener(eventTag, listener);
		}

		/// <summary>
		/// 移除监听
		/// </summary>
		public void RemoveListener(string eventTag, System.Action<IEventMessage> listener)
		{
			InternalSystem.RemoveListener(eventTag, listener);
		}

		/// <summary>
		/// 发送事件消息
		/// </summary>
		public void SendMessage(string eventTag, IEventMessage message)
		{
			InternalSystem.Broadcast(eventTag, message);
		}
	}
}