using Photon.Pun;
using UnityEngine;

public class Blade_DashState : EntityState
{
    private Vector2 _dashDir;
    private Blade _blade;
    private PhotonView _photonView;
    private TrailRenderer _trailRenderer;

    public Blade_DashState(Entity entity, StateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        _blade = _entity.GetComponent<Blade>();
        _photonView = _entity.GetComponent<PhotonView>();
        _trailRenderer = _entity.GetComponentInChildren<TrailRenderer>();
    }

    public override void Enter()
    {
        base.Enter();
        _trailRenderer.emitting = true;
        _dashDir = _entity.MoveInput != Vector2.zero ? _entity.MoveInput.normalized : Vector2.up;
        _stateTimer = _blade.DashDuration;

        if (_photonView.IsMine)
            _photonView.RPC("RPC_StartDashTrail", RpcTarget.Others);
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
        _trailRenderer.emitting = false;
        _entity.SetVelocity(0, 0);

        if (_photonView.IsMine)
            _photonView.RPC("RPC_EndDashTrail", RpcTarget.Others);
    }
}
