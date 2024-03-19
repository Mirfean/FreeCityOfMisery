using UnityEngine;
using Random = System.Random;

namespace Assets._Scripts.ExploreScene.Environment
{
    [RequireComponent(typeof(Collider))]
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] protected bool _disabled;
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected Material _baseMaterial;
        [SerializeField] protected Material _outlineMaterial;
        [SerializeField] protected bool _isInRange;

        [SerializeField] protected string[] messages;

        [SerializeField] protected InteractionToInventory _interactionToInventory;

        // Start is called before the first frame update
        protected void Start()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
                if (_spriteRenderer == null)
                {
                    _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                }
            }

            if (_baseMaterial == null) _baseMaterial = _spriteRenderer.material;

            if (_outlineMaterial == null) _outlineMaterial = Resources.Load("Materials/outline_material") as Material;

            if (messages == null) messages = new string[1];

            _interactionToInventory = GetComponent<InteractionToInventory>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnMouseUpAsButton()
        {
            if (_isInRange && !_disabled)
            {
                Interaction();
                SendFirstMessage();
            }
            else if (_isInRange && _disabled && messages.Length > 0)
            {
                SendRandomMessage();
            }
            else
            {
                Player._POPUP_("Gówno");
            }
        }

        protected virtual void Interaction()
        {
            Debug.Log(gameObject.name + " interaction");
            if (_interactionToInventory == null)
                return;
            else
            {
                var item = _interactionToInventory.GetFirstGetItem();
                if (item == null) return;

                if (InventoryPlayer.Instance.AddItem(item))
                {
                    Debug.Log("Added item " + item.ItemName);
                    //_disabled = true;

                    _interactionToInventory.RemoveFirstGetItem();

                    //DEBUG ONLY
/*                    _spriteRenderer.material = _baseMaterial;
                    _spriteRenderer.color = Color.gray;*/
                }
                else
                {
                    Debug.Log("Inventory full");
                }

            }
        }

        private void OnMouseOver()
        {
            if (_isInRange) _spriteRenderer.material = _outlineMaterial;
        }

        internal void OnMouseExit()
        {
            _spriteRenderer.material = _baseMaterial;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.tag == "Player")
            {
                _isInRange = true;

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player") _isInRange = false;
        }

        private void SendFirstMessage()
        {
            if (messages != null && messages.Length > 0)
                Player._POPUP_(messages[0]);
            else Debug.Log("No messages");
        }

        private void SendRandomMessage()
        {
            if (messages != null && messages.Length > 0)
            {
                Random rnd = new Random();
                Player._POPUP_(messages[rnd.Next(messages.Length)]);
            }
            else Debug.Log("No messages");
        }
    }
}