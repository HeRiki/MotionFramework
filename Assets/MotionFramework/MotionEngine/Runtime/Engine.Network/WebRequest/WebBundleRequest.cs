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
	public class WebBundleRequest : AbstractWebRequest
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
			if (States != EWebLoadStates.None)
				throw new Exception($"{nameof(WebBundleRequest)} is downloading yet : {URL}");

			States = EWebLoadStates.Loading;

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
				States = EWebLoadStates.Failed;
			}
			else
			{
				CacheBundle = DownloadHandlerAssetBundle.GetContent(CacheRequest);
				if (CacheBundle == null)
					States = EWebLoadStates.Failed;
				else
					States = EWebLoadStates.Succeed;
			}
		}
	}
}