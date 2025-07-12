using UnityEngine;

public class Player_KnockbackState : EntityState
{
    private Vector2 _direction;
    private float _force;
    private float _duration;

    public Player_KnockbackState(Entity entity, StateMachine stateMachine, string animBoolName): base(entity, stateMachine, animBoolName) 
    { 
    }

    public void Setup(Vector2 direction, float force, float duration)
    {
        _direction = direction;
        _force = force;
        _duration = duration;
    }

    public override void Enter()
    {
        base.Enter();
        _rb.linearVelocity = Vector2.zero; // stop current motion
        _rb.AddForce(_direction * _force, ForceMode2D.Impulse);
        _stateTimer = _duration;
    }

    public override void Update()
    {
        base.Update();
        if (_stateTimer < 0)
            _stateMachine.ChangeState(_entity.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
        _rb.linearVelocity = Vector2.zero;
    }
}
