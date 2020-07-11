using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
	Vector2Int pos;
	bool isDestroyed=false;
	public Action OnExplosion;
    public Color destroyedColor = Color.black;
    SpriteRenderer renderer;
	// Start is called before the first frame update
	void Start()
    {
		pos = TileManager.singleton.World2ImagePos(transform.position);
		TileManager.singleton.tiles[pos.x, pos.y].isHouse = true;
		GameManager.singleton.onNewTick += CheckDamage;
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public virtual void CheckDamage()
    {
		if (isDestroyed)
			return;
		int fireDamage = TileManager.singleton.tiles[pos.x, pos.y].fireValue;
		if (fireDamage > 190)
		{
			print("BUILDING DESTROYED");
			GetComponent<ParticleSystem>().Play();
			isDestroyed = true;
			if(OnExplosion!=null)
				OnExplosion();

            if (renderer)
                renderer.color = destroyedColor;


        }
    }

	
	
}
