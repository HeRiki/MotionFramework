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
		private readonly List<string> _nodeNames = new List<string>();
		private readonly FsmSystem _system = new FsmSystem();

		/// <summary>
		/// 当前运行的节点名称
		/// </summary>
		public string Current
		{
			get
			{
				return _system.CurrentNodeName;
			}
		}

		/// <summary>
		/// 之前运行的节点名称
		/// </summary>
		public string Previous
		{
			get
			{
				return _system.PreviousNodeName;
			}
		}

		/// <summary>
		/// 添加一个流程节点
		/// 注意：流程节点会按照添加的先后顺序执行
		/// </summary>
		public void AddNode(IFsmNode node)
		{
			_system.AddNode(node);
			if (_nodeNames.Contains(node.Name) == false)
				_nodeNames.Add(node.Name);
		}

		/// <summary>
		/// 运行流程系统
		/// </summary>
		public void Run()
		{
			if (_nodeNames.Count > 0)
				_system.Run(_nodeNames[0], null);
			else
				AppLog.Log(ELogType.Warning, "Procedure system dont has any node.");
		}

		/// <summary>
		/// 更新流程系统
		/// </summary>
		public void Update()
		{
			_system.Update();
		}

		/// <summary>
		/// 切换流程节点
		/// </summary>
		public void Switch(string nodeName)
		{
			_system.Transition(nodeName);
		}

		/// <summary>
		/// 切换至下个流程节点
		/// </summary>
		public void SwitchNext()
		{
			int index = _nodeNames.IndexOf(_system.CurrentNodeName);
			if (index >= _nodeNames.Count - 1)
			{
				AppLog.Log(ELogType.Warning, $"Current node {_system.CurrentNodeName} is end node.");
			}
			else
			{
				Switch(_nodeNames[index + 1]);
			}
		}

		/// <summary>
		/// 切换至上个流程节点
		/// </summary>
		public void SwitchLast()
		{
			int index = _nodeNames.IndexOf(_system.CurrentNodeName);
			if (index <= 0)
			{
				AppLog.Log(ELogType.Warning, $"Current node {_system.CurrentNodeName} is begin node.");
			}
			else
			{
				Switch(_nodeNames[index - 1]);
			}
		}
	}
}