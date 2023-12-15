using Assets._Scripts.ExploreScene.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
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
        Player._TELEPORT_(_whereILead.SpawnPoint);
        if (screenTransport == null) screenTransport = FindObjectOfType<ScreenTransport>();
        screenTransport.PlayCut(_whereILead.transform, 0.5f);
    }
}
