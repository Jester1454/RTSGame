using System.Collections;
using System.Collections.Generic;
using ObjectBehavior;
using UnityEngine;

public class AIManager : MonoBehaviour {

    public static AIManager instance;
    public List<AI> AIList;
	public int countAI;
    [HideInInspector]
    public List<Transform> Targets;

    private List<Base> Bases;

	[SerializeField]
	private float RateChangeTarget;

	public delegate void SwitchTarget();
    public event SwitchTarget changeTarget = delegate { };

	void Awake()
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
        
        Bases = new List<Base>();
        
        countAI = Bases.Count;

        Targets = new List<Transform>();

        AIList = new List<AI>();
        for (int i = 0; i < countAI;i++)
        {
            AI ai = new AI(Bases[i]);
            AIList.Add(ai);
        }
        UpdateTargets();
        StartCoroutine(ChangeTarget());
	}


    public void BaseCapture()
    {
        int countCaptureBase = 0;
        int countNeutralBase = 0;
        
        for (int i = 0; i < Bases.Count; i++)
        {
            if (Bases[i].side == Faction.Player)
                countCaptureBase++;
            if (Bases[i].side == Faction.Neutral)
                countNeutralBase++;
        }
        if (countNeutralBase == 0)
        {
            if (countCaptureBase == Bases.Count)
                GameManager.instance.GameWin(); // Win
            if(countCaptureBase == 0)
                GameManager.instance.GameOver(); //Lose 
        }
        
        UpdateTargets();
    }


    private void UpdateTargets()
    {
        Targets.Clear();
//        Targets.Add(PlayerBase);
//        Targets.Add(PlayerTarget);
        foreach(AI ai in AIList)
        {
            if (!ai.BaseIsDead)
            {
                Targets.Add(ai.Base.ObjectTransform);
                Targets.Add(ai.CurrentTarget);
            }                
        }
    }

    public Transform GetAITarget(Faction side)
    {
        if (side == Faction.Player)
            return null;
        int AINumber=0;
        for (int i = 0; i < countAI; i++)
        {
            if (side == AIList[i].Base.side)
            {
                AINumber = i;
                break;
            }
        }

        return AIList[AINumber].CurrentTarget;
    }
    
    private int RandomTarget(AI ai)
    {
        int target = 0;

        do
        {
            target = Random.Range(0, Targets.Count);
        }
        while (ai.Base.ObjectTransform.position == Targets[target].position);

        return target;
    }

    public void AddBase(GameObject _base)
    {
        Bases.Add(_base.GetComponent<Base>());
        
        AI ai = new AI(_base.GetComponent<Base>());
        AIList.Add(ai);
        
        UpdateTargets();
    }

    IEnumerator ChangeTarget() 
    {
        while (true)
        {
            foreach (AI ai in AIList)
            {
                ai.CurrentTarget = Targets[RandomTarget(ai)];
                //Debug.Log("я выбрал таргет по имени "+ ai.CurrentTarget.gameObject.name);
                changeTarget();
            }
            yield return new WaitForSeconds(RateChangeTarget);
            UpdateTargets();
        }
    }
}
