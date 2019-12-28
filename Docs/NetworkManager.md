### 网络管理器 (NetworkManager)

```C#
using MotionFramework.Network;

public class Test
{
	public void Start()
	{
		// 注意：ProtoPackageParser是我们自定义的网络包解析器
		NetworkManager.Instance.ConnectServer("127.0.0.1", 10002, typeof(ProtoPackageParser));
		NetworkManager.Instance.MonoPackageCallback += OnHandleMonoPackage;
	}

	public void Send()
	{
		// 在网络连接成功之后可以发送消息
		if(NetworkManager.Instance.State == ENetworkState.Connected)
		{
			C2R_Login msg = new C2R_Login();
			msg.Account = "test";
			msg.Password = "1234567";
			NetworkManager.Instance.SendMsg(msg);
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
1. [MotionGame/Runtime/Game.Network/NetworkManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/MotionGame/Runtime/Game.Network/NetworkManager.cs)