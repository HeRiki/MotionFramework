//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework.Resource
{
	/// <summary>
	/// 资源系统模式
	/// </summary>
	public enum EAssetSystemMode
	{
		/// <summary>
		/// AssetDatabase模式
		/// </summary>
		EditorMode,

		/// <summary>
		/// Resources模式
		/// </summary>
		ResourcesMode,

		/// <summary>
		/// AssetBundle模式
		/// </summary>
		BundleMode,
	}

	/// <summary>
	/// 资源文件加载器的状态
	/// </summary>
	internal enum EAssetFileLoaderStates
	{
		None = 0,
		LoadDepends,
		CheckDepends,
		LoadAssetFile,
		CheckAssetFile,
		LoadAssetFileSuccess,
		LoadAssetFileFail,
	}

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