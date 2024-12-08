using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static CharacterStatus;

public class AnimationController : MonoBehaviour
{
    public static AnimationController Instance { get; set; }

    public enum ActionType
    {
        ATTACK,
        REACT,
        RESET
    }

    private ActionType actionType;
    private Animator animator;

    private AnimationEvent animationEvent;
    private GameObject skillObject;
    private GameObject auraObject;
    private float time;

    private int currentState;
    private float transitionDelay;
    private Skill skill;
    private Vector3 pos;

    // Idles
    public readonly int Idle = Animator.StringToHash("idle");

    // Movement
    public readonly int Walk = Animator.StringToHash("walk");
    // Combat
    public readonly int Combat = Animator.StringToHash("combat");
    public readonly int Attack = Animator.StringToHash("attack");
    public readonly int Hurt = Animator.StringToHash("hurt");
    public readonly int Heal = Animator.StringToHash("heal");
    public readonly int Die = Animator.StringToHash("die");


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    public void ChangeAnimation(int state, float transitionDuration, float delayDuration, int layer)
    {
        if (state == currentState || Time.time < transitionDelay)
            return;

        transitionDelay = Time.time + delayDuration;

        animator?.CrossFade(state, transitionDuration, layer);
        currentState = state;
    }
    private bool isSkillNotPlaying()
    {
        if (skillObject.GetComponentInChildren<ParticleSystem>().isPlaying)
            return false;
        else
            return true;
    }

    private bool isAuraNotPlaying()
    {
        if (auraObject.GetComponentInChildren<ParticleSystem>().isPlaying)
            return false;
        else
            return true;
    }

    public IEnumerator AttackAnimation(Skill skill, Vector3 pos)
    {
        ChangeAnimation(Attack, 0.1f, 0, 0);
        this.skill = skill;
        this.pos = pos;
        StartCoroutine(PlayAuraAnim());
        yield return new WaitForSeconds(GetAnimationClip(Attack).length);
        ChangeAnimation(Combat, 0.1f, 0, 0);
    }

    public void StartPlayAuraAnim()
    {

    }

    private IEnumerator PlayAuraAnim()
    {
        auraObject = Instantiate(skill.aura, this.transform.position, Quaternion.identity);
        yield return new WaitUntil(isAuraNotPlaying);
        Destroy(auraObject);
    }

    public void StartPlaySkillAnim()
    {
        StartCoroutine(PlaySkillAnim());
    }

    private IEnumerator PlaySkillAnim()
    {
        skillObject = Instantiate(skill.effect, pos, Quaternion.identity);
        yield return new WaitUntil(isAuraNotPlaying);
        Destroy(skillObject);
    }

    public IEnumerator ReactAnimation(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

            ChangeAnimation(Hurt, 0.1f, 0, 0);
            yield return new WaitForSeconds(GetAnimationClip(Hurt).length);
            ChangeAnimation(Combat, 0.1f, 0, 0);
        
    }

    public void ResetAnimation()
    {

    }

    //private bool isTriggerSkill()
    //{
    //    return triggerSkill;
    //}

    //public void AlertObservers()
    //{
    //    PostOffice.instance.SendToPostOffice(Hurt);
    //    triggerSkill = true;
    //        Debug.Log("anim");
       
        
    //    //else
    //    //{
    //    //    triggerSkill = false;
    //    //}
    //}

    public AnimationClip GetAnimationClip(int animation)
    {
        foreach (AnimationClip clip in animator?.runtimeAnimatorController.animationClips)
        {
            if (Animator.StringToHash(clip.name) == animation)
                return clip;
        }

        return null;
    }
}
