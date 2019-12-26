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

namespace MotionFramework.Debug
{
	[DebugAttribute("实体池", 105)]
	internal class AssetPoolWindow : IDebugWindow
	{
		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;

		public void OnInit()
		{
		}
		public void OnGUI()
		{
			var pools = PoolManager.Instance.DebugAllPools;
			DebugConsole.GUILable($"池总数：{pools.Count}");

			_scrollPos = DebugConsole.GUIBeginScrollView(_scrollPos, 30);
			foreach (var pair in pools)
			{
				string content = $"[{pair.Value.Location}] CacheCount = {pair.Value.Count} SpwanCount = {pair.Value.SpawnCount}";
				if (pair.Value.States == EAssetProviderStates.Failed)
					DebugConsole.GUIRedLable(content);
				else
					DebugConsole.GUILable(content);
			}
			DebugConsole.GUIEndScrollView();
		}
	}
}