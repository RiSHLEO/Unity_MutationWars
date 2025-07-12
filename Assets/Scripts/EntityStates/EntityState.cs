using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class EntityState
{
    protected StateMachine _stateMachine;
    protected string _animBoolName;

    protected Animator _anim;
    protected Rigidbody2D _rb;
    protected Entity _entity;
    protected PlayerInputSet _input;
    protected float _stateTimer;
    protected Player_SkillManager _skillManager;
    protected Blade_DashState _bladeDashState;

    public EntityState(Entity entity, StateMachine stateMachine, string animBoolName)
    {
        _entity = entity;
        _stateMachine = stateMachine;
        _animBoolName = animBoolName;

        _anim = entity.anim;
        _rb = entity.rb;
        _input = entity.InputSet;
        _skillManager = entity.SkillManager;
    }

    public virtual void Enter()
    {
        _anim.SetBool(_animBoolName, true);
    }

    public virtual void Update()
    {
        _stateTimer -= Time.deltaTime;
        OnAbilityPress();
    }

    private void OnAbilityPress()
    {
        if (_input.Player.Ability.WasPressedThisFrame())
        {
            if (_entity is Veil veil && veil.InvulState != null && _skillManager.Invul.CanUseSkill()
                && _stateMachine.currentState != veil.InvulState)
            {
                Debug.Log("InvulState");
                _stateMachine.ChangeState(veil.InvulState);
            }

            if (_entity is Blade blade && blade.DashState != null && _skillManager.Dash.CanUseSkill())
            {
                _skillManager.Dash.SetSkillCooldown();
                _stateMachine.ChangeState(blade.DashState);
            }

            if (_entity is Stone stone && stone.KnockbackState != null && _skillManager.Knockback.CanUseSkill())
            {
                _skillManager.Knockback.TryActivate();
            }
        }
    }

    public virtual void Exit()
    {
        _anim.SetBool(_animBoolName, false);
    }
}
