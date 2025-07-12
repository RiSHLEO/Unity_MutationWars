using UnityEngine;

public class Blade : Entity
{
    public Blade_DashState DashState { get; private set; }
    public float DashDuration = 0.25f;
    public float DashSpeed = 20f;

    protected override void Awake()
    {
        base.Awake();
        DashState = new Blade_DashState(this, StateMachine, "dash");
    }

    protected override void Start()
    {
        base.Start();
    }
}

