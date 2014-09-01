using UnityEngine;
using System.Collections;
using GmUpdate;

public class GameUpdateInterface : MonoBehaviour, GameUpdateListeners
{
	public static GameUpdateInterface instance;

	public SliderTween slider;

	void Awake ()
	{
		instance = this;
	}

	public void StartDownloading ()
	{
		GameUpdate.instance.AddElementListener (this);
		StartCoroutine (GameUpdate.instance.StartResDownload ());
	}

	#region GameUpdateListeners implementation

	void GameUpdateListeners.DownloadFile (float progress, string info)
	{
		slider.SetSliderInfo (progress, info);
	}

	void GameUpdateListeners.DownloadFileFinish ()
	{
		slider.SetSliderInfo (1, Localization.Get ("gameUpdate.donwload.done"));
		ScenesManager.instance.DelaySuccessLoading ();
	}

	void GameUpdateListeners.UnZipFile ()
	{

	}

	void GameUpdateListeners.UnZipFileFinish ()
	{

	}

	#endregion


}
