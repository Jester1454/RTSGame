namespace PrototypeScripts
{
    public class MainConfig
    {
        public UnitPrototype PlayerUnit;
        public UnitPrototype EnemyUnit;
        public BasePrototype PlayerBase;
        public BasePrototype EnemyBase;
        public BasePrototype NeutralBase;
        public TowerPrototype PlayerTower;
        public TowerPrototype EnemyTower;
        
        public MainConfig()
        {
            PlayerUnit = new UnitPrototype();   
            EnemyUnit = new UnitPrototype();   
            EnemyBase = new BasePrototype();   
            PlayerBase = new BasePrototype();   
            NeutralBase = new BasePrototype();   
            PlayerTower = new TowerPrototype();   
            EnemyTower = new TowerPrototype();   
        }
    }
}