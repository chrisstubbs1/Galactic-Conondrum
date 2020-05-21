using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour
{
    static public Hero S;

    public float gameRestartDelay = 2f;

    public float health = 100;
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float PUhealth = 20;

    AudioSource audio;
    public AudioClip Lasersound;

    // Ship status information
    [SerializeField]
    private float _shieldLevel = 1; // Add the underscore!
    // Weapon fields
    public Weapon[] weapons;

    public bool _____________________;
    public Bounds bounds;

    // Declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    // Create a WeaponFireDelegate field named fireDelegate.
    public WeaponFireDelegate fireDelegate;

    private bool ShieldUp = true;
    private float maxHealth;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
        audio.volume = 0.2f;
        audio.loop = true;
        S = this;
        bounds = Utils.CombineBoundsOfChildren(this.gameObject);
        
    }


    // Use this for initialization
    void Start()
    {
        // Reset the weapons to start _Hero with 1 blaster
        if (MainMenu.shipselection == 1)
        {
            ClearWeapons();
            weapons[0].SetType(WeaponType.blaster);
        }

        else if (MainMenu.shipselection == 2)
        {
            ClearWeapons();
            weapons[0].SetType(WeaponType.spread);
        }

        else if (MainMenu.shipselection == 3)
        {
            ClearWeapons();
            weapons[0].SetType(WeaponType.laser);
        }

        else
        {
            ClearWeapons();
            weapons[0].SetType(WeaponType.missile);
        }

        if(MainMenu.difficulty == 3 && MainMenu.shipselection != 4)
        {
            health = health / 2;
        }
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        bounds.center = transform.position;

        // constrain to screen
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
        if (off != Vector3.zero)
        {  // we need to move ship back on screen
            pos -= off;
            transform.position = pos;
        }

        // rotate the ship to make it feel more dynamic
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        // Use the fireDelegate to fire Weapons
        // First, make sure the Axis("Jump") button is pressed
        // Then ensure that fireDelegate isn't null to avoid an error
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        { // 1
            fireDelegate();
        }

        if (Input.GetKeyDown("space") && MainMenu.shipselection == 3)
        {
            audio.clip = Lasersound;
            audio.Play();
        }
        
        if(Input.GetKeyUp("space"))
        {
            audio.Stop();
        }

        if (health <= 0)
        {
            Main.S.WaveTime = 20;
            Main.WaveClearer = false;
            Main.isBoss = false;
            Main.S.DelayedRestart(gameRestartDelay);
            Destroy(this.gameObject);
        }
    }
    // This variable holds a reference to the last triggering GameObject
    public GameObject lastTriggerGo = null;

    void OnTriggerEnter(Collider other)
    {
        // Find the tag of other.gameObject or its parent GameObjects
        GameObject go = Utils.FindTaggedParent(other.gameObject);
        // If there is a parent with a tag
        if (go != null)
        {
            print("Triggered: " + go.name);
            // Make sure it's not the same triggering go as last time
            if (go == lastTriggerGo)
            { // 2
                return;
            }
            lastTriggerGo = go; // 3
            if (go.tag == "Enemy")
            {
                // If the shield was triggered by an enemy
                // Decrease the level of the shield by 1
                if (ShieldUp == true)
                {
                    shieldLevel--;                 
                }
                else
                {
                    switch (MainMenu.difficulty)
                    {
                        case (1):
                            health -= 5;
                            break;
                        case (2):
                            health -= 10;
                            LoseWeapon();
                            break;
                        case (3):
                            health -= 15;
                            LoseWeapon();
                            break;
                        default:
                            health -= 10;
                            LoseWeapon();
                            break;
                    }
                    
                }
                // Destroy the enemy
                Destroy(go); // 4
            }
            else if(go.tag == "BossBullet")
            {
                // If the shield was triggered by an enemy
                // Decrease the level of the shield by 1
                if (ShieldUp == true)
                {
                    shieldLevel--;
                }
                else
                {
                    switch (MainMenu.difficulty)
                    {
                        case (1):
                            health -= 10;
                            break;
                        case (2):
                            health -= 10;
                            LoseWeapon();
                            break;
                        case (3):
                            health -= 20;
                            LoseWeapon();
                            break;
                        default:
                            health -= 10;
                            LoseWeapon();
                            break;
                    }
                }
                // Destroy the enemy
                Destroy(go); // 4
            }
            else if(go.tag == "Boss")
            {
                Main.S.DelayedRestart(gameRestartDelay);
                Destroy(this.gameObject);
            }
            else if (go.tag == "PowerUp")
            {
                // If the shield was triggerd by a PowerUp
                AbsorbPowerUp(go);
            }
            else
            {
                print("Triggered: " + go.name); // Move this line here!
            }
        }
    }

    public Vector3 Location()
    {
        return this.transform.position;
    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield: // If it's the shield
                ShieldUp = true;
                shieldLevel++;
                break;

            case WeaponType.health:
                if (health + PUhealth >= maxHealth)
                {
                    health = maxHealth;
                }
                else
                {
                    health += PUhealth;
                }
                break;

            default: // If it's any Weapon PowerUp
                     // Check the current weapon type
                if (pu.type == weapons[0].type)
                {
                    // then increase the number of weapons of this type
                    Weapon w = GetEmptyWeaponSlot(); // Find an available weapon
                    if (w != null)
                    {
                        // Set it to pu.type
                        w.SetType(pu.type);
                    }
                }
                else
                {
                    // If this is a different weapon
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }
    
    void LoseWeapon()
    {
        if (weapons[1].type != WeaponType.none)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i].type == WeaponType.none)
                {
                    weapons[i - 1].type = WeaponType.none;
                }

                else if(i + 1 == weapons.Length)
                {
                    weapons[i].type = WeaponType.none;
                }

                else 
                {

                }
            }
        }
    }

    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }
    void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel); // 1
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4); // 2
            // If the shield is going to be set to less than zero
            if (value <= 0)
            {
                ShieldUp = false;
                // Tell Main.S to restart the game after a delay
            }
            
        }
    }

}