//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework.Resource
{
	/// <summary>
	/// 资源文件类型
	/// </summary>
	public enum EAssetFileType
	{
		/// <summary>
		/// 主资源
		/// </summary>
		MainAsset,

		/// <summary>
		/// 场景资源
		/// </summary>
		SceneAsset,

		/// <summary>
		/// 资源包
		/// </summary>
		Package,
	}

	/// <summary>
	/// 资源系统模式
	/// </summary>
	public enum EAssetSystemMode
	{
		/// <summary>
		/// AssetDatabase加载模式
		/// </summary>
		EditorMode,

		/// <summary>
		/// Resource加载模式
		/// </summary>
		ResourceMode,

		/// <summary>
		/// AssetBundle加载模式
		/// </summary>
		BundleMode,
	}

	/// <summary>
	/// 资源文件加载器的状态
	/// </summary>
	public enum EAssetFileLoaderStates
	{
		None = 0,
		LoadDepends,
		CheckDepends,
		LoadAssetFile,
		CheckAssetFile,
		LoadAssetFileOK,
		LoadAssetFileFailed,
	}

	/// <summary>
	/// 资源提供者的状态
	/// </summary>
	public enum EAssetProviderStates
	{
		None = 0,
		Loading,
		Checking,
		Succeed,
		Failed,
	}
}