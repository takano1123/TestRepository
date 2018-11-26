//----------------------------------------------------------
//　作成日　2018/10/13
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　時間関連
//----------------------------------------------------------
//　作成時　2018/10/09　時計のアニメーション
//　　追加　2018/10/23　制限時間の実装と、画面切り替え
//----------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockManagement : MonoBehaviour
{
    //　時計の表示順（土台下、赤中、緑前）
    private const int clockBackOrder = (int)GameMap.Order.orderClock;
    private const int redNeedleOrder = clockBackOrder + 1;
    private const int greenNeedleOrder = redNeedleOrder + 1;

    public GameObject redNeedle;			//　赤針
    public GameObject greenNeedle;		//　緑針

    private Transform redAngle;			//　赤の角度
    private Transform greenAngle;       //　緑の角度
	public Quaternion initialAngle;		//　初期角度

	public float maxSeconds;				//　最大時間
    public float angle;							//　1秒ごとに動く赤針の角度
    public float time;							//　時間を数える変数
	public float totalTime;					//　合計時間

    public float fTimeMax;                  //　フィーバーの終了時間
    public float fTotalTime;                //　フィーバーの合計時間
    public float fAngle;                    //　フィーバーの1秒ごとの緑針の角度

    void Start ()
    {
        redAngle = NeedleCreate(redNeedle, redNeedleOrder,initialAngle);
        greenAngle = NeedleCreate(greenNeedle, greenNeedleOrder,initialAngle);
        angle = (360.0f / maxSeconds)　*　-1.0f;
        fAngle = (360.0f / fTimeMax) * -1.0f;
        time = 0.0f;
        totalTime = 0.0f;
        fTotalTime = 0.0f;
    }
	
	void Update ()
    {
        time += Time.deltaTime;
        //　フィーバーでないとき
        if(!GameManagement.manager.feverScript.fever)
        {
            if (time > 1.0f)
            {
                totalTime += time;
                redAngle.Rotate(0.0f, 0.0f, angle);

                time = 0.0f;

                //　制限時間を超えたとき
                if (totalTime > maxSeconds)
                {
                    //　クリアシーンに移動
                    SceneManager.LoadScene((int)GameMap.SceneName.ClearScene);
                }
            }
        }
        //　フィーバーのとき
        else if(GameManagement.manager.feverScript.fever)
        {
            if(time > 1.0f)
            {
                fTotalTime += time;
                greenAngle.Rotate(0.0f, 0.0f, fAngle);

                time = 0.0f;

                if(fTotalTime > fTimeMax)
                {
                    //　フィーバーの終了
                    GameManagement.manager.feverScript.finish();
                    fTotalTime = 0.0f;
                    greenAngle.rotation = Quaternion.identity;
                }
            }
        }
    }

    //　自分を親にする設定関数
    void SetParent(GameObject obj)
    {
        obj.transform.parent = transform;
    }

    //　針の生成関数
    Transform NeedleCreate(GameObject obj,int order,Quaternion angle)
    {
        GameObject needle = Instantiate(obj, transform.position, angle);
        FC.SettingOrderInLayer(needle, order);

        return needle.transform;
    }
}
