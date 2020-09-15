using System.Collections;
using System.IO;
using UnityEngine;

public class ApplyMaterial: MonoBehaviour
{
	string assetName = "cubemat";
	string saveTo = "";
	AssetBundle matBundle;
	Material[] materials;
	bool assetsLoaded = false;
	int counter = 0;
	int index = 0;
	private void Start()
	{
		StartCoroutine(SaveAndDownload("http://localhost:5000/download", assetName));
	}

	//Method that initiates the call to download AssetBundles
	IEnumerator SaveAndDownload(string url, string asstName)
	{
		saveTo=Application.persistentDataPath+'/'+asstName;
		Debug.Log(saveTo);
		transform.gameObject.AddComponent<LoadAssetFromServer>();
		transform.GetComponent<LoadAssetFromServer>().DownloadAssets(url, asstName);
		yield return new WaitUntil(() => File.Exists(saveTo));
		SetMaterial();
	}

	//Method to load the Materials from AssetBundles to Material array
	void SetMaterial()
	{
		matBundle=AssetBundle.LoadFromFile(saveTo);
		materials=matBundle.LoadAllAssets<Material>();
		assetsLoaded=true;
	}
	private void Update()
	{
		//only do the operations if the AssetBunldes are loaded
		if(assetsLoaded)
		{
			counter++;
			//Changing the material on certain intervals
			if(counter==1000)
			{
				counter=0;
				transform.gameObject.GetComponent<Renderer>().sharedMaterial=materials[index];
				index++;
				index=(index==materials.Length) ? 0 : index;
			}

		}

	}
}
