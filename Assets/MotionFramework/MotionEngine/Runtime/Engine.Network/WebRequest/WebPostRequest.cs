//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace MotionFramework.Network
{
	public class WebPostRequest : AbstractWebRequest
	{
		public string PostContent = null;

		public WebPostRequest(string url) : base(url)
		{
		}
		public override IEnumerator DownLoad()
		{
			// Check fatal
			if (string.IsNullOrEmpty(PostContent))
				throw new Exception($"{nameof(WebPostRequest)} post content is null or empty : {URL}");

			// Check fatal
			if (States != EWebLoadStates.None)
				throw new Exception($"{nameof(WebPostRequest)} is downloading yet : {URL}");

			States = EWebLoadStates.Loading;

			// 投递数据
			byte[] bodyRaw = Encoding.UTF8.GetBytes(PostContent);

			// 下载文件
			CacheRequest =  new UnityWebRequest(URL, UnityWebRequest.kHttpVerbPOST);
			UploadHandlerRaw uploadHandler = new UploadHandlerRaw(bodyRaw);
			DownloadHandlerBuffer downloadhandler = new DownloadHandlerBuffer();
			CacheRequest.uploadHandler = uploadHandler;
			CacheRequest.downloadHandler = downloadhandler;
			CacheRequest.disposeDownloadHandlerOnDispose = true;
			CacheRequest.timeout = NetworkDefine.WebRequestTimeout;
			yield return CacheRequest.SendWebRequest();

			// Check error
			if (CacheRequest.isNetworkError || CacheRequest.isHttpError)
			{
				LogSystem.Log(ELogType.Warning, $"Failed to request web post : {URL} Error : {CacheRequest.error}");
				States = EWebLoadStates.Failed;
			}
			else
			{
				States = EWebLoadStates.Succeed;
			}
		}

		public string GetResponse()
		{
			if (States == EWebLoadStates.Succeed)
				return CacheRequest.downloadHandler.text;
			else
				return null;
		}
	}
}