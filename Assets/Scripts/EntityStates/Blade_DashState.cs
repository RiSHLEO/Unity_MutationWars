using UnityEngine;

public class Blade_DashState : EntityState
{
    private Vector2 _dashDir;
    private Blade _blade;

    public Blade_DashState(Entity entity, StateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        _blade = _entity.GetComponent<Blade>();
    }

    public override void Enter()
    {
        base.Enter();
        _dashDir = _entity.MoveInput != Vector2.zero ? _entity.MoveInput.normalized : Vector2.up;
        _stateTimer = _blade.DashDuration;
    }

    public override void Update()
    {
        base.Update();
        _entity.SetVelocity(_blade.DashSpeed * _dashDir.x, _blade.DashSpeed * _dashDir.y);

        if (_stateTimer < 0)
            _stateMachine.ChangeState(_entity.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
        _entity.SetVelocity(0, 0);
    }
}
