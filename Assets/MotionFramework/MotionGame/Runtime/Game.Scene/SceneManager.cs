﻿//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Debug;
using MotionFramework.Resource;

namespace MotionFramework.Scene
{
	/// <summary>
	/// 场景管理器
	/// </summary>
	public sealed class SceneManager : IGameModule
	{
		public static readonly SceneManager Instance = new SceneManager();

		private AssetScene _mainScene;
		private readonly List<AssetScene> _additionScenes = new List<AssetScene>();


		private SceneManager()
		{
		}
		public void Awake()
		{
		}
		public void Start()
		{
		}
		public void Update()
		{
		}
		public void LateUpdate()
		{
		}
		public void OnGUI()
		{
			string mainSceneName = string.Empty;
			if (_mainScene != null)
				mainSceneName = _mainScene.Location;
			DebugConsole.GUILable($"[{nameof(SceneManager)}] Main scene : {mainSceneName}");
			DebugConsole.GUILable($"[{nameof(SceneManager)}] Addition scene count : {_additionScenes.Count}");
		}


		/// <summary>
		/// 切换主场景，之前的主场景以及附加场景将会被卸载
		/// </summary>
		/// <param name="location">场景资源地址</param>
		/// <param name="activeOnLoad">加载完成时是否激活场景</param>
		/// <param name="callback">场景加载完毕的回调</param>
		public void ChangeMainScene(string location, bool activeOnLoad, System.Action<SceneInstance> callback)
		{
			if (_mainScene != null)
			{
				UnLoadAllAdditionScenes();
				_mainScene.UnLoad();
				_mainScene = null;
			}

			_mainScene = new AssetScene(location);
			_mainScene.Load(false, activeOnLoad, callback);
		}

		/// <summary>
		/// 在当前主场景的基础上加载附加场景
		/// </summary>
		/// <param name="location">场景资源地址</param>
		/// <param name="activeOnLoad">加载完成时是否激活场景</param>
		/// <param name="callback">场景加载完毕的回调</param>
		public void LoadAdditionScene(string location, bool activeOnLoad, System.Action<SceneInstance> callback)
		{
			AssetScene scene = TryGetAdditionScene(location);
			if (scene != null)
			{
				LogSystem.Log(ELogType.Warning, $"The addition scene {location} is already load.");
				return;
			}

			AssetScene newScene = new AssetScene(location);
			_additionScenes.Add(newScene);
			newScene.Load(true, activeOnLoad, callback);
		}

		/// <summary>
		/// 获取场景当前的加载进度，如果场景不存在返回0
		/// </summary>
		public int GetSceneLoadProgress(string location)
		{
			if (_mainScene != null)
			{
				if (_mainScene.Location == location)
					return _mainScene.Progress;
			}

			AssetScene scene = TryGetAdditionScene(location);
			if (scene != null)
				return scene.Progress;

			LogSystem.Log(ELogType.Warning, $"Not found scene {location}");
			return 0;
		}

		/// <summary>
		/// 检测场景是否加载完毕，如果场景不存在返回false
		/// </summary>
		public bool CheckSceneIsDone(string location)
		{
			if (_mainScene != null)
			{
				if (_mainScene.Location == location)
					return _mainScene.IsDone;
			}

			AssetScene scene = TryGetAdditionScene(location);
			if (scene != null)
				return scene.IsDone;

			LogSystem.Log(ELogType.Warning, $"Not found scene {location}");
			return false;
		}


		// 卸载所有附加场景
		private void UnLoadAllAdditionScenes()
		{
			for (int i = 0; i < _additionScenes.Count; i++)
			{
				_additionScenes[i].UnLoad();
			}
			_additionScenes.Clear();
		}

		// 尝试获取一个附加场景，如果不存在返回NULL
		private AssetScene TryGetAdditionScene(string location)
		{
			for (int i = 0; i < _additionScenes.Count; i++)
			{
				if (_additionScenes[i].Location == location)
					return _additionScenes[i];
			}
			return null;
		}
	}
}