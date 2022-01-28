using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CommonUtility;
using State = StateMachine<Actor>.State;

public class Actor : MonoBehaviour
{
    [SerializeField] AudioSource loopSource;
    [SerializeField] AudioSource shotSource;
    [SerializeField] AudioClip attackSound;
    [SerializeField] GameObject[] attackEffects;
    [SerializeField] LayerMask enemyMask;

    private Joystick joystick;
    private EnemyBase enemyBase;
    private Rigidbody2D rigid;
    private Transform currentform;
    private Transform _transform;
    private Animator animator;
    private SpriteRenderer render;
    private StateMachine<Actor> stateMachine;

    private readonly int HorizontalHash = Animator.StringToHash("Horizontal");
    private readonly int VerticalHash = Animator.StringToHash("Vertical");
    private readonly int IsMoveingHash = Animator.StringToHash("IsMoveing");
    private readonly int IsAttackHash = Animator.StringToHash("IsAttack");

    private float speed = 100f;
    private float horizontal, vertical;
    private float power, defence;
    private float multiplyPowe = 3f;
    private float rayRadius = 3f;
    private bool deathRequested = false;
    private bool isEnableInput = true;

	void Start()
    {
        //初期化
        var manager = GameManager.instance;
        if (manager.SaveState.Hp <= 0)
		{
            GameManager.instance.SaveState.Hp = manager.SaveState.MaxHp / 2;
        }
        power = manager.SaveState.Power;
        defence = manager.SaveState.Defence;

        //キャッシュ
        _transform = transform;
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        joystick = GameObject.FindWithTag("Joystick").GetComponent<Joystick>();

        //ステートマシン
        stateMachine = new StateMachine<Actor>(this);
        stateMachine.AddTransition<StateAttack, StateMove>((int)Event.DoMove);
        stateMachine.AddTransition<StateMove, StateAttack>((int)Event.DoAttack);
        stateMachine.AddAnyTransition<StateDeath>(((int)Event.DoDeath));
        stateMachine.Start<StateMove>();

        InitLocationDirection();
    }
    private void Update()
    {
        stateMachine.Update();
		if (isEnableInput)
		{
            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;

            float abs = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            float volume = Mathf.Clamp01(abs);
            loopSource.volume = volume;
		}
		else
		{
            horizontal = 0;
            vertical = 0;
		}
    }
    private void FixedUpdate()
	{
        stateMachine.FixedUpdate();
	}


    //------------------------------------------
    // 外部共有関数
    //------------------------------------------
    public delegate void OnBuffStartNotifyer(SOItemBase item, float elapse);
    public delegate void OnBuffEndNotifyer(SOItemBase item);
    public delegate void OnDamageNotifyer(float damage);
    public OnBuffStartNotifyer OnBuffStartNotifyerHandler;
    public OnBuffEndNotifyer OnBuffEndNotifyerHandler;
    public event OnDamageNotifyer OnDamageNotifyerHandler;


    public float Power { set => power = value; }
    public float Defence { set => defence = value; }
	public bool IsEnableInput { set => isEnableInput = value; }

	public void ApplyDamage(float damage)
    {
        if (!deathRequested)
        {
            Camera.main.transform.DOShakePosition(0.5f, 0.3f);
            if (GameManager.instance.SaveState.Hp <= 0)
            {
                deathRequested = true;
                GameManager.instance.SaveState.Hp = 0;
                stateMachine.Dispatch(((int)Event.DoDeath));
            }
			else
			{
                float val = (damage / 2) - (defence / 3);
                float calcDamage = Mathf.Clamp(val, 0.1f, float.MaxValue);
                OnDamageNotifyerHandler?.Invoke(calcDamage);
                GameManager.instance.SaveState.Hp -= calcDamage;
			}
        }
    }


    //------------------------------------------
    // 内部共有関数
    //------------------------------------------
    private void InitLocationDirection()
	{
        var manager = GameManager.instance;
        Vector3 location = Vector3.zero;
        var objs = GameObject.FindGameObjectsWithTag("Entrance");
        foreach(var obj in objs)
		{
            var comp = obj.GetComponent<Entrance>();
            if (manager.EntryType != EntryType.Unset && manager.EntryType != comp.EntryType)
			{
                location = comp.GetVector3();
			}
		}
        if (location != Vector3.zero)
        {
            transform.position = location;
            render.flipX = true;
            animator.SetFloat(HorizontalHash, 0f);
            animator.SetFloat(VerticalHash, -1f);
        }
    }
    private void SetAnimation(float hor, float ver)
	{
        if (horizontal != 0 || vertical != 0)
		{
            var value = Mathf.Sign(horizontal);
            if (value == -1) render.flipX = true;
            else if (value == 1) render.flipX = false;

			animator.SetFloat(HorizontalHash, hor);
			animator.SetFloat(VerticalHash, ver);
			animator.SetBool(IsMoveingHash, true);

        }
        else animator.SetBool(IsMoveingHash, false);

    }
    private Vector3 GetDirection()
	{
        var direction = new Vector3(horizontal, vertical, 0).normalized;
        direction = _transform.TransformDirection(direction) * speed;
        return direction;
	}


