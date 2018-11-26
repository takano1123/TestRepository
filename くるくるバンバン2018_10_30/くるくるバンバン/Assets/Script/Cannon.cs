//----------------------------------------------------------
//　作成日　2018/10/18
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　大砲関連
//----------------------------------------------------------
//　作成時　2018/10/18　自分の ObjectBase の取得
//　　追加　2018/10/25　爆弾とぶつかった後の修理時間の追加
//　　追加　2018/10/26　修理中の解除追加
//----------------------------------------------------------
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public ObjectBase ob;
	public bool repairFlag;             //　修理中のフラグ
	public GameObject repairPlefab;     //　修理中のプレハブ
	public GameObject repairObj;        //　修理中のオブジェクト
    public float time;                  //　時間
    public float repairTime;            //　修理中の時間

    void Start()
    {
        ob = GetComponent<ObjectBase>();
		repairFlag = false;
		repairObj = null;
        time = 0.0f;
    }
    void Update()
    {
        //　修理中のとき
		if(repairFlag)
		{
            time += Time.deltaTime;
            if(time > repairTime)
            {
                repairFlag = false;
                time = 0.0f;
                Destroy(repairObj);
            }
		}
    }

    //　修理状態化
    public void RepairConditioning()
    {
        repairObj = Instantiate(repairPlefab, transform.position, Quaternion.identity);
        FC.SettingOrderInLayer(repairObj, (int)GameMap.Order.orderCannon + 1);
        repairFlag = true;
    }
}