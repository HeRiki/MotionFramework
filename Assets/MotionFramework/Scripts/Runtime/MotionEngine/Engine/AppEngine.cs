//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionFramework
{
	public class AppEngine : IMotionEngine
	{
		public static readonly AppEngine Instance = new AppEngine();

		/// <summary>
		/// 模块封装类
		/// </summary>
		private class ModuleWrapper
		{
			public int Priority { private set; get; }
			public IMotionModule Module { private set; get; }

			public ModuleWrapper(IMotionModule module, int priority)
			{
				Module = module;
				Priority = priority;
			}
		}

		private readonly List<ModuleWrapper> _coms = new List<ModuleWrapper>(100);
		private MonoBehaviour _behaviour;
		private bool _isDirty = false;


		/// <summary>
		/// 私有构造函数
		/// </summary>
		private AppEngine()
		{
		}

		/// <summary>
		/// 查询游戏模块是否存在
		/// </summary>
		public bool Contains(System.Type moduleType)
		{
			for (int i = 0; i < _coms.Count; i++)
			{
				if (_coms[i].Module.GetType() == moduleType)
					return true;
			}
			return false;
		}

		/// <summary>
		/// 创建游戏模块
		/// </summary>
		/// <typeparam name="T">模块类</typeparam>
		/// <param name="priority">运行时的优先级，优先级越大越早执行。如果没有设置优先级，那么会按照添加顺序执行</param>
		public T CreateModule<T>(int priority = 0) where T : class, IMotionModule
		{
			return CreateModule<T>(null, priority);
		}

		/// <summary>
		/// 创建游戏模块
		/// </summary>
		/// <typeparam name="T">模块类</typeparam>
		/// <param name="createParam">创建参数</param>
		/// <param name="priority">运行时的优先级，优先级越大越早执行。如果没有设置优先级，那么会按照添加顺序执行</param>
		public T CreateModule<T>(System.Object createParam, int priority = 0) where T : class, IMotionModule
		{
			if (priority < 0)
				throw new Exception("The priority can not be negative");

			if (Contains(typeof(T)))
				throw new Exception($"Game module {typeof(T)} is already existed");

			// 如果没有设置优先级
			if (priority == 0)
			{
				int minPriority = GetMinPriority();
				priority = --minPriority;
			}

			AppLog.Log(ELogType.Log, $"Create game module : {typeof(T)}");
			T module = Activator.CreateInstance<T>();
			ModuleWrapper wrapper = new ModuleWrapper(module, priority);
			wrapper.Module.OnCreate(createParam);
			_coms.Add(wrapper);
			_isDirty = true;
			return module;
		}

		/// <summary>
		/// 获取游戏模块
		/// </summary>
		/// <typeparam name="T">模块类</typeparam>
		public T GetModule<T>() where T : class, IMotionModule
		{
			System.Type type = typeof(T);
			for (int i = 0; i < _coms.Count; i++)
			{
				if (_coms[i].Module.GetType() == type)
					return _coms[i].Module as T;
			}

			AppLog.Log(ELogType.Warning, $"Not found game module {type}");
			return null;
		}

		/// <summary>
		/// 获取当前模块里最小的优先级
		/// </summary>
		private int GetMinPriority()
		{
			int minPriority = 0;
			for (int i = 0; i < _coms.Count; i++)
			{
				if (_coms[i].Priority < minPriority)
					minPriority = _coms[i].Priority;
			}
			return minPriority; //小于等于零
		}

		void IMotionEngine.Initialize(MonoBehaviour behaviour)
		{
			if (_behaviour != null)
				throw new Exception($"{nameof(AppEngine)} is already initialized.");

			_behaviour = behaviour;
		}
		void IMotionEngine.OnUpdate()
		{
			// 如果有新模块需要重新排序
			if (_isDirty)
			{
				_isDirty = false;
				_coms.Sort((left, right) =>
				{
					if (left.Priority > right.Priority)
						return -1;
					else if (left.Priority == right.Priority)
						return 0;
					else
						return 1;
				});
			}

			// 轮询所有模块
			for (int i = 0; i < _coms.Count; i++)
			{
				_coms[i].Module.OnUpdate();
			}
		}
		void IMotionEngine.OnGUI()
		{
			for (int i = 0; i < _coms.Count; i++)
			{
				_coms[i].Module.OnGUI();
			}
		}

		#region 协程相关
		/// <summary>
		/// 开启一个协程
		/// </summary>
		public Coroutine StartCoroutine(IEnumerator coroutine)
		{
			if (_behaviour == null)
				throw new Exception($"{nameof(AppEngine)} is not initialize. Use AppEngine.Initialize");
			return _behaviour.StartCoroutine(coroutine);
		}

		/// <summary>
		/// 停止一个协程
		/// </summary>
		/// <param name="coroutine"></param>
		public void StopCoroutine(Coroutine coroutine)
		{
			if (_behaviour == null)
				throw new Exception($"{nameof(AppEngine)} is not initialize. Use AppEngine.Initialize");
			_behaviour.StopCoroutine(coroutine);
		}

		/// <summary>
		/// 停止所有协程
		/// </summary>
		public void StopAllCoroutines()
		{
			if (_behaviour == null)
				throw new Exception($"{nameof(AppEngine)} is not initialize. Use AppEngine.Initialize");
			_behaviour.StopAllCoroutines();
		}
		#endregion
	}
}