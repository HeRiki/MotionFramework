//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework
{
	public interface IModule
	{
		/// <summary>
		/// 创建模块
		/// </summary>
		void OnCreate(System.Object createParam);

		/// <summary>
		/// 启动模块（在首次Update之前被调用，仅被执行一次）
		/// </summary>
		void OnStart();

		/// <summary>
		/// 轮询模块
		/// </summary>
		void OnUpdate();

		/// <summary>
		/// GUI绘制（可以显示模块的一些关键信息）
		/// </summary>
		void OnGUI();
	}
}