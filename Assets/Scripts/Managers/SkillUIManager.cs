using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cooldownFill;
    [SerializeField] private TextMeshProUGUI cooldownText;

    public static SkillUIManager Instance { get; private set; }
    private Skill_Base trackedSkill;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetSkill(Skill_Base skill)
    {
        trackedSkill = skill;
        iconImage.sprite = skill.Data.Icon;
    }

    private void Update()
    {
        if (trackedSkill == null) return;

        float remaining = trackedSkill.CooldownRemaining();
        float max = trackedSkill.Data.Cooldown;

        if (remaining > 0)
        {
            // Skill is on cooldown
            float fillAmount = remaining / max;
            cooldownFill.fillAmount = fillAmount;

            int timeLeft = Mathf.CeilToInt(remaining);
            cooldownText.text = timeLeft.ToString();
        }
        else
        {
            // Skill is ready
            cooldownFill.fillAmount = 0;
            cooldownText.text = "";
        }
    }
}
