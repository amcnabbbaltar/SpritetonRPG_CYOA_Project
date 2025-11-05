using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tactics2D
{
    [CreateAssetMenu(menuName = "Tactics2D/Behavior Tile")]
    public class BehaviorTile : DataTile
    {
        [Tooltip("ScriptableObject that defines this tile's custom behavior.")]
        public ScriptableObject behaviorAsset;

        public ITileBehavior GetBehaviorInstance()
        {
            return behaviorAsset as ITileBehavior;
        }
    }
}
