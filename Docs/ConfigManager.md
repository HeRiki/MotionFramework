### 配表管理器 (ConfigManager)

[FlashExcel](https://github.com/gmhevinci/FlashExcel)导表工具会自动生成表格相关的CS脚本和二进制数据文件
```C#
using MotionFramework.Config;

// 这里扩展了获取数据的方法
public partial class CfgHero
{
	public static CfgHeroTab GetCfgTab(int key)
	{
		CfgHero cfg = ConfigManager.Instance.GetConfig(EConfigType.Hero.ToString()) as CfgHero;
		return cfg.GetTab(key) as CfgHeroTab;
	}
}
```

创建配表管理器
```C#
public void Start()
{
	// 设置参数
	var ceateParam = new ConfigManager.CreateParameters();
	createParam.BaseFolderPath = "Config";

	// 创建模块
	AppEngine.Instance.CreateModule<ConfigManager>(ceateParam);
}
```

加载表格
```C#
using MotionFramework.Config;

public class Test
{
	public void Start()
	{
		// 优先加载多语言表
		ConfigManager.Instance.Load("AutoGenerateLanguage", OnLanguagePrepare);	
	}

	private void OnLanguagePrepare(AssetConfig config)
	{
		// 多语言表加载完毕后，加载剩余其它表格
		ConfigManager.Instance.Load("Hero", OnConfigPrepare);
	}
	
	private void OnConfigPrepare(AssetConfig config)
	{
		// 打印表格数据
		if (config is CfgHero)
		{
			CfgHeroTab tab1 = CfgHero.GetCfgTab(1001);
			Debug.Log($"{tab1.Name}");
			CfgHeroTab tab2 = CfgHero.GetCfgTab(1002);
			Debug.Log($"{tab2.Name}");
		}
	}
}
```

1. [MotionModule/Module.Config/ConfigManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionFramework/Scripts/Runtime/MotionModule/Module.Config/ConfigManager.cs)