using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour, IItem
{
    public static event Action<int> OnGemCollect;
    public int worth = 1;

    public void Collect()
    {
        OnGemCollect.Invoke(worth);
        SoundEffectManager.Play("Gem");
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
