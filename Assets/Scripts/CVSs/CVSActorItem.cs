using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CVSActorItem : MonoBehaviour
{
    [SerializeField] AudioClip itemUseSound;
    [SerializeField] AudioClip clickSound;
    [SerializeField] Text itemNumText;
    [SerializeField] Image itemImage;
    [SerializeField] Button useBtn;
    [SerializeField] Button nextBtn;

    private Actor actor;
    private AudioSource audioSource;
    private int storageIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        actor = GameObject.FindWithTag("Player").GetComponent<Actor>();
        var itemList = GameManager.instance.SaveItems.list;
        if (itemList != null && itemList.Count > 0)
        {
            var detail = GameManager.instance.GetItemDetail(itemList[0].itemID);
            itemImage.sprite = detail.Sprite;
            itemNumText.text = "残り : " + itemList[0].num.ToString() + "コ";
        }

        GameManager.instance.OnItemUsedNotifyerHandler = OnItemUsedReciever;
        GameManager.instance.OnItemRefleshedNotifyerHandler = OnItemRefleshedReciever;
        GameManager.instance.OnItemRemovedNotifyerHandler = OnItemRemovedReciever;

        useBtn.onClick.AddListener(OnItemUseButton);
        nextBtn.onClick.AddListener(OnItemNextButton);
    }


    //------------------------------------------
    // イベント通知
    //------------------------------------------
    private void OnItemRemovedReciever()
    {
        OnItemNextButton();
    }
    private void OnItemRefleshedReciever()
	{
        var itemList = GameManager.instance.SaveItems.list;
        if (itemList != null && itemList.Count > 0)
        {
            var item = itemList[storageIndex];
            itemNumText.text = "残り : " + item.num.ToString() + "コ";
            var detail = GameManager.instance.GetItemDetail(item.itemID);
            itemImage.sprite = detail.Sprite;
        }
    }
    private void OnItemUsedReciever(SOItemBase item)
	{
        if (actor.enabled)
		{
            item.OnExecute(actor);
		}
	}


    //------------------------------------------
    // UI登録
    //------------------------------------------
    private void OnItemUseButton()
    {
        if (actor.enabled)
        {
            var itemList = GameManager.instance.SaveItems.list;
            if (itemList != null && itemList.Count > 0)
            {
                if (itemList[storageIndex] != null)
                {
                    audioSource.PlayOneShot(itemUseSound);
                    GameManager.instance.UseItem(itemList[storageIndex].itemID);
                }
            }
        }
    }
    private void OnItemNextButton()
    {
        audioSource.PlayOneShot(clickSound);
        var itemList = GameManager.instance.SaveItems.list;
        if (itemList != null && itemList.Count > 0)
        {
            if (itemList.Count - 1 > storageIndex) storageIndex++;
            else storageIndex = 0;

            var item = itemList[storageIndex];
            itemNumText.text = "残り : " + item.num.ToString() + "コ";
            var detail = GameManager.instance.GetItemDetail(item.itemID);
            itemImage.sprite = detail.Sprite;
        }
        else
        {
            itemNumText.text = "残り : 0コ";
            itemImage.sprite = null;
        }
    }
}
