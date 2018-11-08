using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public delegate void PlayerDeath();

    public static event PlayerDeath playerDeath;

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private ushort lives = 1;

    private static bool takeDamage;
    private static float timeInvinsible = 5f;

    private float blinkTimer = 0f;
    private float blinkFrequency = .05f;
    private bool blink = true;


    private void Start()
    {
        takeDamage = true;
    }


    private void Update()
    {
        if (DamageAnimation()) return;


        if (PatrolEnemy.PatrolVehicle != null)
        {
            if (Vector3.Distance(transform.position, PatrolEnemy.PatrolVehicle.transform.position) <= 200f)
            {
                ReduceAndQueryHealth();
            }
        }

        foreach (var kamakazi in EnemyManager.EnemyList)
        {
            if (kamakazi != null)
            {
                if (Distance(transform.position, kamakazi.transform.position))
                {
                    ReduceAndQueryHealth();
                }
            }
        }
    }


    private bool Distance(Vector3 _a, Vector3 _b)
    {
        return Vector3.Distance(_a, _b) <= 200f;
    }


    private bool DamageAnimation()
    {
        if (!takeDamage)
        {
            Blinking();
            return true;
        }

        sr.enabled = true;

        var emissionModule = ps.emission;
        emissionModule.enabled = true;
        
        return false;
    }

    private void Blinking()
    {
        if (blinkTimer <= blinkFrequency)
        {
            blinkTimer += Time.deltaTime;

            sr.enabled = blink;

            var emissionModule = ps.emission;
            emissionModule.enabled = blink;
        }
        else
        {
            blinkTimer = 0f;
            blink = !blink;
        }
    }

    private void ReduceAndQueryHealth()
    {
        if (lives - 1 <= 0)
        {
            lives = 0;
            pauseMenu.SetActive(true, true);

            return;
        }

        lives--;
    }

    public static IEnumerator SetInvinsible()
    {
        takeDamage = false;
        yield return new WaitForSeconds(timeInvinsible);
        takeDamage = true;
    }
}