### 补丁管理器 (PatchManager)

创建补丁管理器  
```C#
public void Start()
{
	// 设置参数
	var createParam = new PatchManager.CreateParameters();	
	patchCreateParam.ChannelID = 0; //渠道ID
	patchCreateParam.DeviceID = 0; //设备唯一ID
	patchCreateParam.TestFlag = 0; //测试包标记

	createParam.WebServers = new Dictionary<RuntimePlatform, string>();
	createParam.WebServers.Add(RuntimePlatform.Android, "127.0.0.1/WEB/Android/GameVersion.php");
	createParam.WebServers.Add(RuntimePlatform.IPhonePlayer, "127.0.0.1/WEB/Iphone/GameVersion.php");

	createParam.CDNServers = new Dictionary<RuntimePlatform, string>();
	createParam.CDNServers.Add(RuntimePlatform.Android, "127.0.0.1/CDN/Android");
	createParam.CDNServers.Add(RuntimePlatform.IPhonePlayer, "127.0.0.1/CDN/Iphone");

	// 服务器的默认地址：在运行的平台没有设置服务器地址的时候，会走默认的地址。
	createParam.DefaultWebServerIP = "127.0.0.1/WEB/PC/GameVersion.php";
	createParam.DefaultCDNServerIP = "127.0.0.1/CDN/PC";

	// 创建模块
	AppEngine.Instance.CreateModule<PatchManager>(createParam);
}
```

更详细的教程请参考示例代码
1. [MotionModule/Module.Patch/PatchManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/Scripts/Runtime/MotionModule/Module.Patch/PatchManager.cs)