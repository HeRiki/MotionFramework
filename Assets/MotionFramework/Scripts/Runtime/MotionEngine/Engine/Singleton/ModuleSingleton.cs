﻿//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------

namespace MotionFramework
{
	public abstract class ModuleSingleton<T> where T : class, IMotionModule
	{
		private static T _instance;
		public static T Instance
		{
			get
			{
				if (_instance == null)
					AppLog.Log(ELogType.Error, $"{typeof(T)} is not create. Use {nameof(AppEngine.CreateModule)} create.");
				return _instance;
			}
		}

		protected ModuleSingleton()
		{
			if (_instance != null)
				throw new System.Exception($"{typeof(T)} instance already created.");
			_instance = this as T;
		}
	}
}