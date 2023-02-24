using UnityEngine;

namespace Plugins.ArmyFights.Core.Team
{
    [CreateAssetMenu(menuName = "Team", fileName = "Team")]
    public class Team : ScriptableObject
    {
        [field: SerializeField]
        public bool Side { get; private set; }
        
        [field: SerializeField]
        public Color Color { get; private set; }
    }
}
