using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnedexGrid : MonoBehaviour
{
	[SerializeField] GameObject hide;
	[SerializeField] Image image;
	[SerializeField] Text levelText;
	[SerializeField] Text nameText;


	public void DoHide()
	{
		gameObject.SetActive(false);
		hide.SetActive(true);
	}
	public void DisplayGrid(SOEnemyData data)
	{
		gameObject.SetActive(true);
		hide.SetActive(true);
		foreach (var id in GameManager.instance.SaveEnedex.ids)
		{
			if(id == data.ID)
			{
				hide.SetActive(false);
				image.sprite = data.EnemySprite;
				levelText.text = "レベル：" + data.Level.ToString();
				nameText.text = data.Name.ToString();
				return;
			}
		}
	}
}
