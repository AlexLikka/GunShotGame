using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor.Experimental.GraphView;

public class PlayerCharacter : MonoBehaviour
{
    CharacterController cc;
    public float speed;
    Animator animator;
    bool isAlive = true;
    public float turnSpeed;
    public Rigidbody shell;
    public Transform muzzle;
    public float launchForce = 10;
    public AudioSource shootAudioSource;
    bool attacking = false;
    public float attackTime;
    float hp;
    public float hpmax = 100;
    public Slider hpSlider;
    public Image hpFillImage;
    public Color hpColorFull = Color.green;
    public Color hpColorNull = Color.red;
    public ParticleSystem explosionEffect;
    System.Random ran = new System.Random();

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        hp = hpmax;
        RefreshHealthHUD();
    }

    public void Move(Vector3 v)
    {
        if (!isAlive)
            return;
        if (attacking)
            return;

        Vector3 movement = v * speed;
        cc.SimpleMove(movement);

        if(animator != null)
        {
            animator.SetFloat("Speed", cc.velocity.magnitude);
        }
    }

    public void Rotate(Vector3 Lookdir)
    {
        var targetPos = transform.position + Lookdir;
        var characterPos = transform.position;

        characterPos.y = 0;
        targetPos.y = 0;

        var faceToTargetDir = targetPos - characterPos;

        var faceToQuat = Quaternion.LookRotation(faceToTargetDir);
        
        Quaternion slerp = Quaternion.Slerp(transform.rotation, faceToQuat, turnSpeed*Time.deltaTime);

        transform.rotation = slerp;
    }
    public void Attack()
    {
        if (!isAlive)
            return;
        if (attacking)
            return;

        var shellInstance = Instantiate(shell, muzzle.position, muzzle.rotation) as Rigidbody;
        shellInstance.velocity = launchForce * muzzle.forward;

        if (animator)
        {
            animator.SetTrigger("Attack");
        }

        attacking = true;
        shootAudioSource.Play();

        Invoke("RefreshAttack", attackTime);

    }

    void RefreshAttack()
    {
        attacking = false;
    }

    public void Death()
    {
        isAlive = false;
        explosionEffect.transform.parent = null;
        explosionEffect.gameObject.SetActive(true);
        ParticleSystem.MainModule mainModule = explosionEffect.main;
        Destroy(explosionEffect.gameObject, mainModule.duration);


        gameObject.SetActive(false);
    }
    public void ReInstant()
    {
        GameObject newobject = Instantiate(gameObject);
        newobject.transform.position = new Vector3(ran.Next(-50, 50), 0, ran.Next(-50, 50));
        newobject.transform.localEulerAngles = Vector3.zero;
        newobject.SetActive(true);
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        RefreshHealthHUD();
        if (hp <= 0 && isAlive)
        {
            Death();
            ReInstant();
        }
    }
    public void RefreshHealthHUD()
    {
        hpSlider.value = hp;
        hpFillImage.color = Color.Lerp(hpColorNull, hpColorFull, hp / hpmax);
    }

    // Start is called before the first frame update

    void Update()
    {
        
    }
}
