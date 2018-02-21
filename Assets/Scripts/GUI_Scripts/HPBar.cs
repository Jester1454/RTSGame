using UnityEngine;
using ObjectBehavior;

public class HPBar : MonoBehaviour {

    public UISlider slider;
    public UILabel hpText;
    float maxHP;

	void Start () 
    {
        slider.gameObject.SetActive(true);
        hpText.gameObject.SetActive(true);
        RTSObject rtsObject = GetComponent<RTSObject>();
        maxHP = rtsObject.maxHP;
        OnHPCahnged(maxHP);
        rtsObject.enemyAttack += OnHPCahnged;
	}

    public void OnHPCahnged(float HP)
    {
        slider.value = HP/maxHP;
        hpText.text = HP.ToString();
    }

    private void OnDisable()
    {
        if (slider != null && hpText != null)
        {
            slider.gameObject.SetActive(false);
            hpText.gameObject.SetActive(false);
        }
    }
}
