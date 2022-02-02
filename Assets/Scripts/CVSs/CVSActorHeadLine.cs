using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CommonUtility;

public class CVSActorHeadLine : MonoBehaviour
{
    [SerializeField] AudioClip clickSound;
    [SerializeField] GameObject propS;
    [SerializeField] GameObject propL;
    [SerializeField] Button propBtn;
    [SerializeField] Image fillArea;
    [SerializeField] Image menuImage;
    [SerializeField] Sprite openSprite;
    [SerializeField] Sprite closeSprite;
    [SerializeField] Text propTextS;
    [SerializeField] Text propTextL;

    private AudioSource audioSource;
    private float slideSpeed = 5f;
    private bool isOpen = false;

	void Start()
    {
        audioSource = GetComponent<AudioSource>();
        propBtn.onClick.AddListener(OnPropBtn);
        StartCoroutine(PropChanger());
    }

    void Update()
    {
        var actorData = GameManager.instance.SaveState;
        float expect = (actorData.Hp / actorData.MaxHp);
        float value = Mathf.Lerp(fillArea.fillAmount, expect, Time.deltaTime * slideSpeed);
        fillArea.fillAmount = value;
    }

    private void OnPropBtn()
	{
        audioSource.PlayOneShot(clickSound);
		if (!isOpen)
		{
            propS.SetActive(false);
            propL.SetActive(true);
            menuImage.sprite = closeSprite;
            isOpen = true;
		}
		else
		{
            propL.SetActive(false);
            propS.SetActive(true);
            menuImage.sprite = openSprite;
            isOpen = false;
        }
    }
    private IEnumerator PropChanger()
	{
        var data = GameManager.instance.SaveState;
        string levelStr = data.Level.ToString();
        string goldStr = Mathf.CeilToInt(data.Gold).ToString();
        string expStr = data.Exp.ToString("F");
        string reqExpStr = data.RequireExp.ToString("F");
        string maxHpStr = data.MaxHp.ToString("F");
        string powerStr = data.Power.ToString("F");
        string defenceStr = data.Defence.ToString("F");
        string luckStr = data.Luck.ToString("F");

        string propS = "所持金：" + goldStr + "G\n";
        propS += "必要経験値：" + reqExpStr + "Exp\n";
        propS += "獲得経験値：" + expStr + "Exp";

        string propL = "ステータス\n";
        propL += "レベル：" + levelStr + "\n";
        propL += "所持金：" + goldStr + "G\n";
        propL += "必要経験値：" + reqExpStr + "Exp\n";
        propL += "獲得経験値：" + expStr + "Exp\n";
        propL += "最大HP：" + maxHpStr + "\n";
        propL += "攻撃力：" + powerStr + "\n";
        propL += "防御力：" + defenceStr + "\n";
        propL += "幸運値：" + luckStr;

        propTextS.text = propS;
        propTextL.text = propL;

        yield return new WaitForSeconds(3f);
        StartCoroutine(PropChanger());
    }
}
