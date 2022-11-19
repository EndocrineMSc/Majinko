using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDemo : MonoBehaviour
{

    Animator anim;
    Rigidbody2D rigidBody2D;
    SpriteRenderer rend;
    int physicsFrame = 0;

    public bool spamWalk;
    public bool spamAttack;
    public bool spamHurt;
    public bool spamDeath;
    public bool spamSpawn;
    public bool noFullRoutine;
    public float walkSpeed;
    public bool deathMotion;

    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();

    }





    void FixedUpdate()
    {



        if (noFullRoutine == false)
        {

            // a quick demonstration of all the character animations



            // We start with the idle animation

            if (physicsFrame == 150)                             // Spawn
            {

                rend.enabled = true;
                anim.SetTrigger("Spawn");

            }
            else if (physicsFrame >= 250 && physicsFrame < 400) // Then we walk to the right for 3 seconds
                rigidBody2D.velocity = new Vector2(walkSpeed, 0);
            else if (physicsFrame == 400)                       // Stop for 1 second
                rigidBody2D.velocity = new Vector2(0, 0);
            else if (physicsFrame == 450)                       // Turn around
                Flip();
            else if (physicsFrame >= 450 && physicsFrame < 700) // Walk to the left for 5 seconds
                rigidBody2D.velocity = new Vector2(-walkSpeed, 0);
            else if (physicsFrame == 700)                       // Stop
                rigidBody2D.velocity = new Vector2(0, 0);
            else if (physicsFrame == 750)                       // Attack
                anim.SetTrigger("Attack");
            else if (physicsFrame == 900)                       // Attack the other direction
            {

                anim.SetTrigger("Attack");
                Flip();

            }
            else if (physicsFrame == 1050)                       // Hurt
                anim.SetTrigger("Hurt");
            else if (physicsFrame == 1100)                       // Hurt again
                anim.SetTrigger("Hurt");
            else if (physicsFrame == 1150)                       // Death
                anim.SetTrigger("Death");
            else if (physicsFrame >= 1150 && physicsFrame < 1170 && deathMotion == true)
                rigidBody2D.velocity = new Vector2(-1.0f, 0);
            else
                rigidBody2D.velocity = new Vector2(0, 0);



            anim.SetFloat("Speed", rigidBody2D.velocity.x);

        }
        else if (spamWalk == true)
            anim.SetFloat("Speed", 1.0f);
        else if (spamAttack == true && physicsFrame % 75 == 0)
            anim.SetTrigger("Attack");
        else if (spamHurt == true && physicsFrame % 75 == 0)
            anim.SetTrigger("Hurt");
        else if (spamDeath == true && physicsFrame % 75 == 0)
            anim.SetTrigger("Death");
        else if (spamSpawn == true && physicsFrame % 100 == 0)
            anim.SetTrigger("Spawn");




        physicsFrame++;

    }








    void Flip()
    {

        // Flip

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }






}
