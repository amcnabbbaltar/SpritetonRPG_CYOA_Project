using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tactics2D
{
    public class InterpreterSystem: MonoBehaviour, IInterpreter
    {

        public void ActivateTrigger(string group)
        {
            switch (group)
            {
                case "A":
                    ActionSystem.Instance.OpenDoor(group);
                    Debug.Log("Activate Action A");
                    break;
                case "B":
                    Debug.Log("Activate Action B");
                    break;
                case "C":
                    Debug.Log("Activate Action C");
                    break;
                default:
                    Debug.Log("Not Defined");
                    break;
            }
        }

        public void DeactivateTrigger(string group)
        {
            switch (group)
            {
                case "A":
                    ActionSystem.Instance.CloseDoor(group);
                    Debug.Log("Deactivation Action A");
                    break;
                case "B":
                    Debug.Log("Deactivation Action B");
                    break;
                case "C":
                    Debug.Log("Deactivation Action C");
                    break;
                default:
                    Debug.Log("Not Defined");
                    break;
            }
        }
    }
}