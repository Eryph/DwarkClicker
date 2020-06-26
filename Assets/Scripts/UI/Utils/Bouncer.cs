using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{

    [SerializeField] private float _timeToBounce = 0.5f;
    [SerializeField] private float _bounceAmount = 0.5f;

    private float _timeStamp = 0;
    private Vector3 _upPos = Vector3.zero;
    private Vector3 _downPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        _upPos = transform.localPosition + Vector3.up * _bounceAmount;
        _downPos = transform.localPosition + Vector3.down * _bounceAmount;
    }

    // Update is called once per frame
    void Update()
    {
       
        _timeStamp += Time.deltaTime;
        float t = _timeStamp / _timeToBounce;
        t = t * t * (3f - 2f * t);
        transform.localPosition = Vector3.Lerp( _downPos, _upPos, t);
        if (_timeStamp >= _timeToBounce)
        {
            Vector3 tmp = _upPos;
            _upPos = _downPos;
            _downPos = tmp;
            _timeStamp = 0;
        }
    }
}
