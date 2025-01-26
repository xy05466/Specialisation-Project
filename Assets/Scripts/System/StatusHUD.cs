using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusHUD : MonoBehaviour
{
    //public Slider healthSlider;
    //public Slider easeHealthSlider;
    //public Slider mpSlider;
    //public Slider easeMpSlider;
    public Image hpBar;
    public Image easeHpBar;
    public TMP_Text hpValue;
    public Image mpBar;
    public Image easeMpBar;
    public TMP_Text mpValue;
    private TMP_Text value;
    float currentMana = 0;
    float maxMana = 0;
    float lerpSpeed = 0.05f;
    float elapsedTime = 0;
    float waitTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetStatusHUD(UnitStatus status)
    {
        float currentHealth = status.health * (100 / status.maxHealth);
        float currentMana = status.mana * (100 / status.maxMana);

        hpBar.fillAmount = currentHealth / 100;
        easeHpBar.fillAmount = hpBar.fillAmount;
        hpValue.SetText(status.health + "/" + status.maxHealth);

        mpBar.fillAmount = currentMana / 100;
        mpValue.SetText(status.mana + "/" + status.maxMana);
    }

    public void SetMPHUD(float mMana, float currMana)
    {

            maxMana += mMana;
            currentMana += currMana;
            //currentMana *= (100 / maxMana);
            mpBar.fillAmount = currentMana * (100 / maxMana) / 100;
            mpValue.SetText(currentMana + "/" + maxMana);

    }

    public void SimpleSetHP(CharacterStatus status, float hp, bool isIncrease)
    {
        hpBar.fillAmount = (status.health * (100 / status.maxHealth)) / 100;
        hpValue.SetText(status.health + "/" + status.maxHealth);
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
        //currentMana -= mp;
        //while (mpSlider.value != easeMpSlider.value)
        //{
        //    easeMpSlider.value = Mathf.Lerp(easeMpSlider.value, currentMana, lerpSpeed);
        //}
        //mpValue.SetText(currentMana + "/" + maxMana);
        //StartCoroutine(GraduallySetMPBar(mp, isIncrease, 10, 0.05f, mpBar));
    }

    public float GetMP()
    {
        return currentMana;
    }

    IEnumerator GraduallySetMPBar(float amount, bool isIncrease, int fillTimes, float fillDelay, Image bar)
    {
        float percentage = 1 / (float)fillTimes;

        if (isIncrease)
        {

                for (int fillStep = 0; fillStep < fillTimes; fillStep++)
                {
                    float _fAmount = amount * percentage;
                    float _dAmount = _fAmount / maxMana;
                    currentMana += _fAmount;
                    bar.fillAmount += _dAmount;
                    if (currentMana <= maxMana)
                        mpValue.SetText(currentMana + "/" + maxMana);
                    yield return new WaitForSeconds(fillDelay);
                }
            
        }
        else
        {
                for (int fillStep = 0; fillStep < fillTimes; fillStep++)
                {
                    float _fAmount = amount * percentage;
                    float _dAmount = _fAmount / maxMana;
                    currentMana -= _fAmount;
                    bar.fillAmount -= _dAmount;
                    if (currentMana >= 0)
                        mpValue.SetText(currentMana + "/" + maxMana);

                    yield return new WaitForSeconds(fillDelay);
                }
            
        }
    }

    IEnumerator GraduallySetHPBar(UnitStatus status, float amount, bool isIncrease, int fillTimes, float fillDelay, Image bar)
    {
        float percentage = 1 / (float)fillTimes;
        
        if (isIncrease)
        {
            if (bar == hpBar)
            {
                for (int fillStep = 0; fillStep < fillTimes; fillStep++)
                {
                    float _fAmount = amount * percentage;
                    float _dAmount = _fAmount / status.maxHealth;
                    status.health += _fAmount;
                    bar.fillAmount += _dAmount;
                    if (status.health <= status.maxHealth)
                        hpValue.SetText(status.health + "/" + status.maxHealth);
                    yield return new WaitForSeconds(fillDelay);
                }
            }
            else
            {
                for (int fillStep = 0; fillStep < fillTimes; fillStep++)
                {
                    float _fAmount = amount * percentage;
                    float _dAmount = _fAmount / status.maxMana;
                    status.mana += _fAmount;
                    bar.fillAmount += _dAmount;
                    if (status.mana <= status.maxMana)
                        mpValue.SetText(status.mana + "/" + status.maxMana);
                    yield return new WaitForSeconds(fillDelay);
                }
            }
        }
        else
        {
            if (bar == hpBar)
            {
                for (int fillStep = 0; fillStep < fillTimes; fillStep++)
                {
                    float _fAmount = amount * percentage;
                    float _dAmount = _fAmount / status.maxHealth;
                    status.health -= _fAmount;
                    bar.fillAmount -= _dAmount;
                    if (status.health >= 0)
                        hpValue.SetText(status.health + "/" + status.maxHealth);

                    yield return new WaitForSeconds(fillDelay);
                }
            }
            else
            {
                for (int fillStep = 0; fillStep < fillTimes; fillStep++)
                {
                    float _fAmount = amount * percentage;
                    float _dAmount = _fAmount / status.maxMana;
                    status.mana -= _fAmount;
                    bar.fillAmount -= _dAmount;
                    if (status.mana >= 0)
                        mpValue.SetText(status.mana + "/" + status.maxMana);

                    yield return new WaitForSeconds(fillDelay);
                }
            }
        }
    }
}
