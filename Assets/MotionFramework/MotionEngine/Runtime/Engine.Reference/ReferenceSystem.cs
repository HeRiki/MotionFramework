//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

namespace MotionFramework.Reference
{
	/// <summary>
	/// 引用系统
	/// </summary>
	public class ReferenceSystem
	{
		public readonly static ReferenceSystem Instance = new ReferenceSystem();

		private readonly Dictionary<Type, ReferencePool> _pools = new Dictionary<Type, ReferencePool>();

		/// <summary>
		/// 对象池初始容量
		/// </summary>
		public int InitCapacity = 100;

		/// <summary>
		/// 对象池的数量
		/// </summary>
		public int Count
		{
			get
			{
				return _pools.Count;
			}
		}


		private ReferenceSystem()
		{
		}

		/// <summary>
		/// 清除所有对象池
		/// </summary>
		public void ClearAll()
		{
			foreach (KeyValuePair<Type, ReferencePool> pair in _pools)
			{
				pair.Value.Clear();
			}
			_pools.Clear();
		}

		/// <summary>
		/// 申请引用对象
		/// </summary>
		public IReference Spawn(Type type)
		{
			if (_pools.ContainsKey(type) == false)
			{
				_pools.Add(type, new ReferencePool(type, InitCapacity));
			}
			return _pools[type].Spawn();
		}

		/// <summary>
		/// 申请引用对象
		/// </summary>
		public T Spawn<T>() where T : class, IReference, new()
		{
			Type type = typeof(T);
			return Spawn(type) as T;
		}

		/// <summary>
		/// 回收引用对象
		/// </summary>
		public void Release(IReference item)
		{
			Type type = item.GetType();
			if (_pools.ContainsKey(type) == false)
			{
				_pools.Add(type, new ReferencePool(type, InitCapacity));
			}
			_pools[type].Release(item);
		}

		/// <summary>
		/// 批量回收列表集合
		/// </summary>
		public void Release<T>(List<T> items) where T : class, IReference, new()
		{
			Type type = typeof(T);
			if (_pools.ContainsKey(type) == false)
			{
				_pools.Add(type, new ReferencePool(type, InitCapacity));
			}

			for (int i = 0; i < items.Count; i++)
			{
				_pools[type].Release(items[i]);
			}
		}

		/// <summary>
		/// 批量回收数组集合
		/// </summary>
		public void Release<T>(T[] items) where T : class, IReference, new()
		{
			Type type = typeof(T);
			if (_pools.ContainsKey(type) == false)
			{
				_pools.Add(type, new ReferencePool(type, InitCapacity));
			}

			for (int i = 0; i < items.Length; i++)
			{
				_pools[type].Release(items[i]);
			}
		}

		#region 调试专属方法
		public Dictionary<Type, ReferencePool> DebugGetAllPools
		{
			get { return _pools; }
		}
		#endregion
	}
}