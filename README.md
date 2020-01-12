# MotionFramework
MotionFramework是一套基于Unity3D引擎的游戏框架。框架整体遵循**轻量化、易用性、低耦合、扩展性强**的设计理念。工程结构清晰，代码注释详细，是作为无框架经验的公司、独立开发者、以及初学者们推荐的游戏框架。

![image](https://github.com/gmhevinci/MotionFramework/raw/master/Docs/Image/img1.png)

## 支持版本
Unity2017.4 && Unity2018.4

## 开发环境
C# && .Net4.x

## 核心系统

1. [框架引擎](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/MotionEngine.md) **(MotionEngine)** - 游戏框架的核心类，它负责游戏模块的创建和管理。在核心系统的基础上还提供了游戏开发过程中常用的管理器，可以帮助开发者加快游戏开发速度。

2. [框架日志](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/MotionLog.md) **(MotionLog)** - 游戏框架的日志系统，开发者通过注册可以监听框架日志。

3. [控制台](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Engine.Console.md) **(Engine.Console)** - 在游戏运行的时候，通过内置的控制台可以方便查看调试信息。控制台内置了游戏模块，游戏日志，应用详情，资源系统，引用池，游戏对象池等窗口。开发者可以扩展自定义窗口。

4. [引用池](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Engine.Reference.md) **(Engine.Reference)** - 用于C#引用类型的对象池，对于频繁创建的引用类型，使用引用池可以帮助减少GC。

5. [资源系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Engine.Resource.md) **(Engine.Resource)** - 资源系统提供了三种加载方式：AssetDatabase加载方式，Resources加载方式，AssetBundle加载方式，三种方式可以自由切换。业务逻辑支持协程，异步，委托多种方式。

6. [补丁系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Engine.Patch.md) **(Engine.Patch)** - 通用的补丁系统，可以实现资源热更。支持版本回退，支持区分审核版本，测试版本，线上版本，支持灰度更新。

7. [网络系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Engine.Network.md) **(Engine.Network)** - 异步IOCP SOCKET长连接方案，支持TCP和UDP协议。支持同时建立多个通信频道，例如连接逻辑服务器的同时还可以连接聊天服务器。不同的通信频道支持使用不同的网络包编码解码器，开发者可以扩展支持ProtoBuf的网络包编码解码器，也可以使用自定义的序列化和反序列化方案。

8. [有限状态机](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Engine.AI.FSM.md) **(Engine.AI.FSM)** - 有限状态机和流程。和有限状态机的网状结构不同，流程是线性结构。流程可以帮助我们可以将复杂的业务逻辑拆分简化，例如：资源热更新流程。

9. [寻路系统](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Engine.AI.AStar.md) **(Engine.AI.AStar)** - 通用的A星寻路系统，支持2D网格，蜂窝网格，节点网格等结构。

## 管理器介绍
游戏开发过程中常用的管理器

1. [事件管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Module.Event.md) **(EventManager)**

2. [网络管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Module.Network.md) **(NetworkManager)**

3. [资源管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Module.Resource.md) **(ResourceManager)**

4. [补丁管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Module.Patch.md) **(PatchManager)**

5. [音频管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Module.Audio.md) **(AudioManager)**

6. [配表管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Module.Config.md) **(ConfigManager)**

7. [场景管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Module.Scene.md) **(SceneManager)**

8. [游戏对象池管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Module.Pool.md) **(GameObjectPoolManager)**

9. [状态机管理器](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Module.FSM.md) **(FsmManager)**

## 工具介绍
内置的相关工具介绍

1. [资源打包工具](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Editor.AssetBuilder.md) **(AssetBuilder)**

2. [资源导入工具](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Editor.AssetImporter.md) **(AssetImporter)**

3. [资源引用搜索工具](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Editor.AssetSearch.md) **(AssetSearch)**

4. [特效性能查看工具](https://github.com/gmhevinci/MotionFramework/blob/master/Docs/Editor.ParticleProfiler.md) **(ParticleProfiler)**

## DEMO
1. [Demo1](https://github.com/gmhevinci/Demo1) 使用ILRuntime热更新方案的演示例子。

2. [Demo2](https://github.com/gmhevinci/Demo2) 使用XLua热更新方案的演示例子。

## 声明
作者本人将会一直维护该框架，提交的Issue会在48小时内解决，欢迎加入社区QQ群：654986302