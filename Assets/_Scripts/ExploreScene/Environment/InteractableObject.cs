using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts.ExploreScene.Environment
{
    [RequireComponent(typeof(Collider))]
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] bool _disabled;
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected Material _baseMaterial;
        [SerializeField] protected Material _outlineMaterial;
        [SerializeField] protected bool _isInRange;

        [SerializeField] protected string[] messages;

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

            if(_baseMaterial == null) _baseMaterial = _spriteRenderer.material;

            if(_outlineMaterial == null) _outlineMaterial = Resources.Load("Materials/outline_material") as Material;

            if (messages == null) messages = new string[1];
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
            }
            else if (_isInRange && _disabled && messages.Length > 0)
            {
                Player._POPUP_(messages[0]);
            }
            else
            {
                Player._POPUP_("Gówno");
            }
        }

        protected virtual void Interaction()
        {
            Debug.Log(gameObject.name + " interaction");
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
    }
}