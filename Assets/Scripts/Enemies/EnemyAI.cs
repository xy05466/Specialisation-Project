using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum Behaviour
    {
        BOLD,
        CAUTIOUS

    }

    public Behaviour behaviour;
    public enum Action
    {
        ATTACK,
        DEFEND,
        FLEE
    }

    private float rand;
    private int numMaxHealth;
    private UnitStatus toHeal;
    //private void Attack(UnitStatus target)
    //{

        
    //}

    

    public UnitStatus Defend(List<UnitStatus> party, UnitStatus enemy)
    {
        if (toHeal.health == toHeal.maxHealth)
        {
            foreach (UnitStatus own in party)
            {
                if (own.health != own.maxHealth)
                {
                    foreach (Skill skill in enemy.skills)
                    {
                        if (skill.currentType == own.heal)
                        {
                            toHeal = own;
                        }
                    }
                }
            }
        }
        return toHeal;
    }

    public Action Decide(UnitStatus target, List<UnitStatus> party, UnitStatus enemy)
    {
        rand = Random.Range(0, 10);
        if (behaviour == Behaviour.BOLD)
        {
                if (enemy.health > target.health)
                {
                    if (rand > 2)
                    {
                        return Action.ATTACK;
                    }
                    else
                    {
                        foreach (UnitStatus own in party)
                        {
                            if (own.health != own.maxHealth)
                            {
                                foreach (Skill skill in enemy.skills)
                                {
                                    if (skill.currentType == own.heal)
                                    {
                                        toHeal = own;
                                        return Action.DEFEND;
                                    }
                                }
                            }
                        }
                        return Action.ATTACK;
                    }
                }
                if (enemy.health < target.health)
                {
                    if (rand > 4)
                    {
                        return Action.ATTACK;
                    }
                    else
                    {
                        foreach (UnitStatus own in party)
                        {
                            if (own.health != own.maxHealth)
                            {
                                foreach (Skill skill in enemy.skills)
                                {
                                    if (skill.currentType == own.heal)
                                    {
                                        toHeal = own;
                                        return Action.DEFEND;
                                    }
                                }
                            }
                        }
                        return Action.ATTACK;
                    }
                }
        }
        else if (behaviour == Behaviour.CAUTIOUS)
        {
                if (enemy.health > target.health)
                {
                    if (rand > 4)
                    {
                        return Action.ATTACK;
                    }
                    else
                    {
                        return Action.DEFEND;
                    }
                }
                if (enemy.health < target.health)
                {
                    if (rand > 6)
                    {
                        return Action.ATTACK;
                    }
                    else
                    {
                        return Action.DEFEND;
                    }
                }
        }
        numMaxHealth = 0;
        return Action.ATTACK;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
