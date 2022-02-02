using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class CVSDiceing : MonoBehaviour
{
	class SpinItem
	{
		public SpinItem(ItemID itemID, SlotRewardType rewardType, float rate, string messege, int id)
		{
			this.itemID = itemID;
			this.rewardType = rewardType;
			this.rate = rate;
			this.messege = messege;
			this.id = id;
		}
		public SpinItem(float gold, SlotRewardType rewardType, float rate, string messege, int id)
		{
			this.gold = gold;
			this.rewardType = rewardType;
			this.rate = rate;
			this.messege = messege;
			this.id = id;
		}
		public ItemID itemID;
		public float gold;
		public SlotRewardType rewardType;
		public string messege;
		public float rate;
		public int id;
	}
	enum SlotRewardType { Gold, Item, }

	[SerializeField] AudioClip spiningSound;
	[SerializeField] AudioClip rewordSound;

	[SerializeField] GameObject mainPanel;

	[SerializeField] Button closeBtn;
	[SerializeField] Button spinBtn;
	[SerializeField] Button spinAdBtn;
	[SerializeField] Button spinGoldBtn;
	[SerializeField] Image spinBtnImane;
	[SerializeField] Image spiningImage;
	[SerializeField] Text leastAdSpinText;
	[SerializeField] Text rewordText;
	[SerializeField] RectTransform spinRect;
	[SerializeField] Gradient spiningGradient;
	[SerializeField] Color firstColor;

	[SerializeField] GoogleAds googleAds;

	private List<SpinItem> spinItems = new List<SpinItem>();
	private Coroutine spinCoroutine;
	private AudioSource audioSource;

	private float spinDuration = 0.001f;
	private float spinSpeed = 5f;
	private int spinTime = 2;
	private int rewardSpin = 0;

	private int ADD_SPACE = 93;
	private int SPIN_STARTY = -606;
	private int SPIN_ENDY = 606;


	private void Start()
	{
		InitializeDicts();
		spinRect.anchoredPosition = new Vector2(0, SPIN_STARTY);
		rewordText.text = "5口 1広告なのだよ\nいい商品がゲットできるかもなのだよ～";
		spinBtnImane.DOGradientColor(spiningGradient, 1f).SetLoops(-1, LoopType.Yoyo);
		spiningImage.DOGradientColor(spiningGradient, 1f).SetLoops(-1, LoopType.Yoyo);

		audioSource = GetComponent<AudioSource>();
		closeBtn.onClick.AddListener(OnCloseBtn);
		spinBtn.onClick.AddListener(OnSpinStartBtn);
		spinAdBtn.onClick.AddListener(OnSpinIncrementByAdBtn);

		googleAds.OnEarnedRewardNotifyerHandler = OnRewardEarnedReciever;
	}
	private void InitializeDicts()
	{
		spinItems.Add(new SpinItem(10f, SlotRewardType.Gold, 80f,"ツイてなかったのだよ\nまたやってくかいなのだよ～" ,1));
		spinItems.Add(new SpinItem(20f, SlotRewardType.Gold, 70f, "これで色々買えるのだよ\nまたやってくかいなのだよ～", 2));
		spinItems.Add(new SpinItem(ItemID.Potion05, SlotRewardType.Item, 60f, "そこそこいいものなのだよ\nまだまだいけるのだよ～", 3));
		spinItems.Add(new SpinItem(ItemID.Potion06, SlotRewardType.Item, 50f, "おめでとうなのだよ\n大事に使ってなのだよ～", 4));
		spinItems.Add(new SpinItem(50f, SlotRewardType.Gold, 40f, "これで色々買えるのだよ\nまだまだこれからなのだよ～", 5));
		spinItems.Add(new SpinItem(ItemID.PBuff03, SlotRewardType.Item, 30f, "強い敵にも勝てるかもなのだよ\n大事に使ってなのだよ～", 6));
		spinItems.Add(new SpinItem(ItemID.DBuff03, SlotRewardType.Item, 20f, "防御は大事なのだよ\n運がいいのだよ～", 7));
		spinItems.Add(new SpinItem(ItemID.PBuff04, SlotRewardType.Item, 10f, "お客さんツイてるのだよ\nまだまだこれからなのだよ～", 8));
		spinItems.Add(new SpinItem(ItemID.DBuff04, SlotRewardType.Item, 9f, "かなり運がいいのだよ\nもっと回してほしいのだよ～", 9));
		spinItems.Add(new SpinItem(ItemID.PBuff05, SlotRewardType.Item, 8f, "びっくりしたのだよ\nこれはかなり上物なのだよ～", 10));
		spinItems.Add(new SpinItem(ItemID.DBuff05, SlotRewardType.Item, 7f, "かなり高品質なものなのだよ\nこれからが勝負なのだよ～", 11));
		spinItems.Add(new SpinItem(100f, SlotRewardType.Gold, 6f, "お客さんお金持ちなのだよ\nスコシ分けてほしいのだよ～", 12));
		spinItems.Add(new SpinItem(200f, SlotRewardType.Gold, 5f, "スゴイ！スゴイのだよ\n大当たりなのだよ～", 13));
		spinItems.Add(new SpinItem(500f, SlotRewardType.Gold, 4f, "カランコロンカラン！！\n出ました特賞なのだよ～～", 14));
	}

	private void OnSpinStart(float maxRange)
	{
		if (spinCoroutine == null)
		{
			spinCoroutine = StartCoroutine(SpinStart(maxRange));
		}
	}
	private IEnumerator SpinStart(float maxRange)
	{
		float rndNum = UnityEngine.Random.Range(0f, maxRange);
		float min = spinItems.Min(c => Math.Abs(c.rate - rndNum));
		var getItem = spinItems.First(c => Math.Abs(c.rate - rndNum) == min);

		float spinEnd = SPIN_ENDY;
		var spiningVector = new Vector2(0, SPIN_STARTY);
		spinRect.anchoredPosition = spiningVector;
		rewordText.text = "回り始めるのだよ\nいいアイテムが出でるといいのだよ～";

		audioSource.PlayOneShot(spiningSound);
		for (int i = 0; i < spinTime; i++)
		{
			if (i == spinTime - 1)
			{
				spinEnd = SPIN_STARTY + (ADD_SPACE * (getItem.id - 1));
			}
			while (true)
			{
				if (i == spinTime - 1)
				{
					if (spinRect.anchoredPosition.y >= spinEnd)
					{
						spiningVector.y = spinEnd;
						spinRect.DOJumpAnchorPos(spiningVector, 25, 2, 0.5f);
						break;
					}
				}
				else
				{
					if (spinRect.anchoredPosition.y >= spinEnd)
					{
						spiningVector.y = SPIN_STARTY;
						spinRect.anchoredPosition = spiningVector;
						break;
					}
				}
				spiningVector.y += spinSpeed;
				spinRect.anchoredPosition = spiningVector;
				yield return new WaitForSeconds(spinDuration);
			}
		}
		yield return null;

		audioSource.PlayOneShot(rewordSound);
		rewordText.text = getItem.messege.ToString();
		switch (getItem.rewardType)
		{
			case SlotRewardType.Gold:
				GameManager.instance.SaveState.Gold += getItem.gold;
				break;
			case SlotRewardType.Item:
				GameManager.instance.PushItem(getItem.itemID);
				break;
		}
		yield return null;

		spinCoroutine = null;
	}


	private void OnSpinStartBtn()
	{
		if (rewardSpin > 0)
		{
			rewardSpin--;
			leastAdSpinText.text = "残り : " + rewardSpin.ToString() + "回転なのだよ～";
			OnSpinStart(100f);
		}
		else
		{
			leastAdSpinText.text = "広告を見て補充するのだよ～";
		}
	}

	private void OnRewardEarnedReciever()
	{
		rewardSpin += 5;
		leastAdSpinText.text = "残り : " + rewardSpin.ToString() + "回転なのだよ～";
	}
	private void OnSpinIncrementByAdBtn()
	{
		googleAds.ShowReawrd();
	}
	private void OnCloseBtn()
	{
		mainPanel.SetActive(false);
	}
}
