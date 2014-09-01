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
		GameUpdate.instance.StartResDownload ();
	}

	#region GameUpdateListeners implementation

	void GameUpdateListeners.DownloadFile (float progress, string info)
	{
		slider.SetSliderInfo (progress, info);
	}

	void GameUpdateListeners.DownloadFileFinish ()
	{
		//slider.SetSliderInfo (1, Localization.Get ("gameUpdate.donwload.done"));

	}

	void GameUpdateListeners.UnZipFile (float progress, string info)
	{
		slider.SetSliderInfo (progress, info);
	}

	void GameUpdateListeners.UnZipFileFinish ()
	{
		ScenesManager.instance.DelaySuccessLoading ();
	}

	#endregion


}
