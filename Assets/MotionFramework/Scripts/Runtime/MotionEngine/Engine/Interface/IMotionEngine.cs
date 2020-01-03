//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using UnityEngine;

namespace MotionFramework
{
	public interface IMotionEngine
	{
		void Initialize(MonoBehaviour behaviour);
		void OnUpdate();
		void OnGUI();
	}
}