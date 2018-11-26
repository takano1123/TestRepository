//----------------------------------------------------------
//　作成日　2018/10/09
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　レール関連
//----------------------------------------------------------
//　作成時　2018/10/09　レールの生成とアニメーション
//　　追加　2018/10/09　アニメーションを ObjectBase に任せる
//----------------------------------------------------------
using UnityEngine;

public class LaneManagement : MonoBehaviour
{
    private const int laneNum = 7;          //　生成レーンの数
    public SpriteRenderer[] renderers;      //　レーンのレンダー
    public Sprite[] sprites;                //　スプライト
    public GameObject lanesPrefab;          //　レーンのプレハブ　

    public float animeSpeed;        //　アニメーションのスピード
    public float time;              //　今の時間
    public int nowPoint;            //　今アニメーションしているレーンの番号
    private int black;              //　黒矢印
    private int red;                //　赤矢印

    void Start ()
    {
        renderers = new SpriteRenderer[laneNum];
        time = 0.0f;
        nowPoint = 0;
        black = 0;
        red = 1;
        CreateChild();
    }
	
	void Update ()
    {
        //　アニメスピードを超えたとき
        if(animeSpeed < time)
        {
            //　前回のレーン番号を保存
            int before = nowPoint;
            //　今指してるレーンの番号がレーンの個数を超えないとき
            if (nowPoint < (laneNum - 1))
            {
                nowPoint++;
            }
            else
            {
                nowPoint = 0;
            }

            renderers[before].sprite = sprites[black];
            renderers[nowPoint].sprite = sprites[red];

            time = 0.0f;
        }

        time++;
	}

    //　レーン子供生成
    void CreateChild()
    {
        Vector2 vector2 = transform.position;
        for (int i = 0; i < laneNum; i++)
        {
            GameObject obj = null;
            obj = Instantiate(lanesPrefab, vector2, Quaternion.identity);
            SetParent(obj);
            renderers[i] = SetRenderer(obj);
            FC.SettingOrderInLayer(obj, GameMap.Order.orderLane);
            vector2.y -= GameMap.mapChipSize;
        }
    }

    //　SpriteRendererを返す関数
    SpriteRenderer SetRenderer(GameObject obj)
    {
        return obj.GetComponent<SpriteRenderer>();
    }

    //　自分を親にする設定関数
    void SetParent(GameObject obj)
    {
        obj.transform.parent = transform;
    }
}
