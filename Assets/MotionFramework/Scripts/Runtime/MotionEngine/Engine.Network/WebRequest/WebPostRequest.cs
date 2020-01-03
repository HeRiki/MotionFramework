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
	public class WebPostRequest : WebRequest
	{
		public string PostContent { private set; get; }

		public WebPostRequest(string url, string post) : base(url)
		{
			PostContent = post;
		}
		public override IEnumerator DownLoad()
		{
			// Check fatal
			if (string.IsNullOrEmpty(PostContent))
				throw new Exception($"{nameof(WebPostRequest)} post content is null or empty : {URL}");

			// Check fatal
			if (States != EWebRequestStates.None)
				throw new Exception($"{nameof(WebPostRequest)} is downloading yet : {URL}");

			States = EWebRequestStates.Loading;

			// 投递数据
			byte[] bodyRaw = Encoding.UTF8.GetBytes(PostContent);

			// 下载文件
			CacheRequest = new UnityWebRequest(URL, UnityWebRequest.kHttpVerbPOST);
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
				AppLog.Log(ELogType.Warning, $"Failed to request web post : {URL} Error : {CacheRequest.error}");
				States = EWebRequestStates.Failed;
			}
			else
			{
				States = EWebRequestStates.Succeed;
			}
		}

		public string GetResponse()
		{
			if (States == EWebRequestStates.Succeed)
				return CacheRequest.downloadHandler.text;
			else
				return null;
		}
	}
}