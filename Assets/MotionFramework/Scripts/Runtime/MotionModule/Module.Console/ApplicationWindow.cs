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

			_scrollPos = AppConsole.GUIBeginScrollView(_scrollPos, 0);

			GUILayout.Space(space);
			AppConsole.GUILable($"Unity Version : {Application.unityVersion}");
			AppConsole.GUILable($"Unity Pro License : {Application.HasProLicense()}");
			AppConsole.GUILable($"Application Version : {Application.version}");
			AppConsole.GUILable($"Application Install Path : {Application.dataPath}");
			AppConsole.GUILable($"Application Persistent Path : {Application.persistentDataPath}");

			GUILayout.Space(space);
			AppConsole.GUILable($"OS : {SystemInfo.operatingSystem}");
			AppConsole.GUILable($"OS Memory : {SystemInfo.systemMemorySize / 1000}GB");
			AppConsole.GUILable($"CPU : {SystemInfo.processorType}");
			AppConsole.GUILable($"CPU Core : {SystemInfo.processorCount}");

			GUILayout.Space(space);
			AppConsole.GUILable($"Device Model : {SystemInfo.deviceModel}");
			AppConsole.GUILable($"Device Name : {SystemInfo.deviceName}");
			AppConsole.GUILable($"Device Type : {SystemInfo.deviceType}");

			GUILayout.Space(space);
			AppConsole.GUILable($"Graphics Device Name : {SystemInfo.graphicsDeviceName}");
			AppConsole.GUILable($"Graphics Device Type : {SystemInfo.graphicsDeviceType}");
			AppConsole.GUILable($"Graphics Memory : {SystemInfo.graphicsMemorySize / 1000}GB");
			AppConsole.GUILable($"Graphics Shader Level : {SystemInfo.graphicsShaderLevel}");
			AppConsole.GUILable($"Multi-threaded Rendering : {SystemInfo.graphicsMultiThreaded}");
			AppConsole.GUILable($"Max Cubemap Size : {SystemInfo.maxCubemapSize}");
			AppConsole.GUILable($"Max Texture Size : {SystemInfo.maxTextureSize}");

			GUILayout.Space(space);
			AppConsole.GUILable($"Supports Accelerometer : {SystemInfo.supportsAccelerometer}"); //加速计硬件
			AppConsole.GUILable($"Supports Gyroscope : {SystemInfo.supportsGyroscope}"); //陀螺仪硬件
			AppConsole.GUILable($"Supports Audio : {SystemInfo.supportsAudio}"); //音频硬件
			AppConsole.GUILable($"Supports GPS : {SystemInfo.supportsLocationService}"); //GPS硬件

			GUILayout.Space(space);
			AppConsole.GUILable($"Screen DPI : {Screen.dpi}");
			AppConsole.GUILable($"Game Resolution : {Screen.width} x {Screen.height}");
			AppConsole.GUILable($"Device Resolution : {Screen.currentResolution.width} x {Screen.currentResolution.height}");
			AppConsole.GUILable($"Graphics Quality : {QualitySettings.names[QualitySettings.GetQualityLevel()]}");

			GUILayout.Space(space);
			long memory = Profiler.GetTotalReservedMemoryLong() / 1000000;
			AppConsole.GUILable($"Total Memory : {memory}MB");
			memory = Profiler.GetTotalAllocatedMemoryLong() / 1000000;
			AppConsole.GUILable($"Used Memory : {memory}MB");
			memory = Profiler.GetTotalUnusedReservedMemoryLong() / 1000000;
			AppConsole.GUILable($"Free Memory : {memory}MB");
			memory = Profiler.GetMonoHeapSizeLong() / 1000000;
			AppConsole.GUILable($"Total Mono Memory : {memory}MB");
			memory = Profiler.GetMonoUsedSizeLong() / 1000000;
			AppConsole.GUILable($"Used Mono Memory : {memory}MB");

			GUILayout.Space(space);
			AppConsole.GUILable($"Battery Level : {SystemInfo.batteryLevel}");
			AppConsole.GUILable($"Battery Status : {SystemInfo.batteryStatus}");
			AppConsole.GUILable($"Network Status : {GetNetworkState()}");
			AppConsole.GUILable($"Elapse Time : {GetElapseTime()}");
			AppConsole.GUILable($"Time Scale : {Time.timeScale}");

			AppConsole.GUIEndScrollView();
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