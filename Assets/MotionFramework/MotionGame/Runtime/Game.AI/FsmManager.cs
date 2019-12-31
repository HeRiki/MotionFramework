//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using MotionFramework.Debug;

namespace MotionFramework.AI
{
	/// <summary>
	/// 状态机管理器
	/// </summary>
	public sealed class FsmManager : IGameModule
	{
		public static readonly FsmManager Instance = new FsmManager();

		/// <summary>
		/// 状态机系统
		/// </summary>
		public readonly FsmSystem InternalSystem = new FsmSystem();

		/// <summary>
		/// 初始运行状态
		/// </summary>
		private int _runState;


		private FsmManager()
		{
		}
		public void Awake()
		{
		}
		public void Start()
		{
			InternalSystem.Run(_runState);
		}
		public void Update()
		{
			InternalSystem.Update();
		}
		public void LateUpdate()
		{
		}
		public void OnGUI()
		{
			DebugConsole.GUILable($"[{nameof(FsmManager)}] FSM : {InternalSystem.RunStateType}");
		}


		/// <summary>
		/// 添加一个状态节点
		/// </summary>
		public void AddState(FsmState state)
		{
			InternalSystem.AddState(state);
		}

		/// <summary>
		/// 改变状态机状态
		/// </summary>
		public void ChangeState(int stateType)
		{
			InternalSystem.ChangeState(stateType);
		}

		/// <summary>
		/// 设置初始运行状态
		/// </summary>
		public void SetDefaultRunState(int runStateType)
		{
			_runState = runStateType;
		}
	}
}