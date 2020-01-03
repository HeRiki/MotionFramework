//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionFramework.Editor
{
	[CreateAssetMenu]
	public class BuildSetting : ScriptableObject
	{
		/// <summary>
		/// 文件夹打包规则
		/// </summary>
		[Serializable]
		public enum EFolderPackRule
		{
			/// <summary>
			/// 收集该文件夹
			/// </summary>
			Collect,

			/// <summary>
			/// 忽略该文件夹
			/// </summary>
			Ignore,
		}

		/// <summary>
		/// AssetBundle命名规则
		/// </summary>
		[Serializable]
		public enum EBundleNameRule
		{
			None,

			/// <summary>
			/// 以文件路径命名
			/// </summary>
			TagByFilePath,

			/// <summary>
			/// 以文件夹路径命名（该文件夹下所有文件被打到一个Bundle文件里）
			/// </summary>
			TagByFolderPath,

			/// <summary>
			/// 以文件名称命名
			/// </summary>
			TagByFileName,

			/// <summary>
			/// 以文件夹名称命名（该文件夹下所有文件被打到一个Bundle文件里）
			/// </summary>
			TagByFolderName,
		}

		[Serializable]
		public class Wrapper
		{
			public string FolderPath = string.Empty;
			public EFolderPackRule PackRule = EFolderPackRule.Collect;
			public EBundleNameRule NameRule = EBundleNameRule.TagByFilePath;
		}

		/// <summary>
		/// 打包路径列表
		/// </summary>
		[SerializeField]
		public List<Wrapper> Elements = new List<Wrapper>();
	}
}