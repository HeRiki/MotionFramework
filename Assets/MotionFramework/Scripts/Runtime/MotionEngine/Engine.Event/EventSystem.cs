//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

namespace MotionFramework.Event
{
	/// <summary>
	/// 事件系统
	/// </summary>
	public class EventSystem
	{
		private readonly Dictionary<int, List<Action<IEventMessage>>> _listeners = new Dictionary<int, List<Action<IEventMessage>>>();

		/// <summary>
		/// 清空所有监听
		/// </summary>
		public void ClearListeners()
		{
			foreach (int eventId in _listeners.Keys)
			{
				_listeners[eventId].Clear();
			}
			_listeners.Clear();
		}

		/// <summary>
		/// 注册监听
		/// </summary>
		public void AddListener(int eventId, Action<IEventMessage> listener)
		{
			if (_listeners.ContainsKey(eventId) == false)
				_listeners.Add(eventId, new List<Action<IEventMessage>>());

			if (_listeners[eventId].Contains(listener) == false)
				_listeners[eventId].Add(listener);
		}

		/// <summary>
		/// 移除监听
		/// </summary>
		public void RemoveListener(int eventId, Action<IEventMessage> listener)
		{
			if (_listeners.ContainsKey(eventId))
			{
				if (_listeners[eventId].Contains(listener))
					_listeners[eventId].Remove(listener);
			}
		}

		/// <summary>
		/// 广播事件
		/// </summary>
		/// <param name="msg">消息类</param>
		public void Broadcast(int eventId, IEventMessage msg)
		{
			if (_listeners.ContainsKey(eventId) == false)
			{
				AppLog.Log(ELogType.Warning, $"Not found listener eventId : {eventId}");
				return;
			}

			List<Action<IEventMessage>> listeners = _listeners[eventId];
			for(int i=0; i< listeners.Count; i++)
			{
				listeners[i].Invoke(msg);
			}
		}

		/// <summary>
		/// 获取所有监听器的总数
		/// </summary>
		public int GetAllListenerCount()
		{
			int count = 0;
			foreach(var list in _listeners)
			{
				count += list.Value.Count;
			}
			return count;
		}
	}
}