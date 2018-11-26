//----------------------------------------------------------
//　作成日　2018/10/23
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　マウスのポインタ
//----------------------------------------------------------
//　作成時　2018/10/23　マウスの追従と、アニメーション
//　　追加　2018/10/23　マウスのクリックによるシーン切り替えの追加
//　　修正　2018/10/24　シーンの判定を GameMap の情報を参照して行う
//----------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pointa : MonoBehaviour
{
	ObjectBase ob;                                              //　自分の ObjectBase 情報
	public Sprite[] sprites;                                        //　見た目
	public int index;												//　アニメーション用の見た目の指定用
	private Vector3 position;									//　位置座標
	private Vector3 screenToWorldPointPosition;     //　スクリーン座標をワールド座標に変換した位置座標

    public float animeSpeed;		//　アニメーションのスピード
	public float time;					//　時間計測用

	void Start ()
	{
		ob = GetComponent<ObjectBase>();
		time = 0.0f;
	}

	void Update ()
	{
		// Vector3でマウス位置座標を取得する
		position = Input.mousePosition;
		// Z軸修正
		position.z = 10f;
		// マウス位置座標をスクリーン座標からワールド座標に変換する
		screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
		// ワールド座標に変換されたマウス座標を代入
		gameObject.transform.position = screenToWorldPointPosition;

		time += Time.deltaTime;

		//　時間がアニメスピードを超えたとき
		if(time > animeSpeed)
		{
			//　スプライトを書き換える
			index++;
			if(index == 2)
			{
				index = 0;
			}

			ob.renderer.sprite = sprites[index];
			time = 0.0f;
		}

		//　マウスの左クリックがあるとき
		if (Input.GetMouseButtonDown(0))
		{
			//　クリアシーンあるいはゲームオーバーシーンのとき
			if (GameMap.currentScene == GameMap.SceneName.ClearScene
				|| GameMap.currentScene == GameMap.SceneName.GameOver)
			{
				//　タイトルに移動
				SceneManager.LoadScene((int)GameMap.SceneName.Title);
			}
			//　タイトルのとき
			else if(GameMap.currentScene == GameMap.SceneName.Title)
			{
				//　ゲームシーンに移動
				SceneManager.LoadScene((int)GameMap.SceneName.GameScene);
			}
		}
	}
}
