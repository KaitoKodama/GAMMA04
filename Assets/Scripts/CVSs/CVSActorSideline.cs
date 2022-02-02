using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CommonUtility;

public class CVSActorSideline : MonoBehaviour
{
	[SerializeField] AudioClip clickSound;
	[SerializeField] AudioClip gradeSound;

	[SerializeField] GameObject ptBox;

	[Header("UIイメージ")]
	[SerializeField] Image menuImage;

	[Header("UIボタン")]
	[SerializeField] Button homeBtn;
	[SerializeField] Button menuBtn;
	[SerializeField] Button powerBtn;
	[SerializeField] Button defenceBtn;
	[SerializeField] Button maxHpBtn;
	[SerializeField] Button luckBtn;

	[Header("UIテキスト")]
	[SerializeField] Text ptTxt;
	[SerializeField] Text powerTxt;
	[SerializeField] Text defenceTxt;
	[SerializeField] Text maxHpTxt;
	[SerializeField] Text luckTxt;

	[Header("スプライト")]
	[SerializeField] Sprite openSprite;
	[SerializeField] Sprite closeSprite;

	private Actor actor;
	private AudioSource audioSource;
	private float addPower = 0.5f;
	private float addDefence = 0.5f;
	private float addMaxHp = 10f;
	private float addLuck = 0.25f;

	private bool isOpen = false;

	private void Start()
	{
		ptBox.SetActive(false);
		audioSource = GetComponent<AudioSource>();
		actor = GameObject.FindWithTag("Player").GetComponent<Actor>();
		homeBtn.onClick.AddListener(OnHomeBtn);
		menuBtn.onClick.AddListener(OnMenuBtn);
		powerBtn.onClick.AddListener(OnPtPowerBtn);
		defenceBtn.onClick.AddListener(OnPtDefenceBtn);
		maxHpBtn.onClick.AddListener(OnPtMaxHpBtn);
		luckBtn.onClick.AddListener(OnPtLuckBtn);
	}

	private void OnMenuBtn()
	{
		audioSource.PlayOneShot(clickSound);
		if (!isOpen)
		{
			ptBox.SetActive(true);
			menuImage.sprite = closeSprite;
			ptTxt.text = "ステータスポイント\n" + GameManager.instance.SaveState.Gifted.ToString();
			powerTxt.text = "攻撃力：" + GameManager.instance.SaveState.Power.ToString("F");
			defenceTxt.text = "防御力：" + GameManager.instance.SaveState.Defence.ToString("F");
			maxHpTxt.text = "最大HP：" + GameManager.instance.SaveState.MaxHp.ToString("F");
			luckTxt.text = "幸運値：" + GameManager.instance.SaveState.Luck.ToString("F");
			isOpen = true;
		}
		else
		{
			ptBox.SetActive(false);
			menuImage.sprite = openSprite;
			isOpen = false;
		}
		GameManager.instance.OnSaveData();
	}
	private void OnHomeBtn()
	{
		audioSource.PlayOneShot(clickSound);
		GameManager.instance.OnSaveData();
		GameManager.instance.OnSceneTransit(SceneName.TitleScene);
	}


	private void OnPtPowerBtn()
	{
		if (GameManager.instance.SaveState.Gifted > 0)
		{
			audioSource.PlayOneShot(gradeSound);
			GameManager.instance.SaveState.Gifted--;
			GameManager.instance.SaveState.Power += addPower;
			actor.Power = GameManager.instance.SaveState.Power;
			powerTxt.text = "攻撃力：" + GameManager.instance.SaveState.Power.ToString("F");
			ptTxt.text = "ステータスポイント\n" + GameManager.instance.SaveState.Gifted.ToString();
			actor.OnStateChangedNotifyer();
		}
	}
	private void OnPtDefenceBtn()
	{
		if (GameManager.instance.SaveState.Gifted > 0)
		{
			audioSource.PlayOneShot(gradeSound);
			GameManager.instance.SaveState.Gifted--;
			GameManager.instance.SaveState.Defence += addDefence;
			actor.Defence = GameManager.instance.SaveState.Defence;
			defenceTxt.text = "防御力：" + GameManager.instance.SaveState.Defence.ToString("F");
			ptTxt.text = "ステータスポイント\n" + GameManager.instance.SaveState.Gifted.ToString();
			actor.OnStateChangedNotifyer();
		}
	}
	private void OnPtMaxHpBtn()
	{
		if (GameManager.instance.SaveState.Gifted > 0)
		{
			audioSource.PlayOneShot(gradeSound);
			GameManager.instance.SaveState.Gifted--;
			GameManager.instance.SaveState.MaxHp += addMaxHp;
			maxHpTxt.text = "最大HP：" + GameManager.instance.SaveState.MaxHp.ToString("F");
			ptTxt.text = "ステータスポイント\n" + GameManager.instance.SaveState.Gifted.ToString();
			actor.OnStateChangedNotifyer();
		}
	}
	private void OnPtLuckBtn()
	{
		if (GameManager.instance.SaveState.Gifted > 0)
		{
			audioSource.PlayOneShot(gradeSound);
			GameManager.instance.SaveState.Gifted--;
			GameManager.instance.SaveState.Luck += addLuck;
			luckTxt.text = "幸運値：" + GameManager.instance.SaveState.Luck.ToString("F");
			ptTxt.text = "ステータスポイント\n" + GameManager.instance.SaveState.Gifted.ToString();
			actor.OnStateChangedNotifyer();
		}
	}
}
