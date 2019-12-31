//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MotionFramework.Network
{
	public class WebBundleRequest : WebRequest
	{
		/// <summary>
		/// 缓存的AssetBundle
		/// </summary>
		public AssetBundle CacheBundle { private set; get; }

		public WebBundleRequest(string url) : base(url)
		{
		}
		public override IEnumerator DownLoad()
		{
			// Check fatal
			if (States != EWebRequestStates.None)
				throw new Exception($"{nameof(WebBundleRequest)} is downloading yet : {URL}");

			States = EWebRequestStates.Loading;

			// 下载文件
#if UNITY_2017_4
			CacheRequest = UnityWebRequest.GetAssetBundle(URL);
#else
			CacheRequest = UnityWebRequestAssetBundle.GetAssetBundle(URL);
#endif
			CacheRequest.disposeDownloadHandlerOnDispose = true;
			CacheRequest.timeout = NetworkDefine.WebRequestTimeout;
			yield return CacheRequest.SendWebRequest();

			// Check error
			if (CacheRequest.isNetworkError || CacheRequest.isHttpError)
			{
				LogSystem.Log(ELogType.Warning, $"Failed to download web bundle : {URL} Error : {CacheRequest.error}");
				States = EWebRequestStates.Failed;
			}
			else
			{
				CacheBundle = DownloadHandlerAssetBundle.GetContent(CacheRequest);
				if (CacheBundle == null)
					States = EWebRequestStates.Failed;
				else
					States = EWebRequestStates.Succeed;
			}
		}
	}
}