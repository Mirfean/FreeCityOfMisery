using System;
using System.Collections;
using UnityEngine;

public class ScreenTransport : MonoBehaviour
{
    [SerializeField] GameObject Screen;
    [SerializeField] AudioClip movingSound;

    public static Action<int> playCut;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayCut(Transform soundPlace, float time = 1f)
    {
        StartCoroutine(ShowScreen(time));
        AudioSource.PlayClipAtPoint(movingSound, soundPlace.position);
    }

    IEnumerator ShowScreen(float time = 1f)
    {
        Screen.SetActive(true);
        yield return new WaitForSeconds(time);
        Screen.SetActive(false);
    }

}
