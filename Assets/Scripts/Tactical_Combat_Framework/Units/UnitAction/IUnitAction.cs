using UnityEngine;
using System.Collections;

namespace Tactics2D
{
    public interface IUnitAction
    {
        string ActionName { get; }
        bool CanExecute(Unit unit);
        IEnumerator Execute(Unit unit);
    }
}
