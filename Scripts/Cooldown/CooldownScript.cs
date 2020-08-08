using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownScript : MonoBehaviour
{
    public Image abilityImage1;
    public PlayerMovement phaseCooldown;
    public bool ability1OnCooldown;

    public Image abilityImage2;
    public TimeBody rewindCooldown;
    public bool ability2OnCooldown;

    private void Start()
    {
        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
    }

    void Update()
    {
        PhaseRush();
        Rewind();
    }

    void PhaseRush()
    {
        if (phaseCooldown.isPhasing && ability1OnCooldown == false)
        {
            ability1OnCooldown = true;
            abilityImage1.fillAmount = 1;
        }

        if (ability1OnCooldown)
        {
            abilityImage1.fillAmount -= 1 / phaseCooldown.phaseCooldownConstant * Time.deltaTime;

            if(abilityImage1.fillAmount <= 0)
            {
                abilityImage1.fillAmount = 0;
                ability1OnCooldown = false;
            }
        }
    }
    void Rewind()
    {
        if (rewindCooldown.isOnCooldown && ability2OnCooldown == false)
        {
            ability2OnCooldown = true;
            abilityImage2.fillAmount = 1;
        }

        if (ability2OnCooldown)
        {
            abilityImage2.fillAmount -= 1 / rewindCooldown.cooldownConstant * Time.deltaTime;

            if (abilityImage2.fillAmount <= 0)
            {
                abilityImage2.fillAmount = 0;
                ability2OnCooldown = false;
            }
        }
    }
}
