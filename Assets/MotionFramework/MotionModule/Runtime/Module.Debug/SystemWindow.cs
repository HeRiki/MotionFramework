//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace MotionFramework.Debug
{
	[DebugAttribute("系统", 102)]
	internal class SystemWindow : IDebugWindow
	{
		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;

		public void OnInit()
		{
		}
		public void OnGUI()
		{
			int space = 15;

			_scrollPos = DebugConsole.GUIBeginScrollView(_scrollPos, 0);

			GUILayout.Space(space);
			DebugConsole.GUILable($"Unity Version : {Application.unityVersion}");
			DebugConsole.GUILable($"Unity Pro License : {Application.HasProLicense()}");
			DebugConsole.GUILable($"Application Version : {Application.version}");
			DebugConsole.GUILable($"Application Install Path : {Application.dataPath}");
			DebugConsole.GUILable($"Application Persistent Path : {Application.persistentDataPath}");

			GUILayout.Space(space);
			DebugConsole.GUILable($"OS : {SystemInfo.operatingSystem}");
			DebugConsole.GUILable($"OS Memory : {SystemInfo.systemMemorySize / 1000}GB");
			DebugConsole.GUILable($"CPU : {SystemInfo.processorType}");
			DebugConsole.GUILable($"CPU Core : {SystemInfo.processorCount}");

			GUILayout.Space(space);
			DebugConsole.GUILable($"Device Model : {SystemInfo.deviceModel}");
			DebugConsole.GUILable($"Device Name : {SystemInfo.deviceName}");
			DebugConsole.GUILable($"Device Type : {SystemInfo.deviceType}");

			GUILayout.Space(space);
			DebugConsole.GUILable($"Graphics Device Name : {SystemInfo.graphicsDeviceName}");
			DebugConsole.GUILable($"Graphics Device Type : {SystemInfo.graphicsDeviceType}");
			DebugConsole.GUILable($"Graphics Memory : {SystemInfo.graphicsMemorySize / 1000}GB");
			DebugConsole.GUILable($"Graphics Shader Level : {SystemInfo.graphicsShaderLevel}");
			DebugConsole.GUILable($"Multi-threaded Rendering : {SystemInfo.graphicsMultiThreaded}");
			DebugConsole.GUILable($"Max Cubemap Size : {SystemInfo.maxCubemapSize}");
			DebugConsole.GUILable($"Max Texture Size : {SystemInfo.maxTextureSize}");

			GUILayout.Space(space);
			DebugConsole.GUILable($"Supports Accelerometer : {SystemInfo.supportsAccelerometer}"); //加速计硬件
			DebugConsole.GUILable($"Supports Gyroscope : {SystemInfo.supportsGyroscope}"); //陀螺仪硬件
			DebugConsole.GUILable($"Supports Audio : {SystemInfo.supportsAudio}"); //音频硬件
			DebugConsole.GUILable($"Supports GPS : {SystemInfo.supportsLocationService}"); //GPS硬件

			GUILayout.Space(space);
			DebugConsole.GUILable($"Screen DPI : {Screen.dpi}");
			DebugConsole.GUILable($"Game Resolution : {Screen.width} x {Screen.height}");
			DebugConsole.GUILable($"Device Resolution : {Screen.currentResolution.width} x {Screen.currentResolution.height}");
			DebugConsole.GUILable($"Graphics Quality : {QualitySettings.names[QualitySettings.GetQualityLevel()]}");

			GUILayout.Space(space);
			long memory = Profiler.GetTotalReservedMemoryLong() / 1000000;
			DebugConsole.GUILable($"Total Memory : {memory}MB");
			memory = Profiler.GetTotalAllocatedMemoryLong() / 1000000;
			DebugConsole.GUILable($"Used Memory : {memory}MB");
			memory = Profiler.GetTotalUnusedReservedMemoryLong() / 1000000;
			DebugConsole.GUILable($"Free Memory : {memory}MB");
			memory = Profiler.GetMonoHeapSizeLong() / 1000000;
			DebugConsole.GUILable($"Total Mono Memory : {memory}MB");
			memory = Profiler.GetMonoUsedSizeLong() / 1000000;
			DebugConsole.GUILable($"Used Mono Memory : {memory}MB");

			GUILayout.Space(space);
			DebugConsole.GUILable($"Battery Level : {SystemInfo.batteryLevel}");
			DebugConsole.GUILable($"Battery Status : {SystemInfo.batteryStatus}");
			DebugConsole.GUILable($"Network Status : {GetNetworkState()}");
			DebugConsole.GUILable($"Elapse Time : {GetElapseTime()}");
			DebugConsole.GUILable($"Time Scale : {Time.timeScale}");

			DebugConsole.GUIEndScrollView();
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