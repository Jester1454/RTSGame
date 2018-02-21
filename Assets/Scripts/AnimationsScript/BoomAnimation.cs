using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomAnimation : MonoBehaviour 
{

    private SpriteRenderer mesh;
    public Sprite[] BoomMaterial;
    private Sprite OriginMaterial;

    private void Start()
    {
        mesh = GetComponent<SpriteRenderer>();
        OriginMaterial = mesh.sprite;
    }

    public void  Boom()
    {
        StartCoroutine(StartBoom());
    }

    IEnumerator StartBoom()
    {
        for (int i = 0; i < BoomMaterial.Length; i++)
        {
            mesh.sprite = BoomMaterial[i];
            yield return new WaitForSeconds(0.2f);
        }
        this.Recycle();

        mesh.sprite = OriginMaterial;
    }

}
