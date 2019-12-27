### 音频管理器 (AudioManager)

音频管理器使用范例
```C#
using MotionFramework.Audio;

public class Test
{
	public void Start()
	{
		// 播放短音效
		AudioManager.Instance.PlaySound("Audio/UISound/click");

		// 播放背景音乐
		bool loop = true;
		AudioManager.Instance.PlayMusic("Audio/Music/cityBgMusic", loop);

		// 全部静音
		AudioManager.Instance.Mute(true);

		// 背景音乐静音设置
		AudioManager.Instance.Mute(EAudioLayer.Music, true);

		// 背景音乐音量设置
		AudioManager.Instance.Volume(EAudioLayer.Music. 0.5f);
	}
}
```

更详细的教程请参考示例代码
1. [MotionGame/Runtime/Game.Audio/AudioManager.cs](https://github.com/gmhevinci/MotionFramework/blob/master/Assets/MotionGame/Runtime/Game.Audio/AudioManager.cs)