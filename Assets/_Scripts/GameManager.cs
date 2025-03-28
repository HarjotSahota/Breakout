﻿using UnityEngine;
using TMPro;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Ball ball;
    [SerializeField] private Transform bricksContainer;
    public ParticleSystem ExplosionParticles; 
    public ParticleSystem FlashParticles;


    private int brick = 0;
    [SerializeField] private BrickCounterUI brickCounter;

    private int currentBrickCount;
    private int totalBrickCount;

    private void OnEnable()
    {
        InputHandler.Instance.OnFire.AddListener(FireBall);
        ball.ResetBall();
        totalBrickCount = bricksContainer.childCount;
        currentBrickCount = bricksContainer.childCount;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnFire.RemoveListener(FireBall);
    }

    private void FireBall()
    {
        AudioManager.instance.PlaySfx("Fire");
        ball.FireBall();
    }

    public void OnBrickDestroyed(Vector3 position)
    {
        // fire audio here
        AudioManager.instance.PlaySfx("Brick");
        // implement particle effect here
        PlayHitEffects(position);
        // add camera shake here

        // implementing coin text
        brick ++;
        brickCounter.UpdateScore(brick);

        currentBrickCount--;
        Debug.Log($"Destroyed Brick at {position}, {currentBrickCount}/{totalBrickCount} remaining");
        if(currentBrickCount == 0) SceneHandler.Instance.LoadNextScene();
    }

    public void KillBall()
    {
        maxLives--;
        // update lives on HUD here
        AudioManager.instance.PlaySfx("Miss");
        // game over UI if maxLives < 0, then exit to main menu after delay
        ball.ResetBall();
    }

    public void PlayHitEffects(Vector3 position){
        ExplosionParticles.transform.position = position;
        FlashParticles.transform.position = position;
        ExplosionParticles.Play();
        FlashParticles.Play();
    }
}   
