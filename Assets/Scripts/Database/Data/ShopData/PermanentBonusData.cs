namespace DwarfClicker.Database
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PermanentBonus", menuName = "Data/Shop/PermanentBonus")]
    public class PermanentBonusData : MonoBehaviour
    {
        [SerializeField] private float _permanentGoldBonusAdd = 0.1f;
        [SerializeField] private float _permanentResBonusAdd = 0.1f;
        [SerializeField] private float _permanentToolBonusAdd = 0.1f;

        public float PermanentGoldBonusAdd { get { return _permanentGoldBonusAdd; } }
        public float PermanentResBonusAdd { get { return _permanentResBonusAdd; } }
        public float PermanentToolBonusAdd { get { return _permanentToolBonusAdd; } }
    }
}