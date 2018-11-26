//----------------------------------------------------------
//　作成日　2018/10/08
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　爆弾関連
//----------------------------------------------------------
//　作成時　2018/10/08　爆弾のパネルのアニメーション
//　　追加　2018/10/18　Sprite 変更を ObjectBase にまかせる
//　　追加　2018/10/21　砲弾に当たったときのパネルの表裏変更（XOR演算）関数
//                    を追加
//　　追加　2018/10/21　すべてのパネルが表の時、自分削除
//----------------------------------------------------------
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public ObjectBase ob;
    private const int animeNum = 8;             //　アニメーション用の枚数
    public SpriteRenderer[] panelRenderer;      //　パネルのレンダー
    public int panelState;                      //　パネルの状態（2進数で管理）

    public Sprite[] panel_B;        //　Bのパネルのアニメ素材
    public Sprite[] panel_A;        //　Aのパネルのアニメ素材
    public Sprite[] panel_N;        //　Nのパネルのアニメ素材
    public Sprite[] panel_G;        //　Gのパネルのアニメ素材
    private int index_B;            //　Bのパネルのインデックス
    private int index_A;            //　Aのパネルのインデックス
    private int index_N;            //　Nのパネルのインデックス
    private int index_G;            //　Gのパネルのインデックス

    public int animeFinished;       //　アニメが終わったフラグ
    public float animeSpeed;        //　アニメのスピード
    public float time;              //　アニメ管理の時間
    public float moveSpeed;         //  移動速度

    private　void Start()
    {
        panelRenderer = new SpriteRenderer[GameManagement.panelNum];
        time = 0.0f;
        panelState = Random.Range(0, 15);
        //　各パネルインデックスの初期化
        for (int i = 0; i < animeNum; i++)
        {
            //　パネルごとの裏表の確認
            int temp = panelState & (1 << i);
            //　裏の時
            if(temp == 0)
            {
                //　パネルごとに裏にする
                switch(i)
                {
                    //　Bを裏化
                    case 3:
                        index_B = 0;
                        break;
                    //　Aを裏化
                    case 2:
                        index_A = 0;
                        break;
                    //　Nを裏化
                    case 1:
                        index_N = 0;
                        break;
                    //　Gを裏化
                    case 0:
                        index_G = 0;
                        break;
                }
            }
            //　それ以外のとき表
            else
            {
                //　パネルごとに表にする
                switch (i)
                {
                    //　Bを表化
                    case 3:
                        index_B = 1;
                        break;
                    //　Aを表化
                    case 2:
                        index_A = 1;
                        break;
                    //　Nを表化
                    case 1:
                        index_N = 1;
                        break;
                    //　Gを表化
                    case 0:
                        index_G = 1;
                        break;
                }
            }
        }
        //　各パネルの初期化
        for(int i = 0; i < GameManagement.panelNum; i++)
        {
            panelRenderer[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
            GameObject obj = panelRenderer[i].gameObject;
            FC.SettingOrderInLayer(obj, (int)GameMap.Order.orderBomb + 1);
        }
        animeFinished = 0;
    }

    private void Update ()
    {
        //　アニメスピードがタイムを超えたとき、アニメが８枚更新されてないとき
		if(animeSpeed < time　&& animeFinished < animeNum)
        {
            panelRenderer[0].sprite = panel_B[index_B];
            panelRenderer[1].sprite = panel_A[index_A];
            panelRenderer[2].sprite = panel_N[index_N];
            panelRenderer[3].sprite = panel_G[index_G];

            for (int i = 0; i < GameManagement.panelNum; i++)
            {
                //　ビットマップ作製
                int bitmap = 1 << i;
                //　
                int displayJudgment = panelState & bitmap;
                //　裏になったとき
                if (displayJudgment == 0)
                {
                    //　パネルごとに裏にする
                    switch (i)
                    {
                        //　Bを裏化
                        case 3:
                            if (index_B < 7)
                            {
                                index_B++;
                            }
                            break;
                        //　Aを裏化
                        case 2:
                            if (index_A < 7)
                            {
                                index_A++;
                            }
                            break;
                        //　Nを裏化
                        case 1:
                            if (index_N < 7)
                            {
                                index_N++;
                            }
                            break;
                        //　Gを裏化
                        case 0:
                            if (index_G < 7)
                            {
                                index_G++;
                            }
                            break;
                    }
                }
                //　それ以外のとき表
                else
                {
                    //　パネルごとに表にする
                    switch (i)
                    {
                        //　Bを表化
                        case 3:
                            if (index_B > 0)
                            {
                                index_B--;
                            }
                            break;
                        //　Aを表化
                        case 2:
                            if (index_A > 0)
                            {
                                index_A--;
                            }
                            break;
                        //　Nを表化
                        case 1:
                            if (index_N > 0)
                            {
                                index_N--;
                            }
                            break;
                        //　Gを表化
                        case 0:
                            if (index_G > 0)
                            {
                                index_G--;
                            }
                            break;
                    }
                }
            }
            time = 0;
            animeFinished++;
        }

        //　アニメ終了時、すべてのパネルが表の時
        if (animeFinished == animeNum)
        {
            //　表のとき
            if (panelState == GameManagement.surface)
            {
                //　スコアの加算
                GameManagement.manager.plusScore();
                //　自分削除
                ob.ObjectDestroy();
            }
        }
        time++;

        //　移動
        Vector3 vector3 = transform.position;
        vector3.y -= moveSpeed;
        transform.position = vector3;
    }

    //　生成時の ObjectBase 取得用
    public ObjectBase SetObejectBase()
    {
        ob = GetComponent<ObjectBase>();
        return ob;
    }

    //　パネルの表裏変更
    public void XOROperation(int ammunitionNum)
    {
        //　フィーバーでないとき
        if (!GameManagement.manager.feverScript.fever)
        {
            //　XOR演算
            panelState = panelState ^ ammunitionNum;
            animeFinished = 0;
        }
        //　フィーバーのとき
        else if (GameManagement.manager.feverScript.fever)
        {
            //　OR演算
            panelState = panelState | ammunitionNum;
            animeFinished = 0;
        }
    }
}
