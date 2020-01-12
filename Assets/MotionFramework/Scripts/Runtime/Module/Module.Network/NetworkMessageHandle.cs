//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

namespace MotionFramework.Network
{
	public class NetworkMessageHandler
	{
		private static Dictionary<int, Type> _types = new Dictionary<int, Type>();
		
		/// <summary>
		/// 注册非热更的消息类型
		/// </summary>
		public static void RegisterMonoMessageType(int msgID, Type classType)
		{
			// 判断是否重复
			if (_types.ContainsKey(msgID))
				throw new Exception($"NetMessage {msgID} already exist.");

			_types.Add(msgID, classType);
		}

		public static Type Handle(int msgID)
		{
			Type type;
			if (_types.TryGetValue(msgID, out type))
			{
				return type;
			}
			else
			{
				throw new KeyNotFoundException($"NetMessage {msgID} is not define.");
			}
		}
		public static Type TryHandle(int msgID)
		{
			Type type;
			_types.TryGetValue(msgID, out type);
			return type;
		}
	}
}