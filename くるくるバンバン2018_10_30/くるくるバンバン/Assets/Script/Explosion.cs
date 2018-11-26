//----------------------------------------------------------
//　作成日　2018/10/30
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　爆発
//----------------------------------------------------------
//　作成時　2018/10/30　爆発のアニメーション
//----------------------------------------------------------
using UnityEngine;

public class Explosion : MonoBehaviour
{
	public SpriteRenderer renderer;
	public Sprite[] sprites;

	public int index;
	public float time;
	public float maxTime;

	// Use this for initialization
	void Start ()
	{
		renderer = GetComponent<SpriteRenderer>();
		index = 0;
		time = 0.0f;
		maxTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		time += Time.deltaTime;
		if(time > maxTime)
		{
			if(index == sprites.Length - 1)
			{
				Destroy(gameObject);
			}
			else
			{
				index++;
			}

			renderer.sprite = sprites[index];
		}
	}
}
