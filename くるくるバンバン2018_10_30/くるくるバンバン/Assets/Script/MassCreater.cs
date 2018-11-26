//----------------------------------------------------------
//　作成日　2018/10/14
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　マスパネル関連
//----------------------------------------------------------
//　作成時　2018/10/14　パネル生成
//　　追加　2018/10/18　マスパネルの生成時の値私を関数化
//　　追加　2018/10/21　GameManagement に作ったマスパネルの
//　　　　　　　　　　　　ObjectBase の情報を渡すようにした
//　　追加　2018/10/28　自分の生成するパネルを決める
//----------------------------------------------------------
using UnityEngine;

public class MassCreater : MonoBehaviour
{
    //　コライダー用の情報
    [System.Serializable]
    //　マスパネルに持たせる、見た目の管理をする構造体
    public struct PanelSet
    {
        public Sprite sprite;       //　見た目
        public int num;             //　数

        public PanelSet(Sprite s, int i) : this()
        {
            sprite = s;
            num = i;
        }
    }

    public ObjectBase ob;

    public const int panelNum = 15;     //　パネルの枚数

    public PanelSet[] panels = new PanelSet[panelNum];      //　セット数（ラインナップ：見た目、数）
    public Sprite[] sprites = new Sprite[panelNum];         //　スプライトセット用

    public PanelSet fPanelSet;      //　フィーバー用

    public GameObject panelPrefab;
    public GameObject nextPanel;        //　次に使う候補のパネル（実体）
    public GameObject normalPanel;      //　通常パネル
    public Vector2 target;      //　候補のパネルを置く場所
    public int myNum;           // 自分の数字
    public int min;             //　生成するマスの最低
    public int max;             //　生成するマスの最大

    public int feverStart;

    private void Start()
    {
        ob = GetComponent<ObjectBase>();
        for (int i = 0; i < panelNum; i++)
        {
            panels[i] = new PanelSet(sprites[i], (i + 1));
        }

        target = transform.position;
        target.x -= 192.0f;
        nextPanel = null;
        feverStart = 0;
        //　自分の番号から生成するパネルを決める
        switch (myNum)
        {
            case 0:
                min = 0;
                max = 2;
                break;
            case 1:
                min = 3;
                max = 5;
                break;
            case 2:
                min = 6;
                max = 9;
                break;
            case 3:
                min = 10;
                max = 12;
                break;
            case 4:
                min = 13;
                max = 15;
                break;
        }
    }

    void Update()
    {
        //　フィーバーじゃないとき
        if (!GameManagement.manager.feverScript.fever)
        {
            if (nextPanel == null)
            {
                int temp = Random.Range(min, max);
                nextPanel = Create(panels[temp]);
                ob.AcquisitionOfChildren();
            }
        }
        //　フィーバーのとき
        else if (GameManagement.manager.feverScript.fever)
        {
            if (nextPanel == null)
            {
                nextPanel = Create(fPanelSet);
                ob.AcquisitionOfChildren();
            }
        }
    }
    //　パネル生成
    public GameObject Create(PanelSet set)
    {
        Vector3 vector3 = transform.position;
        vector3.x += GameMap.mapChipSize;

        GameObject obj = Instantiate(panelPrefab, vector3, Quaternion.identity);

        MassPanel m = obj.GetComponent<MassPanel>();
        m.InitialSetting(set, target, transform);

        //　爆弾の ObejectBase 情報を代入
        for (int i = 0; i < GameManagement.mCNum; i++)
        {
            if (GameManagement.manager.c_MassPanel[i] == null)
            {
                GameManagement.manager.c_MassPanel[i] = m.SetObejectBase();
                break;
            }
        }

        FC.SettingOrderInLayer(obj, GameMap.Order.orderIMass);
        return obj;
    }
}