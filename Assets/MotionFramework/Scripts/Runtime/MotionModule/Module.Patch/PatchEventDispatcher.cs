//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using MotionFramework.Event;

namespace MotionFramework.Patch
{
	internal static class PatchEventDispatcher
	{
		public static void SendPatchStatesChangeMsg(string currentStates)
		{
			PatchEventMessageDefine.PatchStatesChange msg = new PatchEventMessageDefine.PatchStatesChange();
			msg.CurrentStates = currentStates;
			EventManager.Instance.SendMessage(EPatchEventMessageTag.PatchManagerEvent.ToString(), msg);
		}
		public static void SendFoundNewAPPMsg(string newVersion)
		{
			PatchEventMessageDefine.FoundNewAPP msg = new PatchEventMessageDefine.FoundNewAPP();
			msg.NewVersion = newVersion;
			EventManager.Instance.SendMessage(EPatchEventMessageTag.PatchManagerEvent.ToString(), msg);
		}
		public static void SendDownloadFilesProgressMsg(int totalDownloadCount, int currentDownloadCount, long totalDownloadSizeKB, long currentDownloadSizeKB)
		{
			PatchEventMessageDefine.DownloadFilesProgress msg = new PatchEventMessageDefine.DownloadFilesProgress();
			msg.TotalDownloadCount = totalDownloadCount;
			msg.CurrentDownloadCount = currentDownloadCount;
			msg.TotalDownloadSizeKB = totalDownloadSizeKB;
			msg.CurrentDownloadSizeKB = currentDownloadSizeKB;
			EventManager.Instance.SendMessage(EPatchEventMessageTag.PatchManagerEvent.ToString(), msg);
		}
		public static void SendWebFileDownloadFailedMsg(string filePath)
		{
			PatchEventMessageDefine.WebFileDownloadFailed msg = new PatchEventMessageDefine.WebFileDownloadFailed();
			msg.FilePath = filePath;
			EventManager.Instance.SendMessage(EPatchEventMessageTag.PatchManagerEvent.ToString(), msg);
		}
		public static void SendWebFileMD5VerifyFailedMsg(string filePath)
		{
			PatchEventMessageDefine.WebFileMD5VerifyFailed msg = new PatchEventMessageDefine.WebFileMD5VerifyFailed();
			msg.FilePath = filePath;
			EventManager.Instance.SendMessage(EPatchEventMessageTag.PatchManagerEvent.ToString(), msg);
		}
		public static void SendPatchOverMsg()
		{
			PatchEventMessageDefine.PatchOver msg = new PatchEventMessageDefine.PatchOver();
			EventManager.Instance.SendMessage(EPatchEventMessageTag.PatchManagerEvent.ToString(), msg);
		}
	}
}