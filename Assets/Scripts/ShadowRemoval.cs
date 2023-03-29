using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRemoval : MonoBehaviour
{
    RaycastHit2D[] hit;
    int frames = 0;
    void FixedUpdate()
    {
        frames++;
        if (frames == 4)
        {
            frames = 0;


            hit = Physics2D.CircleCastAll(transform.position, 2f, new Vector2(0, 0), 3f);

            foreach (var item in hit)
            {
          
            }
        }
    }
}
