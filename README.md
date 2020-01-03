# MotionFramework
MotionFramework是一套基于Unity3D引擎的游戏框架。框架整体遵循**轻量化、易用性、低耦合、扩展性强**的设计理念。工程结构清晰，代码注释详细，是作为无框架经验的公司、独立开发者、以及初学者们推荐的游戏框架。

![image](https://github.com/gmhevinci/MotionFramework/raw/master/Docs/Image/img1.png)

## 支持版本
Unity2017.4 && Unity2018.4

## 开发环境
C# && .Net4.x

## 核心系统

1. [引擎](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/AppEngine.md) **(AppEngine)** - AppEngine是框架的核心系统，它负责游戏模块的创建和管理。在核心系统的基础上，框架提供了游戏开发过程中常用的管理器，可以帮助开发者加快游戏开发速度。

2. [日志](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/AppLog.md) **(AppLog)** - 框架内部统一的日志系统，外部业务逻辑通过监听可以接收日志信息。

3. [控制台](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/AppConsole.md) **(AppConsole)** - 在游戏发布到手机等设备进行游戏的时候，通过内置的控制台可以方便查看调试信息。控制台内置了模块，日志，系统，资源系统，引用系统，游戏对象池等窗口。控制台还提供了开发者自定义窗口的接口。

4. [事件系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/EventSystem.md) **(EventSystem)** - 基于字符串为KEY的事件监听机制。例如：一些游戏逻辑相关的枚举值可以转换为字符串作为事件的KEY。

5. [流程系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/ProcedureSystem.md) **(ProcedureSystem)** - 和有限状态机的网状结构不同，流程系统是线性结构。使用流程系统，我们可以将复杂的业务逻辑拆分简化，例如：资源热更新流程。

6. [引用池系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/ReferenceSystem.md) **(ReferenceSystem)** - 用于C#引用类型的对象池，对于频繁创建的引用类型，使用引用池可以帮助减少GC。

7. [资源系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/AssetSystem.md) **(AssetSystem)** - 资源系统提供了三种加载方式：AssetDatabase加载方式，Resources加载方式，AssetBundle加载方式，三种方式可以自由切换。业务逻辑支持协程，异步，委托多种方式。

8. [网络系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Network.md) **(Network)** - 异步IOCP SOCKET长连接方案，支持TCP和UDP协议。还支持同时建立多个通信通道，例如连接逻辑服务器的同时还可以连接聊天服务器。不同的通信频道支持使用不同的网络包解析器。我们可以定义支持ProtoBuf的网络包解析器，当然也可以使用自己的序列化和反序列化方案。

## 模块介绍
游戏开发过程中常用的管理器

1. [事件管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/EventManager.md) **(EventManager)**

2. [网络管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/NetworkManager.md) **(NetworkManager)**

3. [资源管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/ResourceManager.md) **(ResourceManager)**

4. [补丁管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/PatchManager.md) **(PatchManager)**

4. [音频管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/AudioManager.md) **(AudioManager)**

5. [配表管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/ConfigManager.md) **(ConfigManager)**

5. [场景管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/SceneManager.md) **(SceneManager)**

6. [状态机管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/FsmManager.md) **(FsmManager)**

7. [对象池管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/PoolManager.md) **(PoolManager)**

## 工具介绍
内置的相关工具介绍

1. [资源打包工具](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/AssetBuilder.md) **(AssetBuilder)**

2. [资源导入工具](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/AssetImporter.md) **(AssetImporter)**

3. [资源引用搜索工具](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/AssetSearch.md) **(AssetSearch)**

4. [特效性能查看工具](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/ParticleProfiler.md) **(ParticleProfiler)**

## DEMO
1. [Demo1](https://github.com/gmhevinci/Demo1) 使用ILRuntime热更新方案的演示例子。

2. [Demo2](https://github.com/gmhevinci/Demo2) 使用XLua热更新方案的演示例子。

## 声明
作者本人将会一直维护该框架，提交的Issue会在48小时内解决，欢迎加入社区QQ群：654986302