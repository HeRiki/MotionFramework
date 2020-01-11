//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Pool;
using MotionFramework.Resource;

namespace MotionFramework.Console
{
	[ConsoleAttribute("游戏对象池", 105)]
	internal class GameObjectPoolWindow : IConsoleWindow
	{
		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;

		public void OnCreate()
		{
		}
		public void OnGUI()
		{
			var pools = PoolManager.Instance.GetAllPools;
			ConsoleSystem.GUILable($"池总数：{pools.Count}");

			_scrollPos = ConsoleSystem.GUIBeginScrollView(_scrollPos, 30);
			foreach (var pair in pools)
			{
				string content = $"[{pair.Value.Location}] CacheCount = {pair.Value.Count} SpwanCount = {pair.Value.SpawnCount}";
				if (pair.Value.States == EAssetProviderStates.Fail)
					ConsoleSystem.GUIRedLable(content);
				else
					ConsoleSystem.GUILable(content);
			}
			ConsoleSystem.GUIEndScrollView();
		}
	}
}