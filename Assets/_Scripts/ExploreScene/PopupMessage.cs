using PixelCrushers;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : MonoBehaviour
{
    [SerializeField] AnimationClip _fadingAnim;
    [SerializeField] Animator _animator;
    [SerializeField] float _animTime;
    [SerializeField] TMPro.TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] Image _textBackground;

    const float _textBackgroundDefaultHeight = 50;

    // Start is called before the first frame update
    void Start()
    {
        /*_textBackgroundDefaultHeight = _textBackground.rectTransform.sizeDelta.y;*/
    
    }

    public void PlayFade(string message)
    {
        _textMeshProUGUI.text = message;
        _textMeshProUGUI.ForceMeshUpdate();

        Debug.Log("Eloooo " + _textMeshProUGUI.textInfo.lineCount);

/*        if (_textBackgroundDefaultHeight == 0)
            _textBackgroundDefaultHeight = _textBackground.rectTransform.sizeDelta.y;*/

        _textBackground.rectTransform.sizeDelta = new Vector3(
            _textBackground.rectTransform.sizeDelta.x,
            _textBackgroundDefaultHeight + ((_textMeshProUGUI.textInfo.lineCount - 1) * 25));


        //Make animation length dependent on message length
        if (message.Length > 30)
        {
            _animTime = _fadingAnim.length + message.Length * 0.02f - 0.6f;
        }
            
        StartCoroutine(fading(_animTime));
    }

    IEnumerator fading(float anim_length = 0)
    {
        if (anim_length == 0) anim_length = _fadingAnim.length;
        _animator.Play(_fadingAnim.name, 1, anim_length);
        yield return new WaitForSeconds(anim_length);
        gameObject.SetActive(false);
        yield return null;
    }

    float GetAnimationLength(AnimationClip clip)
    {
        return clip.length;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
