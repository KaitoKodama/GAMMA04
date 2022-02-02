using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CVSTextRollBase : MonoBehaviour
{
	[SerializeField] GameObject scrollObj;
	[SerializeField] Text messegeText;
	[SerializeField] RectTransform cvsRect;
	[SerializeField] float duration = 1.5f;
	[TextArea(1, 15)][SerializeField] string messege;



	protected virtual void OnStart()
	{
		StartCoroutine(ScrollStart());
	}
	private IEnumerator ScrollStart()
	{
		var rect = scrollObj.GetComponent<RectTransform>();
		rect.anchoredPosition = new Vector2(0, -10000);
		messegeText.text = messege;
		yield return null;

		var baseRect = ((cvsRect.rect.height / 2) + (rect.rect.height / 2));
		Vector2 location = new Vector2(0, baseRect * -1);
		rect.anchoredPosition = location;

		while (true)
		{
			baseRect = ((cvsRect.rect.height / 2) + (rect.rect.height / 2));
			location.y += duration;
			scrollObj.transform.localPosition = location;
			yield return new WaitForSeconds(0.01f);
			if (location.y >= baseRect)
			{
				OnScrollEnd();
				break;
			}
		}
	}
	protected virtual void OnScrollEnd() { }
}
