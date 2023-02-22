using UnityEngine;

namespace Plugins.ArmyFights.Example.Scripts
{
    using System;
    using TMPro;
    
    public class SoldiersCounterView : MonoBehaviour
    {
        [SerializeField]
        private EcsSoldiersCounterSystem soldiersCounter;
    
        [SerializeField]
        private TMP_Text text;
        
        private void OnEnable()
        {
            soldiersCounter.OnChangedValue += SetText;
        }
    
        private void OnDisable()
        {
            soldiersCounter.OnChangedValue -= SetText;
        }
        
        private void SetText()
        {
            text.text = soldiersCounter.Count.ToString();
        }
    }
}
