using Language.Lua;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.Yarn;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace WIELKAPOLSKA
{
    public class LuaScriptsBase : YarnCustomCommands
    {
        [SerializeField] DialogueDatabase dialogue;

        [SerializeField] List<string> _visitedNodes; // this could be a HashSet if we want more efficient search

        Action<string> ADD_CONVERSATION;

        public override void RegisterFunctions()
        {
            // This MUST be called here:
            base.RegisterFunctions();

            //Lua.RegisterFunction("my_command", this, SymbolExtensions.GetMethodInfo(() => MyCustomCommand("", 0f, false)));

            Lua.RegisterFunction(nameof(isVisited), this, SymbolExtensions.GetMethodInfo(() => isVisited(string.Empty)));
            Lua.RegisterFunction(nameof(NodeComplete), this, SymbolExtensions.GetMethodInfo(() => NodeComplete(string.Empty)));
        }

        public override void UnregisterFunctions()
        {
            // This MUST be called here:
            base.UnregisterFunctions();

            Lua.UnregisterFunction(nameof(isVisited));
            Lua.UnregisterFunction(nameof(NodeComplete));
        }

        void Start()
        {
            _visitedNodes = new List<string>();

            ADD_CONVERSATION += addConv;

            //Add get from SaveData

            // registers a function "random()" in Yarn
            // note how the C# func name ("GetRandomValue") can be different from the Yarn func name ("random")
        }

        private void OnDestroy()
        {
            ADD_CONVERSATION -= addConv;
        }

        void addConv(string conv)
        {
            _visitedNodes.Add(conv);
        }

        public float GetRandomValue()
        {
            //return Random.Range(0, 100); // return a random number between 0 and 99 (will never return 100)
            return 0;
        }

        public override void OnConversationStart(Transform actor)
        {
            base.OnConversationStart(actor);
            Debug.Log("OnConversationStart");
        }

        // this is called via DialogueRunner.OnNodeComplete (a UnityEvent assigned in the inspector)
        // which is fire every time the player leaves a Yarn node "nodeName"
        public void NodeComplete(string nodeName)
        {
            _visitedNodes.Add(nodeName);
        }

        public bool isVisited(string nodeName)
        {
            //foreach (string node in _visitedNodes) Debug.Log(node);

            Debug.Log("test test test");
            //var id = DialogueManager.MasterDatabase.GetConversation(nodeName);
            //Debug.Log(id);

            return _visitedNodes.Contains(nodeName);
            //return _visitedNodes.Contains(nodeName);
        }

        void SaveGame()
        {

        }
    }
}

