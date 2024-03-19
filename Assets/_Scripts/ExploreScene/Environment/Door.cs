using Assets._Scripts.ExploreScene.Environment;
using UnityEngine;

public class Door : InteractableObject
{
    [SerializeField] bool _isClosed;

    [SerializeField] Transform _spawnPoint;

    [SerializeField] Door _whereILead;

    [SerializeField] ScreenTransport screenTransport;

    [SerializeField] string[] _interactionClosed;

    public Transform SpawnPoint { get => _spawnPoint; set => _spawnPoint = value; }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (screenTransport == null) FindObjectOfType<ScreenTransport>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Interaction()
    {
        if(!_isClosed && !_disabled)
        {
            Player._TELEPORT_(_whereILead.SpawnPoint);
            if (screenTransport == null) screenTransport = FindObjectOfType<ScreenTransport>();
            screenTransport.PlayCut(_whereILead.transform, 0.5f);
            return;
        }
        
        if (_interactionToInventory.ExpectItem && !_disabled)
        {
            Debug.Log("Expecting item " + _interactionToInventory.GetFirstExpected());
            if (InventoryPlayer.Instance.CheckIfItemInInventory(_interactionToInventory.GetFirstExpected().ID))
            {
                Debug.Log("Item found");
                _interactionToInventory.UseRemoveItemFromInventory(_interactionToInventory.GetFirstExpected());
                _interactionToInventory.RemoveFirstFromExpected();

                if (_interactionToInventory.GetFirstExpected() == null)
                    _isClosed = false;
                
                Debug.Log("Using item to open door");

                //DEBUG ONLY
                _spriteRenderer.material = _baseMaterial;
                _spriteRenderer.color = Color.gray;
            }
            else Debug.Log("Item not found in inventory");
        }
    }
}
