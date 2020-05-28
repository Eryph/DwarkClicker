using DwarfClicker.UI.TradingPost;
using Preprod;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInitializer : MonoBehaviour
{
    [SerializeField] private TradingPostDisplayer _tpDisplayer = null;
    [SerializeField] private MineDisplayer _mineDisplayer = null;
    [SerializeField] private ForgeDisplayer _forgeDisplayer = null;
    [SerializeField] private InnDisplayer _innDisplayer = null;

    private void Start()
    {
        _tpDisplayer.Init();
        _mineDisplayer.Init();
        _forgeDisplayer.Init();
        _innDisplayer.Init();
    }
}
