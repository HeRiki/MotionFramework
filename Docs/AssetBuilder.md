### 资源打包工具 (AssetBuilder)

![image](https://github.com/gmhevinci/MotionFramework/raw/master/Docs/Image/img101.png)

**界面说明**  
```
Build Version : 补丁包的版本号
Build Output Path : 打包完成后的输出路径（在工程目录下）。该路径无法修改！
Force Rebuild : 强制重建会删除当前平台下所有的补丁文件，并重新生成补丁文件

Compression : Assetbundle的压缩格式
Append Hash : 生成的AssetBundle文件名称添加Hash信息
Disable Write Type Tree : 禁止写入TypeTree，建议不勾选
Ignore Type Tree Chanages : 忽略TypeTree变化，建议勾选
```

**打包规则设置界面**  

![image](https://github.com/gmhevinci/MotionFramework/raw/master/Docs/Image/img101_2.png)

不同项目对资源的处理及打包规则都有所不同，我们可以通过以下几项设置来定制化自己的打包规则。  
1. Collect : 收集文件夹内资源进行打包（包括所有子文件夹）
2. Ignore : 忽略文件夹内资源不参与打包（包括所有子文件夹）
3. TagByFilePath : AssetBundle标签按照文件路径设置
4. TagByFolderPath : AssetBundle标签按照文件所在的文件夹路径设置
5. TagByFileName : AssetBundle标签按照文件名字设置
6. TagByFolderName : AssetBundle标签按照文件所在的文件夹名字设置  

注意：TagByFolder会将文件夹内所有资源打在一个AssetBundle文件里。


**加密方式**  
要实现Bundle文件加密，只需要实现下面代码
```C#
// 注意：这是一个静态类，类名和方法名固定不能改变。
public static class AssetEncrypter
{
	private const string StrEncryptFolderName = "/Assembly/";

	/// <summary>
	/// 检测文件是否需要加密
	/// </summary>
	public static bool Check(string path)
	{
		return path.Contains(StrEncryptFolderName);
	}

	/// <summary>
	/// 对数据进行加密，并返回加密后的数据
	/// </summary>
	public static byte[] Encrypt(byte[] data)
	{
		// 这里使用你的加密算法
		return data;
	}
}
```

**生成结果**  
生成成功后会在输出目录下找到新生成的补丁文件夹。  
![image](https://github.com/gmhevinci/MotionFramework/raw/master/Docs/Image/img101_1.png)

**补丁文件**  
每次打包都会生成一个名为patch.bytes的补丁文件，补丁文件内包含了所有资源的信息，例如：名称，版本，大小，MD5
```C#
//读取package.bytes文件
using System;
using System.Collections;
using MotionFramework;
using MotionFramework.Resource;
using MotionFramework.Patch;

public class Test
{
	private PatchFile _patchFile;

	public void Start()
	{
		AppEngine.Instance.StartCoroutine(DownLoad());
	}

	public IEnumerator DownLoad()
	{
		// 从流文件夹内读取补丁文件
		string filePath = AssetPathHelper.MakeStreamingLoadPath(PatchDefine.StrPatchFileName);
		string url = AssetPathHelper.ConvertToWWWPath(filePath);

		// Download file
		WebDataRequest download = new WebDataRequest(url);
		yield return download.DownLoad();

		// Check result
		if (download.LoadState == EWebRequestStates.Succeed)
		{
			// 解析补丁文件
			_patchFile = new PatchFile();
			_patchFile.Parse(download.GetText());	
		}
		download.Dispose();
	}
}
```

**Jenkins支持**
```
安卓打包调用静态方法：BuildPackage.BuildAndroid
苹果打包调用静态方法：BuildPackage.BuildIOS

命令行参数
"forceBuild=false"
"buildVersion=100"
```