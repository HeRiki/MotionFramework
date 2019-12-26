//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class BuildSettingData
{
	public static BuildSetting Setting;

	static BuildSettingData()
	{
		// 加载配置文件
		Setting = AssetDatabase.LoadAssetAtPath<BuildSetting>(EditorDefine.BuilderSettingFilePath);
		if (Setting == null)
		{
			Debug.LogWarning($"Create new BuildSetting.asset : {EditorDefine.BuilderSettingFilePath}");
			Setting = ScriptableObject.CreateInstance<BuildSetting>();
			EditorTools.CreateFileDirectory(EditorDefine.BuilderSettingFilePath);
			AssetDatabase.CreateAsset(Setting, EditorDefine.BuilderSettingFilePath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
		else
		{
			Debug.Log("Load BuildSetting.asset ok");
		}
	}

	/// <summary>
	/// 存储文件
	/// </summary>
	public static void SaveFile()
	{
		if (Setting != null)
		{
			EditorUtility.SetDirty(Setting);
			AssetDatabase.SaveAssets();
		}
	}

	/// <summary>
	/// 添加元素
	/// </summary>
	public static void AddElement(string folderPath)
	{
		if (IsContainsElement(folderPath) == false)
		{
			BuildSetting.Wrapper element = new BuildSetting.Wrapper();
			element.FolderPath = folderPath;
			Setting.Elements.Add(element);
			SaveFile();
		}
	}

	/// <summary>
	/// 移除元素
	/// </summary>
	public static void RemoveElement(string folderPath)
	{
		for (int i = 0; i < Setting.Elements.Count; i++)
		{
			if (Setting.Elements[i].FolderPath == folderPath)
			{
				Setting.Elements.RemoveAt(i);
				break;
			}
		}
		SaveFile();
	}

	/// <summary>
	/// 编辑元素
	/// </summary>
	public static void ModifyElement(string folderPath, BuildSetting.EFolderPackRule packRule, BuildSetting.EBundleNameRule nameRule)
	{
		// 注意：这里强制修改忽略文件夹的命名规则为None
		if (packRule == BuildSetting.EFolderPackRule.Ignore)
		{
			nameRule = BuildSetting.EBundleNameRule.None;
		}
		else
		{
			if (nameRule == BuildSetting.EBundleNameRule.None)
				nameRule = BuildSetting.EBundleNameRule.TagByFilePath;
		}

		for (int i = 0; i < Setting.Elements.Count; i++)
		{
			if (Setting.Elements[i].FolderPath == folderPath)
			{
				Setting.Elements[i].PackRule = packRule;
				Setting.Elements[i].NameRule = nameRule;
				break;
			}
		}
		SaveFile();
	}

	/// <summary>
	/// 是否包含元素
	/// </summary>
	public static bool IsContainsElement(string folderPath)
	{
		for (int i = 0; i < Setting.Elements.Count; i++)
		{
			if (Setting.Elements[i].FolderPath == folderPath)
				return true;
		}
		return false;
	}

	/// <summary>
	/// 获取所有的打包路径
	/// </summary>
	public static List<string> GetAllCollectPath()
	{
		List<string> result = new List<string>();
		for (int i = 0; i < Setting.Elements.Count; i++)
		{
			BuildSetting.Wrapper wrapper = Setting.Elements[i];
			if (wrapper.PackRule == BuildSetting.EFolderPackRule.Collect)
				result.Add(wrapper.FolderPath);
		}
		return result;
	}

	/// <summary>
	/// 是否收集该资源
	/// </summary>
	public static bool IsCollectAsset(string assetPath)
	{
		for (int i = 0; i < Setting.Elements.Count; i++)
		{
			BuildSetting.Wrapper wrapper = Setting.Elements[i];
			if (wrapper.PackRule == BuildSetting.EFolderPackRule.Collect)
			{
				if (assetPath.StartsWith(wrapper.FolderPath))
					return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 是否忽略该资源
	/// </summary>
	public static bool IsIgnoreAsset(string assetPath)
	{
		for (int i = 0; i < Setting.Elements.Count; i++)
		{
			BuildSetting.Wrapper wrapper = Setting.Elements[i];
			if (wrapper.PackRule == BuildSetting.EFolderPackRule.Ignore)
			{
				if (assetPath.StartsWith(wrapper.FolderPath))
					return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 获取资源的打包标签名称
	/// </summary>
	public static string GetAssetTagName(string assetPath)
	{
		// 注意：一个资源有可能被多个规则覆盖
		List<BuildSetting.Wrapper> filterWrappers = new List<BuildSetting.Wrapper>();
		for (int i = 0; i < Setting.Elements.Count; i++)
		{
			BuildSetting.Wrapper wrapper = Setting.Elements[i];
			if (assetPath.StartsWith(wrapper.FolderPath))
			{
				filterWrappers.Add(wrapper);
			}
		}

		// 我们使用路径最深层的规则
		BuildSetting.Wrapper findWrapper = null;
		for (int i = 0; i < filterWrappers.Count; i++)
		{
			BuildSetting.Wrapper wrapper = filterWrappers[i];
			if (findWrapper == null)
			{
				findWrapper = wrapper;
				continue;
			}
			if (wrapper.FolderPath.Length > findWrapper.FolderPath.Length)
				findWrapper = wrapper;
		}

		// 如果没有找到命名规则
		if (findWrapper == null)
		{
			return assetPath.Remove(assetPath.LastIndexOf("."));
		}

		// 根据规则设置获取标签名称
		if (findWrapper.NameRule == BuildSetting.EBundleNameRule.None)
		{
			// 注意：如果依赖资源来自于忽略文件夹，那么会触发这个异常
			throw new Exception($"BuildSetting has depend asset in ignore folder : {findWrapper.FolderPath}");
		}
		else if (findWrapper.NameRule == BuildSetting.EBundleNameRule.TagByFileName)
		{
			return Path.GetFileNameWithoutExtension(assetPath);
		}
		else if (findWrapper.NameRule == BuildSetting.EBundleNameRule.TagByFilePath)
		{
			return assetPath.Remove(assetPath.LastIndexOf("."));
		}
		else if (findWrapper.NameRule == BuildSetting.EBundleNameRule.TagByFolderName)
		{
			string temp = Path.GetDirectoryName(assetPath);
			return Path.GetFileName(temp);
		}
		else if (findWrapper.NameRule == BuildSetting.EBundleNameRule.TagByFolderPath)
		{
			return Path.GetDirectoryName(assetPath);
		}
		else
		{
			throw new NotImplementedException($"{findWrapper.NameRule}");
		}
	}
}