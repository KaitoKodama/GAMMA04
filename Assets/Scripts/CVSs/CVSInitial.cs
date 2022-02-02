using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CVSInitial : CVSTextRollBase
{
	[SerializeField] Button finishBtn;

	private void Start()
	{
		base.OnStart();
		finishBtn.onClick.AddListener(OnFinishBtn);
	}

	private void OnFinishBtn()
	{
		GameManager.instance.OnSceneTransit(SceneName.Stage01);
		GameManager.instance.SaveState.SetStagePatch(StagePatch.TutorialCompleted);
	}
	protected override void OnScrollEnd()
	{
		GameManager.instance.OnSceneTransit(SceneName.Stage01);
		GameManager.instance.SaveState.SetStagePatch(StagePatch.TutorialCompleted);
	}
}
