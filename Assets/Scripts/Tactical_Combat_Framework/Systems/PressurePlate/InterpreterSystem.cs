using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tactics2D
{
    public class InterpreterSystem : MonoBehaviour
    {
        public static InterpreterSystem Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void ProcessTrigger(string group)
        {
            switch (group)
            {
                case "A":
                    print("Activate Action A");
                    break;
                case "B":
                    print("Activate Action B");
                    break;
                case "C":
                    print("Activate Action C");
                    break;
                default:
                    print("Not Defined");
                    break;
            }
        }
    }
}