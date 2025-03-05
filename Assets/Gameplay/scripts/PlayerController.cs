using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    PlayerCharacter character;

    private void Start()
    {
        character = GetComponent<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            character.Attack();

        }

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        character.Move(new Vector3(h, 0, v));

        var Lookdir = Vector3.forward * v + Vector3.right * h;

        if(Lookdir.magnitude !=0)
        {
            character.Rotate(Lookdir);
        }


    }
}
