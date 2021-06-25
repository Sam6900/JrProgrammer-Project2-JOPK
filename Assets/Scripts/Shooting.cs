using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] float bulletForce;
    [SerializeField] float shootingStateWaitTime;
    public float fireRate;
    private float shootingTimeCount;
    public bool isWagonWheelActive;

    private Vector2 shootDir;
    private Vector2 nullVector = new Vector2(0, 0);

    public static bool isShooting;

    public GameObject bulletPrefab;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWagonWheelActive = false; 
    }

    void Update()
    {
        if (!isWagonWheelActive)
        {
            GetInput();
        }
        shootingTimeCount += Time.fixedDeltaTime;
        if (shootingTimeCount > fireRate)
        {
            if (shootDir != nullVector && !isWagonWheelActive)
            {
                Shoot(shootDir);
            }
            if (isWagonWheelActive)
            {
                ShootInAllDir();
            }
            shootingTimeCount = 0;
        }
    }

    // Gets Input key and gives shoot direction based on it 
    void GetInput()
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
}
