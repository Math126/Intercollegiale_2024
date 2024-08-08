using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;

public class PlayerCrouch : MonoBehaviour
{
    private bool crouching = false;
    private float normalHeight;
    public KeyCode Crouch = KeyCode.C;
    // Start is called before the first frame update
    void Start()
    {
        normalHeight = GetComponent<CapsuleCollider>().height;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Crouch))
        {
            if (crouching)
            {
                crouching = false;
            }
            else
            {
                crouching = true;
            }
        }

        if (crouching)
        {
            //Debug.Log("Crouching!!!");
            GetComponent<CapsuleCollider>().height = normalHeight / 2;
        }
        else
        {
            GetComponent<CapsuleCollider>().height = normalHeight;
        }
    }
}
