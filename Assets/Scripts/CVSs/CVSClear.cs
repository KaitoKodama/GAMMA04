using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CVSClear : MonoBehaviour
{
	[SerializeField] GameObject clearTxt;
	[SerializeField] RectTransform cvsRect;
	[SerializeField] Button pressBtn;
	[TextArea(1, 15)] [SerializeField] string messege;
	private float duration = 1.5f;

	private void OnEnable()
	{
		StartCoroutine(BeginCompleted());
	}

	private IEnumerator BeginCompleted()
	{
		pressBtn.onClick.AddListener(OnDisableBtn);
		var audioSource = GetComponent<AudioSource>();
		audioSource.volume = 0;
		audioSource.Play();
		audioSource.DOFade(1, 10f);
		yield return null;

		var rect = clearTxt.GetComponent<RectTransform>();
		var txt = clearTxt.GetComponent<Text>();
		rect.anchoredPosition = new Vector2(0, -10000);
		txt.text = messege;
		yield return null;

		var baseRect = ((cvsRect.rect.height / 2) + (rect.rect.height / 2));
		Vector2 location = new Vector2(0, baseRect * -1);
		rect.anchoredPosition = location;

		while (true)
		{
			baseRect = ((cvsRect.rect.height / 2) + (rect.rect.height / 2));
			location.y += duration;
			clearTxt.transform.localPosition = location;
			yield return new WaitForSeconds(0.01f);
			if (location.y >= baseRect)
			{
				break;
			}
		}
	}

	private void OnDisableBtn()
	{
		gameObject.SetActive(false);
		GameManager.instance.OnSceneTransit(SceneName.TitleScene);
	}
}
