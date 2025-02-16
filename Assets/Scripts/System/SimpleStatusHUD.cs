using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleStatusHUD : MonoBehaviour
{
    public Image hpBar;
    public Image easeHpBar;
    public TMP_Text hpValue;
    public Image mpBar;
    public Image easeMpBar;
    public TMP_Text mpValue;
    public GameObject mpIncOverlay;
    public GameObject hpIncOverlay;
    public GameObject deadOverlay;
    public TMP_Text name;
    float currentMana = 0;
    float maxMana = 0;
    float lerpSpeed = 0.05f;
    float elapsedTime = 0;
    float waitTime = 3f;
    //private TMP_Text value;

    public void SetName(CharacterStatus status)
    {
        name.text = status.charName;
    }



    public void SimpleSetHP(CharacterStatus status)
    {
        hpBar.fillAmount = (status.health * (100 / status.maxHealth)) / 100;
        hpValue.SetText(status.health + "/" + status.maxHealth);
    }

    public void SimpleSetHP(UnitStatus status)
    {
        hpBar.fillAmount = (status.health * (100 / status.maxHealth)) / 100;
        hpValue.SetText(status.health + "/" + status.maxHealth);
    }

    public void SimpleSetMP(CharacterStatus status)
    {
        mpBar.fillAmount = (status.mana * (100 / status.maxMana)) / 100;
        mpValue.SetText(status.mana + "/" + status.maxMana);
    }

    public void BattleSetMP(UnitStatus status)
    {
        mpBar.fillAmount = (status.mana * (100 / status.maxMana)) / 100;
        mpValue.SetText(status.mana + "/" + status.maxMana);
    }

    public void SetStatusActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

    public void SetConditionOverlay(CharacterStatus.Condition condition)
    {
        switch (condition)
        {
            case CharacterStatus.Condition.NORMAL:
                deadOverlay.SetActive(false);
                break;
            case CharacterStatus.Condition.DEAD:
                deadOverlay.SetActive(true);
                break;
        }
    }

    public void SetBuffOverlay(CharacterStatus.Buff buff)
    {
        switch (buff)
        {
            case CharacterStatus.Buff.NORMAL:
                hpIncOverlay.SetActive(false);
                mpIncOverlay.gameObject.SetActive(false);
                break;
            case CharacterStatus.Buff.INCMP:
                hpIncOverlay.SetActive(false);
                mpIncOverlay.SetActive(true);
                break;
            case CharacterStatus.Buff.INCHP:
                hpIncOverlay.SetActive(true);
                mpIncOverlay.SetActive(false); 
                break;
        }
    }

    public IEnumerator SetHP(UnitStatus status, float hp, bool isIncrease)
    {
        if (isIncrease)
        {
            if (status.health + hp > status.maxHealth)
            {
                status.health = status.maxHealth;
            }
            else
            {
                status.health += hp;
            }
        }
        else
        {
            if (status.health - hp < 0)
            {
                status.health = 0;
            }
            else
            {
                status.health -= hp;
            }
        }

        hpBar.fillAmount = (status.health * (100 / status.maxHealth)) / 100;
        hpValue.SetText(status.health + "/" + status.maxHealth);
        while (elapsedTime < waitTime)
        {
            easeHpBar.fillAmount = Mathf.Lerp(easeHpBar.fillAmount, hpBar.fillAmount, lerpSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        easeHpBar.fillAmount = hpBar.fillAmount;
        elapsedTime = 0;
        yield return null;
    }



    public IEnumerator SetMP(float mp, bool isIncrease)
    {
        if (isIncrease)
        {
            if (currentMana + mp > maxMana)
            {
                currentMana = maxMana;
            }
            else
            {
                currentMana += mp;
            }

        }
        else
        {
            if (currentMana - mp < 0)
            {
                currentMana = 0;
            }
            else
            {
                currentMana -= mp;
            }
        }

        mpBar.fillAmount = (currentMana * (100 / maxMana)) / 100;
        mpValue.SetText(currentMana + "/" + maxMana);
        while (elapsedTime < waitTime)
        {
            easeMpBar.fillAmount = Mathf.Lerp(easeMpBar.fillAmount, mpBar.fillAmount, lerpSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        easeMpBar.fillAmount = mpBar.fillAmount;
        elapsedTime = 0;
        yield return null;
    }
}
