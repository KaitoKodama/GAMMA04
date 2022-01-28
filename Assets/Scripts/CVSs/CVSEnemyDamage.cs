using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CVSEnemyDamage : MonoBehaviour
{
	class DamagePool
	{
		public DamagePool(GameObject obj)
		{
			this.obj = obj;
			text = obj.GetComponent<Text>();
		}
		public void Do(Vector3 pos)
		{
			obj.transform.position = pos;
			obj.transform.DOMoveY(25f, duration).SetRelative();
		}

		public GameObject obj;
		public Text text;
		private float duration = 0.5f;
	}
	[SerializeField] Transform prefabParent;
	[SerializeField] GameObject damagePrefab;
	[SerializeField] GameObject enemyInfoBox;
	[SerializeField] Text enemyInfoTxt;
	[SerializeField] Image fillImage;

	private List<DamagePool> damagePools;
	private EnemyBase currentEnemy;
	private Camera _camera;
	private float barSpeed = 10f;
	private int poolNum = 10;
	private bool isAnyDamage = false;


	private void Awake()
	{
		SetDamagePool();
	}
	private void Start()
	{
		enemyInfoBox.SetActive(false);
		_camera = Camera.main;
	}
	private void Update()
	{
		if (isAnyDamage)
		{
			if (currentEnemy != null)
			{
				float fitHp = currentEnemy.Hp / currentEnemy.Maxhp;
				float value = Mathf.Lerp(fillImage.fillAmount, fitHp, Time.deltaTime * barSpeed);
				fillImage.fillAmount = value;

				if (currentEnemy.Hp <= 0 || Mathf.Approximately(fillImage.fillAmount, fitHp)) 
				{
					enemyInfoBox.SetActive(false);
					isAnyDamage = false;
				}
				else
				{
					enemyInfoBox.SetActive(true);
					var position = currentEnemy.transform.position;
					position.y += 3f;
					var screen = _camera.WorldToScreenPoint(position);
					enemyInfoBox.transform.position = screen;
				}
			}
		}
	}


	//------------------------------------------
	// 内部共有関数
	//------------------------------------------
	private void SetDamagePool()
	{
		damagePools = new List<DamagePool>(poolNum);
		for (int i = 0; i < poolNum; i++)
		{
			var obj = Instantiate(damagePrefab, prefabParent);
			obj.SetActive(false);
			var pool = new DamagePool(obj);
			damagePools.Add(pool);
		}
	}
	private DamagePool GetDamagePool()
	{
		foreach (var pool in damagePools) 
		{
			if(!pool.obj.activeSelf)
			{
				return pool;
			}
		}
		return null;
	}


	//------------------------------------------
	// 外部共有関数
	//------------------------------------------
	public void DisplayHealth(EnemyBase enemy)
	{
		isAnyDamage = true;
		currentEnemy = enemy;
		enemyInfoTxt.text = enemy.Data.Name + "\nレベル：" + enemy.Data.Level;
	}
	public void DisplayDamage(EnemyBase enemy, float damage)
	{
		var pool = GetDamagePool();
		if (pool != null)
		{
			pool.obj.SetActive(true);
			pool.text.text = damage.ToString("F");

			var position = enemy.transform.position;
			position.y += 3f;
			var screen = _camera.WorldToScreenPoint(position);
			screen.x += Random.Range(-50, 50);
			screen.y += Random.Range(-50, 50);
			pool.Do(screen);
		}
	}
}
