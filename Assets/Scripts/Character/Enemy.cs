using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CommonUtility;
using State = StateMachine<Enemy>.State;

public class Enemy : EnemyBase
{
    [SerializeField] GameObject attackEffect;
    private Actor actor;
    private StateMachine<Enemy> stateMachine;

	private void Start()
	{
        stateMachine = new StateMachine<Enemy>(this);
        stateMachine.AddTransition<StateAttack, StateIdle>((int)Event.DoIdle);
        stateMachine.AddTransition<StateIdle, StateAttack>((int)Event.DoAttack);
        stateMachine.AddTransition<StateAttack, StateAttack>((int)Event.DoAttack);
        stateMachine.AddAnyTransition<StateDeath>(((int)Event.DoDeath));
        stateMachine.Start<StateIdle>();
    }
	void Update()
    {
		if (isInit)
		{
            stateMachine.Update();
		}
	}


    //------------------------------------------
    // 内部共有関数
    //------------------------------------------


    //------------------------------------------
    // 抽象クラスの実装
    //------------------------------------------
	public override void ApplyDamage(float damage)
	{
        if (!deathRequested && isInit)
        {
            if (hp <= 0)
            {
                deathRequested = true;
                hp = 0;
                stateMachine.Dispatch(((int)Event.DoDeath));
            }
            else
            {
                float val = (damage / 2) - (data.Defence / 3);
                float calcDamage = Mathf.Clamp(val, 0.1f, float.MaxValue);
                hp -= calcDamage;
                OnDamageNotifyerHandler?.Invoke(this, calcDamage);
            }
        }
    }

	public override void OnEnterAttackArea(Actor actor)
	{
        if (isInit)
        {
            if (this.actor == null)
            {
                this.actor = actor;
            }
            stateMachine.Dispatch(((int)Event.DoAttack));
            isAttackEnable = true;
        }
    }

	public override void OnExitAttackArea()
	{
        if (isInit)
        {
            stateMachine.Dispatch(((int)Event.DoIdle));
            isAttackEnable = false;
        }
    }


	//------------------------------------------
	// ステートマシン
	//------------------------------------------
	enum Event
	{
        DoIdle, DoAttack, DoDeath,
	}
    private class StateIdle : State
	{
        Tweener tweener;
		protected override void OnEnter(State prevState)
		{
            if (tweener == null)
            {
                tweener = owner._transform.DOScaleY(owner.scaleY + 0.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
            else tweener.Restart();
		}
		protected override void OnExit(State nextState)
		{
            tweener.Pause();
		}
	}
    private class StateAttack : State
	{
        Tweener tweener;
        protected override void OnEnter(State prevState)
        {
            if (tweener == null)
            {
                tweener = owner._transform.DOPunchScale(new Vector3(0f, 1f, 0f), owner.data.Duration, 5).SetRelative().
                    SetEase(Ease.InExpo).OnComplete(OnApplyDamage);
            }
            else tweener.Restart();
        }
		protected override void OnExit(State nextState)
		{
            tweener?.Pause();
		}
		private void OnApplyDamage()
		{
            if (owner.actor.enabled)
            {
                owner.attackEffect.SetActive(false);
                owner.attackEffect.SetActive(true);
                owner.actor.ApplyDamage(owner.data.Power);
                owner.OnAttackNotifyerHandler?.Invoke();
                if (owner.actor.enabled && owner.isAttackEnable) stateMachine.Dispatch(((int)Event.DoAttack));
                else stateMachine.Dispatch(((int)Event.DoIdle));
            }
            else stateMachine.Dispatch(((int)Event.DoIdle));
        }
    }
    private class StateDeath : State
	{
        float duration = 0.5f;
        protected override void OnEnter(State prevState)
        {
            GameManager.instance.AddEXP(owner.data.Exp);
            GameManager.instance.SaveState.Gold += owner.data.Gold;
            owner._transform.DOScaleY(0, duration).SetEase(Ease.InExpo).OnComplete(OnScalingEnd);
        }
        private void OnScalingEnd()
        {
            GameManager.instance.PushEnedex(owner.data.ID);
            GameManager.instance.OnSaveData();
            owner.enabled = false;
            owner.gameObject.SetActive(false);
            owner.OnDeathNotifyerHandler?.Invoke(owner);
        }
    }
}
