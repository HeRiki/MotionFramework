//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework.Event
{
	public abstract class EventMessage<TEvent> : IEventMessage
	{
		/// <summary>
		/// 事件ID
		/// </summary>
		public static readonly int ID = typeof(TEvent).GetHashCode();

		/// <summary>
		/// 事件ID
		/// </summary>
		public int EventId
		{
			get
			{
				return ID;
			}
		}
	}
}