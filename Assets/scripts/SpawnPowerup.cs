using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * provides a framework for determining which type
 * of power up one is
 * while still allowing for the polymorphic array
 * */
public enum PowerupType
{
    TRIPPLE_SHOT,
    RAPID_FIRE,
    POWERUP_COUNT
}



/**
 * a powerup will give the player unique abilities for the duration
 * they will always spawn in the middle of the platform given to them
 * */
public class Powerup
{
    /**
     * the graphic that will be associated with the powerup
     * */
    protected GameObject powerupGameObject;

    /**
     * the graphic that shows over the player when they
     * have the specific powerup
     **/ 
    protected Sprite inUseGraphic;

    /**
     * stores the location of the powerup on the screen
     * */
    protected Vector2 location;

    /**
     * how much time the object has to be picked up before it despawns
     * */
    protected float time;

    /**
     * how much time the power up has for it to be in effect
     * */
    protected float powerTime;

    /**
     * current time for either usage or for waiting to be picked up
     * */
    protected float currentTime;

    /**
     * referenec to the player who picked up the powerup
     **/ 
    protected GameObject player;

    /**
     * what type of power up is it?
     * */
    protected PowerupType type;

    /**
     * holds the collision radius of the powerup
     * */
    protected float radius;

    /**
     * returns if the powerup has been picked up or not
     * */
    public bool isPickedUp()
    {
        return (this.powerupGameObject != null);
    }

    /**
     * getters
     * */
    public Vector2 getLocation() { return location; }
    public float getTime() { return time; }
    public float getPowerTime() { return powerTime; }
    public Sprite getInUseGraphic() { return inUseGraphic; }

    /**
     * tells players what the powerup does
     * */
    public string effectDescription;

    void Start() {
        player = null;
     }

    /**
     * constructor
     * */
    public Powerup(GameObject powerupObject, GameObject spawnPlatform, float time, float powerTime, PowerupType type, float radius)
    {
        //inUseGraphic = ;
        // location = spawnPlatform.transform.position + new Vector3(0, spawnPlatform.transform.lossyScale.y);
        this.time = 10f;
        this.powerTime = powerTime;
        this.currentTime = 0;
        this.type = type;
        this.radius = radius;

        this.powerupGameObject = GameObject.Instantiate(powerupObject);
        this.powerupGameObject.transform.position = new Vector3(99f, 99f, 0f);
        this.powerupGameObject.tag = "Powerup";
    }

    /**
     * removes the powerup entirely
     * */
    public void destroy()
    {
        if(this.powerupGameObject != null)
        {
            GameObject.DestroyObject(this.powerupGameObject);
            this.powerupGameObject = null;
        }
    }

    /**
     * updates the object including both of the times
     * */
    public bool update(float delta)
    {
        currentTime += delta;
        float maxTime = 0;

        //in this case, the powerup is not in use
        if (player == null)
        {
            maxTime = time;
        }
        else
        {
            maxTime = powerTime;
        }
        
        if (currentTime > maxTime)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /**
     * allows a player to pick up the powerup
     * */
    public void pickUp(GameObject player)
    {
        currentTime = 0;
        this.player = player;
        addPowerup();
        destroy();
    }

    /**
     * finds all objects that are current colliding with the powerup
     * */
    public List<GameObject> getCollisions()
    {
        if (powerupGameObject == null)
        {
            //lets work the garbage collector overtime, not like its doing anything useful anyway
            return new List<GameObject>();
        }

        List<GameObject> collisions = new List<GameObject>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            Vector2 pos = players[i].transform.position;
            Vector2 thisPos = this.powerupGameObject.transform.position;

            float distance = Vector2.Distance(pos, thisPos);
            if (distance < radius)
            {
                collisions.Add(players[i]);
            }
        }

        return collisions;
    }

    /**
     * adds the powerup to the player
     * */
    public void addPowerup()
    {
        if (this.player != null)
        {
            this.player.GetComponent<PlayerPowerupController>().addPowerup(type);
        }
    }

    /**
     * removes the powerup from the player
     * */
    public void removePowerup()
    {
        if (this.player != null)
        {
            this.player.GetComponent<PlayerPowerupController>().removePowerup(type);
        }
    }   
}

/**
 * shoots 3 shots instead of 1
 * */
