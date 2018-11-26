//----------------------------------------------------------
//　作成日　2018/10/16
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　ゲーム全体の流れを一括管理
//----------------------------------------------------------
//　作成時　2018/10/16　各オブジェクトの ObjectBase の格納
//　　追加　2018/10/17　アーム用の「アームで持つ」「アームで放す」
//                     関数２つの実装
//　　追加　2018/10/18　大砲との当たり判定を実装
//　　　　　　　　　　　　砲弾の情報追加
//　　追加　2018/10/21　大砲と爆弾の当たり判定追加
//　　追加　2018/10/21　砲弾と爆弾の当たり判定追加
//　　追加　2018/10/21　爆弾が表の確認用の変数追加
//　　追加　2018/10/24　スコア書き換え用の関数追加
//　　追加　2018/10/26　大砲の修理中化の追加、修理中判定の追加
//　　追加　2018/10/27　フィーバーのセット化関数の追加
//----------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    public const int mCNum = 5;             //　マスクリエイターとパネルの数
    public const int bCNum = 3;             //　爆弾クリエイターの数
    public const int bNum = 99;              //　爆弾の数
    public const int cannonNum = 3;         //　大砲の数
    public const int lMNum = 3;             //　レーンクリエイターの数
    public const int amNum = 30;            //　砲弾の数
    public const int panelNum = 4;          //　パネルの数
    public const int surface = 15;          //　爆弾が表の確認用
    public const int supportNum = 3;        //　サポートパネルの数

    public ObjectBase c_Clock;
    public ObjectBase c_Arm;
    public ObjectBase[] c_MassCreater = new ObjectBase[mCNum];
    public ObjectBase[] c_MassPanel = new ObjectBase[mCNum];
    public ObjectBase[] c_BombCreater = new ObjectBase[bCNum];
    public ObjectBase[] c_Bomb = new ObjectBase[bNum];
    public ObjectBase[] c_Cannon = new ObjectBase[cannonNum];
    public ObjectBase[] c_LaneManagement = new ObjectBase[lMNum];
    public ObjectBase[] c_Ammunition = new ObjectBase[amNum];
    public ObjectBase[] c_Supportpanel = new ObjectBase[supportNum];

    public Cannon[] cannonsScript = new Cannon[cannonNum];
    public FeverManagement feverScript;

    public ScoreDisplay score;

    public static GameManagement manager;

    public void LateUpdate()
    {
        DeterminationOfCannonAndPanel();
        DeterminationOfCannonAndBomb();
        DeterminationOfBombsAndAmmunition();
        GameOverTransition();
    }

    //　アームで持つ
    public void HoldOnArm()
    {
        //　存在するすべてのマスパネルを総当たり
        foreach (ObjectBase panel in c_MassPanel)
        {
            //　もしアームとマスパネルが衝突していたら
            if (FC.Collider(c_Arm.colliderSet, panel.colliderSet))
            {
                //　マスパネルをアームの子供にする
                panel.transform.parent = c_Arm.transform;
                c_Arm.AcquisitionOfChildren();
                break;
            }
        }
    }
    //　アームで放す
    public void SetFree()
    {
        //　もしアームに子供がいたとき
        if (FC.ConfirmationOfChild(c_Arm.transform))
        {
            //　子供を開放する
            c_Arm.childSet[0].pos.parent = null;
            c_Arm.childSet[0] = new ObjectBase.ColliderSet();
        }
    }
    //　大砲とパネルの判定
    public void DeterminationOfCannonAndPanel()
    {
        //　すべてのパネルの確認
        foreach (ObjectBase panel in c_MassPanel)
        {
            if (panel != null)
            {
                //　パネルの親がアームでないとき
                if (panel.transform.parent != c_Arm.transform)
                {
                    //　すべての大砲の確認
                    foreach (ObjectBase cannon in c_Cannon)
                    {
                        //　大砲が使えるとき
                        if (cannon.GetComponent<Cannon>().repairFlag == false)
                        {
                            //　大砲との当たり判定をする
                            if (FC.Collider(cannon.colliderSet, panel.colliderSet))
                            {
                                MassPanel m = panel.GetComponent<MassPanel>();
                                m.ChangeableAmmunition(cannon.renderer.sortingOrder - 1, cannon.colliderSet.pos.position);
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
    //　大砲と爆弾の判定
    public void DeterminationOfCannonAndBomb()
    {
        foreach (ObjectBase cannon in c_Cannon)
        {
            //　大砲が使えるとき
            if (cannon.GetComponent<Cannon>().repairFlag == false)
            {
                foreach (ObjectBase bomb in c_Bomb)
                {
                    if (bomb != null)
                    {
                        if (FC.Collider(cannon.colliderSet, bomb.colliderSet))
                        {
                            //　修理常態化
                            cannon.GetComponent<Cannon>().RepairConditioning();
                            bomb.ObjectDestroy();
                            break;
                        }
                    }
                }
            }
        }
    }
    //　爆弾と砲弾の判定
    public void DeterminationOfBombsAndAmmunition()
    {
        foreach (ObjectBase bomb in c_Bomb)
        {
            if (bomb != null)
            {
                foreach (ObjectBase ammunition in c_Ammunition)
                {
                    if (ammunition != null)
                    {
                        if (FC.Collider(bomb.colliderSet, ammunition.colliderSet))
                        {
                            Bomb b = bomb.GetComponent<Bomb>();
                            Ammunition a = ammunition.GetComponent<Ammunition>();

                            b.XOROperation(a.num);
                            ammunition.ObjectDestroy();
                        }
                    }
                }
            }
        }
    }
    //　スコアの追加
    public void plusScore()
    {
        score.plusScore();
    }
    //　大砲と爆弾クリエイターのペア化
    public Cannon SetCannonAndBombCreatorPair(ObjectBase ob)
    {
        //　爆弾クリエイターの繰り返し
        for (int c = 0; c < cannonNum; c++)
        {
            //　爆弾クリエイターと大砲が同じ位置の時
            if (Mathf.Abs(ob.colliderSet.pos.position.x - c_Cannon[c].colliderSet.pos.position.x) <
                ob.colliderSet.myRect.width / 2 + c_Cannon[c].colliderSet.myRect.width / 2)
            {
                return c_Cannon[c].GetComponent<Cannon>();
            }
        }
        return null;
    }
    //　ゲームオーバー移行
    public void GameOverTransition()
    {
        if(cannonsScript[0].repairFlag && cannonsScript[1].repairFlag && cannonsScript[2].repairFlag)
        {
            SceneManager.LoadScene((int)GameMap.SceneName.GameOver);
        }
    }
    //　クリエイターとサポートパネルのセット化
    public BombCreater CreatorAndSupportSet(ObjectBase.ColliderSet support)
    {
        for (int i = 0; i < bCNum; i++)
        {
            if (Mathf.Abs(support.pos.position.x - c_BombCreater[i].colliderSet.pos.position.x) <
                    support.myRect.width / 2 + c_BombCreater[i].colliderSet.myRect.width / 2)
            {
                return c_BombCreater[i].GetComponent<BombCreater>();
            }
        }
        return null;
    }
}