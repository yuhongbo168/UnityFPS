using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Damabeable representedDamageable;
    public GameObject healthIconPrefab;

    IEnumerator Start()
    {
        if (representedDamageable == null)
        {
            yield break;
        }
        yield return null;

        
        if (healthBar != null)
        {
            healthBar.maxValue = representedDamageable.startingHealth;
            healthBar.value = representedDamageable.startingHealth;
        }

    }


    public void ChangeHealthUI(Damabeable damageable)
    {
        
        if (healthBar != null)
        {
            healthBar.value = damageable.CurrentHealth;
        }

    }
}
