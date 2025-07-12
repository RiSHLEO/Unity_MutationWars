using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash Dash {  get; private set; }
    public Skill_Invulnerability Invul {  get; private set; }
    public Skill_Knockback Knockback { get; private set; }

    private void Awake()
    {
        Dash = GetComponentInChildren<Skill_Dash>();
        Invul = GetComponentInChildren<Skill_Invulnerability>();
        Knockback = GetComponentInChildren<Skill_Knockback>();
    }
}
