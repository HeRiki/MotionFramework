//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Utility;
using UnityEngine;

namespace MotionFramework.Console
{
	/// <summary>
	/// 控制台
	/// </summary>
	public static class AppConsole
	{
		/// <summary>
		/// 窗口封装类
		/// </summary>
		private class WindowWrapper : IComparer<WindowWrapper>, IComparable<WindowWrapper>
		{
			public Type ClassType;
			public string Title;
			public int Priority;
			public IConsoleWindow Instance;

			public int CompareTo(WindowWrapper other)
			{
				return Compare(this, other);
			}
			public int Compare(WindowWrapper a, WindowWrapper b)
			{
				return a.Priority.CompareTo(b.Priority);
			}
		}

		/// <summary>
		/// 控制台节点列表
		/// </summary>
		private readonly static List<WindowWrapper> _wrappers = new List<WindowWrapper>();

		// GUI相关
		private static bool _visibleToggle = false;
		private static int _showIndex = 0;
		private static Texture _bgTexture;
		private static string[] _toolbarTitles;

		/// <summary>
		/// 初始化控制台
		/// </summary>
		public static void Initialize()
		{
			// 加载背景纹理
			_bgTexture = Resources.Load<Texture>("builtin_background");
			if (_bgTexture == null)
				UnityEngine.Debug.LogWarning("Not found builtin_background texture in Resources folder.");

			// 获取所有调试类
			List<Type> allTypes = AssemblyUtility.GetAssignableAttributeTypes(typeof(IConsoleWindow), typeof(ConsoleAttribute));
			for (int i = 0; i < allTypes.Count; i++)
			{
				ConsoleAttribute attribute = (ConsoleAttribute)Attribute.GetCustomAttribute(allTypes[i], typeof(ConsoleAttribute));
				WindowWrapper wrapper = new WindowWrapper()
				{
					ClassType = allTypes[i],
					Title = attribute.Title,
					Priority = attribute.Order,
				};
				_wrappers.Add(wrapper);
			}

			// 根据优先级排序
			_wrappers.Sort();

			// 创建实例类
			for (int i = 0; i < _wrappers.Count; i++)
			{
				WindowWrapper wrapper = _wrappers[i];
				wrapper.Instance = (IConsoleWindow)Activator.CreateInstance(wrapper.ClassType);
				wrapper.Instance.OnCreate();
			}

			// 标题列表
			List<string> titles = new List<string>();
			for (int i = 0; i < _wrappers.Count; i++)
			{
				titles.Add(_wrappers[i].Title);
			}
			_toolbarTitles = titles.ToArray();
		}

		/// <summary>
		/// 绘制GUI
		/// </summary>
		public static void DrawGUI()
		{
			// 注意：GUI接口只能在OnGUI内部使用
			InitGlobalStyle();

			GUILayout.BeginHorizontal();
			{
				// 绘制背景
				if (_visibleToggle && _bgTexture != null)
					GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _bgTexture, ScaleMode.StretchToFill, true);

				// 显示开关
				if (GUILayout.Button("X", GUIButtonStyle, GUILayout.Width(GUIButtonStyle.fixedHeight)))
					_visibleToggle = !_visibleToggle;
				if (_visibleToggle == false)
					return;

				// 绘制节点按钮
				_showIndex = GUILayout.Toolbar(_showIndex, _toolbarTitles, GUIToolbarStyle);
			}
			GUILayout.EndHorizontal();

			// 绘制选中节点
			for (int i = 0; i < _wrappers.Count; i++)
			{
				if (_showIndex != i)
					continue;
				WindowWrapper wrapper = _wrappers[i];
				wrapper.Instance.OnGUI();
			}
		}

		#region GUI辅助方法
		private static bool _initGlobalStyle = false;
		public static GUIStyle GUIToolbarStyle { private set; get; }
		public static GUIStyle GUIButtonStyle { private set; get; }
		public static GUIStyle GUIToogleStyle1 { private set; get; }
		public static GUIStyle GUIToogleStyle2 { private set; get; }
		public static GUIStyle GUITextFieldStyle { private set; get; }
		public static GUIStyle GUILableStyle { private set; get; }
		public static GUIStyle GUIRichLabelStyle { private set; get; }
		public static int GUIRichLabelFontSize { private set; get; }

		/// <summary>
		/// 创建一些高度和字体大小固定的控件样式
		/// </summary>
		private static void InitGlobalStyle()
		{
			if (_initGlobalStyle == false)
			{
				_initGlobalStyle = true;

				GUIToolbarStyle = new GUIStyle(GUI.skin.button);
				GUIToolbarStyle.fontSize = 28;
				GUIToolbarStyle.fixedHeight = 40;

				GUIButtonStyle = new GUIStyle(GUI.skin.button);
				GUIButtonStyle.fontSize = 28;
				GUIButtonStyle.fixedHeight = 40;

				GUIToogleStyle1 = new GUIStyle(GUI.skin.button);
				GUIToogleStyle1.fontSize = 26;
				GUIToogleStyle1.fixedHeight = 35;

				GUIToogleStyle2 = new GUIStyle(GUI.skin.box);
				GUIToogleStyle2.fontSize = 26;
				GUIToogleStyle2.fixedHeight = 35;

				GUITextFieldStyle = new GUIStyle(GUI.skin.textField);
				GUITextFieldStyle.fontSize = 22;
				GUITextFieldStyle.fixedHeight = 30;

				GUILableStyle = new GUIStyle(GUI.skin.label);
				GUILableStyle.fontSize = 24;

				GUIRichLabelStyle = GUIStyle.none;
				GUIRichLabelStyle.richText = true;
				GUIRichLabelFontSize = 24;
			}
		}

		public static Vector2 GUIBeginScrollView(Vector2 pos, int fixedViewHeight)
		{
			float scrollWidth = Screen.width;
			float scrollHeight = Screen.height - GUIButtonStyle.fixedHeight - fixedViewHeight - 10;
			return GUILayout.BeginScrollView(pos, GUILayout.Width(scrollWidth), GUILayout.Height(scrollHeight));
		}
		public static void GUIEndScrollView()
		{
			GUILayout.EndScrollView();
		}
		public static bool GUIToggle(string name, bool checkFlag)
		{
			GUIStyle style = checkFlag ? GUIToogleStyle1 : GUIToogleStyle2;
			if (GUILayout.Button(name, style))
			{
				checkFlag = !checkFlag;
			}
			return checkFlag;
		}
		public static void GUILable(string text)
		{
			GUILayout.Label($"<size={GUIRichLabelFontSize}><color=white>{text}</color></size>", GUIRichLabelStyle);
		}
		public static void GUIRedLable(string text)
		{
			GUILayout.Label($"<size={GUIRichLabelFontSize}><color=red>{text}</color></size>", GUIRichLabelStyle);
		}
		public static void GUIYellowLable(string text)
		{
			GUILayout.Label($"<size={GUIRichLabelFontSize}><color=yellow>{text}</color></size>", GUIRichLabelStyle);
		}
		#endregion
	}
}