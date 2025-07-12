using UnityEngine;

public class Player_IdleState : EntityState
{
    public Player_IdleState(Entity player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _entity.SetVelocity(0f, 0f);
    }
    public override void Update()
    {
        base.Update();

        if (_entity.MoveInput.x != 0 || _entity.MoveInput.y != 0)
            _stateMachine.ChangeState(_entity.MoveState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
