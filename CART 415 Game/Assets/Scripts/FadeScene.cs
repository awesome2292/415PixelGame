﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScene : MonoBehaviour
{
    //FADESCENE CLASS
    //Responsible for triggering the FadeIn and FadeOut animations when changing scenes

    public Scene newScene;
    public Animator animator;
    void Update()
    {
        if (Input.anyKey)
        {
            FadeToLevel(newScene.handle);
        }
    }


    public void FadeToLevel(int levelIndex)
    {
        animator.SetTrigger("FadeTrigger");
    }

    public void OnFadeComplete()
    {
        DontDestroyOnLoad(GameObject.Find("Sky_loop"));
        SceneManager.LoadScene(newScene.handle);
    }
}