public class PowerupTrippleShot : Powerup
{
    /**
     * constructor
     * */
    public PowerupTrippleShot(GameObject spawnPlatform, GameObject powerupObject)
        : base(powerupObject, spawnPlatform, 5, 5, PowerupType.TRIPPLE_SHOT, 2f)
    {
        effectDescription = "Shoots 3 shots instead of 1, and each arrow forms a 15 degree angle around your original shot";
    }
}

/**
 * removes the wind up time for all shots allowing you to spam
 * */
public class PowerupRapidFire : Powerup
{
    /**
     * constructor
     * */
    public PowerupRapidFire(GameObject spawnPlatform, GameObject powerupObject)
        : base(powerupObject, spawnPlatform, 5, 5, PowerupType.RAPID_FIRE, 2f)
    {
        effectDescription = "Decreases the amount of time required to reach max charge to 0";
    }
}

/**
 * handles everything about powerups
 * */
public class SpawnPowerup : MonoBehaviour
{

    /**
     * takes the spawn rate and determines how much time will be
     * needed for each powerup to spawn
     * */
    float timeForSpawn;

    /**
     * how much time will be needed for the next powerup to spawn
     * */
    float currentTime;
    

    /**
     * a list of the powerups that are currently registered
     * */
    private List<Powerup> currentPowerups;

    /**
     * how many powerups should spawn in a given amount of time
     * */
    [SerializeField] public float spawnRate;

    /**
     * list of the gameObjects for each powerup
     * in the order specified
     * */
    [SerializeField] private GameObject[] powerupObjects;

    /**
     * all the platforms where a powerup could potentially spawn
     * */
    private GameObject[] spawnPlatforms;

    /**
     * initalizes the lists
     * */
    void Awake()
    {
        currentPowerups = new List<Powerup>();
    }

    /**
     * sets variables up
     * */
	void Start () {
        spawnPlatforms = GameObject.FindGameObjectsWithTag("Platform");
        timeForSpawn = 1 / spawnRate;
        currentTime = 0;
	}
	
    /**
     * updates the spawner class and creates a new powerup if needed
     * */
	void Update () {
        currentTime += Time.deltaTime;
        
        //in this case, we need to spawn a powerup
        if (currentTime >= timeForSpawn)
        {
            PowerupType rand = (PowerupType)Random.Range(0, (float)PowerupType.POWERUP_COUNT);
            Powerup powerup = null;

            GameObject startingPlatform = spawnPlatforms[Random.Range(0, spawnPlatforms.Length)];

            //spawn the powerup randomly
            switch (rand) {
                case PowerupType.TRIPPLE_SHOT:
                    powerup = new PowerupTrippleShot(startingPlatform, powerupObjects[0]);
                    break;
                case PowerupType.RAPID_FIRE:
                    powerup = new PowerupRapidFire(startingPlatform, powerupObjects[1]);
                    break;
                default:
                    powerup = null;
                    break;
            }

            currentTime = 0;
            spawnPowerup(powerup);
        }

        //go through each powerup and update it to see if it needs to be removed or not
        //we also want to test if a player is in contact with it
        //if so, add it to the player
        for (int i = currentPowerups.Count-1; i >= 0; i--)
        {
            currentPowerups[i].addPowerup();

            //add the powerup to the player each frame
            //this is because when one is removed, it takes it off the player, but it is very possible that they
            //player picked up multiple powerups
            //this creates a refresh powerup effect
            List<GameObject> collisions = new List<GameObject>();
            if(currentPowerups[i].isPickedUp())
                collisions = currentPowerups[i].getCollisions();

            if (collisions.Count > 0)
            {
                currentPowerups[i].pickUp(collisions[0]);
            }
        }

        for(int i = currentPowerups.Count - 1; i >= 0; i--)
            if (!currentPowerups[i].update(Time.deltaTime))
            {
                removePowerup(i);
            }
	}

    /**
     * spawns a powerup on a random platform
     * */
    public void spawnPowerup(Powerup powerup)
    {
        if (powerup != null)
        {
            currentPowerups.Add(powerup);
        }
    }

    /**
     * removes a powerup that has timed out
     * */
    public void removePowerup(int powerup) {
        currentPowerups[powerup].removePowerup();
        currentPowerups[powerup].destroy();
        currentPowerups.RemoveAt(powerup);
        //remove the power up from the player
    }
    
}


