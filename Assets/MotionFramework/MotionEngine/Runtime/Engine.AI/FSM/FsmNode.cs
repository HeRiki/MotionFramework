//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework.AI
{
	public abstract class FsmNode
	{
		/// <summary>
		/// 节点类型
		/// </summary>
		public int Type { get; set; }

		public FsmNode(int type)
		{
			Type = type;
		}
		public abstract void OnEnter();
		public abstract void OnUpdate();
		public abstract void OnExit();
		public virtual void OnHandleMessage(object msg) { }
	}
}