//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using UnityEngine;

namespace MotionFramework.Resource
{
	public class AssetOperationHandle
	{
		private IAssetProvider _provider;

		internal AssetOperationHandle(IAssetProvider provider)
		{
			_provider = provider;
		}

		/// <summary>
		/// 当前的加载状态
		/// </summary>
		public EAssetProviderStates States
		{
			get { return _provider.States; }
		}

		/// <summary>
		/// 加载进度
		/// </summary>
		public float Progress
		{
			get
			{
				return _provider.Progress;
			}
		}

		/// <summary>
		/// 是否加载完毕
		/// </summary>
		public bool IsDone
		{
			get
			{
				return _provider.IsDone;
			}
		}

		/// <summary>
		/// 完成委托
		/// </summary>
		public event System.Action<AssetOperationHandle> Completed
		{
			add
			{
				if (_provider.IsDone)
					value.Invoke(this);
				else
					_provider.Callback += value;
			}
			remove
			{
				_provider.Callback -= value;
			}
		}

		/// <summary>
		/// 最终结果
		/// </summary>
		public System.Object Result
		{
			get
			{
				return _provider.Result;
			}
		}

		/// <summary>
		/// 初始化的游戏对象（只限于请求的资源对象类型为GameObject）
		/// </summary>
		public GameObject InstantiateObject
		{
			get
			{
				if (_provider.Result == null)
					return null;
				return UnityEngine.Object.Instantiate(_provider.Result as GameObject);
			}
		}
	}
}