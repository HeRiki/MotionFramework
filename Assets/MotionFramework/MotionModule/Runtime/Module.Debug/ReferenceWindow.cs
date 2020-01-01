//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Reference;

namespace MotionFramework.Debug
{
	[DebugAttribute("引用池", 104)]
	internal class ReferenceWindow : IDebugWindow
	{
		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;

		public void OnInit()
		{
		}
		public void OnGUI()
		{
			var pools = ReferenceSystem.Instance.DebugGetAllPools;
			DebugConsole.GUILable($"池总数：{pools.Count}");

			_scrollPos = DebugConsole.GUIBeginScrollView(_scrollPos, 30);
			foreach (var pair in pools)
			{
				DebugConsole.GUILable($"[{pair.Value.ClassType.FullName}] CacheCount = {pair.Value.Count} SpwanCount = {pair.Value.SpawnCount}");
			}
			DebugConsole.GUIEndScrollView();
		}
	}
}