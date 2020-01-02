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
		private readonly Dictionary<string, List<Action<IEventMessage>>> _listeners = new Dictionary<string, List<Action<IEventMessage>>>();

		/// <summary>
		/// 清空所有监听
		/// </summary>
		public void ClearListeners()
		{
			foreach (string type in _listeners.Keys)
			{
				_listeners[type].Clear();
			}
			_listeners.Clear();
		}

		/// <summary>
		/// 注册监听
		/// </summary>
		public void AddListener(string eventTag, Action<IEventMessage> listener)
		{
			if (_listeners.ContainsKey(eventTag) == false)
				_listeners.Add(eventTag, new List<Action<IEventMessage>>());

			if (_listeners[eventTag].Contains(listener) == false)
				_listeners[eventTag].Add(listener);
		}

		/// <summary>
		/// 移除监听
		/// </summary>
		public void RemoveListener(string eventTag, Action<IEventMessage> listener)
		{
			if (_listeners.ContainsKey(eventTag))
			{
				if (_listeners[eventTag].Contains(listener))
					_listeners[eventTag].Remove(listener);
			}
		}

		/// <summary>
		/// 广播事件
		/// </summary>
		/// <param name="eventTag">事件标签</param>
		/// <param name="msg">消息类</param>
		public void Broadcast(string eventTag, IEventMessage msg)
		{
			if (_listeners.ContainsKey(eventTag) == false)
			{
				LogHelper.Log(ELogType.Warning, $"Not found listener eventTag : {eventTag}");
				return;
			}

			List<Action<IEventMessage>> listeners = _listeners[eventTag];
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