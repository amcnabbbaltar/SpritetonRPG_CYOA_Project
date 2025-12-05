using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics2D
{
    public interface IPressurePlateInterpreter
    {
        void ActivateTrigger(string group);
        void DeactivateTrigger(string group);
    }
}