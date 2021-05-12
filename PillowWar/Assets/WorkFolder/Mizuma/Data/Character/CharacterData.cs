using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    public CharacterData(GameObject _myObject)
    {
        myObject = _myObject;
        Transform t = myObject.transform;
        myBodyTransform = t;
        myPillowTransform = t.GetChild(2).transform;
        myCameraTransform = t.GetChild(3).transform;
        myBodyRigidbody = myObject.GetComponent<Rigidbody>();
        myPillowRigidbody = null;
        hp = GameManager.Instance.ruleData.maxHp;
    }

    public GameObject myObject;
    public Transform myBodyTransform;
    public Transform myPillowTransform;
    public Transform myCameraTransform;
    public Rigidbody myBodyRigidbody;
    public Rigidbody myPillowRigidbody;

    public int hp;
}
