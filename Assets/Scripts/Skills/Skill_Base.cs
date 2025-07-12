using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General Details")]
    [SerializeField] private SkillDataSO _skillData;
    private float _lastTimeUsed;

    protected virtual void Awake()
    {
        _lastTimeUsed -= _skillData.Cooldown;
    }

    public bool CanUseSkill()
    {
        if (OnCooldown())
            return false;

        return true;
    }

    public SkillDataSO Data => _skillData;
    private bool OnCooldown() => Time.time < _lastTimeUsed + _skillData.Cooldown;
    public void SetSkillCooldown() => _lastTimeUsed = Time.time;
    public float CooldownRemaining() => Mathf.Max(0, (_lastTimeUsed + _skillData.Cooldown) - Time.time);
}
