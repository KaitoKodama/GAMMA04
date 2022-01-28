using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CommonUtility;

public class CVSTitle : MonoBehaviour
{
    [SerializeField] AudioClip txtSound;
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip openSound;

    [SerializeField] GameObject bgmMaster;
    [SerializeField] GameObject chara01;
    [SerializeField] GameObject chara02;
    [SerializeField] GameObject howtoPanel;
    [SerializeField] Button startBtn;
    [SerializeField] Button howtoOpenBtn;
    [SerializeField] Button howtoCloseBtn;
    [SerializeField] Image startImage;
    [SerializeField] Text titleText;

    private AudioSource audioSource;
    private string titleStr = "レベルを上げて\nダンジョン攻略";

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startBtn.onClick.AddListener(OnStartBtn);
        howtoOpenBtn.onClick.AddListener(OnOpenBtn);
        howtoCloseBtn.onClick.AddListener((OnCloseBtn));
        StartCoroutine(OnStartEffect());
    }
    private void Update()
    {
        if (Music.IsJustChangedBar())
        {
            chara01.transform.DOPunchScale(new Vector3(0, 0.5f, 0), 0.3f).SetRelative();
        }
        else if (Music.IsJustChangedBeat())
        {
            chara02.transform.DOPunchScale(new Vector3(0, 0.5f, 0), 0.3f).SetRelative();
        }
    }
    private IEnumerator OnStartEffect()
	{
        titleText.text = "";
        yield return new WaitForSeconds(2f);

        audioSource.clip = txtSound;
        audioSource.Play();
        titleText.DOText(titleStr, 1f);
        yield return new WaitForSeconds(1f);

        audioSource.Stop();
        bgmMaster.SetActive(true);
        startImage.DOFade(0.5f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnStartBtn()
	{
        GameManager.instance.OnSceneTransit(SceneName.Stage01);
	}
    private void OnOpenBtn()
	{
        audioSource.PlayOneShot(openSound);
        howtoPanel.SetActive(true);
    }
    private void OnCloseBtn()
	{
        audioSource.PlayOneShot(clickSound);
        howtoPanel.SetActive(false);
    }
}
