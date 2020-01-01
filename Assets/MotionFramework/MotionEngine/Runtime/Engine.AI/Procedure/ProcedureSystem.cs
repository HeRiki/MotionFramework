//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections.Generic;

namespace MotionFramework.AI
{
	/// <summary>
	/// 流程系统
	/// </summary>
	public class ProcedureSystem
	{
		private readonly List<int> _nodeTypes = new List<int>();
		private readonly FsmSystem _system = new FsmSystem();

		/// <summary>
		/// 添加一个节点
		/// 注意：节点会按照添加的先后顺序执行
		/// </summary>
		public void AddNode(FsmNode node)
		{
			_system.AddNode(node);
			if (_nodeTypes.Contains(node.Type) == false)
				_nodeTypes.Add(node.Type);
		}

		/// <summary>
		/// 运行流程系统
		/// </summary>
		public void Run()
		{
			if (_nodeTypes.Count > 0)
				_system.Run(_nodeTypes[0], null);
			else
				Logger.Log(ELogType.Warning, "Procedure system dont has any node.");
		}

		/// <summary>
		/// 更新流程系统
		/// </summary>
		public void Update()
		{
			_system.Update();
		}

		/// <summary>
		/// 当前运行的节点类型
		/// </summary>
		public int Current()
		{
			return _system.CurrentNodeType;
		}

		/// <summary>
		/// 切换流程节点
		/// </summary>
		public void Switch(int nodeType)
		{
			_system.Transition(nodeType);
		}

		/// <summary>
		/// 切换至下个流程节点
		/// </summary>
		public void SwitchNext()
		{
			int index = _nodeTypes.IndexOf(_system.CurrentNodeType);
			if (index >= _nodeTypes.Count - 1)
			{
				Logger.Log(ELogType.Warning, $"Current node {_system.CurrentNodeType} is end node.");
			}
			else
			{
				Switch(_nodeTypes[index + 1]);
			}
		}

		/// <summary>
		/// 切换至上个流程节点
		/// </summary>
		public void SwitchLast()
		{
			int index = _nodeTypes.IndexOf(_system.CurrentNodeType);
			if (index <= 0)
			{
				Logger.Log(ELogType.Warning, $"Current node {_system.CurrentNodeType} is begin node.");
			}
			else
			{
				Switch(_nodeTypes[index - 1]);
			}
		}
	}
}