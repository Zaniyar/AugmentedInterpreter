using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obj;
    private bool isActive;
    void Start()
    {
        isActive = false;
        obj.SetActive(isActive);
    }

    // Update is called once per frame
    public void showObj()
    {
        isActive = !isActive;
        obj.SetActive(isActive);
    }
}
