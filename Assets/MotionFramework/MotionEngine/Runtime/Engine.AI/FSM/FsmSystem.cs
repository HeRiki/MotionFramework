//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

namespace MotionFramework.AI
{
	/// <summary>
	/// 有限状态机
	/// </summary>
	public class FsmSystem
	{
		private readonly List<FsmNode> _nodes = new List<FsmNode>();
		private FsmNode _curNode;
		private FsmNode _preNode;
		private FsmGraph _graph;

		/// <summary>
		/// 当前运行的节点类型
		/// </summary>
		public int CurrentNodeType
		{
			get { return _curNode != null ? _curNode.Type : -1; }
		}

		/// <summary>
		/// 之前运行的节点类型
		/// </summary>
		public int PreviousNodeType
		{
			get { return _preNode != null ? _preNode.Type : -1; }
		}

		/// <summary>
		/// 加入一个节点
		/// </summary>
		public void AddNode(FsmNode node)
		{
			if (node == null)
				throw new ArgumentNullException();

			if (_nodes.Contains(node) == false)
			{
				_nodes.Add(node);
			}
			else
			{
				Logger.Log(ELogType.Warning, $"Node {node.Type} already existed");
			}
		}

		/// <summary>
		/// 启动状态机
		/// </summary>
		/// <param name="runNodeType">初始运行的节点类型</param>
		/// <param name="graph">节点转换关系图，如果为NULL则不检测转换关系</param>
		public void Run(int runNodeType, FsmGraph graph)
		{
			_graph = graph;
			_curNode = GetNode(runNodeType);
			_preNode = GetNode(runNodeType);

			if (_curNode != null)
				_curNode.OnEnter();
			else
				Logger.Log(ELogType.Error, $"Not found run node : {runNodeType}");
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
		public void Transition(int nodeType)
		{
			FsmNode node = GetNode(nodeType);
			if (node == null)
			{
				Logger.Log(ELogType.Error, $"Can not found node {nodeType}");
				return;
			}

			// 检测转换关系
			if (_graph != null)
			{
				if (_graph.CanTransition(_curNode.Type, node.Type) == false)
				{
					Logger.Log(ELogType.Error, $"Can not transition {_curNode} to {node}");
					return;
				}
			}

			Logger.Log(ELogType.Log, $"Transition {_curNode} to {node}");
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
			Transition(PreviousNodeType);
		}
		
		/// <summary>
		/// 接收消息
		/// </summary>
		public void HandleMessage(object msg)
		{
			if (_curNode != null)
				_curNode.OnHandleMessage(msg);
		}

		private bool IsContains(int nodeType)
		{
			for (int i = 0; i < _nodes.Count; i++)
			{
				if (_nodes[i].Type == nodeType)
					return true;
			}
			return false;
		}
		private FsmNode GetNode(int nodeType)
		{
			for (int i = 0; i < _nodes.Count; i++)
			{
				if (_nodes[i].Type == nodeType)
					return _nodes[i];
			}
			return null;
		}
	}
}
