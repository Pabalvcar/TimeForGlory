using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : AbstractEnemy
{

    public int goldStolen;
    public float extraDamageForEachRune;
    

    void Start()
    {
        StartCoroutine(PlayAnimationEvery10Seconds());
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    protected override void FixedUpdate()
    {

    }

    private IEnumerator PlayAnimationEvery10Seconds()
    {
        while (true)
        {
            animator.SetFloat("doAnimation", 1); // Set the boolean parameter to true
            yield return new WaitForSeconds(0.75f); // Wait for 1 second (adjust this value based on your animation length)
            animator.SetFloat("doAnimation", 0); // Set the boolean parameter to false to stop the animation
            yield return new WaitForSeconds(10f); // Wait for 10 seconds before starting the animation again
        }
    }
}
