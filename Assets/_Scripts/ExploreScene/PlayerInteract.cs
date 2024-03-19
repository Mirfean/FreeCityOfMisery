using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Triangles playerInput;
    [SerializeField] Animator animator;
    [SerializeField] bool stillMove;
    [SerializeField, Range(0f, 10f)] float speed;

    [SerializeField] Transform _spriteTransform;
    bool _rotatedSprite;

    bool _canMove = false;

    Coroutine moveCoroutine;

    [SerializeField] GameObject popupBubble;
    [SerializeField] PopupMessage popupMessage;

    [SerializeField] AudioSource _movestepSound;

    public static Action<Transform> _TELEPORT_;

    public static Action<string> _POPUP_;

    private void Awake()
    {
        playerInput = new Triangles();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.Move.performed += Move;
        playerInput.Player.Move.canceled += stopMovement;
        playerInput.Player.StartFight.performed += _DEBUG_StartFight;

        _TELEPORT_ += Teleport;
        _POPUP_ += ShowPopUp;
    }

    private void _DEBUG_StartFight(InputAction.CallbackContext context)
    {
        Debug.Log("Heh");
        //Some changes before the fight starts
        ExploreManager.StartFightScene();
    }

    private void OnDisable()
    {
        playerInput.Disable();
        _TELEPORT_ -= Teleport;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (popupMessage == null) popupBubble.GetComponent<PopupMessage>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(InputAction.CallbackContext ctx)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(move(ctx.ReadValue<Vector2>()));
        //StartCoroutine(move(ctx.ReadValue<Vector2>()));
    }

    IEnumerator move(Vector2 movement)
    {
        animator.SetBool("walk", true);
        float SoundTime = 0.5f;
        float currentTime = 0;
        if (movement.x < 0)
        {
            movement.x = -1;
            if (!_rotatedSprite)
            {
                _spriteTransform.Rotate(0, -180, 0);
                transform.position += new Vector3(speed * 10 * Time.deltaTime * movement.x, 0, 0);
                _rotatedSprite = true;
            }

        }

        else
        {
            movement.x = 1;
            if (_rotatedSprite)
            {
                _spriteTransform.Rotate(0, 180, 0);
                transform.position += new Vector3(speed * 10 * Time.deltaTime * movement.x, 0, 0);
                _rotatedSprite = false;
            }

        }

        while (playerInput.Player.Move.ReadValue<Vector2>().x != 0)
        {
            transform.position += new Vector3(speed * Time.deltaTime * movement.x, 0, 0);
            yield return null;
            if (currentTime < Time.time)
            {
                _movestepSound.Play();
                currentTime = Time.time + SoundTime;
            }

        }
        moveCoroutine = null;
    }

    void stopMovement(InputAction.CallbackContext ctx)
    {
        stillMove = false;
        animator.SetBool("walk", false);    // refactor
    }



    public void Teleport(Transform newPosition)
    {
        transform.position = newPosition.position;
    }

    void ShowPopUp(string text = "blank")
    {
        Debug.Log("popup " + popupBubble.name);
        popupBubble.SetActive(true);
        popupBubble.GetComponent<PopupMessage>().PlayFade(text);
    }

    public static void DebugMessage(string mes = "object")
    {
        Debug.Log(mes + " is interacted");
    }
}
