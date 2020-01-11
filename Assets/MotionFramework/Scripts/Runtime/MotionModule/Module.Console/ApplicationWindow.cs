//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace MotionFramework.Console
{
	[ConsoleAttribute("系统", 102)]
	internal class ApplicationWindow : IConsoleWindow
	{
		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;

		public void OnCreate()
		{
		}
		public void OnGUI()
		{
			int space = 15;

			_scrollPos = ConsoleSystem.GUIBeginScrollView(_scrollPos, 0);

			GUILayout.Space(space);
			ConsoleSystem.GUILable($"Unity Version : {Application.unityVersion}");
			ConsoleSystem.GUILable($"Unity Pro License : {Application.HasProLicense()}");
			ConsoleSystem.GUILable($"Application Version : {Application.version}");
			ConsoleSystem.GUILable($"Application Install Path : {Application.dataPath}");
			ConsoleSystem.GUILable($"Application Persistent Path : {Application.persistentDataPath}");

			GUILayout.Space(space);
			ConsoleSystem.GUILable($"OS : {SystemInfo.operatingSystem}");
			ConsoleSystem.GUILable($"OS Memory : {SystemInfo.systemMemorySize / 1000}GB");
			ConsoleSystem.GUILable($"CPU : {SystemInfo.processorType}");
			ConsoleSystem.GUILable($"CPU Core : {SystemInfo.processorCount}");

			GUILayout.Space(space);
			ConsoleSystem.GUILable($"Device Model : {SystemInfo.deviceModel}");
			ConsoleSystem.GUILable($"Device Name : {SystemInfo.deviceName}");
			ConsoleSystem.GUILable($"Device Type : {SystemInfo.deviceType}");

			GUILayout.Space(space);
			ConsoleSystem.GUILable($"Graphics Device Name : {SystemInfo.graphicsDeviceName}");
			ConsoleSystem.GUILable($"Graphics Device Type : {SystemInfo.graphicsDeviceType}");
			ConsoleSystem.GUILable($"Graphics Memory : {SystemInfo.graphicsMemorySize / 1000}GB");
			ConsoleSystem.GUILable($"Graphics Shader Level : {SystemInfo.graphicsShaderLevel}");
			ConsoleSystem.GUILable($"Multi-threaded Rendering : {SystemInfo.graphicsMultiThreaded}");
			ConsoleSystem.GUILable($"Max Cubemap Size : {SystemInfo.maxCubemapSize}");
			ConsoleSystem.GUILable($"Max Texture Size : {SystemInfo.maxTextureSize}");

			GUILayout.Space(space);
			ConsoleSystem.GUILable($"Supports Accelerometer : {SystemInfo.supportsAccelerometer}"); //加速计硬件
			ConsoleSystem.GUILable($"Supports Gyroscope : {SystemInfo.supportsGyroscope}"); //陀螺仪硬件
			ConsoleSystem.GUILable($"Supports Audio : {SystemInfo.supportsAudio}"); //音频硬件
			ConsoleSystem.GUILable($"Supports GPS : {SystemInfo.supportsLocationService}"); //GPS硬件

			GUILayout.Space(space);
			ConsoleSystem.GUILable($"Screen DPI : {Screen.dpi}");
			ConsoleSystem.GUILable($"Game Resolution : {Screen.width} x {Screen.height}");
			ConsoleSystem.GUILable($"Device Resolution : {Screen.currentResolution.width} x {Screen.currentResolution.height}");
			ConsoleSystem.GUILable($"Graphics Quality : {QualitySettings.names[QualitySettings.GetQualityLevel()]}");

			GUILayout.Space(space);
			long memory = Profiler.GetTotalReservedMemoryLong() / 1000000;
			ConsoleSystem.GUILable($"Total Memory : {memory}MB");
			memory = Profiler.GetTotalAllocatedMemoryLong() / 1000000;
			ConsoleSystem.GUILable($"Used Memory : {memory}MB");
			memory = Profiler.GetTotalUnusedReservedMemoryLong() / 1000000;
			ConsoleSystem.GUILable($"Free Memory : {memory}MB");
			memory = Profiler.GetMonoHeapSizeLong() / 1000000;
			ConsoleSystem.GUILable($"Total Mono Memory : {memory}MB");
			memory = Profiler.GetMonoUsedSizeLong() / 1000000;
			ConsoleSystem.GUILable($"Used Mono Memory : {memory}MB");

			GUILayout.Space(space);
			ConsoleSystem.GUILable($"Battery Level : {SystemInfo.batteryLevel}");
			ConsoleSystem.GUILable($"Battery Status : {SystemInfo.batteryStatus}");
			ConsoleSystem.GUILable($"Network Status : {GetNetworkState()}");
			ConsoleSystem.GUILable($"Elapse Time : {GetElapseTime()}");
			ConsoleSystem.GUILable($"Time Scale : {Time.timeScale}");

			ConsoleSystem.GUIEndScrollView();
		}

		private string GetNetworkState()
		{
			string internetState = string.Empty;
			if (Application.internetReachability == NetworkReachability.NotReachable)
				internetState = "not reachable";
			else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
				internetState = "carrier data network";
			else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
				internetState = "local area network";
			return internetState;
		}
		private string GetElapseTime()
		{
			int day = (int)(Time.realtimeSinceStartup / 86400f);
			int hour = (int)((Time.realtimeSinceStartup % 86400f) / 3600f);
			int sec = (int)(((Time.realtimeSinceStartup % 86400f) % 3600f) / 60f);
			return $"{day}天{hour}小时{sec}分";
		}
	}
}