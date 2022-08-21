using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organ : MonoBehaviour
{
    public System.Type organType;
    public System.Guid id;
    public OrganSerial serialOrgan;
    public string organName;

    void Start(){
        
    }

    void Update(){
        
    }

    public OrganSerial getSerialOrgan() {
        return new OrganSerial(gameObject);
    }
}