    //------------------------------------------
    // アニメーションイベント
    //------------------------------------------
    public void OnAttackEffectBegin()
	{
		if (enemyBase != null)
		{
            var rndNum = Random.Range(0, attackEffects.Length);
            foreach (var el in attackEffects)
			{
                el.SetActive(false);
			}

            float damage = power;
            if (Utility.Probability((25 + GameManager.instance.SaveState.Luck) / 2)) damage *= multiplyPowe;
            enemyBase.ApplyDamage(damage);
            shotSource.PlayOneShot(attackSound);
            attackEffects[rndNum].SetActive(true);
            attackEffects[rndNum].transform.DOShakePosition(0.1f, 0.1f);
            attackEffects[rndNum].transform.LookAt(currentform.position);
		}
    }


	//------------------------------------------
	// 持続系効果
	//------------------------------------------
	public IEnumerator SetPowerBuff(SOItemBase item, float elapse, float additive)
	{
        power += additive;
        OnBuffStartNotifyerHandler?.Invoke(item, elapse);
        yield return new WaitForSeconds(elapse);

        power -= additive;
        OnBuffEndNotifyerHandler?.Invoke(item);
    }
    public IEnumerator SetDefenceBuff(SOItemBase item, float elapse, float additive)
    {
        defence += additive;
        OnBuffStartNotifyerHandler?.Invoke(item, elapse);
        yield return new WaitForSeconds(elapse);

        defence -= additive;
        OnBuffEndNotifyerHandler?.Invoke(item);
    }


    //------------------------------------------
    // ステートマシン
    //------------------------------------------
    enum Event
	{
        DoMove, DoAttack, DoDeath, 
	}
    private class StateMove : State
	{
        protected override void OnUpdate()
		{
            var origin = owner._transform.position;
            var hit = Physics2D.OverlapCircle(origin, owner.rayRadius, owner.enemyMask);
            if (hit != null)
            {
                var obj = hit.gameObject;
                var target = obj.GetComponent<EnemyBase>();
                target.OnEnterAttackArea(owner);
                owner.enemyBase = target;
                owner.currentform = obj.transform;
                stateMachine.Dispatch(((int)Event.DoAttack));
            }
			else
			{
                owner.SetAnimation(owner.horizontal, owner.vertical);
			}
        }
		protected override void OnFixedUpdate()
		{
            owner.rigid.AddForce(owner.GetDirection());
		}
	}
    private class StateAttack : State
	{
		protected override void OnEnter(State prevState)
		{
            owner.animator.SetBool(owner.IsAttackHash, true);
        }
		protected override void OnUpdate()
		{
            var origin = owner._transform.position;
            var hit = Physics2D.OverlapCircle(origin, owner.rayRadius, owner.enemyMask);
            if (hit == null)
            {
                owner.enemyBase.OnExitAttackArea();
                owner.enemyBase = null;
                owner.currentform = null;
                stateMachine.Dispatch(((int)Event.DoMove));
            }
			else
			{
                var dir = (owner.currentform.position - owner._transform.position).normalized;
                owner.SetAnimation(dir.x, dir.y);
			}
        }
		protected override void OnFixedUpdate()
		{
            owner.rigid.AddForce(owner.GetDirection());
        }
		protected override void OnExit(State nextState)
		{
            owner.animator.SetBool(owner.IsAttackHash, false);
        }
	}
    private class StateDeath : State
	{
		protected override void OnEnter(State prevState)
		{
            owner.enabled = false;
            owner.gameObject.SetActive(false);
            GameManager.instance.SaveState.Exp = Mathf.Clamp(GameManager.instance.SaveState.Exp / 2, 0, float.MaxValue);
            GameManager.instance.EntryType = EntryType.Unset;
            GameManager.instance.OnSceneTransit(SceneName.Stage01);
        }
	}
}
