//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework.Resource
{
	/// <summary>
	/// 资源提供者
	/// </summary>
	internal interface IAssetProvider
	{
		/// <summary>
		/// 资源对象的名称
		/// </summary>
		string AssetName { get; }

		/// <summary>
		/// 资源对象的类型
		/// </summary>
		System.Type AssetType { get; }

		/// <summary>
		/// 最终结果
		/// </summary>
		System.Object Result { get; }

		/// <summary>
		/// 当前的加载状态
		/// </summary>
		EAssetProviderStates States { get; }

		/// <summary>
		/// 资源操作句柄
		/// </summary>
		AssetOperationHandle Handle { get; }

		/// <summary>
		/// 用户请求的回调
		/// </summary>
		System.Action<AssetOperationHandle> Callback { set; get; }

		/// <summary>
		/// 加载进度
		/// </summary>
		float Progress { get; }

		/// <summary>
		/// 是否完毕（成功或失败）
		/// </summary>
		bool IsDone { get; }

		/// <summary>
		/// 轮询更新方法
		/// </summary>
		void Update();
	}
}