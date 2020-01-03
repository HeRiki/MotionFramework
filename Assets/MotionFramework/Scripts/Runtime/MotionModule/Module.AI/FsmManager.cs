//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections.Generic;
using MotionFramework.Console;

namespace MotionFramework.AI
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
			public FsmGraph Graph;

			/// <summary>
			/// 初始运行的节点
			/// </summary>
			public string RunNode;

			/// <summary>
			/// 节点列表
			/// </summary>
			public List<IFsmNode> Nodes;
		}

		private readonly FsmSystem _system = new FsmSystem();
		private FsmGraph _graph;
		private string _runNode;


		void IMotionModule.OnCreate(System.Object param)
		{
			CreateParameters createParam = param as CreateParameters;
			if (createParam == null)
				throw new Exception($"{nameof(FsmManager)} create param is invalid.");

			if (createParam.Nodes == null || createParam.Nodes.Count == 0)
				AppLog.Log(ELogType.Error, "Fsm nodes is null or empty");

			_graph = createParam.Graph;
			_runNode = createParam.RunNode;
			for(int i=0; i< createParam.Nodes .Count; i++)
			{
				_system.AddNode(createParam.Nodes[i]);
			}
		}
		void IMotionModule.OnStart()
		{
			_system.Run(_runNode, _graph);
		}
		void IMotionModule.OnUpdate()
		{
			_system.Update();
		}
		void IMotionModule.OnGUI()
		{
			AppConsole.GUILable($"[{nameof(FsmManager)}] FSM : {_system.CurrentNodeName}");
		}

		/// <summary>
		/// 当前运行的节点类型
		/// </summary>
		public string CurrentNodeName
		{
			get { return _system.CurrentNodeName; }
		}

		/// <summary>
		/// 之前运行的节点类型
		/// </summary>
		public string PreviousNodeName
		{
			get { return _system.PreviousNodeName; }
		}

		/// <summary>
		/// 转换节点
		/// </summary>
		public void Transition(string nodeName)
		{
			_system.Transition(nodeName);
		}

		/// <summary>
		/// 接收消息
		/// </summary>
		public void HandleMessage(object msg)
		{
			_system.HandleMessage(msg);
		}
	}
}