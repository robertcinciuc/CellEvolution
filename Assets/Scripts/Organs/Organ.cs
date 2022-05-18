using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organ : MonoBehaviour
{
    public System.Type organType;
    public System.Guid id;
    public SerialOrgan serialOrgan;
    public string name;

    void Start(){
        
    }

    void Update(){
        
    }

    public SerialOrgan getSerialOrgan() {
        return new SerialOrgan(transform.parent.gameObject);
    }
}
