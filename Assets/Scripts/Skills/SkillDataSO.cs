using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Data", menuName = "Game/SkillData")]
public class SkillDataSO : ScriptableObject
{
    public string SkillName;
    public Sprite Icon;
    public float Cooldown;
}
