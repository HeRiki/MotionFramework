//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.AI;
using MotionFramework.Resource;
using MotionFramework.Utility;

namespace MotionFramework.Patch
{
	internal class FsmGetDonwloadList : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmGetDonwloadList(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.GetDonwloadList.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.GetDonwloadList);
			GetDownloadList();
		}
		void IFsmNode.OnUpdate()
		{
		}
		void IFsmNode.OnExit()
		{
		}
		void IFsmNode.OnHandleMessage(object msg)
		{
		}

		private void GetDownloadList()
		{
			PatchSystem.Instance.DownloadList.Clear();

			// 临时下载列表
			List<PatchElement> downloadList = new List<PatchElement>(1000);

			// 准备下载列表
			foreach (var pair in PatchSystem.Instance.WebPatchFile.Elements)
			{
				PatchElement element = pair.Value;

				// 先检测APP里的资源
				PatchElement appElement;
				if (PatchSystem.Instance.AppPatchFile.Elements.TryGetValue(element.Name, out appElement))
				{
					if (appElement.MD5 == element.MD5)
						continue;
				}

				// 再检测沙盒里的资源
				PatchElement sandboxElement;
				if (PatchSystem.Instance.SandboxPatchFile.Elements.TryGetValue(element.Name, out sandboxElement))
				{
					if (sandboxElement.MD5 != element.MD5)
						downloadList.Add(element);
				}
				else
				{
					downloadList.Add(element);
				}
			}

			// 检测已经存在的文件
			// 注意：如果玩家在加载过程中强制退出，下次再进入的时候跳过已经加载的文件
			List<string> removeList = new List<string>();
			foreach (var element in downloadList)
			{
				string filePath = AssetPathHelper.MakePersistentLoadPath(element.Name);
				if (System.IO.File.Exists(filePath))
				{
					string md5 = HashUtility.FileMD5(filePath);
					if (md5 == element.MD5)
						removeList.Add(element.Name);
				}
			}
			foreach (var name in removeList)
			{
				for (int i = 0; i < downloadList.Count; i++)
				{
					if (downloadList[i].Name == name)
					{
						downloadList.RemoveAt(i);
						break;
					}
				}
			}

			// 如果下载列表为空
			if(downloadList.Count == 0)
			{
				_system.SwitchNext();
			}
			else
			{
				// 最后添加到正式下载列表里
				PatchSystem.Instance.DownloadList.AddRange(downloadList);
				downloadList.Clear();

				// 发送事件后流程不再继续
				int totalDownloadCount = PatchSystem.Instance.DownloadList.Count;
				long totalDownloadSizeKB = 0;
				foreach (var element in PatchSystem.Instance.DownloadList)
				{
					totalDownloadSizeKB += element.SizeKB;
				}
				PatchEventDispatcher.SendFoundUpdateFiles(totalDownloadCount, totalDownloadSizeKB);
			}
		}
	}
}