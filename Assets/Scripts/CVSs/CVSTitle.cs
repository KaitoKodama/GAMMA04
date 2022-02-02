using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CommonUtility;

public class CVSTitle : MonoBehaviour
{
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip openSound;

    [SerializeField] List<AnyDictionary<BTN, Button>> buttons;
    [SerializeField] List<AnyDictionary<OBJ, GameObject>> objects;
    [SerializeField] RectTransform ttlRect01;
    [SerializeField] RectTransform ttlRect02;
    [SerializeField] Image startImage;
    [SerializeField] Text stateText;
    [SerializeField] Text resetText;

    private AudioSource audioSource;

    enum BTN { Start, SetOpen, HowOpen, SetClose, HowClose, SetCright, SetReset, SetState, Reset, }
    enum OBJ { Chara01, Chara02, HowPanel, SetPanel, SetCrightBox, SetResetBox, SetStateBox, }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        startImage.DOFade(0.5f, 1f).SetLoops(-1, LoopType.Yoyo);
        Utility.GetDICVal(OBJ.Chara01, objects).transform.DOPunchScale(new Vector3(0, 0.3f, 0), 5f, 1).SetLoops(-1, LoopType.Yoyo);
        Utility.GetDICVal(OBJ.Chara02, objects).transform.DOPunchScale(new Vector3(0, 0.2f, 0), 4f, 1).SetLoops(-1, LoopType.Yoyo);

        Utility.GetDICVal(BTN.Start, buttons).onClick.AddListener(OnStartBtn);
        Utility.GetDICVal(BTN.SetOpen, buttons).onClick.AddListener(OnSetOpenBtn);
        Utility.GetDICVal(BTN.HowOpen, buttons).onClick.AddListener(OnHowOpenBtn);

        Utility.GetDICVal(BTN.SetClose, buttons).onClick.AddListener(OnSetCloseBtn);
        Utility.GetDICVal(BTN.HowClose, buttons).onClick.AddListener(OnHowCloseBtn);
        Utility.GetDICVal(BTN.SetCright, buttons).onClick.AddListener(OnSetCrightBtn);
        Utility.GetDICVal(BTN.SetReset, buttons).onClick.AddListener(OnSetResetBtn);
        Utility.GetDICVal(BTN.SetState, buttons).onClick.AddListener(OnSetStateBtn);

        Utility.GetDICVal(BTN.Reset, buttons).onClick.AddListener(OnResetBtn);


        StartCoroutine(OnStartEffect());
    }


    private IEnumerator OnStartEffect()
	{
        yield return new WaitForSeconds(0.5f);
        
        ttlRect01.anchoredPosition = new Vector2(-400f, -167f);
        ttlRect02.anchoredPosition = new Vector2(400f, -273f);
        ttlRect01.DOAnchorPosX(400f, 1f).SetEase(Ease.OutBounce);
        ttlRect02.DOAnchorPosX(-400f, 1f).SetEase(Ease.OutBounce);
    }

    private void DisableSetPanels()
	{
        audioSource.PlayOneShot(clickSound);
        Utility.GetDICVal(OBJ.SetCrightBox, objects).SetActive(false);
        Utility.GetDICVal(OBJ.SetStateBox, objects).SetActive(false);
        Utility.GetDICVal(OBJ.SetResetBox, objects).SetActive(false);
    }

    //------------------------------------------
    // ファーストボタン
    //------------------------------------------
    private void OnStartBtn()
	{
        audioSource.PlayOneShot(clickSound);
        if (GameManager.instance.SaveState.IsPatchHigherThanRequire(StagePatch.TutorialCompleted))
		{
            GameManager.instance.OnSceneTransit(SceneName.Stage01);
        }
		else
		{
            GameManager.instance.OnSceneTransit(SceneName.TutorialScene);
        }
        
	}
    private void OnHowOpenBtn()
	{
        audioSource.PlayOneShot(openSound);
        Utility.GetDICVal(OBJ.HowPanel, objects).SetActive(true);
    }
    private void OnSetOpenBtn()
    {
        audioSource.PlayOneShot(openSound);
        Utility.GetDICVal(OBJ.SetPanel, objects).SetActive(true);
    }

    //------------------------------------------
    // セカンドボタン
    //------------------------------------------
    private void OnHowCloseBtn()
    {
        audioSource.PlayOneShot(clickSound);
        Utility.GetDICVal(OBJ.HowPanel, objects).SetActive(false);
    }
    private void OnSetCloseBtn()
    {
        audioSource.PlayOneShot(clickSound);
        Utility.GetDICVal(OBJ.SetPanel, objects).SetActive(false);
    }

    private void OnSetCrightBtn()
	{
        DisableSetPanels();
        Utility.GetDICVal(OBJ.SetCrightBox, objects).SetActive(true);
    }
    private void OnSetStateBtn()
    {
        DisableSetPanels();
        Utility.GetDICVal(OBJ.SetStateBox, objects).SetActive(true);

        var data = GameManager.instance.SaveState;
        string levelStr = data.Level.ToString();
        string goldStr = Mathf.CeilToInt(data.Gold).ToString();
        string expStr = data.Exp.ToString("F");
        string reqExpStr = data.RequireExp.ToString("F");
        string maxHpStr = data.MaxHp.ToString("F");
        string powerStr = data.Power.ToString("F");
        string defenceStr = data.Defence.ToString("F");
        string luckStr = data.Luck.ToString("F");

        string state = "レベル：" + levelStr + "\n";
        state += "所持金：" + goldStr + "G\n";
        state += "必要経験値：" + reqExpStr + "Exp\n";
        state += "獲得経験値：" + expStr + "Exp\n";
        state += "最大HP：" + maxHpStr + "\n";
        state += "攻撃力：" + powerStr + "\n";
        state += "防御力：" + defenceStr + "\n";
        state += "幸運値：" + luckStr;
        stateText.text = state;
    }
    private void OnSetResetBtn()
	{
        DisableSetPanels();
        Utility.GetDICVal(OBJ.SetResetBox, objects).SetActive(true);
    }


    //------------------------------------------
    // サードボタン
    //------------------------------------------
    private void OnResetBtn()
    {
        audioSource.PlayOneShot(clickSound);
        resetText.text = "初期化を完了しました";
        GameManager.instance.DeleteSaveData();
    }
}
