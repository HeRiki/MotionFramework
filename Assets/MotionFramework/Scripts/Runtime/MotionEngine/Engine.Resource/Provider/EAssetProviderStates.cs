//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework.Resource
{
	/// <summary>
	/// 资源提供者的状态
	/// </summary>
	public enum EAssetProviderStates
	{
		None = 0,
		Loading,
		Checking,
		Success,
		Fail,
	}
}