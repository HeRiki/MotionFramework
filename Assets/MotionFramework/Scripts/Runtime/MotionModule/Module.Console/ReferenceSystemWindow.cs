//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Reference;

namespace MotionFramework.Console
{
	[ConsoleAttribute("引用系统", 104)]
	internal class ReferenceSystemWindow : IConsoleWindow
	{
		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;

		public void OnCreate()
		{
		}
		public void OnGUI()
		{
			var pools = ReferenceSystem.Instance.GetAllPools;
			AppConsole.GUILable($"池总数：{pools.Count}");

			_scrollPos = AppConsole.GUIBeginScrollView(_scrollPos, 30);
			foreach (var pair in pools)
			{
				AppConsole.GUILable($"[{pair.Value.ClassType.FullName}] CacheCount = {pair.Value.Count} SpwanCount = {pair.Value.SpawnCount}");
			}
			AppConsole.GUIEndScrollView();
		}
	}
}