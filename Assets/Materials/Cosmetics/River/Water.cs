﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour 
{

public float speedX = 0.1f;
public float speedY = 0.1f;
private float curX;
private float curY;

void Awake ()
    { 
        curX= GetComponent<Renderer>().material.mainTextureOffset.x;
        curY= GetComponent<Renderer>().material.mainTextureOffset.y;
    }

void Update ()
    {
        curX += Time.deltaTime * speedX;
        curY += Time.deltaTime * speedY; //Mathf.Sin()
        GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", new Vector2(curX, curY));
        //GetComponent<Renderer>().material.a
    }
}