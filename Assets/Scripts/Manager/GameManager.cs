using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using CommonUtility;

public class GameManager : MonoBehaviour
{
	//------------------------------------------
	// シングルトン
	//------------------------------------------
	static public GameManager instance;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
		OnLoadData();
	}

	private void Start()
	{
		circle.transform.DORotate(new Vector3(0, 0, -360), 5f).SetEase(Ease.Linear).SetRelative().SetLoops(-1, LoopType.Incremental);
		group = cvsGroup.GetComponent<CanvasGroup>();
		audioSource = GetComponent<AudioSource>();
		cvsGroup.SetActive(false);
		group.alpha = 0;
	}

	//------------------------------------------
	// シーン遷移
	//------------------------------------------
	[SerializeField] GameObject circle;
	[SerializeField] GameObject cvsGroup;
	private EntryType entryType = EntryType.Unset;
	private AudioSource audioSource;
	private CanvasGroup group;
	private float fadeDurate = 1f;
	private bool isTransiting = false;

	public EntryType EntryType { get => entryType; set => entryType = value; }
	public void OnSceneTransit(SceneName nextScene, AudioClip clip = null)
	{
		if (!isTransiting)
		{
			isTransiting = true;
			if (clip != null)
			{
				audioSource.PlayOneShot(clip);
			}
			StartCoroutine(SceneTransition(nextScene));
		}
	}
	private IEnumerator SceneTransition(SceneName nextScene)
	{
		OnSaveData();
		cvsGroup.SetActive(true);
		group.DOFade(1, fadeDurate);
		yield return new WaitForSeconds(fadeDurate);

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene.ToString());
		yield return new WaitUntil(() => asyncLoad.isDone);

		group.DOFade(0, fadeDurate);
		yield return new WaitForSeconds(fadeDurate);
		cvsGroup.SetActive(false);

		isTransiting = false;
	}


	//------------------------------------------
	// データセーブ
	//------------------------------------------
	private string saveStateKey = "save_state_key";
	private string saveItemKey = "save_items_key";
	private string saveEnedexKey = "save_enedex_key";

	[SerializeField]private SaveState saveState = new SaveState();
	private SaveItems saveItems = new SaveItems();
	private SaveEnedex saveEnedex = new SaveEnedex();

	public SaveState SaveState { get => saveState; set => saveState = value; }
	public SaveItems SaveItems { get => saveItems; }
	public SaveEnedex SaveEnedex { get => saveEnedex; }

	public void OnSaveData()
	{
		string itemJson = JsonUtility.ToJson(saveItems);
		string stateJson = JsonUtility.ToJson(saveState);
		string enedexJson = JsonUtility.ToJson(saveEnedex);
		PlayerPrefs.SetString(saveItemKey, itemJson);
		PlayerPrefs.SetString(saveStateKey, stateJson);
		PlayerPrefs.SetString(saveEnedexKey, enedexJson);
		PlayerPrefs.Save();
	}

	public void OnLoadData()
	{
		if (!PlayerPrefs.HasKey(saveStateKey)) saveState = new SaveState();
		else
		{
			string stateStr = PlayerPrefs.GetString(saveStateKey);
			saveState = JsonUtility.FromJson<SaveState>(stateStr);
		}	

		if (!PlayerPrefs.HasKey(saveItemKey)) saveItems = new SaveItems();
		else
		{
			string itemStr = PlayerPrefs.GetString(saveItemKey);
			saveItems = JsonUtility.FromJson<SaveItems>(itemStr);
		}

		if (!PlayerPrefs.HasKey(saveEnedexKey)) saveEnedex = new SaveEnedex();
		else
		{
			string enedexStr = PlayerPrefs.GetString(saveEnedexKey);
			saveEnedex = JsonUtility.FromJson<SaveEnedex>(enedexStr);
		}
	}
	public void DeleteSaveData()
	{
		PlayerPrefs.DeleteKey(saveStateKey);
		PlayerPrefs.DeleteKey(saveItemKey);
		PlayerPrefs.DeleteKey(saveEnedexKey);
		PlayerPrefs.Save();

		saveState = new SaveState();
		saveItems = new SaveItems();
		saveEnedex = new SaveEnedex();
	}


	//------------------------------------------
	// アイテム
	//------------------------------------------
	[Header("アイテム情報の登録")]
	[SerializeField] List<SOItemBase> items = new List<SOItemBase>();

	public List<SOItemBase> Items { get => items; }
	public delegate void OnItemRemovedNotifyer();
	public delegate void OnItemRefleshNotifyer();
	public delegate void OnItemUsedNotifyer(SOItemBase item);
	public OnItemRemovedNotifyer OnItemRemovedNotifyerHandler;
	public OnItemRefleshNotifyer OnItemRefleshedNotifyerHandler;
	public OnItemUsedNotifyer OnItemUsedNotifyerHandler;

	public SaveItem GetSavedItem(ItemID itemID)
	{
		if (saveItems.list != null)
		{
			foreach (var saved in saveItems.list)
			{
				if (saved.itemID == itemID)
				{
					return saved;
				}
			}
		}
		var newItem = new SaveItem(itemID);
		return newItem;
	}
	public SOItemBase GetItemDetail(ItemID itemID)
	{
		foreach (var item in items)
		{
			if (item.Id == itemID)
			{
				return item;
			}
		}
		return null;
	}
	public void PushItem(ItemID itemID)
	{
		bool isExist = false;
		foreach (var item in saveItems.list)
		{
			if(item.itemID == itemID)
			{
				isExist = true;
				item.num++;
			}
		}
		if (!isExist)
		{
			saveItems.list.Add(new SaveItem(itemID));
		}
		OnItemRefleshedNotifyerHandler?.Invoke();
		OnSaveData();
	}
	public void UseItem(ItemID itemID)
	{
		foreach (var item in saveItems.list)
		{
			if (item.itemID == itemID)
			{
				if (item.num > 0)
				{
					item.num--;
					OnItemUsedNotifyerHandler?.Invoke(GetItemDetail(item.itemID));
					if (item.num <= 0)
					{
						saveItems.list.Remove(item);
						OnItemRemovedNotifyerHandler?.Invoke();
					}
					else OnItemRefleshedNotifyerHandler?.Invoke();
					OnSaveData();
					return;
				}
			}
		}
	}

	//------------------------------------------
	// ステート操作
	//------------------------------------------
	[SerializeField] AudioClip levelupSound;
	public void AddEXP(float exp)
	{
		saveState.Exp += exp;
		if (saveState.Exp >= saveState.RequireExp)
		{
			audioSource.PlayOneShot(levelupSound);
			saveState.Exp = 0;
			saveState.Level++;

			saveState.RequireExp = ((saveState.RequireExp * 1.1f) + (saveState.Level * 15)) / 5;
			saveState.Gifted += 3;
		}
	}


	//------------------------------------------
	// 敵図鑑
	//------------------------------------------
	public void PushEnedex(int id)
	{
		bool isExist = false;
		foreach (var eneID in saveEnedex.ids)
		{
			if (id == eneID)
			{
				isExist = true;
				break;
			}
		}
		if (!isExist) saveEnedex.ids.Add(id);
	}
}
