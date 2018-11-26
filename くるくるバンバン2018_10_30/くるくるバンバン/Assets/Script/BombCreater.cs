//----------------------------------------------------------
//　作成日　2018/10/14
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　爆弾発生装置object関連
//----------------------------------------------------------
//　作成時　2018/10/09　爆弾生成
//　　追加　2018/10/21　GameManagement に作った爆弾の
//　　　　　　　　　　　　ObjectBase の情報を渡すようにした
//　　追加　2018/10/28　フィーバー状態の追加
//----------------------------------------------------------
using UnityEngine;

public class BombCreater : MonoBehaviour
{
    public ObjectBase ob;
    public GameObject bombPrefab;
    public GameObject bombEntity;       //　自分の作った爆弾
    public Cannon theOtherCannon;       //　相方の大砲の cannon 情報
    public float time;                  //　時間
    public float fCreateTime;           //　フィーバーの爆弾生成インターバル
    public int myNum;

    void Start ()
    {
        ob = GetComponent<ObjectBase>();
        bombEntity = null;
        switch (myNum)
        {
            case 0:
                time = 0.5f;
                break;
            case 1:
                time = 1.0f;
                break;
            case 2:
                time = 1.5f;
                break;
        }
        time = 1.5f;
        fCreateTime = 1.5f;
	}

    void Update()
    {
        //　フィーバーじゃないとき
        if (!GameManagement.manager.feverScript.fever)
        {
            //　修理中じゃないとき
            if (!theOtherCannon.repairFlag)
            {
                //　自分が作った爆弾がないとき
                if (bombEntity == null)
                {
                    for (int i = 0; i < GameManagement.bCNum; i++)
                    {
                        if (GameManagement.manager.c_Bomb[i] == null)
                        {
                            bombEntity = Instantiate(bombPrefab, transform.position, Quaternion.identity);
                            Bomb b = bombEntity.GetComponent<Bomb>();
                            GameManagement.manager.c_Bomb[i] = b.SetObejectBase();
                            FC.SettingOrderInLayer(bombEntity, GameMap.Order.orderBomb);
                            break;
                        }
                    }
                }
            }
        }
        else if(GameManagement.manager.feverScript.fever)
        {
            time += Time.deltaTime;
            if (time > fCreateTime)
            {
                //　ヌルが出るたびに生成
                for (int i = 0; i < GameManagement.bNum; i++)
                {
                    if (GameManagement.manager.c_Bomb[i] != null)
                    {
                        continue;
                    }
                    else if (GameManagement.manager.c_Bomb[i] == null)
                    {
                        GameObject obj = Instantiate(bombPrefab, transform.position, Quaternion.identity);
                        Bomb b = obj.GetComponent<Bomb>();
                        GameManagement.manager.c_Bomb[i] = b.SetObejectBase();
                        FC.SettingOrderInLayer(obj, GameMap.Order.orderBomb);
                        break;
                    }
                }
                time = 0.0f;
            }
        }
    }
}
