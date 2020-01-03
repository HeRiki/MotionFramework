//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionFramework.Pool
{
	/// <summary>
	/// 游戏对象池管理器
	/// </summary>
	public sealed class PoolManager : ModuleSingleton<PoolManager>, IMotionModule
	{
		private readonly Dictionary<string, GameObjectPool> _pools = new Dictionary<string, GameObjectPool>();
		private GameObject _root;


		void IMotionModule.OnCreate(object createParam)
		{
			_root = new GameObject("[PoolManager]");
			_root.transform.position = Vector3.zero;
			_root.transform.eulerAngles = Vector3.zero;
			UnityEngine.Object.DontDestroyOnLoad(_root);
		}
		void IMotionModule.OnStart()
		{
		}
		void IMotionModule.OnUpdate()
		{
		}
		void IMotionModule.OnGUI()
		{
		}

		/// <summary>
		/// 创建指定资源的游戏对象池
		/// </summary>
		public void CreatePool(string location, int capacity)
		{
			if (_pools.ContainsKey(location))
			{
				AppLog.Log(ELogType.Warning, $"Asset is already existed : {location}");
				return;
			}
			CreatePoolInternal(location, capacity);
		}
		private GameObjectPool CreatePoolInternal(string location, int capacity)
		{
			GameObjectPool pool = new GameObjectPool(_root.transform, location, capacity);
			_pools.Add(location, pool);
			return pool;
		}

		/// <summary>
		/// 是否都已经加载完毕
		/// </summary>
		public bool IsAllDone()
		{
			foreach (var pair in _pools)
			{
				if (pair.Value.IsDone == false)
					return false;
			}
			return true;
		}

		/// <summary>
		/// 销毁所有对象池及其资源
		/// </summary>
		public void DestroyAll()
		{
			foreach (var pair in _pools)
			{
				pair.Value.Destroy();
			}
			_pools.Clear();
		}

		/// <summary>
		/// 异步方式获取一个游戏对象
		/// </summary>
		public void Spawn(string location, Action<GameObject> callbcak)
		{
			if (_pools.ContainsKey(location))
			{
				_pools[location].Spawn(callbcak);
			}
			else
			{
				// 如果不存在创建游戏对象池
				GameObjectPool pool = CreatePoolInternal(location, 0);
				pool.Spawn(callbcak);
			}
		}

		/// <summary>
		/// 同步方式获取一个游戏对象
		/// </summary>
		public GameObject Spawn(string location)
		{
			if (_pools.ContainsKey(location))
			{
				return _pools[location].Spawn();
			}
			else
			{
				// 如果不存在创建游戏对象池
				GameObjectPool pool = CreatePoolInternal(location, 0);
				return pool.Spawn();
			}
		}

		/// <summary>
		/// 回收一个游戏对象
		/// </summary>
		public void Restore(string location, GameObject obj)
		{
			if (obj == null)
				return;

			if (_pools.ContainsKey(location))
			{
				_pools[location].Restore(obj);
			}
			else
			{
				AppLog.Log(ELogType.Error, $"GameObjectPool does not exist : {location}");
			}
		}

		#region 调试专属方法
		internal Dictionary<string, GameObjectPool> GetAllPools
		{
			get { return _pools; }
		}
		#endregion
	}
}