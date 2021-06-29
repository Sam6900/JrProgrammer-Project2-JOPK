using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private float bulletForce;
    [SerializeField] private float shootingStateWaitTime;
    [HideInInspector] public bool isWagonWheelActive;
    [HideInInspector] public static bool isShooting;
    public float fireRate;
    private float shootingTimeCount;
    private Vector2 shootDir;
    private Vector2 nullVector = new Vector2(0, 0);

    private Animator animator;
    [SerializeField] private GameObject bulletPrefab;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isWagonWheelActive = false;
    }

    private void Start()
    {
    }

    void Update()
    {
        HandleShooting();
    }
    
    void GetShootDir()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                shootDir = new Vector2(-1, 1);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                shootDir = new Vector2(1, 1);
            }
            else
            {
                shootDir = Vector2.up;
            }
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                shootDir = new Vector2(-1, 1);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                shootDir = new Vector2(-1, -1);
            }
            else
            {
                shootDir = Vector2.left;
            }
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                shootDir = new Vector2(1, 1);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                shootDir = new Vector2(1, -1);
            }
            else
            {
                shootDir = Vector2.right;
            }
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                shootDir = new Vector2(-1, -1);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                shootDir = new Vector2(1, -1);
            }
            else
            {
                shootDir = Vector2.down;
            }
        }

        else
        {
            shootDir = nullVector;
        }
    }

    void ShootInAllDir()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            Shoot(Vector2.up);
            Shoot(Vector2.left);
            Shoot(Vector2.right);
            Shoot(Vector2.down);
            Shoot(new Vector2(-1, 1));
            Shoot(new Vector2(1, 1));
            Shoot(new Vector2(1, -1));
            Shoot(new Vector2(-1, -1));
        }
    }

    void HandleShooting()
    {
        if (!isWagonWheelActive)
        {
            GetShootDir();
        }

        shootingTimeCount += Time.deltaTime;
        if (shootingTimeCount > fireRate)
        {
            if (shootDir != nullVector && !isWagonWheelActive)
            {
                Shoot(shootDir);
            }
            else if (isWagonWheelActive)
            {
                ShootInAllDir();
            }
            shootingTimeCount = 0;
        }
    }

    // Shoots and changes animation state when shooting
    void Shoot(Vector2 dir)
    {
        isShooting = true;
        animator.SetBool("isShooting", true);
        animator.SetFloat("Vertical", shootDir.y);
        animator.SetFloat("Horizontal", shootDir.x);

        GameObject bullet = Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(dir.normalized * bulletForce, ForceMode2D.Impulse);

        // Changes isShooting to false after waitTime
        StartCoroutine(ChangeIsShottingAnimParameter());
    }

    IEnumerator ChangeIsShottingAnimParameter()
    {
        yield return new WaitForSeconds(shootingStateWaitTime);
        isShooting = false;
        animator.SetBool("isShooting", false);
    }

    private void OnEnable()
    {
        isShooting = false;
    }
}
