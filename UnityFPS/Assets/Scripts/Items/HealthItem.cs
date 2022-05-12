using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    public int Health = 1;
    // Start is called before the first frame update



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
                Damabeable damabeable = collision.gameObject.GetComponent<Damabeable>();

                if (damabeable != null)
                {
                    if (damabeable.CurrentHealth != damabeable.startingHealth)
                    {
                        damabeable.AddHealth(Health);
                    }

                    damabeable.OnSetHealth.Invoke(damabeable);
                }

                Destroy(this.gameObject);         

        }
    }
 
}
