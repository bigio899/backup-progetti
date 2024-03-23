using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [SerializeField] float shiftSpeed = 10f;
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float stamina = 5f;
    [SerializeField] Animator anim;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float staminaRefill = 5f;
    [SerializeField] Image pistolUI, rifleUI, miniGunUI, cusror;
    bool isShifting = false;
    private int health;
    bool isGrounded = true;
    [SerializeField] GameObject pistol, rifle, miniGun;
    float currentSpeed;
    Rigidbody rb;
    Vector3 direction;
    bool isPistol, isRifle, isMiniGun;
    // Un riferimento alla sorgente audio
    [SerializeField] AudioSource characterSounds;
    // Un riferimento al clip audio di salto
    [SerializeField] AudioClip jump;
    public enum Weapons
    {
        None,
        Pistol,
        Rifle,
        MiniGun
    }
    Weapons weapons = Weapons.None;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = movementSpeed;
        anim = GetComponent<Animator>();
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
        direction = transform.TransformDirection(direction);
        if (direction.x != 0 || direction.z != 0)
        {
            anim.SetBool("Run", true);
            // Se la sorgente audio non riproduce alcun suono e siamo a terra, allora...
            if (!characterSounds.isPlaying && isGrounded)
            {
                // Riproduzione del suono
                characterSounds.Play();
            }
        }
        if (direction.x == 0 && direction.z == 0)
        {
            anim.SetBool("Run", false);
            // Disattivare il suono se il personaggio si ferma
            characterSounds.Stop();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
            anim.SetBool("Jump", true);
            // Disabilitazione del suono di esecuzione
            characterSounds.Stop();
            // Creare una sorgente audio temporanea per il salto
            AudioSource.PlayClipAtPoint(jump, transform.position);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isShifting = true;
            if (stamina > 0)
            {
                stamina -= Time.deltaTime;
                currentSpeed = shiftSpeed;
            }
            else
            {
                currentSpeed = movementSpeed;
            }
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            stamina += Time.deltaTime;
            currentSpeed = movementSpeed;
        }

        if (stamina > 5f)
        {
            stamina = 5f;
        }
        else if (stamina < 0f)
        {
            stamina = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && isPistol)
        {
            ChooseWeapon(Weapons.Pistol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && isRifle)
        {
            ChooseWeapon(Weapons.Rifle);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && isMiniGun)
        {
            ChooseWeapon(Weapons.MiniGun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChooseWeapon(Weapons.None);
        }

    }

    private IEnumerator RunTimer()
    {
        yield return new WaitForSeconds(.5f);
    }
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + direction * currentSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        anim.SetBool("Jump", false);
    }

    public void ChooseWeapon(Weapons weapons)
    {
        anim.SetBool("Pistol", weapons == Weapons.Pistol);
        anim.SetBool("Assault", weapons == Weapons.Rifle);
        anim.SetBool("MiniGun", weapons == Weapons.MiniGun);
        anim.SetBool("NoWeapon", weapons == Weapons.None);
        pistol.SetActive(weapons == Weapons.Pistol);
        rifle.SetActive(weapons == Weapons.Rifle);
        miniGun.SetActive(weapons == Weapons.MiniGun);
        if (weapons != Weapons.None)
        {
            cusror.enabled = true;
        }
        else
        {
            cusror.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "pistol":
                if (!isPistol)
                {
                    isPistol = true;
                    ChooseWeapon(Weapons.Pistol);
                    pistolUI.color = Color.white;
                }
                break;
            case "rifle":
                if (!isRifle)
                {
                    isRifle = true;
                    ChooseWeapon(Weapons.Rifle);
                    rifleUI.color = Color.white;
                }
                break;
            case "minigun":
                if (!isMiniGun)
                {
                    isMiniGun = true;
                    ChooseWeapon(Weapons.MiniGun);
                    miniGunUI.color = Color.white;
                }
                break;
            default:
                break;
        }
        Destroy(other.gameObject);
    }

    public void ChangeHealth(int count)
    {
        // sottrazione della salute
        health -= count;
        // se la salute è pari o inferiore a zero, allora...
        if (health <= 0)
        {
            //qualcosa sta per accadere
            //Attivazione dell'animazione di morte
            anim.SetBool("Die", true);
            //Rimozione dell'arma
            ChooseWeapon(Weapons.None);
            //La disattivazione dello script PlayerController rende il giocatore incapace di muoversi
            this.enabled = false;
        }
    }
}