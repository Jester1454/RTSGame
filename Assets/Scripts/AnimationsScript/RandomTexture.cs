using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RandomTexture : MonoBehaviour {

	public Sprite[] Sprites;

	void Start () 
    {
		SpriteRenderer mesh = GetComponent<SpriteRenderer>();
        int i = Random.Range(0, Sprites.Length);
        mesh.sprite = Sprites[i];
	}
}
