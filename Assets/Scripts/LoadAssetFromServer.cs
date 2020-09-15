using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadAssetFromServer : MonoBehaviour
{
	byte[] bytes;
	string saveTo = "";

	//Method to be called from out of the script
	public void DownloadAssets(string url, string assetName)
	{
		StartCoroutine(SaveAndDownload(url, assetName));
	}
	//Coroutine to download the AssetBundle
	public IEnumerator SaveAndDownload(string url, string assetName)
	{
		saveTo = Application.persistentDataPath + '/' + assetName;
		if (!File.Exists(saveTo))
		{
			UnityWebRequest request = new UnityWebRequest(url + '/' + assetName);
			request.downloadHandler = new DownloadHandlerBuffer();
			yield return request.SendWebRequest();
			bytes = request.downloadHandler.data;
			File.WriteAllBytes(saveTo, bytes);
		}
	}
}
