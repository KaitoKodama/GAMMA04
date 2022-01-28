using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CVSStore : MonoBehaviour
{
	[SerializeField] AudioClip buySound;
	[SerializeField] AudioClip openSound;
	[SerializeField] AudioClip clickSound;

	[SerializeField] Transform itemParent;
	[SerializeField] GameObject itemPrefab;
	[SerializeField] Image itemImage;
	[SerializeField] Text itemName;
	[SerializeField] Text itemGold;
	[SerializeField] Text myGold;
	[SerializeField] Text itemExplain;
	[SerializeField] Text itemNum;
	[SerializeField] Button acceptBtn;
	[SerializeField] Button cancelBtn;
	[SerializeField] Sprite storeIcon;

	private AudioSource audioSource;
	private SOItemBase targetItem;

	private void Start()
	{
		var items = GameManager.instance.Items;
		foreach (var item in items)
		{
			var obj = Instantiate(itemPrefab, itemParent);
			var script = obj.GetComponent<Item>();

			script.InitItem(item);
			script.ItemClickNotifyerHandler += ItemSelectedReciever;
		}
		acceptBtn.onClick.AddListener(OnAccepetdBtn);
		cancelBtn.onClick.AddListener(OnCancededBtn);
	}

	private void OnEnable()
	{
		if(audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
		}
		audioSource.PlayOneShot(openSound);
		myGold.text = "所持金：" + Mathf.CeilToInt(GameManager.instance.SaveState.Gold).ToString() + "G";
		targetItem = null;
		ResetBoxes();
	}
	private void ResetBoxes()
	{
		itemImage.sprite = storeIcon;
		itemName.text = "ヨウコソ";
		itemGold.text = "Welcome";
		itemNum.text = "";
		itemExplain.text = "アイテムを選択してください";
	}

	private void OnAccepetdBtn()
	{
		float has = Mathf.CeilToInt(GameManager.instance.SaveState.Gold);
		if (targetItem != null)
		{
			int require = targetItem.RequireGold;
			myGold.text = "所持金：" + has.ToString() + "G";
			if (has >= require)
			{
				audioSource.PlayOneShot(buySound);
				GameManager.instance.SaveState.Gold -= require;
				var saved = GameManager.instance.GetSavedItem(targetItem.Id);
				GameManager.instance.PushItem(targetItem.Id);
				itemNum.text = "所持数：" + saved.num + "コ";
				itemExplain.text = "ご購入ありがとうございました";
			}
			else
			{
				itemExplain.text = "ゴールドが不足しています";
			}
		}
		else itemExplain.text = "アイテムを選択してください";
	}
	private void OnCancededBtn()
	{
		gameObject.SetActive(false);
	}

	private void ItemSelectedReciever(SOItemBase item)
	{
		audioSource.PlayOneShot(clickSound);
		targetItem = item;
		itemImage.sprite = item.Sprite;
		itemName.text = item.Name;
		itemExplain.text = item.Explain;
		itemGold.text = Mathf.CeilToInt(item.RequireGold).ToString() + "G";
		var saved = GameManager.instance.GetSavedItem(item.Id);
		if (saved != null)
		{
			itemNum.text = "所持数：" + (saved.num - 1).ToString("F") + "コ";
		}
		else itemNum.text = "所持数：0コ";
	}
}
