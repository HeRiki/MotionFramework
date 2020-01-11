//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Resource;

namespace MotionFramework.Config
{
	/// <summary>
	/// 配表管理器
	/// </summary>
	public sealed class ConfigManager : ModuleSingleton<ConfigManager>, IMotionModule
	{
		/// <summary>
		/// 游戏模块创建参数
		/// </summary>
		public class CreateParameters
		{
			/// <summary>
			/// 基于AssetSystem.AssetRootPath的相对路径
			/// 注意：所有的配表文件必须都放在该文件夹下
			/// </summary>
			public string BaseFolderPath;
		}

		private Dictionary<string, AssetConfig> _configs = new Dictionary<string, AssetConfig>();
		private string _baseFolderPath;


		void IMotionModule.OnCreate(System.Object param)
		{
			CreateParameters createParam = param as CreateParameters;
			if (createParam == null)
				throw new Exception($"{nameof(ConfigManager)} create param is invalid.");

			_baseFolderPath = createParam.BaseFolderPath;
		}
		void IMotionModule.OnUpdate()
		{
		}
		void IMotionModule.OnGUI()
		{
		}

		/// <summary>
		/// 加载配表
		/// </summary>
		/// <param name="cfgName">配表文件名称</param>
		public void Load(string cfgName, System.Action<AssetConfig> callback)
		{
			// 防止重复加载
			if (_configs.ContainsKey(cfgName))
			{
				AppLog.Log(ELogType.Error, $"Config {cfgName} is already existed.");
				return;
			}

			AssetConfig config = ConfigHandler.Handle(cfgName);
			if (config != null)
			{
				string location = $"{_baseFolderPath}/{cfgName}";
				_configs.Add(cfgName, config);
				config.Init(location);
				config.Load(callback);
			}
			else
			{
				AppLog.Log(ELogType.Error, $"Config type {cfgName} is invalid.");
			}
		}

		/// <summary>
		/// 获取配表
		/// </summary>
		/// <param name="cfgName">配表文件名称</param>
		public AssetConfig GetConfig(string cfgName)
		{
			if (_configs.ContainsKey(cfgName))
			{
				return _configs[cfgName];
			}

			AppLog.Log(ELogType.Error, $"Not found config {cfgName}");
			return null;
		}

		/// <summary>
		/// 获取配表
		/// </summary>
		public T GetConfig<T>() where T : AssetConfig
		{
			System.Type type = typeof(T);
			foreach(var pair in _configs)
			{
				if (pair.Value.GetType() == type)
					return pair.Value as T;
			}

			AppLog.Log(ELogType.Error, $"Not found config {type}");
			return null;
		}
	}
}