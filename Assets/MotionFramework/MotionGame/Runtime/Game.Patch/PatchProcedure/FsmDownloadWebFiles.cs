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
	public class FsmDownloadWebFiles : FsmState
	{
		private ProcedureSystem _system;

		public FsmDownloadWebFiles(ProcedureSystem system) : base((int)EPatchStates.DownloadWebFiles)
		{
			_system = system;
		}

		public override void Enter()
		{
			PatchManager.SendPatchStatesChangeMsg((EPatchStates)_system.Current());
			AppEngine.Instance.StartCoroutine(Download(_system));
		}
		public override void Execute()
		{
		}
		public override void Exit()
		{
		}

		public IEnumerator Download(ProcedureSystem system)
		{
			// 检测磁盘空间不足
			// TODO 检测磁盘空间不足

			// 计算下载文件的总大小
			int totalDownloadCount = PatchManager.Instance.DownloadList.Count;
			long totalDownloadSizeKB = 0;
			foreach (var element in PatchManager.Instance.DownloadList)
			{
				totalDownloadSizeKB += element.SizeKB;
			}

			// 开始下载列表里的所有资源
			PatchManager.Log(ELogType.Log, $"Begine download web files : {PatchManager.Instance.DownloadList.Count}");
			long currentDownloadSizeKB = 0;
			int currentDownloadCount = 0;
			foreach (var element in PatchManager.Instance.DownloadList)
			{
				// 注意：资源版本号只用于确定下载路径
				string url = PatchManager.MakeWebDownloadURL(element.Version.ToString(), element.Name);
				string savePath = AssetPathHelper.MakePersistentLoadPath(element.Name);
				element.SavePath = savePath;
				PatchManager.CreateFileDirectory(savePath);

				// 创建下载器
				WebFileRequest download = new WebFileRequest(url, savePath);
				yield return download.DownLoad(); //文件依次加载（在一个文件加载完毕后加载下一个）
				PatchManager.Log(ELogType.Log, $"Web file is done : {url}");

				// 检测是否下载失败
				if (download.States != EWebRequestStates.Succeed)
				{
					_system.Switch((int)EPatchStates.PatchError);
					PatchManager.SendWebFileDownloadFailedMsg(url);
					yield break;
				}

				// 立即释放加载器
				download.Dispose();
				currentDownloadCount++;
				currentDownloadSizeKB += element.SizeKB;
				PatchManager.SendDownloadFilesProgressMsg(totalDownloadCount, currentDownloadCount, totalDownloadSizeKB, currentDownloadSizeKB);
			}

			// 验证下载文件的MD5
			foreach (var element in PatchManager.Instance.DownloadList)
			{
				string md5 = HashUtility.FileMD5(element.SavePath);
				if (md5 != element.MD5)
				{
					_system.Switch((int)EPatchStates.PatchError);
					PatchManager.Log(ELogType.Error, $"Web file md5 verification error : {element.Name}");
					PatchManager.SendWebFileMD5VerifyFailedMsg(element.Name);
					yield break;
				}
			}

			// 最后清空下载列表
			PatchManager.Instance.DownloadList.Clear();
			_system.SwitchNext();
		}
	}
}