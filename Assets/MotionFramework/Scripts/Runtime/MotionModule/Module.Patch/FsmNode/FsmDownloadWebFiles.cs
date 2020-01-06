//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using MotionFramework.AI;
using MotionFramework.Resource;
using MotionFramework.Network;
using MotionFramework.Event;
using MotionFramework.Utility;

namespace MotionFramework.Patch
{
	internal class FsmDownloadWebFiles : IFsmNode
	{
		private ProcedureSystem _system;
		public string Name { private set; get; }

		public FsmDownloadWebFiles(ProcedureSystem system)
		{
			_system = system;
			Name = EPatchStates.DownloadWebFiles.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStatesChangeMsg(EPatchStates.DownloadWebFiles);
			AppEngine.Instance.StartCoroutine(Download());
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

		private IEnumerator Download()
		{
			// 检测磁盘空间不足
			// TODO 检测磁盘空间不足

			// 计算下载文件的总大小
			int totalDownloadCount = PatchSystem.Instance.DownloadList.Count;
			long totalDownloadSizeKB = 0;
			foreach (var element in PatchSystem.Instance.DownloadList)
			{
				totalDownloadSizeKB += element.SizeKB;
			}

			// 开始下载列表里的所有资源
			PatchHelper.Log(ELogType.Log, $"Begine download web files : {PatchSystem.Instance.DownloadList.Count}");
			long currentDownloadSizeKB = 0;
			int currentDownloadCount = 0;
			foreach (var element in PatchSystem.Instance.DownloadList)
			{
				// 注意：资源版本号只用于确定下载路径
				string url = PatchSystem.Instance.GetWebDownloadURL(element.Version.ToString(), element.Name);
				string savePath = AssetPathHelper.MakePersistentLoadPath(element.Name);
				element.SavePath = savePath;
				PatchHelper.CreateFileDirectory(savePath);

				// 创建下载器
				WebFileRequest download = new WebFileRequest(url, savePath);
				yield return download.DownLoad(); //文件依次加载（在一个文件加载完毕后加载下一个）
				PatchHelper.Log(ELogType.Log, $"Web file is download : {savePath}");

				// 检测是否下载失败
				if (download.States != EWebRequestStates.Succeed)
				{
					PatchEventDispatcher.SendWebFileDownloadFailedMsg(url, element.Name);
					yield break;
				}

				// 立即释放加载器
				download.Dispose();
				currentDownloadCount++;
				currentDownloadSizeKB += element.SizeKB;
				PatchEventDispatcher.SendDownloadFilesProgressMsg(totalDownloadCount, currentDownloadCount, totalDownloadSizeKB, currentDownloadSizeKB);
			}

			// 验证下载文件的MD5
			foreach (var element in PatchSystem.Instance.DownloadList)
			{
				string md5 = HashUtility.FileMD5(element.SavePath);
				if (md5 != element.MD5)
				{
					PatchHelper.Log(ELogType.Error, $"Web file md5 verification error : {element.Name}");
					PatchEventDispatcher.SendWebFileMD5VerifyFailedMsg(element.Name);
					yield break;
				}
			}

			// 最后清空下载列表
			PatchSystem.Instance.DownloadList.Clear();
			_system.SwitchNext();
		}
	}
}