using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CommonUtility;

public class CVSActorEnedexLine : MonoBehaviour
{
    class Enedex
	{
        public Enedex(GameObject obj, EnedexGrid grid)
		{
            this.obj = obj;
            this.grid = grid;
		}
        public EnedexGrid grid;
        public GameObject obj;
	}
    [System.Serializable]
    class EnedexData
	{
        public SOEnemyData[] datas;
    }
    [SerializeField] AudioClip clickSound;

    [SerializeField] Button backBtn;
    [SerializeField] Button nextBtn;
    [SerializeField] Button openBtn;
    [SerializeField] Text pageText;

    [SerializeField] GameObject enedexBox;

    [SerializeField] GameObject enedexGridPrefab;
    [SerializeField] Transform enedexParent;
    [SerializeField] List<EnedexData> enedexDataList;

    private AudioSource audioSource;
    private List<Enedex> enedexList = new List<Enedex>();

    private int currentPage = 0;


	private void Awake()
	{
        int maxLength = 0;
        for (int i = 0; i < enedexDataList.Count; i++)
		{
            if (enedexDataList[i].datas.Length > maxLength)
			{
                maxLength = enedexDataList[i].datas.Length;
            }
		}

		for (int i = 0; i < maxLength; i++)
		{
            var obj = Instantiate(enedexGridPrefab, enedexParent);
            var grid = obj.GetComponent<EnedexGrid>();
            enedexList.Add(new Enedex(obj, grid));
            grid.DoHide();
        }
	}
	void Start()
    {
        pageText.text = "ページ：1/" + enedexDataList.Count.ToString();
        for (int i = 0; i < enedexList.Count; i++)
        {
            if (i < enedexDataList[0].datas.Length)
            {
                enedexList[i].grid.DisplayGrid(enedexDataList[0].datas[i]);
            }
        }
        audioSource = GetComponent<AudioSource>();
        openBtn.onClick.AddListener(OnMenuBtn);
        nextBtn.onClick.AddListener(OnNextBtn);
        backBtn.onClick.AddListener(OnBackBtn);
        enedexBox.SetActive(false);
    }

    private void OnMenuBtn()
	{
        audioSource.PlayOneShot(clickSound);
        enedexBox.SetActive(Utility.FilpFlop(enedexBox.activeSelf));
        RefleshGridByPage();
	}

    private void OnNextBtn()
	{
        audioSource.PlayOneShot(clickSound);
        currentPage++;
        if (currentPage >= enedexDataList.Count) currentPage = enedexDataList.Count - 1;
        RefleshGridByPage();
    }
    private void OnBackBtn()
	{
        audioSource.PlayOneShot(clickSound);
        currentPage--;
        if (currentPage <= 0) currentPage = 0;
        RefleshGridByPage();
    }

    private void RefleshGridByPage()
	{
        pageText.text = "ページ：" + (currentPage + 1).ToString() + "/" + enedexDataList.Count.ToString();
        foreach (var dex in enedexList)
		{
            dex.grid.DoHide();
		}
        for (int i = 0; i < enedexList.Count; i++)
        {
            if (i < enedexDataList[currentPage].datas.Length)
            {
                enedexList[i].grid.DisplayGrid(enedexDataList[currentPage].datas[i]);
            }
        }
    }
}
