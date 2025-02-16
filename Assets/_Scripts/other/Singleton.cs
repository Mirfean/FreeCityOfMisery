﻿using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    _instance = singleton.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;

            //transform.parent = null;
            //DontDestroyOnLoad (this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
