﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class playerControls_Networked : playerStats {

    public string pName;
    public int kills;

    public bool isJumping;

    public float playerSpeed;
    public float jumpHeight;


    private float health = 1;

    private Vector3 targetPos;

    private AudioSource aSource;
    private DartGun_network blowGun;
    private Vector3 mousePos;

    private GameMaster gm;
    private PlayerHolder players;

    private bool delayStart;

    void Awake()
    {

        StatsInit();
        players = GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<PlayerHolder>();
        gm = GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<GameMaster>();
    }

    void Start()
    {
        delayStart = false;

        players.networkPlayers.Add(this.gameObject);
        players.networkPlayerControls.Add(this);

        blowGun = this.GetComponent<DartGun_network>();

        blowGun.setRPM_force(RPM, force);

        aSource = this.GetComponent<AudioSource>();
    }

    void LateStart()
    {
        if (!delayStart)
        {
            players.newSpawnLoc(this.gameObject);
            delayStart = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        LateStart();
        if (!isLocalPlayer)
        {
            return;
        }

        if (!isDead)
        {
            Move();
            Rotate();
            shootControls();
        }
        else
        {
            checkRespawn();
        }
    }

    void FixedUpdate()
    {
        UpdateMousePosition();
        Jump();
    }


    private void Move()
    {
        targetPos = new Vector3(this.transform.position.x + Input.GetAxis("Horizontal"), this.transform.position.y, this.transform.position.z + Input.GetAxis("Vertical"));
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, playerSpeed);
    }

    private void Rotate()
    {
        Vector3 relativePos = mousePos - this.transform.position;
        relativePos = new Vector3(relativePos.x, 0, relativePos.z);
        if (relativePos != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = rotation;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            //Debug.Log("PressedSpace");
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, jumpHeight, 0);

            isJumping = true;
        }
    }

    private void UpdateMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            mousePos = hit.point;
            //gm.test.transform.position = hit.point;  
        }

        //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void shootControls()
    {
        if (Input.GetMouseButton(0))
        {
            blowGun.CmdShoot(mousePos);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == customTags.Ground)
        {

            isJumping = false;
        }
    }

    public void applyDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0 && !isDead)
        {
            players.sortList();
            isDead = true;
            players.newSpawnLoc(this.gameObject);
            model_collider_enabled(false);
            
        }
    }

    private void model_collider_enabled(bool b)
    {
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(b);
        }

        BoxCollider[] bc = this.GetComponents<BoxCollider>();

        for (int i = 0; i < bc.Length; i++)
        {
            bc[i].enabled = b;
        }
        this.GetComponent<Rigidbody>().useGravity = b;
    }

    private void checkRespawn()
    {
        if (!isDead)
        {
            return;
        }
        curRespawnTimer -= Time.deltaTime;
        if (curRespawnTimer <= 0)
        {
            curRespawnTimer = respawnTimer;
            respawn();
        }
    }

    private void respawn()
    {
        isDead = false;
        model_collider_enabled(true);
    }
}
