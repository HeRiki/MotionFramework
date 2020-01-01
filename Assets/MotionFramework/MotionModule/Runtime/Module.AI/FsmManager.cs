﻿//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using MotionFramework.Debug;

namespace MotionFramework.AI
{
	/// <summary>
	/// 状态机管理器
	/// </summary>
	public sealed class FsmManager : IModule
	{
		/// <summary>
		/// 游戏模块全局实例
		/// </summary>
		public static FsmManager Instance { private set; get; }

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
			public int RunNode;
		}

		private readonly FsmSystem _system = new FsmSystem();
		private FsmGraph _graph;
		private int _runNode;


		void IModule.OnCreate(System.Object param)
		{
			CreateParameters createParam = param as CreateParameters;
			if (createParam == null)
				throw new Exception($"{nameof(FsmManager)} create param is invalid.");

			_graph = createParam.Graph;
			_runNode = createParam.RunNode;

			// 全局实例赋值
			Instance = this;
		}
		void IModule.OnStart()
		{
			_system.Run(_runNode, _graph);
		}
		void IModule.OnUpdate()
		{
			_system.Update();
		}
		void IModule.OnGUI()
		{
			DebugConsole.GUILable($"[{nameof(FsmManager)}] FSM : {_system.CurrentNodeType}");
		}

		/// <summary>
		/// 当前运行的节点类型
		/// </summary>
		public int CurrentNodeType
		{
			get { return _system.CurrentNodeType; }
		}

		/// <summary>
		/// 之前运行的节点类型
		/// </summary>
		public int PreviousNodeType
		{
			get { return _system.PreviousNodeType; }
		}

		/// <summary>
		/// 加入一个节点
		/// </summary>
		public void AddState(FsmNode node)
		{
			_system.AddNode(node);
		}

		/// <summary>
		/// 转换节点
		/// </summary>
		public void Transition(int nodeType)
		{
			_system.Transition(nodeType);
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