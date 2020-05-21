using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float health = 500f;
    public float speed = 10;
    public float firerate = .4f;
    public float powerUpDropChance = 1f;
    public float BossSuperAttackTimer = 12;  //12
    public float BossSuperSuperAttackTimer = 45f; //25
    public GameObject BossBullet;
    public GameObject BossBullet2;
    public int SuperSuperAttackbullets = 50;
    public int SuperAttackbullets = 25;


    private float SuperSuperAttackFirerate;
    private float AttackdurationTimer; 
    private float SecondaryFirerate;
    private float FirerateTimer;
    private float SuperAttackTimer = 12f;
    private float SuperSuperAttackTimer = 45f;
    private float SecFireRateTimer;
    private int superattackcount = 0;


    private Renderer rend;


    void Start()
    {
        rend = GetComponent<Renderer>();

        if(Main.S.GetWave() == 15)
        {
            rend.material.color = Color.red;
        }

        switch (MainMenu.difficulty)
        {
            case (1):
                health = health / 2;
                firerate = firerate * 2;
                if (Main.S.GetWave() == 15)
                {
                    health = health * 1.5f;
                }
                break;
            case (2):
                if (Main.S.GetWave() == 15)
                {
                    SuperAttackbullets = SuperAttackbullets * 2;
                    health = health * 2f;
                }               
                break;
            case (3):
                health = health * 1.1f;
                if (Main.S.GetWave() == 15)
                {
                    health = health * 2.2f;
                    SuperAttackbullets = SuperAttackbullets * 3;
                    firerate = firerate / 1.3f;
                }

                break;
            default:
                break;

        }

        SecondaryFirerate = firerate * 3;

        FirerateTimer = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
         Move();
         Attack();

    }

    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    void Attack()
    {

        if(Time.time > FirerateTimer && Time.time < SuperSuperAttackTimer)
        {
            Instantiate(BossBullet, transform.position, Quaternion.identity);
            if(Main.S.GetWave() >= 10 && Time.time > SecFireRateTimer)
            {
                Instantiate(BossBullet, transform.position + new Vector3 (20, 0, 0), Quaternion.identity);
                Instantiate(BossBullet, transform.position + new Vector3(-20, 0, 0), Quaternion.identity);

                SecFireRateTimer = Time.time + SecondaryFirerate;
            }
            FirerateTimer = Time.time + firerate;
        }
        
        else if(Time.time > SuperAttackTimer && Main.S.GetWave() >= 10 && Time.time < SuperSuperAttackTimer)
        {
            for (int i = 0; i < SuperAttackbullets; i++)
            {
                Instantiate(BossBullet2, transform.position + new Vector3(Random.Range(-15f,15f), Random.Range(-15f, 15f), 0), Quaternion.Euler(0f, 0f, Random.Range(60f, 300f)));
            }
            SuperAttackTimer = Time.time + BossSuperAttackTimer;
            FirerateTimer = Time.time + 4;
        }

        else
        {

        }

        if (Main.S.GetWave() >= 15)
        {
            if (Time.time > SuperSuperAttackTimer && Main.S.GetWave() >= 15)
            {

                if (SuperSuperAttackFirerate < Time.time)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Instantiate(BossBullet2, transform.position + new Vector3(Random.Range(-15f, 15f),
                            Random.Range(-15f, 15f), 0), Quaternion.Euler(0f, 0f, Random.Range(90f, 270f)));
                    }
                    SuperSuperAttackFirerate = Time.time + firerate;
                    superattackcount++;
                }

                if (superattackcount > SuperSuperAttackbullets)
                {
                    SuperSuperAttackTimer = Time.time + BossSuperSuperAttackTimer;
                    FirerateTimer = Time.time + 5;
                    SuperAttackTimer = Time.time + BossSuperAttackTimer;
                    superattackcount = 0;
                }

            }
        }

        else
        {
            SuperSuperAttackTimer = Time.time + 50;
        }

    }

    void Move()
    {
        Vector3 tempPos = pos;
        
        if(tempPos.y > 30)
        {
            tempPos.y -= speed * Time.deltaTime;
            pos = tempPos;
        }
       
        
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        if (other.tag == "ProjectileHero")
        {
            Projectile p = other.GetComponent<Projectile>();
            health -= Main.W_DEFS[p.type].damageOnHit;
            if(health <= 0)
            {
                if (MainMenu.difficulty == 1)
                {
                    Main.score += 250 * (2 * (Main.S.GetWave() / 5));
                    if (Main.S.GetWave() == 15)
                    {
                        Main.score += 1000;
                    }
                }
                else if (MainMenu.difficulty == 2)
                {
                    Main.score += 1000 * (2 * (Main.S.GetWave()/5));
                    if (Main.S.GetWave() == 15)
                    {
                        Main.score += 10000;
                    }
                }
                else
                {
                    Main.score += 10000 * (2 * (Main.S.GetWave() / 5));
                    if (Main.S.GetWave() == 15)
                    {
                        Main.score += 100000;
                    }
                }

                Main.isBoss = false;
                Main.S.BossDestroyed(this);
                Destroy(this.gameObject);
            }
            Destroy(other);
        }
    }
}