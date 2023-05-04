using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public int time;

    void Start() {
        StartCoroutine(Timer());
    }

    IEnumerator Timer(){
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
