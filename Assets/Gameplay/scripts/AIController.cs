using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIController : MonoBehaviour
{
    NavMeshAgent agent;
    PlayerCharacter character;
    Transform player;


    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<PlayerCharacter>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("FireControl", 1, 3);

    }

    void FireControl()
    {
        character.Attack();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = player.position;
        transform.LookAt(player.transform);
    }
}
