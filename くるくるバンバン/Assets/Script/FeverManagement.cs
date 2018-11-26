//----------------------------------------------------------
//　作成日　2018/10/27
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　フィーバーの管理
//----------------------------------------------------------
//　作成時　2018/10/27　指示順の表示
//　　追加　2018/10/27　フィーバーフラグの追加
//----------------------------------------------------------
using UnityEngine;

public class FeverManagement : MonoBehaviour
{
    //　サポートパネルの番号
    public enum SupportNum
    {
        Num1,       //　1枚目
        Num2,       //　2枚目
        Num3,       //　3まいめ
    }

    public GameObject[] supportPanelPrefab 
        = new GameObject[GameManagement.supportNum];            //　サポートパネルのプレハブ
    public GameObject[] supportPanel 
        = new GameObject[GameManagement.supportNum];            //　オブジェクト
    public SupportPanel[] sPScript 
        = new SupportPanel[GameManagement.supportNum];          //　サポートパネルのスプライト

    public bool fever;      //　フィーバーのフラグ

    //　縦セット、横ポジション
    public int[,] vs = new int[,]
    {
        { 0, 1, 2, },
        { 2, 0, 1, },
        { 1, 2, 0, },
        { 0, 2, 1, },
        { 1, 0, 2, },
        { 2, 1, 0, },
    };

    //　3枚分のポジション
    Vector3[] vector = new Vector3[3]
    {
        new Vector3(288.0f, 48.0f, 0.0f),
        new Vector3(576.0f, 48.0f, 0.0f),
        new Vector3(864.0f, 48.0f, 0.0f),
    };

    void Start()
    {
        int rand = Random.Range(0, 6);

        //　初期化
        for(int i = 0; i < GameManagement.supportNum; i++)
        {
            supportPanel[i] = Instantiate(supportPanelPrefab[i], vector[vs[rand, i]], Quaternion.identity);
            sPScript[i] = supportPanel[i].GetComponent<SupportPanel>();
            sPScript[i].myNum = (SupportNum)i;
            GameManagement.manager.c_Supportpanel[i] = supportPanel[i].GetComponent<ObjectBase>();
        }
        fever = false;
    }

    void Update()
    {
        // 　フィーバーでないとき
        if (!fever)
        {
            //　爆弾一つ目が破壊されているとき（成功）
            if (sPScript[(int)SupportNum.Num1].destruction)
            {
                //　爆弾二つ目が破壊されているとき（成功）
                if (sPScript[(int)SupportNum.Num2].destruction)
                {
                    //　爆弾三つ目が成功のとき（成功）
                    if (sPScript[(int)SupportNum.Num3].destruction)
                    {
                        FeverStart();
                    }
                    //　一つ目が二回壊されたとき（失敗）
                    else if (sPScript[(int)SupportNum.Num1].failureFlag)
                    {
						ReCreate();
					}
					//　二回連続で二つ目が壊されたとき（失敗）
					else if (sPScript[(int)SupportNum.Num2].failureFlag)
                    {
						ReCreate();
					}
				}
                //　二回連続で一つ目が壊されたとき（失敗）
                else if (sPScript[(int)SupportNum.Num1].failureFlag)
                {
					ReCreate();
				}
				//　爆弾三つ目が破壊されているとき（失敗）
				else if (sPScript[(int)SupportNum.Num3].destruction)
                {
					ReCreate();
				}
			}
            //　爆弾二つ目が破壊されているとき（失敗）
            else if (sPScript[(int)SupportNum.Num2].destruction)
            {
				ReCreate();
			}
			//　爆弾三つ目が破壊されているとき(失敗)
			else if (sPScript[(int)SupportNum.Num3].destruction)
            {
				ReCreate();
			}

			//　大砲が修理中になったとき（失敗）
			if (sPScript[(int)SupportNum.Num1].repairCannon ||
                sPScript[(int)SupportNum.Num2].repairCannon ||
                sPScript[(int)SupportNum.Num3].repairCannon)
            {
                //　すべてのフラグを下す
                sPScript[(int)SupportNum.Num1].FullReset();
                sPScript[(int)SupportNum.Num2].FullReset();
                sPScript[(int)SupportNum.Num3].FullReset();
            }
        }
    }

    //　フィーバーが終わったときの関数
    public void finish()
    {
        fever = false;
		ReCreate();
		OldDestructionDestroyed();
    }

    //　フィーバー開始の関数
    public void FeverStart()
    {
        fever = true;
        OldDestructionDestroyed();
    }

    //　古いもの破壊関数
    public void OldDestructionDestroyed()
    {

        //　爆弾の破壊
        foreach (ObjectBase obj in GameManagement.manager.c_Bomb)
        {
            if (obj != null)
            {
                obj.ObjectDestroy();
            }
        }

        //　パネルの破壊
        foreach (ObjectBase obj in GameManagement.manager.c_MassPanel)
        {
            if (obj != null)
            {
                obj.ObjectDestroy();
            }
        }

        //　砲弾の破壊
        foreach (ObjectBase obj in GameManagement.manager.c_Ammunition)
        {
            if (obj != null)
            {
                obj.ObjectDestroy();
            }
        }

        //　パネルの破壊
        foreach (ObjectBase obj in GameManagement.manager.c_MassPanel)
        {
            if (obj != null)
            {
                obj.ObjectDestroy();
            }
        }
    }

	//　サポートパネルの作り直し
	public void ReCreate()
	{
		//　サポートパネルの削除、再生成
		foreach (GameObject obj in supportPanel)
		{
			Destroy(obj);
		}
		int rand = Random.Range(0, 6);
		for (int i = 0; i < GameManagement.supportNum; i++)
		{
			supportPanel[i] = Instantiate(supportPanelPrefab[i], vector[vs[rand, i]], Quaternion.identity);
			sPScript[i] = supportPanel[i].GetComponent<SupportPanel>();
			sPScript[i].myNum = (SupportNum)i;
			GameManagement.manager.c_Supportpanel[i] = supportPanel[i].GetComponent<ObjectBase>();
		}
	}
}
