//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionFramework.Debug
{
	[DebugAttribute("模块", 100)]
	internal class GameModuleWindow : IDebugWindow
	{
		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;

		public void OnInit()
		{
		}
		public void OnGUI()
		{
			_scrollPos = DebugConsole.GUIBeginScrollView(_scrollPos, 0);
			IEngine motionEngine = AppEngine.Instance;
			motionEngine.OnGUI();
			DebugConsole.GUIEndScrollView();
		}
	}
}