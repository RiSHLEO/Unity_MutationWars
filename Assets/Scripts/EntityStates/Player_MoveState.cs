using UnityEngine;

public class Player_MoveState : EntityState
{
    public Player_MoveState(Entity entity, StateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (_entity.MoveInput.x == 0 && _entity.MoveInput.y == 0)
            _stateMachine.ChangeState(_entity.IdleState);

        Vector2 movement = _entity.MoveInput.normalized;
        _entity.SetVelocity(movement.x * _entity.MoveSpeed, movement.y * _entity.MoveSpeed);
    }
}
