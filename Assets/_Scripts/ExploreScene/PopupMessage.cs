using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMessage : MonoBehaviour
{
    [SerializeField] AnimationClip _fadingAnim;
    [SerializeField] Animator _animator;
    [SerializeField] float _animTime;
    [SerializeField] TMPro.TextMeshProUGUI _textMeshProUGUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayFade(string message)
    {
        _textMeshProUGUI.text = message;
        StartCoroutine(fading());
    }

    IEnumerator fading()
    {

        _animator.Play(_fadingAnim.name);
        yield return new WaitForSeconds(_fadingAnim.length);
        gameObject.SetActive(false);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
