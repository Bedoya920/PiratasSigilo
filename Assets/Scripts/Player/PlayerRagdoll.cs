using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    private CharacterController mainCol;
    private Animator anim;

    Collider[] ragdollCol;
    Rigidbody[] ragdollRb;

    void Start()
    {
        mainCol = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        RagdollParts();
        RagdollOFF();
    }

    public void RagdollON()
    {
        anim.enabled = false;
        foreach(Collider c in ragdollCol)
        {
            c.enabled = true;
        }
        foreach(Rigidbody rb in ragdollRb)
        {
            rb.isKinematic = false;
        }

        mainCol.enabled = false;
        
    }

    void RagdollOFF()
    {
        anim.enabled = true;
        foreach(Collider c in ragdollCol)
        {
            c.enabled = false;
        }
        foreach(Rigidbody rb in ragdollRb)
        {
            rb.isKinematic = true;
        }

        mainCol.enabled = true;
        
    }

    void RagdollParts()
    {
        ragdollCol = GetComponentsInChildren<Collider>();
        ragdollRb = GetComponentsInChildren<Rigidbody>();
    }
}
