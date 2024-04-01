using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfectileState : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
