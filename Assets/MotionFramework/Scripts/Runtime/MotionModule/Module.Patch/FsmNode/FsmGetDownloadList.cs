﻿//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.FSM;
using MotionFramework.Resource;
using MotionFramework.Utility;

namespace MotionFramework.Patch
{
	internal class FsmGetDonwloadList : IFiniteStateNode
	{
		private PatchCenter _center;
		public string Name { private set; get; }

		public FsmGetDonwloadList(PatchCenter center)
		{
			_center = center;
			Name = EPatchStates.GetDonwloadList.ToString();
		}
		void IFiniteStateNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.GetDonwloadList);
			GetDownloadList();
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

		private void GetDownloadList()
		{
			_center.DownloadList.Clear();

			// 临时下载列表
			List<PatchElement> downloadList = new List<PatchElement>(1000);

			// 准备下载列表
			foreach (var pair in _center.WebPatchManifest.Elements)
			{
				PatchElement element = pair.Value;

				// 先检测APP里的清单
				PatchElement appElement;
				if (_center.AppPatchManifest.Elements.TryGetValue(element.Name, out appElement))
				{
					if (appElement.MD5 == element.MD5)
						continue;
				}

				// 再检测沙盒里的清单
				PatchElement sandboxElement;
				if (_center.SandboxPatchManifest.Elements.TryGetValue(element.Name, out sandboxElement))
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
				_center.SwitchNext();
			}
			else
			{
				// 最后添加到正式下载列表里
				_center.DownloadList.AddRange(downloadList);
				downloadList.Clear();

				// 发现新更新文件后，挂起流程系统
				int totalDownloadCount = _center.DownloadList.Count;
				long totalDownloadSizeKB = 0;
				foreach (var element in _center.DownloadList)
				{
					totalDownloadSizeKB += element.SizeKB;
				}
				PatchEventDispatcher.SendFoundUpdateFilesMsg(totalDownloadCount, totalDownloadSizeKB);
			}
		}
	}
}