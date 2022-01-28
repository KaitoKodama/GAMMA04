using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
	private SOItemBase item;

	public delegate void ItemClickNotifyer(SOItemBase item);
	public ItemClickNotifyer ItemClickNotifyerHandler;

	public SOItemBase SOItem { get => item; }
	public void InitItem(SOItemBase item)
	{
		this.item = item;
		GetComponent<Image>().sprite = item.Sprite;
		GetComponent<Button>().onClick.AddListener(() => { ItemClickNotifyerHandler?.Invoke(item); });
	}
}
