using UnityEngine;
using System.Collections;

public class playerControls : MonoBehaviour
{

    public float playerSpeed;
    public float jumpHeight;
    public bool isJumping;

    public float health;

    private Vector3 targetPos;

    private AudioSource aSource;
    private BlowDartGun blowGun;
    private Vector3 mousePos;

    private GameMaster gm;

    void Awake()
    {
        gm = GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<GameMaster>();
        gm.player = this.gameObject;
    }

    void Start()
    {
        blowGun = this.GetComponent<BlowDartGun>();
        aSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.isPaused)
        {
            Move();
            Rotate();

            shootControls();
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
            Debug.Log("PressedSpace");
            Rigidbody rigid = this.GetComponent<Rigidbody>();
            Vector3 jumpForce = new Vector3(0, jumpHeight, 0);
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
            blowGun.Shoot(aSource, mousePos);
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

        if (health <= 0)
        {
            GameObject death = Instantiate(gm.bloodPrefab, this.transform.position, Quaternion.identity) as GameObject;
            death.transform.parent = gm.transform;
            this.GetComponent<Rigidbody>().AddExplosionForce(gm.explosionForce * 50f, this.transform.position, gm.explosionRadius * 50f, gm.explosionUpwardMod * 50f, ForceMode.Impulse);
            Destroy(this.gameObject);
        }
    }
}
