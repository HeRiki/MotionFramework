### 场景管理器 (SceneManager)

```C#
using MotionFramework.Scene;

public class Test
{
	public void Start()
	{
		// 改变主场景
		// 注意：当改变主场景的时候，之前加载的附加场景将会被卸载
		SceneManager.Instance.ChangeMainScene("Scene/Town", true，null);

		// 加载新的附加场景
		SceneManager.Instance.LoadAdditionScene("Scene/Town_sky", true, null);
		SceneManager.Instance.LoadAdditionScene("Scene/Town_river", true, null);

		...

		// 检测场景是否完毕
		bool isDone = SceneManager.Instance.CheckSceneIsDone("Scene/Town")

		// 获取场景加载进度（0-100）
		int progress = SceneManager.Instance.GetSceneLoadProgress("Scene/Town")
	}
}
```