//----------------------------------------------------------
//　作成日　2018/10/16
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　マスパネル関連
//----------------------------------------------------------
//　作成時　2018/10/16　初期位置への移動
//　　追加　2018/10/18　生成されてすぐに GameManagement に情報を渡す
//　　追加　2018/10/18　生成時の値私を関数化
//　　追加　2018/10/18　砲弾生成を実装
//----------------------------------------------------------
using UnityEngine;

public class MassPanel : MonoBehaviour
{
    public ObjectBase ob;
    public MassCreater.PanelSet mySet;          //　自分の見た目と数字,状態を管理するためのセット
    public Vector3 initialPos;                  //　初期位置

    public float returnSpeed;       //　戻る速さ

    Vector3 velocity = Vector3.zero;            //　ベロシティー疑似
    public Transform manufacturer;              //　製造元のトランスフォーム
    public GameObject ammunition;           //　砲弾

    void Start ()
    {
        ob.renderer.sprite = mySet.sprite;
    }

    void Update ()
    {
        //　今の親が製造元のとき
        if (transform.parent == manufacturer)
        {
            //　自分が初期位置にいないとき
            if (transform.position != initialPos)
            {
                //　初期位置に移動する
                transform.position = Vector3.SmoothDamp(transform.position, initialPos, ref velocity, returnSpeed);
            }
        }
        else if (transform.parent == null)
        {
            transform.parent = manufacturer;
        }
    }
    //　パネルから砲弾へ変わる（砲弾生成）
    public void ChangeableAmmunition(int order, Vector3 vector3)
    {
        GameObject obj = Instantiate(ammunition, vector3, Quaternion.identity);
        Ammunition m = obj.GetComponent<Ammunition>();
        m.num = mySet.num;

        FC.SettingOrderInLayer(obj, order);

        ob.ObjectDestroy();
    }

    //　初期設定
    public void InitialSetting(MassCreater.PanelSet set, Vector3 target, Transform trans)
    {
        mySet = set;
        initialPos = target;
        manufacturer = transform.parent = trans;
    }
    //　生成時の ObjectBase 取得用
    public ObjectBase SetObejectBase()
    {
        ob = GetComponent<ObjectBase>();
        return ob;
    }
}