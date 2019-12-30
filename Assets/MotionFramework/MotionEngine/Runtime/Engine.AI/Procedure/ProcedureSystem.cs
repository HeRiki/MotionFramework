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
		/// <summary>
		/// 类型列表
		/// </summary>
		private List<int> _types = new List<int>();

		/// <summary>
		/// 状态机系统
		/// </summary>
		private FsmSystem _system = new FsmSystem();


		public ProcedureSystem()
		{
			// 流程系统不用检测转换关系
			_system.IsCheckRelation = false;
		}

		/// <summary>
		/// 运行流程系统
		/// </summary>
		public void Run()
		{
			if (_types.Count > 0)
				_system.Run(_types[0]);
			else
				LogSystem.Log(ELogType.Warning, "Procedure system dont has any procedure.");
		}

		/// <summary>
		/// 更新流程系统
		/// </summary>
		public void Update()
		{
			_system.Update();
		}

		/// <summary>
		/// 创建并自动添加一个流程
		/// 注意添加的先后顺序
		/// </summary>
		public void AddProcedure(FsmState procedure)
		{
			_system.AddState(procedure);
			if (_types.Contains(procedure.Type) == false)
				_types.Add(procedure.Type);
		}

		/// <summary>
		/// 当前流程类型
		/// </summary>
		public int Current()
		{
			return _system.RunStateType;
		}

		/// <summary>
		/// 切换流程
		/// </summary>
		public void Switch(int procedure)
		{
			_system.ChangeState(procedure);
		}

		/// <summary>
		/// 切换至下一流程
		/// </summary>
		public void SwitchNext()
		{
			int index = _types.IndexOf(_system.RunStateType);
			if (index >= _types.Count - 1)
			{
				LogSystem.Log(ELogType.Warning, $"Current procedure {_system.RunStateType} is the final procedure.");
			}
			else
			{
				Switch(_types[index + 1]);
			}
		}

		/// <summary>
		/// 切换至上一流程
		/// </summary>
		public void SwitchLast()
		{
			int index = _types.IndexOf(_system.RunStateType);
			if (index <= 0)
			{
				LogSystem.Log(ELogType.Warning, $"Current procedure {_system.RunStateType} is the first procedure.");
			}
			else
			{
				Switch(_types[index - 1]);
			}
		}
	}
}