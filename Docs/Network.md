### 网络系统 (Network)

定义网络包解析器
```C#
using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Network;

/// <summary>
/// Protobuf网络消息解析器
/// </summary>
public class ProtoPackageParser : DefaultPackageParser
{
	public ProtoPackageParser()
	{
		// 设置字段类型
		PackageSizeFieldType = EPackageSizeFieldType.UShort;
		MessageIDFieldType = EMessageIDFieldType.UShort;
	}

	protected override byte[] EncodeInternal(object msgObj)
	{
		return ProtobufHelper.Encode(msgObj);
	}

	protected override object DecodeInternal(Type classType, byte[] bodyBytes)
	{
		return ProtobufHelper.Decode(classType, bodyBytes);
	}
}
```

更详细的教程请参考示例代码
1. [MotionGame/Runtime/Game.Network/PackageParser/DefaultPackageParser.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/MotionGame/Runtime/Game.Network/PackageParser/DefaultPackageParser.cs)