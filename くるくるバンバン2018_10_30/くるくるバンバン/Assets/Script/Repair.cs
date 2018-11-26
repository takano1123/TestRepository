//----------------------------------------------------------
//　作成日　2018/10/25
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　修理中の表示
//----------------------------------------------------------
//　作成時　2018/10/25　REPAIR の表示とアニメーション
//----------------------------------------------------------
using UnityEngine;

public class Repair : MonoBehaviour
{
	public const int repairNum = 8;

	public new SpriteRenderer renderer;
	public Sprite[] sprite;
	public float animeSpeed;
	public float time;
	public int index;

	void Start ()
	{
		renderer = GetComponent<SpriteRenderer>();
		time = 0.0f;
		index = 0;
	}
	
	void Update ()
	{
		time += Time.deltaTime;
		if(time > animeSpeed)
		{
			renderer.sprite = sprite[index];

			if (index == repairNum-1)
			{
				index = 0;
			}
			else
			{
				index++;
			}

			time = 0.0f;
		}
	}
}