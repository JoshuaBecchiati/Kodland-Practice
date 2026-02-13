using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform rifleStart;
    [SerializeField] private Text HpText;

    [SerializeField] private GameObject GameOver;
    [SerializeField] private GameObject Victory;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -10f;
    [SerializeField] private CharacterController cc;

    private Vector3 vel;
    public float health = 100f;

    void Start()
    {
        ChangeHealth(health);
    }

    void Update()
    {
        CharacterMovement();

        if (Input.GetMouseButtonDown(0))
        {
            GameObject buf = Instantiate(bullet);
            buf.transform.position = rifleStart.position;
            buf.GetComponent<Bullet>().setDirection(transform.forward);
            buf.transform.rotation = transform.rotation;
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            Collider[] tar = Physics.OverlapSphere(transform.position, 2);
            foreach (var item in tar)
            {
                if (item.tag == "Enemy")
                {
                    Destroy(item.gameObject);
                }
            }
        }

        Collider[] targets = Physics.OverlapSphere(transform.position, 3);
        foreach (var item in targets)
        {
            if (item.tag == "Heal")
            {
                ChangeHealth(50);
                Destroy(item.gameObject);
            }
            if (item.tag == "Finish")
            {
                Win();
            }
            if (item.tag == "Enemy")
            {
                Lost();
            }
        }
    }

    private void CharacterMovement()
    {
        if (cc.isGrounded && vel.y < 0)
        {
            vel.y = -2;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        cc.Move(move * speed * Time.deltaTime);

        vel.y += gravity * Time.deltaTime;
        cc.Move(vel * Time.deltaTime);
    }

    public void ChangeHealth(float hp)
    {
        health += hp;
        if (health > 100)
        {
            health = 100;
        }
        else if (health <= 0)
        {
            Lost();
        }
        HpText.text = health.ToString();
    }

    public void Win()
    {
        GameObject[] nemiciTag = GameObject.FindGameObjectsWithTag("Enemy");
        if(nemiciTag.Length <= 0)
        {
            Victory.SetActive(true);
            Destroy(GetComponent<PlayerLook>());
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Lost()
    {
        GameOver.SetActive(true);
        Destroy(GetComponent<PlayerLook>());
        Cursor.lockState = CursorLockMode.None;
    }
}
