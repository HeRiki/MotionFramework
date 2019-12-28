# MotionFramework
MotionFramework是一套基于Unity3D引擎的游戏框架。框架整体遵循**轻量化、易用性、低耦合、扩展性强**的设计理念。工程结构清晰，代码注释详细，是作为无框架经验的公司、独立开发者、以及初学者们推荐的游戏框架。

![image](https://github.com/gmhevinci/MotionFramework/raw/master/Docs/Image/img1.png)

## 支持版本
Unity2017.4 && Unity2018.4

## 开发环境
C# && .Net4.x

## 核心系统

1. [游戏模块](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Module.md) **(IModule)** - 游戏模块通过注册机制来统一被管理。基于框架的核心系统，内置了游戏开发过程中常用的管理器，例如：事件管理器，网络管理器，资源管理器，音频管理器，配表管理器，场景管理器，状态机管理器，对象池管理器等。

2. [日志系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/LogSystem.md) **(LogSystem)** - 框架内部使用统一的日志系统，外部业务逻辑需要注册才可以接收到框架生成的日志信息。

3. [事件系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/EventSystem.md) **(EventSystem)** - 基于字符串为KEY的事件监听机制。例如：一些游戏逻辑相关的枚举值可以转换为字符串作为事件的KEY。

4. [流程系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/ProcedureSystem.md) **(ProcedureSystem)** - 和有限状态机的网状结构不同，流程系统是线性结构。使用流程系统，我们可以将复杂的业务逻辑拆分简化，例如：资源热更新流程。

5. [引用池系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/ReferenceSystem.md) **(ReferenceSystem)** - 用于C#引用类型的对象池，对于频繁创建的引用类型，使用引用池可以帮助减少GC。

6. [资源系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/AssetSystem.md) **(AssetSystem)** - 资源系统提供了三种加载方式：AssetDatabase加载方式，Resources加载方式，AssetBundle加载方式。资源系统底层是基于引用计数的设计方案。业务逻辑支持协程，异步，委托多种方式。

7. [网络系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Network.md) **(Network)** - 异步IOCP SOCKET长连接方案，支持TCP和UDP协议。还支持同时建立多个通信通道，例如连接逻辑服务器的同时还可以连接聊天服务器。不同的通信频道支持使用不同的网络包解析器。我们可以定义支持ProtoBuf的网络包解析器，当然也可以使用自己的序列化和反序列化方案。

8. [调试控制台](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/DebugConsole.md) **(DebugConsole)** - 在游戏发布运行的时候，通过调试控制台，可以方便我们查看一些调试信息。框架内置了系统，模块，日志，资源列表，引用池，实体池等多个窗口。我们也可以很方便的添加自定义的调试窗口。

## 模块介绍
游戏开发过程中常用的模块使用介绍

1. [事件管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/EventManager.md) **(EventManager)**

2. [网络管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/NetworkManager.md) **(NetworkManager)**

3. [资源管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/ResourceManager.md) **(ResourceManager)**

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