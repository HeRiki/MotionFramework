//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

namespace MotionFramework.AI
{
	/// <summary>
	/// 节点转换关系图
	/// </summary>
	public class FsmGraph
	{
		private readonly Dictionary<int, List<int>> _graph = new Dictionary<int, List<int>>();
		private readonly int _globalNode;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="globalNode">全局节点不受转换关系的限制</param>
		public FsmGraph(int globalNode)
		{
			_globalNode = globalNode;
		}

		/// <summary>
		/// 添加转换关系
		/// </summary>
		/// <param name="nodeType">节点</param>
		/// <param name="transitionNodes">可以转换到的节点列表</param>
		public void AddTransition(int nodeType, List<int> transitionNodes)
		{
			if (transitionNodes == null)
				throw new ArgumentNullException();

			if (_graph.ContainsKey(nodeType))
			{
				Logger.Log(ELogType.Warning, $"Graph node {nodeType} already existed.");
				return;
			}

			_graph.Add(nodeType, transitionNodes);
		}

		/// <summary>
		/// 检测转换关系
		/// </summary>
		public bool CanTransition(int from, int to)
		{
			if (_graph.ContainsKey(from) == false)
			{
				Logger.Log(ELogType.Warning, $"Not found graph node {from}");
				return false;
			}

			if (to == _globalNode)
				return true;

			return _graph[from].Contains(to);
		}
	}
}