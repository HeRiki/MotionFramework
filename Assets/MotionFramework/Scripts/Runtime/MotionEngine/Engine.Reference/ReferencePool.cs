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
	internal class ReferencePool
	{
		private readonly Queue<IReference> _pool;

		/// <summary>
		/// 引用类型
		/// </summary>
		public Type ClassType { private set; get; }


		public ReferencePool(Type type, int capacity)
		{
			ClassType = type;

			// 创建缓存池
			_pool = new Queue<IReference>(capacity);

			// 检测是否继承了专属接口
			Type temp = type.GetInterface(nameof(IReference));
			if (temp == null)
				throw new Exception($"{type.Name} need to inherit form IReference");
		}

		/// <summary>
		/// 内部缓存总数
		/// </summary>
		public int Count
		{
			get { return _pool.Count; }
		}

		/// <summary>
		/// 外部使用总数
		/// </summary>
		public int SpawnCount { private set; get; }

		/// <summary>
		/// 申请引用对象
		/// </summary>
		public IReference Spawn()
		{
			IReference item;
			if (_pool.Count > 0)
			{
				item = _pool.Dequeue();
			}
			else
			{
				item = Activator.CreateInstance(ClassType) as IReference;
			}
			SpawnCount++;
			return item;
		}

		/// <summary>
		/// 回收引用对象
		/// </summary>
		public void Release(IReference item)
		{
			if (item == null)
				return;

			if (item.GetType() != ClassType)
				throw new Exception($"Invalid type {item.GetType()}");

			if (_pool.Contains(item))
				throw new Exception($"The item {item.GetType()} already exists.");

			SpawnCount--;
			item.OnRelease();
			_pool.Enqueue(item);
		}

		/// <summary>
		/// 清空对象池
		/// </summary>
		public void Clear()
		{
			_pool.Clear();
			SpawnCount = 0;
		}
	}
}