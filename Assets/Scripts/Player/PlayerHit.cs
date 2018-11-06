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
    private float blinkFrequency = 1f;
    private bool blink = true;
    
    
    private void Start()
    {
        takeDamage = true;
    }

    
    private void Update()
    {
        if (!takeDamage)
        {
            Blinking();
        }
        else
        {
            sr.enabled = true;
            
            var emissionModule = ps.emission;
            emissionModule.enabled = true;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        var go = other.gameObject;
        var n = go.name;

        Debug.Log("Player Collision with " + n);

        if (!takeDamage) return;

        if (n.Contains(ObjectNames.Kamakazi) || n.Contains(ObjectNames.Chaser))
        {
            ReduceAndQueryHealth();
        }
    }

    private void Blinking()
    {
        if(blinkTimer <= blinkFrequency)
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