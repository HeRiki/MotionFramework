//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Resource;
using MotionFramework.Reference;

namespace MotionFramework.Console
{
	[ConsoleAttribute("资源系统", 103)]
	internal class AssetSystemWindow : IConsoleWindow
	{
		private class InfoWrapper : IReference, IComparer<InfoWrapper>, IComparable<InfoWrapper>
		{
			public string Info;
			public EAssetFileLoaderStates LoadState;
			public int ProviderFailedCount;

			public void OnRelease()
			{
				Info = string.Empty;
				LoadState = EAssetFileLoaderStates.None;
				ProviderFailedCount = 0;
			}
			public int CompareTo(InfoWrapper other)
			{
				return Compare(this, other);
			}
			public int Compare(InfoWrapper a, InfoWrapper b)
			{
				return string.CompareOrdinal(a.Info, b.Info);
			}
		}

		/// <summary>
		/// 加载器总数
		/// </summary>
		private int _loaderTotalCount = 0;

		/// <summary>
		/// 显示信息集合
		/// </summary>
		private List<InfoWrapper> _cacheInfos = new List<InfoWrapper>(1000);

		/// <summary>
		/// 过滤的关键字
		/// </summary>
		private string _filterKey = string.Empty;

		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;


		public void OnCreate()
		{
		}
		public void OnGUI()
		{
			// 过滤信息
			FilterInfos();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("搜索关键字 : ", ConsoleSystem.GUILableStyle, GUILayout.Width(140));
				_filterKey = GUILayout.TextField(_filterKey, ConsoleSystem.GUITextFieldStyle, GUILayout.Width(400));
			}
			GUILayout.EndHorizontal();

			ConsoleSystem.GUILable($"加载器总数：{_loaderTotalCount}");

			_scrollPos = ConsoleSystem.GUIBeginScrollView(_scrollPos, 80);
			for (int i = 0; i < _cacheInfos.Count; i++)
			{
				var element = _cacheInfos[i];
				if (element.LoadState == EAssetFileLoaderStates.LoadAssetFileFail || element.ProviderFailedCount > 0)
					ConsoleSystem.GUIRedLable(element.Info);
				else
					ConsoleSystem.GUILable(element.Info);
			}
			ConsoleSystem.GUIEndScrollView();
		}
		private void FilterInfos()
		{
			// 回收引用
			ReferenceSystem.Release(_cacheInfos);

			// 清空列表
			_cacheInfos.Clear();

			// 绘制显示列表
			var fileLoaders = AssetSystem.GetAllLoaders();
			_loaderTotalCount = fileLoaders.Count;
			foreach (var loader in fileLoaders)
			{
				// 只搜索关键字
				if (string.IsNullOrEmpty(_filterKey) == false)
				{
					if (loader.LoadPath.Contains(_filterKey) == false)
						continue;
				}

				string info = Substring(loader.LoadPath, "/assets/");
				info = info.Replace(".unity3d", string.Empty);
				info = $"{info} = {loader.RefCount}";

				InfoWrapper element = ReferenceSystem.Spawn<InfoWrapper>();
				element.Info = info;
				element.LoadState = loader.States;
				element.ProviderFailedCount = loader.GetFailedProviderCount();

				// 添加到显示列表
				_cacheInfos.Add(element);
			}

			// 重新排序
			_cacheInfos.Sort();
		}
		private string Substring(string content, string key)
		{
			// 返回子字符串第一次出现位置	
			int startIndex = content.IndexOf(key);

			// 如果没有找到匹配的关键字
			if (startIndex == -1)
				return content;

			return content.Substring(startIndex + key.Length);
		}
	}
}