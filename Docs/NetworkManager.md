### 网络管理器 (NetworkManager)

创建网络管理器
```C#
public void Start()
{
	// 创建模块
	AppEngine.Instance.CreateModule<NetworkManager>();
}
```

网络管理器使用示例
```C#
using MotionFramework.Network;

public class Test
{
	public void Start()
	{
		// 注意：ProtoMessagePacker是我们自定义的网络包编码解码器
		NetworkManager.Instance.ConnectServer("127.0.0.1", 10002, typeof(ProtoPackageCoder));
		NetworkManager.Instance.MonoPackageCallback += OnHandleMonoPackage;
	}

	public void Send()
	{
		// 在网络连接成功之后可以发送消息
		if(NetworkManager.Instance.State == ENetworkStates.Connected)
		{
			C2R_Login msg = new C2R_Login();
			msg.Account = "test";
			msg.Password = "1234567";
			NetworkManager.Instance.SendMessage(msg);
		}
	}

	private void OnHandleMonoPackage(INetPackage package)
	{
		Debug.Log($"Handle net message : {package.MsgID}");
		R2C_Login msg = package.MsgObj as R2C_Login;
		if(msg != null)
		{
			Debug.Log(msg.Address);
			Debug.Log(msg.Key);
		}
	}
}
```

更详细的教程请参考示例代码
1. [MotionModule/Module.Network/NetworkManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/Scripts/Runtime/MotionModule/Module.Network/NetworkManager.cs)