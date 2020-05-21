using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    static public Main S;
    static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;
    static public bool isBoss = false;
    static public bool WaveClearer = false;
    static public bool isBossSpawned = false;
    static public int score = 0; 

    public BossEnemy Boss;
    public GameObject Zippy;
    public GameObject Gronk;
    public GameObject Weaver;
    public GameObject Spike;
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemySpawnPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;
    public float WaveTime = 20;
    public float WaveDownTime = 10;
    public Text Wavetext;
    public Text timetext;
    public Text shiptext;
    public Text scoretext;
    public Text healthtext;
    public Text Bosstext;
    public GameObject GameOver;
   


    public GameObject prefabPowerUp;
    private WeaponType[] powerUpFrequency = new WeaponType[] {
WeaponType.blaster,
WeaponType.shield,
WeaponType.health,
WeaponType.spread,
WeaponType.shield,
WeaponType.health,
WeaponType.laser,
WeaponType.shield,
WeaponType.health,
WeaponType.missile,
WeaponType.shield};

    public bool _______________;
    public WeaponType[] activeWeaponTypes;
    public float enemySpawnRate;
    
    public AudioClip AC;
    public AudioClip ACBM;
    public AudioClip ACFBM;
    public AudioClip EXPLe;
    public AudioClip EXPLb;
    private AudioSource ASGO; //GAME OVER CLIP AND AUDIO SOURCE/BOSS MUSIC CLIP AND AUDIO SOURCE/MAIN MUSIC AUDIO SOURCE
    private AudioSource ASMS;
    private AudioSource ASBM;
    private AudioSource ASFBM;
    private AudioSource ExplosionE;
    private AudioSource ExplosionB;

    private float TempWaveTime;
    private float TempDownTime;
    private float tempSpawnRate;
    private int wavecount = 1;
    private BossEnemy bossMAIN;
    private GameObject bossObject;
    private bool ISDEAD = false;

    void Awake()
    {
        isBoss = false;
        WaveClearer = false;
        isBossSpawned = false;
        Wavetext.text = "Wave 1";
        TempDownTime = WaveDownTime;
        TempWaveTime = WaveTime;
        score = 0;

        if(MainMenu.shipselection == 1)
        {
            Zippy.SetActive(true);
            shiptext.text = "Zippy";

        }

        else if (MainMenu.shipselection == 2)
        {
            Gronk.SetActive(true);
            shiptext.text = "Gronk";

        }

        else if(MainMenu.shipselection == 3)
        {
            Weaver.SetActive(true);
            shiptext.text = "Weaver";
        }

        else
        {
            Spike.SetActive(true);
            shiptext.text = "Spike";
        }

        S = this;
        //Set Utils.cambounds
        Utils.SetCameraBounds(this.GetComponent<Camera>());
        //0.5 enemies/second = enemySpawnRate of 2
        enemySpawnRate = 1f / enemySpawnPerSecond;
        //Invoke call spawnenemy() once after a 2 second delay
        Invoke("SpawnEnemy", enemySpawnRate);

        tempSpawnRate = enemySpawnRate;

        // A generic Dictionary with WeaponType as the key
        W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            W_DEFS[def.type] = def;
        }
    }

    void Update()
    {
        WaveManager();
        if(WaveTime >= 0)
        {
            timetext.text = "Time: " + (int)WaveTime;
        }
        else
        {
            timetext.text = "Time: " + (int)WaveDownTime;
        }

        if(Hero.S.health >= 0)
        {
            healthtext.text = "Health: " + Hero.S.health;
        }
        else
        {
            healthtext.text = "Health: 0";
        }
        scoretext.text = "Score: " + score;
        if(isBoss == true)
        {
            bossObject = GameObject.FindWithTag("Boss");
            bossMAIN = bossObject.GetComponent<BossEnemy>();
            Bosstext.text = "Boss: " + (int)bossMAIN.health;
        }
        else
        {
            Bosstext.text = " ";
        }

    }

    public void BossDestroyed(BossEnemy e)
    {
        ExplosionB.Play();
        if (Random.value <= e.powerUpDropChance)
        {
            int ndx;
            if (MainMenu.shipselection == 1)
            {
                ndx = Random.Range(0, 3);
            }
            else if (MainMenu.shipselection == 2)
            {
                ndx = Random.Range(3, 6);
            }
            else if (MainMenu.shipselection == 3)
            {
                ndx = Random.Range(6, 9);
            }
            else
            {
                ndx = Random.Range(9, 12);
            }
            /*
            ndx = Random.Range(0, powerUpFrequency.Length);
            */
            WeaponType puType = powerUpFrequency[ndx];
            // Spawn a PowerUp
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            // Set it to the proper WeaponType
            pu.SetType(puType);
            // Set it to the position of the destroyed ship
            pu.transform.position = e.transform.position;
        }
    }
    public void ShipDestroyed(Enemy e)
    {
        ExplosionE.Play();
        // Potentially generate a PowerUp
        if (Random.value <= e.powerUpDropChance)
        {
            // Random.value generates a value between 0 & 1 (though never == 1)
            // If the e.powerUpDropChance is 0.50f, a PowerUp will be generated
            // 50% of the time. For testing, it's now set to 1f.
            // Choose which PowerUp to pick
            // Pick one from the possibilities in powerUpFrequency
            int ndx;
            if (MainMenu.shipselection == 1)
            {
                ndx = Random.Range(0, 3);
            }
            else if(MainMenu.shipselection == 2)
            {
                ndx = Random.Range(3, 6);
            }
            else if(MainMenu.shipselection == 3)
            {
                ndx = Random.Range(6, 9);
            }
            else
            {
                ndx = Random.Range(9, 12);
            }
            /*
            ndx = Random.Range(0, powerUpFrequency.Length);
            */
            WeaponType puType = powerUpFrequency[ndx];
            // Spawn a PowerUp
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            // Set it to the proper WeaponType
            pu.SetType(puType);
            // Set it to the position of the destroyed ship
            pu.transform.position = e.transform.position;
        }
    }

    public int GetWave()
    {
        return wavecount;
    }

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        // Check to make sure that the key exists in the Dictionary
        // Attempting to retrieve a key that didn't exist, would throw an error,
        // so the following if statement is important.
        if (W_DEFS.ContainsKey(wt))
        {
            return (W_DEFS[wt]);
        }
        // This will return a definition for WeaponType.none,
        // which means it has failed to find the WeaponDefinition
        return (new WeaponDefinition());
    }

    void Start()
    {
        ASMS = this.GetComponent<AudioSource>();
        ASGO = gameObject.AddComponent<AudioSource>();
        ASBM = gameObject.AddComponent<AudioSource>();
        ASFBM = gameObject.AddComponent<AudioSource>();
        ExplosionE = gameObject.AddComponent<AudioSource>();
        ExplosionB = gameObject.AddComponent<AudioSource>();
       
        ExplosionB.clip = EXPLb;
        ExplosionE.clip = EXPLe;
        ASFBM.clip = ACFBM;
        
        ASBM.loop = true;
        ASBM.volume = .4f;
        ASGO.loop = true;
        ASGO.volume = .4f;
        ASFBM.loop = true;
        ASFBM.volume = .4f;

        // Not yet
        //GameObject scoreGO = GameObject.Find("ScoreCounter");
        activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
        for (int i = 0; i < weaponDefinitions.Length; i++)
        {
            activeWeaponTypes[i] = weaponDefinitions[i].type;
        }

    }

    public void SpawnEnemy()
    {
        int ndx = 0;
        //pick a random enemy prefab to instantiate

        if (wavecount % 5 != 0)
        {
            switch (wavecount)
            {
                case 1:
                    ndx = 3;
                    break;
                case 2:
                    ndx = Random.Range(2, 4);
                    break;
                case 3:
                    ndx = Random.Range(1, 4);
                    break;
                case 4:
                    ndx = Random.Range(0, 4);
                    break;
                case 6:
                    ndx = Random.Range(4, 6);
                    break;
                case 7:
                    ndx = Random.Range(3, 6);
                    break;
                case 8:
                    ndx = 6;
                    break;
                case 9:
                    ndx = Random.Range(1, prefabEnemies.Length);
                    break;
                default:
                    ndx = Random.Range(0, prefabEnemies.Length);
                    break;
            }
        }
        else
        {
             
            ndx = 3;

        }

        GameObject go = Instantiate(prefabEnemies[ndx]) as GameObject;
        //position the Enemy above the screen with a random x positions
        Vector3 pos = Vector3.zero;
        float xMin = Utils.camBounds.min.x + enemySpawnPadding;
        float xMax = Utils.camBounds.max.x - enemySpawnPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = Utils.camBounds.max.y + enemySpawnPadding;
        go.transform.position = pos;
        //call spawnEnemy() again in a couple of seconds
        Invoke("SpawnEnemy", enemySpawnRate);
    }

    public void SpawnBoss()
    {

        BossEnemy boss = Instantiate(Boss) as BossEnemy;
        Vector3 pos = Vector3.zero;
        pos.x = 0;
        pos.y = 78;
        boss.health += ((wavecount / 5) * 500);
        boss.transform.position = pos;
        isBossSpawned = true;
    }

    public void DelayedRestart(float delay)
    {
        if (PlayerPrefs.GetInt("HighScore", 0) < score)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        ISDEAD = true;
        ASBM.Stop();
        ASMS.Stop();
        ASFBM.Stop();
        ASGO.clip = AC;
        ASGO.Play();

        // Invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        // Reload _Scene_0 to restart the game
        //Application.LoadLevel("_Scene_0");
        GameOver.SetActive(true);
    }

    public void WaveManager()
    {
        if(isBoss != true)
        {
            WaveTime -= Time.deltaTime;
        }
        else
        {
            if (isBossSpawned == false && (float)wavecount % 5 == 0)
            {
                ASMS.Stop();
                if (wavecount != 15)
                {
                    ASBM.clip = ACBM;
                    ASBM.Play();
                }
                else
                {
                    ASFBM.Play();
                }
                isBossSpawned = true;
                SpawnBoss();
            }
            
            enemySpawnRate = 6;
            
        }

        if (WaveTime <= 0 && ISDEAD != true)
        {
            if (!ASMS.isPlaying)
            {
                ASFBM.Stop();
                ASBM.Stop();
                ASMS.Play();
            }
            WaveClearer = true;
            WaveDownTime -= Time.deltaTime;
            Wavetext.text = "Prepare";
            enemySpawnRate = 10;
        }
        

        if(WaveDownTime <= 0 && WaveClearer == true)
        {
            wavecount++;
            if (wavecount % 5 != 0)
            {
                Wavetext.text = "Wave " + wavecount;
                TempWaveTime += 1;
                WaveTime = TempWaveTime;
            }
            else
            {
                TempWaveTime += 6;
                Wavetext.text = "Boss Wave";
                isBoss = true;
                WaveTime = 1;
                Main.isBossSpawned = false;
            }
            WaveClearer = false;
            WaveDownTime = TempDownTime;
            
            if (wavecount % 5 == 0)
            {
                if (tempSpawnRate != .4f)
                {
                    tempSpawnRate -= .05f;
                    enemySpawnRate = tempSpawnRate;
                }
            }
            else
            {
                if (tempSpawnRate != .4f)
                {
                    tempSpawnRate -= .05f;
                    enemySpawnRate = tempSpawnRate - .01f;
                }
            }
        }

    }

}
