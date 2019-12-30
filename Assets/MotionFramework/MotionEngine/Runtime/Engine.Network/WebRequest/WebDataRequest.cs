//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace MotionFramework.Network
{
	public class WebDataRequest : AbstractWebRequest
	{
		public WebDataRequest(string url) : base(url)
		{
		}
		public override IEnumerator DownLoad()
		{
			// Check fatal
			if (States != EWebLoadStates.None)
				throw new Exception($"{nameof(WebDataRequest)} is downloading yet : {URL}");

			States = EWebLoadStates.Loading;

			// 下载文件
			CacheRequest = new UnityWebRequest(URL, UnityWebRequest.kHttpVerbGET);
			DownloadHandlerBuffer handler = new DownloadHandlerBuffer();
			CacheRequest.downloadHandler = handler;
			CacheRequest.disposeDownloadHandlerOnDispose = true;
			CacheRequest.timeout = NetworkDefine.WebRequestTimeout;
			yield return CacheRequest.SendWebRequest();

			// Check error
			if (CacheRequest.isNetworkError || CacheRequest.isHttpError)
			{
				LogSystem.Log(ELogType.Warning, $"Failed to download web data : {URL} Error : {CacheRequest.error}");
				States = EWebLoadStates.Failed;
			}
			else
			{
				States = EWebLoadStates.Succeed;
			}
		}

		public byte[] GetData()
		{
			if (States == EWebLoadStates.Succeed)
				return CacheRequest.downloadHandler.data;
			else
				return null;
		}
		public string GetText()
		{
			if (States == EWebLoadStates.Succeed)
				return CacheRequest.downloadHandler.text;
			else
				return null;
		}
	}
}