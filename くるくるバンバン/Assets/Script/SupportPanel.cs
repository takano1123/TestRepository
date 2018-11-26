//----------------------------------------------------------
//　作成日　2018/10/27
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　爆発順番の表示するオブジェクト関連
//----------------------------------------------------------
//　作成時　2018/10/27　指示順の表示
//　　追加　2018/10/27　自分の担当爆弾が壊れたかの判定追加
//----------------------------------------------------------
using UnityEngine;

public class SupportPanel : MonoBehaviour
{
    public ObjectBase ob;
    public BombCreater partnerCreater;              //　相手の爆弾クリエイター
    public Cannon partnerCannon;                    //　相手の大砲
    public bool destruction;                        //　破壊したかのフラグ
    public bool failureFlag;                        //　失敗したフラグ
    public FeverManagement.SupportNum myNum;        //　自分の番号
    public Sprite[] sprite;                         //　自分の見た目
    public bool repairCannon;                       //　大砲の修理フラグ

    void Start()
    {
        ob = GetComponent<ObjectBase>();
        destruction = false;
        failureFlag = false;
        partnerCreater = null;
        repairCannon = false;
    }

    void Update()
    {
        //　パートナーがいないとき
        if (partnerCreater == null)
        {
            //　爆弾クリエイターの情報取得
            partnerCreater = GameManagement.manager.CreatorAndSupportSet(ob.colliderSet);
            //　大砲の情報取得
            BombCreater b = partnerCreater.GetComponent<BombCreater>();
            partnerCannon = b.theOtherCannon;
        }
    }

    private void LateUpdate()
    {
        if (partnerCreater != null)
        {
            //　大砲が壊れていないとき
            if (!partnerCannon.repairFlag)
            {
                //　担当爆弾が壊れたとき
                if (partnerCreater.bombEntity == null)
                {
                    if (!destruction)
                    {
                        destruction = true;
                    }
                    else if (destruction)
                    {
                        failureFlag = true;
                    }
                }

                //　パートナーが壊れているとき
                if (destruction)
                {
                    ob.renderer.sprite = sprite[1];
                }
                else
                {
                    ob.renderer.sprite = sprite[0];
                }
            }
            //　大砲が壊れているとき
            else if (partnerCannon.repairFlag)
            {
                repairCannon = true;
            }
        }
    }

    public void FullReset()
    {
        destruction = false;
        failureFlag = false;
        repairCannon = false;
    }
}