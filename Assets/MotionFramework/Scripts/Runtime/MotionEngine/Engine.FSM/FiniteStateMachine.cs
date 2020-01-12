//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

namespace MotionFramework.FSM
{
	/// <summary>
	/// 有限状态机
	/// </summary>
	public class FiniteStateMachine
	{
		private readonly List<IFiniteStateNode> _nodes = new List<IFiniteStateNode>();
		private IFiniteStateNode _curNode;
		private IFiniteStateNode _preNode;
		private FiniteStateGraph _graph;

		/// <summary>
		/// 当前运行的节点名称
		/// </summary>
		public string CurrentNodeName
		{
			get { return _curNode != null ? _curNode.Name : string.Empty; }
		}

		/// <summary>
		/// 之前运行的节点名称
		/// </summary>
		public string PreviousNodeName
		{
			get { return _preNode != null ? _preNode.Name : string.Empty; }
		}

		/// <summary>
		/// 加入一个节点
		/// </summary>
		public void AddNode(IFiniteStateNode node)
		{
			if (node == null)
				throw new ArgumentNullException();

			if (_nodes.Contains(node) == false)
			{
				_nodes.Add(node);
			}
			else
			{
				MotionLog.Log(ELogType.Warning, $"Node {node.Name} already existed");
			}
		}

		/// <summary>
		/// 启动状态机
		/// </summary>
		/// <param name="entryNode">入口节点</param>
		/// <param name="graph">节点转换关系图，如果为NULL则不检测转换关系</param>
		public void Run(string entryNode, FiniteStateGraph graph)
		{
			_graph = graph;
			_curNode = GetNode(entryNode);
			_preNode = GetNode(entryNode);

			if (_curNode != null)
				_curNode.OnEnter();
			else
				MotionLog.Log(ELogType.Error, $"Not found entry node : {entryNode}");
		}

		/// <summary>
		/// 更新状态机
		/// </summary>
		public void Update()
		{
			if (_curNode != null)
				_curNode.OnUpdate();
		}

		/// <summary>
		/// 转换节点
		/// </summary>
		public void Transition(string nodeName)
		{
			if (string.IsNullOrEmpty(nodeName))
				throw new ArgumentNullException();

			IFiniteStateNode node = GetNode(nodeName);
			if (node == null)
			{
				MotionLog.Log(ELogType.Error, $"Can not found node {nodeName}");
				return;
			}

			// 检测转换关系
			if (_graph != null)
			{
				if (_graph.CanTransition(_curNode.Name, node.Name) == false)
				{
					MotionLog.Log(ELogType.Error, $"Can not transition {_curNode} to {node}");
					return;
				}
			}

			MotionLog.Log(ELogType.Log, $"Transition {_curNode} to {node}");
			_preNode = _curNode;
			_curNode.OnExit();
			_curNode = node;
			_curNode.OnEnter();
		}

		/// <summary>
		/// 返回到之前的节点
		/// </summary>
		public void RevertToPreviousNode()
		{
			Transition(PreviousNodeName);
		}
		
		/// <summary>
		/// 接收消息
		/// </summary>
		public void HandleMessage(object msg)
		{
			if (_curNode != null)
				_curNode.OnHandleMessage(msg);
		}

		private bool IsContains(string nodeName)
		{
			for (int i = 0; i < _nodes.Count; i++)
			{
				if (_nodes[i].Name == nodeName)
					return true;
			}
			return false;
		}
		private IFiniteStateNode GetNode(string nodeName)
		{
			for (int i = 0; i < _nodes.Count; i++)
			{
				if (_nodes[i].Name == nodeName)
					return _nodes[i];
			}
			return null;
		}
	}
}
