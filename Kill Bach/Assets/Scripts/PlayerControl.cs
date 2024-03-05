using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public KeyCode playerInput;
    public float attackRange;
    public bool isPressed;
    public AudioClip killSound;
    public AudioClip missSound;
    private AudioSource _audioSource;
    private Image killerImage;

    void Start()
    {
        isPressed = false;
        _audioSource = GetComponent<AudioSource>();
        killerImage = GetComponentInChildren<Image>();
        killerImage.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryKill(List<GameObject> band)
    {
        float currentMinDistance = Single.PositiveInfinity;
        GameObject currentTarget = null;

        if (!isPressed & Input.GetKeyDown(playerInput) & GameManager.instance.gameState == GameManager.GameState.isInGame)
        {
            isPressed = true;
            foreach (var musician in band)
            {
                float distance = musician.transform.position.magnitude;
                if (distance < currentMinDistance)
                {
                    currentMinDistance = distance;
                    currentTarget = musician;
                }
            }

            if (currentMinDistance <= attackRange)
            {
                switch (playerInput)
                {
                    case KeyCode.Q:
                        GameManager.instance.playerQScore += currentTarget.GetComponent<Target>().GetKilled();
                        break;
                    case KeyCode.P:
                        GameManager.instance.playerPScore += currentTarget.GetComponent<Target>().GetKilled();
                        break;
                    case KeyCode.Z:
                        GameManager.instance.playerZScore += currentTarget.GetComponent<Target>().GetKilled();
                        break;
                    case KeyCode.M:
                        GameManager.instance.playerMScore += currentTarget.GetComponent<Target>().GetKilled();
                        break;
                }

                _audioSource.clip = killSound;
                _audioSource.Play();
                killerImage.DOColor(Color.red, 0.5f);
            }
            else
            {
                _audioSource.clip = missSound;
                _audioSource.Play();
                killerImage.DOColor(Color.gray, 0.5f);
                //TODO Miss Function
            }
        }
    }
}
