//Creating assetbundle for UWP
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

public class BuildAssetBundle : MonoBehaviour
{
	//Method to create AssetBundles and plcae them in Assets/ABs folder
	[MenuItem("Assets/Build Asset Bundles")]
	static void BuildABs()
	{
		string path = Application.dataPath + "/ABs";
		// Put the bundles in a folder called "ABs" within the Assets folder.
		BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.WSAPlayer);
		//Calling Upload Method
		MonoBehaviour camMono = Camera.main.GetComponent<MonoBehaviour>();
		camMono.StartCoroutine(UploadAssetsToServer(path));
	}

	//Method to upload the AssetBundles to the NodeJS Server
	static IEnumerator UploadAssetsToServer(string path)
	{
		string uploadURL = "http://localhost:5000/upload";
		WWWForm form = new WWWForm();
		//Storing the files in a form to upload
		foreach (string filePath in Directory.GetFiles(path))
		{
			UnityWebRequest file = UnityWebRequest.Get(filePath);
			yield return file.SendWebRequest();
			form.AddBinaryData("assetbundles", file.downloadHandler.data, Path.GetFileName(filePath));
		}
		//Upload request
		UnityWebRequest req = UnityWebRequest.Post(uploadURL, form);
		yield return req.SendWebRequest();

		if (req.isHttpError || req.isNetworkError)
		{
			Debug.Log(req.error);
		}
		else
		{
			Debug.Log("Upload Success");
			Debug.Log(req.downloadHandler.text);
		}


	}
}