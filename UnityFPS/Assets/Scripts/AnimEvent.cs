using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    public Character CharacterInstance;
    public string jumpeffectName;
    public Damager meleeDamager;
    public string shootingName;

    public EnemyBehaviour enemyBehaviour;


    public void PlayerShootingMagic()
    {
        CharacterInstance.ShootingMagic();
    }
    public void Shooting()
    {
        enemyBehaviour.Shooting();
    }
    public void CanAttack()
    {
        CharacterInstance.EnableMelleAttack();
    }

    public void NotAttack()
    {
        CharacterInstance.DisableMeleeAttack();
    }

    public void StartMeleeAttack()
    {
        CharacterInstance.CanMeleeAttack = false;
    }

    public void AddJumpUpEffect()
    {
        if (CharacterInstance.canLunchjumpUpEffect)
        {
            CharacterInstance.ApplySceneEffect(jumpeffectName);
            CharacterInstance.canLunchjumpUpEffect = false;
        }
       
    }

    public void StartAttack()
    {
        meleeDamager.EnableDamage();
        meleeDamager.disableDamageAterHit = true;
    }

    public void EndAttack()
    {
        if (meleeDamager != null)
        {
            meleeDamager.DisableDamage();
            
        }
    }

}
