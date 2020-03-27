namespace DwarfClicker.Database
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ConsumableBonus", menuName = "Data/Shop/ConsumableBonus")]
    public class ConsumableBonusData : ScriptableObject
    {
        [SerializeField] private float _productionSpeeMult = 2f;
        [SerializeField] private float _bonusTimeMax = 21600f;
        [SerializeField] private float _bonusTimeByAd = 3600f;

        public float ProductionSpeeMult { get { return _productionSpeeMult; } }
        public float BonusTimeMax { get { return _bonusTimeMax; } }
        public float BonusTimeByAd { get { return _bonusTimeByAd; } }
    }
}