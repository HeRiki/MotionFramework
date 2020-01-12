//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.FSM;
using MotionFramework.Resource;
using MotionFramework.Network;

namespace MotionFramework.Patch
{
	internal class FsmParseAppPatchManifest : IFiniteStateNode
	{
		private PatchCenter _center;
		public string Name { private set; get; }

		public FsmParseAppPatchManifest(PatchCenter center)
		{
			_center = center;
			Name = EPatchStates.ParseAppPatchManifest.ToString();
		}
		void IFiniteStateNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.ParseAppPatchManifest);
			MotionEngine.StartCoroutine(DownLoad());
		}
		void IFiniteStateNode.OnUpdate()
		{
		}
		void IFiniteStateNode.OnExit()
		{
		}
		void IFiniteStateNode.OnHandleMessage(object msg)
		{
		}

		private IEnumerator DownLoad()
		{
			// 解析APP里的补丁清单
			string filePath = AssetPathHelper.MakeStreamingLoadPath(PatchDefine.PatchManifestFileName);
			string url = AssetPathHelper.ConvertToWWWPath(filePath);
			WebDataRequest downloader = new WebDataRequest(url);
			yield return downloader.DownLoad();

			if (downloader.States == EWebRequestStates.Success)
			{
				PatchHelper.Log(ELogType.Log, "Parse app patch manifest.");
				_center.ParseAppPatchManifest(downloader.GetText());
				downloader.Dispose();
				_center.SwitchNext();
			}
			else
			{
				throw new System.Exception($"Fatal error : Failed download file : {url}");
			}
		}
	}
}