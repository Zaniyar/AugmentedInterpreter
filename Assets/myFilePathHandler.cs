using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myFilePathHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public int Index { get; set; }


    // Update is called once per frame
    void Update()
    {

    }
    void OnMouseDown()
    {
        getMyCSVpath();
    }
    public void getMyCSVpath()
    {
        Debug.Log("Something here ja?" + this.Index);
    }
}
