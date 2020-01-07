### 补丁系统 (PatchSystem)

**补丁流程**  
补丁流程分为俩个阶段：初始化阶段和下载阶段  

初始化阶段
````
初始化开始(InitiationBegin) -> 检测沙盒是否变脏(CheckSandboxDirty) -> 分析APP内的补丁清单(ParseAppPatchManifest) -> 分析沙盒内的补丁清单(ParseSandboxPatchManifest) -> 初始化结束(InitiationOver)

注意：当初始化结束之后，流程系统会被挂起。发送OperationEvent(EPatchOperation.BeginingRequestGameVersion)事件可以恢复流程系统，然后进入下载阶段。如果是单机游戏不需要和服务器通信，那么可以略过下载阶段。
````

下载阶段
````
请求最新的游戏版本(RequestGameVersion) -> 分析网络上的补丁清单(ParseWebPatchManifest) -> 获取下载列表(GetDonwloadList) -> 下载网络文件到沙盒(DownloadWebFiles) -> 下载网络补丁清单到沙盒(DownloadWebPatchManifest) -> 下载结束(DownloadOver)

注意：当发现更新文件的时候，流程系统会被挂起。发送OperationEvent(EPatchOperation.BeginingDownloadWebFiles)事件可以恢复流程系统。

注意：当请求游戏版本号失败的时候，流程系统会被挂起。发送OperationEvent(EPatchOperation.TryRequestGameVersion)事件可以恢复流程系统，然后再次尝试请求游戏版本号。

注意：当下载网络补丁清单失败的时候，流程系统会被挂起。发送OperationEvent(EPatchOperation.TryDownloadWebPatchManifest)事件可以恢复流程系统，然后再次尝试下载。

注意：当下载网络文件失败的时候，流程系统会被挂起。发送OperationEvent(EPatchOperation.TryDownloadWebFiles)事件可以恢复流程系统，然后再次尝试下载。

注意：当下载的网络文件MD5验证失败的时候，流程系统会被挂起。发送OperationEvent(EPatchOperation.TryDownloadWebFiles)事件可以恢复流程系统，然后再次尝试下载。
````

**补丁事件**  
整个流程抛出的事件
````
PatchEventMessageDefine.PatchStatesChange：补丁流程状态改变
````

下载阶段抛出的事件
````
PatchEventMessageDefine.FoundForceInstallAPP：发现强更安装包
PatchEventMessageDefine.FoundUpdateFiles：发现更新文件
PatchEventMessageDefine.DownloadFilesProgress：下载文件列表进度
PatchEventMessageDefine.GameVersionRequestFailed：游戏版本号请求失败
PatchEventMessageDefine.WebPatchManifestDownloadFailed：网络上补丁清单下载失败
PatchEventMessageDefine.WebFileDownloadFailed：网络文件下载失败
PatchEventMessageDefine.WebFileMD5VerifyFailed：文件MD5验证失败
````

**WEB服务器约定**  
POST约定数据
````
$"{应用程序内置版本}&{渠道ID}&{设备唯一ID}&{测试包标记}"
````

Response约定数据
````
注意：如果不需要强更，安装地址返回空
$"{游戏版本}&{强更包安装地址}"
````

更详细的教程，请参考[Demo1](https://github.com/gmhevinci/Demo1)