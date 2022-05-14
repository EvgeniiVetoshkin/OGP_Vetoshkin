using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AttachPointChange : NetworkBehaviour
{
    [SerializeField]
    public Transform firstArm;
    [SerializeField]
    public Transform secondArm;

    // Update is called once per frame
    private void Start()
    {
        GetComponent<AttachPointChange>().firstArm = SuperFind(transform.parent, "mixamorig:LeftHandMiddle4");
        GetComponent<AttachPointChange>().secondArm = SuperFind(transform.parent, "mixamorig:RightHandMiddle4");
    }
    void Update()
    {
        transform.position = (firstArm.position + secondArm.position) * 0.5f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.05f);
    }


    private Transform SuperFind(Transform current, string s)
    {
        Transform otvet = null;

        otvet = current.Find(s);


        foreach (Transform child in current)
        {
            if (otvet != null)
                return otvet;

            otvet = SuperFind(child, s);
        }

        return otvet;
    }

}
