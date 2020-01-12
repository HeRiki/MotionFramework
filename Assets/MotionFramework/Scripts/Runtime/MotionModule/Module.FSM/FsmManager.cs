//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections.Generic;
using MotionFramework.Console;

namespace MotionFramework.FSM
{
	/// <summary>
	/// 状态机管理器
	/// </summary>
	public sealed class FsmManager : ModuleSingleton<FsmManager>, IMotionModule
	{
		/// <summary>
		/// 游戏模块创建参数
		/// </summary>
		public class CreateParameters
		{
			/// <summary>
			/// 节点转换关系图，如果为NULL则不检测转换关系
			/// </summary>
			public FiniteStateGraph Graph;

			/// <summary>
			/// 入口节点
			/// </summary>
			public string EntryNode;

			/// <summary>
			/// 节点列表
			/// </summary>
			public List<IFiniteStateNode> Nodes;
		}

		private readonly FiniteStateMachine _fsm = new FiniteStateMachine();
		private FiniteStateGraph _graph;
		private string _entryNode;
		private bool _isRun = false;


		void IMotionModule.OnCreate(System.Object param)
		{
			CreateParameters createParam = param as CreateParameters;
			if (createParam == null)
				throw new Exception($"{nameof(FsmManager)} create param is invalid.");

			if (createParam.Nodes == null || createParam.Nodes.Count == 0)
				MotionLog.Log(ELogType.Error, "Fsm nodes is null or empty");

			_graph = createParam.Graph;
			_entryNode = createParam.EntryNode;
			for (int i = 0; i < createParam.Nodes.Count; i++)
			{
				_fsm.AddNode(createParam.Nodes[i]);
			}
		}
		void IMotionModule.OnUpdate()
		{
			_fsm.Update();
		}
		void IMotionModule.OnGUI()
		{
			ConsoleSystem.GUILable($"[{nameof(FsmManager)}] FSM : {_fsm.CurrentNodeName}");
		}

		/// <summary>
		/// 运行状态机
		/// </summary>
		public void Run()
		{
			if (_isRun == false)
			{
				_isRun = true;
				_fsm.Run(_entryNode, _graph);
			}
		}

		/// <summary>
		/// 当前运行的节点类型
		/// </summary>
		public string CurrentNodeName
		{
			get { return _fsm.CurrentNodeName; }
		}

		/// <summary>
		/// 之前运行的节点类型
		/// </summary>
		public string PreviousNodeName
		{
			get { return _fsm.PreviousNodeName; }
		}

		/// <summary>
		/// 转换节点
		/// </summary>
		public void Transition(string nodeName)
		{
			_fsm.Transition(nodeName);
		}

		/// <summary>
		/// 接收消息
		/// </summary>
		public void HandleMessage(object msg)
		{
			_fsm.HandleMessage(msg);
		}
	}
}