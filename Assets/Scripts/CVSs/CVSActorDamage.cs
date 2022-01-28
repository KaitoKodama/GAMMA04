using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CVSActorDamage : MonoBehaviour
{
    [SerializeField] GameObject poolPrefab;
    [SerializeField] Transform poolParent;
    private List<GameObject> poolLists = new List<GameObject>();
    private Transform actorform;
    
    private void Awake()
    {
        SetPool();
    }
    void Start()
    {
        var obj = GameObject.FindWithTag("Player");
        actorform = obj.transform;
        obj.GetComponent<Actor>().OnDamageNotifyerHandler += OnDamageReciever;
    }

    //------------------------------------------
    // デリゲート通知
    //------------------------------------------
    private void OnDamageReciever(float damage)
    {
        var obj = GetPool();
        if (obj != null)
        {
            obj.SetActive(true);
            obj.GetComponent<Text>().text = damage.ToString("F");

            var position = actorform.position;
            position.y += 3f;
            var screen = Camera.main.WorldToScreenPoint(position);
            screen.x += Random.Range(-50, 50);
            screen.y += Random.Range(-50, 50);
            obj.transform.position = screen;
            obj.transform.DOMoveY(-25f, 0.5f).SetRelative();
        }
    }


    //------------------------------------------
    // 内部共有関数
    //------------------------------------------
    private void SetPool()
    {
        for (int i = 0; i < 5; i++)
        {
            var obj = Instantiate(poolPrefab, poolParent);
            obj.SetActive(false);
            poolLists.Add(obj);
        }
    }
    private GameObject GetPool()
    {
        foreach (var el in poolLists)
        {
            if (!el.activeSelf) return el;
        }
        return null;
    }
}
