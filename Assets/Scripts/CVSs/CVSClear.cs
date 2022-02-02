using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CVSClear : CVSTextRollBase
{
	[SerializeField] Button skipBtn;

	private void OnEnable()
	{
		base.OnStart();
		skipBtn.onClick.AddListener(OnSkipBtn);
		var audio = GetComponent<AudioSource>();
		audio.volume = 0;
		audio.DOFade(1, 10);
	}

	private void OnSkipBtn()
	{
		GameManager.instance.OnSceneTransit(SceneName.TitleScene);
	}
}
