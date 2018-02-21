using System.Collections;
using System.IO;
using ObjectBehavior;
using PrototypeScripts;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
	public static SaveDataManager instance;

	public bool SaveInPrefab;
	#region URL
	public static string MainConfigURL 
	{
		get { return "https://jester1454.github.io/SeaBattleConfig.JSON"; }
	}
	#endregion
	
	#region Pathes	
	public static string SavedGamesPath 
	{
		get { return Application.persistentDataPath + Path.DirectorySeparatorChar + "Saves"; }
	}
	
	public static string PlayerUnitPath 
	{
		get { return "Prefabs/Game Prefabs/Units/PlayerUnit"; }
	}
	
	public static string EnemyUnitPath 
	{
		get { return "Prefabs/Game Prefabs/Units/EnemyUnit"; }
	}
	
	public static string PlayerBasePath 
	{
		get { return "Prefabs/Game Prefabs/Bases/PlayerBase"; }
	}
	
	public static string EnemyBasePath 
	{
		get { return "Prefabs/Game Prefabs/Bases/EnemyBase"; }
	}
		
	public static string NeutralBasePath 
	{
		get { return "Prefabs/Game Prefabs/Bases/NeutralBase"; }
	}
	
	public static string PlayerTowerPath 
	{
		get { return "Prefabs/Game Prefabs/Units/Tower/PlayerTower"; }
	}
	
	public static string EnemyTowerPath 
	{
		get { return "Prefabs/Game Prefabs/Units/Tower/TowerEnemy"; }
	}
	
	public static string NameDataSaveFile 
	{
		get { return "Game.json"; }
	}
    #endregion

    public GameObject PlayerUnit;
	public GameObject EnemyUnit;
	public GameObject PlayerBase;
	public GameObject EnemyBase;
	public GameObject NeutralBase;
	public GameObject PlayerTower;
	public GameObject EnemyTower;

	private MainConfig Config;

    public bool IsDowloand
    {
        get
        {
            return isDowloand;
        }

        set
        {
            isDowloand = value;
        }
    }

    private bool isDowloand=false;
	
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		if (instance != this)
		{
			Destroy(gameObject);
		}
        SetConfigFromURL(MainConfigURL);
       // LoadFromReferences();
    }

	public void SetConfigFromURL(string url)
	{
		StartCoroutine(DowloadConfig(url));
	}	

	private void LoadFromReferences()
	{
		Config = new MainConfig();
		Config.PlayerUnit.LoadFromObject(PlayerUnit);
		Config.EnemyUnit.LoadFromObject(EnemyUnit);
		Config.PlayerBase.LoadFromObject(PlayerBase);
		Config.EnemyBase.LoadFromObject(EnemyBase);
		Config.NeutralBase.LoadFromObject(NeutralBase);
		Config.PlayerTower.LoadFromObject(PlayerTower);
		Config.EnemyTower.LoadFromObject(EnemyTower);
		SaveObject(Config, SavedGamesPath + NameDataSaveFile);
        Debug.Log(SavedGamesPath + NameDataSaveFile);
	}
	
	private void LoadFromResources()
	{
		MainConfig config = new MainConfig();
		config.PlayerUnit.LoadFromObject(Resources.Load(PlayerUnitPath, typeof(GameObject)) as GameObject);
		config.EnemyUnit.LoadFromObject(Resources.Load(EnemyUnitPath, typeof(GameObject)) as GameObject);
		config.PlayerBase.LoadFromObject(Resources.Load(PlayerBasePath, typeof(GameObject)) as GameObject);
		config.EnemyBase.LoadFromObject(Resources.Load(EnemyBasePath, typeof(GameObject)) as GameObject);
		config.NeutralBase.LoadFromObject(Resources.Load(NeutralBasePath, typeof(GameObject)) as GameObject);
		config.PlayerTower.LoadFromObject(Resources.Load(PlayerTowerPath, typeof(GameObject)) as GameObject);
		config.EnemyTower.LoadFromObject(Resources.Load(EnemyTowerPath, typeof(GameObject)) as GameObject);
		SaveObject(config, SavedGamesPath + NameDataSaveFile);
	}
	
	private void SaveInPrefabs()
	{
		MainConfig config = LoadSaveData<MainConfig>(SavedGamesPath + NameDataSaveFile);

		PlayerUnit.GetComponent<Unit>().maxHP = config.PlayerUnit.maxHP;
		Attack attack = PlayerUnit.GetComponent<Attack>();
		attack.damage = config.PlayerUnit.Damage;
		attack.damageRate = config.PlayerUnit.DamageRate;
		attack.attackRadius = config.PlayerUnit.AttackRadius;
		PlayerUnit.GetComponent<Movement>().maxSpeed = config.PlayerUnit.maxSpeed;

		EnemyUnit.GetComponent<Unit>().maxHP = config.EnemyUnit.maxHP;
		attack = EnemyUnit.GetComponent<Attack>();
		attack.damage = config.EnemyUnit.Damage;
		attack.damageRate = config.EnemyUnit.DamageRate;
		attack.attackRadius = config.EnemyUnit.AttackRadius;
		EnemyUnit.GetComponent<Movement>().maxSpeed = config.EnemyUnit.maxSpeed;

		Base _base = PlayerBase.GetComponent<Base>();
		_base.maxHP = config.PlayerBase.maxHP;
		_base.spawnRate = config.PlayerBase.SpawnRate;
		_base.ProduceRate = config.PlayerBase.ProduceUnitRate;
		_base.MaxCountUnit = config.PlayerBase.MaxUnit;
        _base.CurrentCountUnit = config.PlayerBase.StartCountUnit;
        attack = PlayerBase.GetComponent<Attack>();
		attack.damage = config.PlayerBase.Damage;
		attack.damageRate = config.PlayerBase.DamageRate;
		attack.attackRadius = config.PlayerBase.AttackRadius;
		
		_base = NeutralBase.GetComponent<Base>();
		_base.maxHP = config.NeutralBase.maxHP;
		_base.spawnRate = config.NeutralBase.SpawnRate;
		_base.ProduceRate = config.NeutralBase.ProduceUnitRate;
		_base.MaxCountUnit = config.NeutralBase.MaxUnit;
        _base.CurrentCountUnit = config.NeutralBase.StartCountUnit;
        attack = NeutralBase.GetComponent<Attack>();
		attack.damage = config.NeutralBase.Damage;
		attack.damageRate = config.NeutralBase.DamageRate;
		attack.attackRadius = config.NeutralBase.AttackRadius;
		
		_base = EnemyBase.GetComponent<Base>();
		_base.maxHP = config.EnemyBase.maxHP;
		_base.spawnRate = config.EnemyBase.SpawnRate;
		_base.ProduceRate = config.EnemyBase.ProduceUnitRate;
		_base.MaxCountUnit = config.EnemyBase.MaxUnit;
        _base.CurrentCountUnit = config.EnemyBase.StartCountUnit;
        attack = EnemyBase.GetComponent<Attack>();
		attack.damage = config.EnemyBase.Damage;
		attack.damageRate = config.EnemyBase.DamageRate;
		attack.attackRadius = config.EnemyBase.AttackRadius;
		

		DefenseTower tower = PlayerTower.GetComponent<DefenseTower>();
		tower.maxHP = config.PlayerTower.maxHP;
		tower.rotateSpeed = config.PlayerTower.RotateSpeed;
		attack = PlayerTower.GetComponent<Attack>();
		attack.damage  = config.PlayerTower.Damage;
		attack.damageRate= config.PlayerTower.DamageRate;
		attack.attackRadius = config.PlayerTower.AttackRadius;
		
		tower = EnemyTower.GetComponent<DefenseTower>();
		tower.maxHP = config.EnemyTower.maxHP;
		tower.rotateSpeed = config.EnemyTower.RotateSpeed;
		attack = EnemyTower.GetComponent<Attack>();
		attack.damage  = config.EnemyTower.Damage;
		attack.damageRate= config.EnemyTower.DamageRate;
		attack.attackRadius = config.EnemyTower.AttackRadius;
	}

	private void SetConfig()
	{
		Config  = LoadSaveData<MainConfig>(SavedGamesPath + NameDataSaveFile);
	}

	public BasePrototype GetBasePrototype(Faction side)
	{
		if (isDowloand)
		{
			if (side == Faction.Player)
				return Config.PlayerBase;
			if (side == Faction.Enemy1)
				return Config.EnemyBase;
			if (side == Faction.Neutral)
				return Config.NeutralBase;
		}
		return null;
	}

	public UnitPrototype GetUnitPrototype(Faction side)
	{
		if (isDowloand)
		{
			if (side == Faction.Player)
				return Config.PlayerUnit;
			if (side == Faction.Enemy1)
				return Config.EnemyUnit;
		}
		return null;
	}

	public TowerPrototype GetTowerPrototype(Faction side)
	{
		if (isDowloand)
		{
			if (side == Faction.Player)
				return Config.PlayerTower;
			if (side == Faction.Enemy1)
				return Config.EnemyTower;
		}
		
		return null;
	}
	
	private IEnumerator DowloadConfig(string url)
	{		
		WWW www = new WWW(url);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			File.WriteAllText(SavedGamesPath + NameDataSaveFile, www.text);
			SetConfig();
			isDowloand = true;
            if (SaveInPrefab)
                SaveInPrefabs();
        }
		else
		{
			Debug.LogError(www.error);
		}     
	}
	
	private T LoadSaveData<T>(string filePath)
	{
		if (File.Exists(filePath))
		{
			string dataAsJson = File.ReadAllText (filePath);
			return  JsonUtility.FromJson<T>(dataAsJson);
		}
		return default(T);
	}

	private void SaveObject<T>(T obj, string path)
	{
		File.WriteAllText(path, JsonUtility.ToJson (obj, true));	
	}
}
