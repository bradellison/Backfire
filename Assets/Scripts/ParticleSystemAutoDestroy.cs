using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour {

    private ParticleSystem _ps;


    public void Start()
    {
        _ps = this.gameObject.GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (_ps == null)
        {
            Destroy(this);
        }
    }
}
