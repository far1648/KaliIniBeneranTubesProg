using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private Animation animClass; // the animation class itself
    private bool hasAnimComp = false; // tells the update() to fire anim functions #1

    private bool animPlaying = false;

    private Vector2 lastPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (TryGetComponent<Animation>(out Animation x))
        {
            animClass = GetComponent<Animation>();
            hasAnimComp = true;
        }

        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 deltaPosNorm = Vector2.zero;
        if (hasAnimComp)
        {
            // deltaPos section
            if (lastPosition != (Vector2)transform.position)
            {
                deltaPosNorm = ((Vector2)transform.position - lastPosition);
                lastPosition = (Vector2)transform.position;
            }
            
            if (!animPlaying)
            {
                if (Mathf.Abs(deltaPosNorm.x) > Mathf.Abs(deltaPosNorm.y))
                {
                    animClass.Play();
                }
                else
                {
                    animClass.Stop();
                }
            }

            if (MathF.Abs(deltaPosNorm.x) > MathF.Abs(deltaPosNorm.y))
            {
                if (deltaPosNorm.x < 0)
                {
                    animClass.clip = getAnimClip("Left.anim");
                    return;
                } animClass.clip = getAnimClip("Right");
            }
            else if ((MathF.Abs(deltaPosNorm.x) < MathF.Abs(deltaPosNorm.y)))
            {
                if (deltaPosNorm.y < 0)
                {
                    animClass.clip = getAnimClip("Down");
                    return;
                } animClass.clip = getAnimClip("Up");
            }
        }
        }

    AnimationClip getAnimClip(string animName)
    {
        return animClass.GetClip(animName);
    } 
}
