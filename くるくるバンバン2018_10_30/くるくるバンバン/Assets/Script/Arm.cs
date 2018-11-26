//----------------------------------------------------------
//　作成日　2018/10/16
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　アーム関連
//----------------------------------------------------------
//　作成時　2018/10/16　マウスの追っかけ
//　　追加　2018/10/17　マスパネルを「つかむ」「放す」の実装
//　　追加　2018/10/18　「つかむ」「放す」のアニメーション実装
//　　追加　2018/10/18　Sprite 変更を ObjectBase にまかせる
//----------------------------------------------------------
using UnityEngine;

public class Arm : MonoBehaviour
{
    private Vector3 position;                       //　位置座標
    private Vector3 screenToWorldPointPosition;     //　スクリーン座標をワールド座標に変換した位置座標
    public ObjectBase ob;                           //　自分のObjectBase
    public Transform childTransform;                //　子供の情報
    public Sprite[] sprites; 

    void Start()
    {
        ob = GetComponent<ObjectBase>();
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3でマウス位置座標を取得する
        position = Input.mousePosition;
        // Z軸修正
        position.z = 10f;
        // マウス位置座標をスクリーン座標からワールド座標に変換する
        screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
        // ワールド座標に変換されたマウス座標を代入
        gameObject.transform.position = screenToWorldPointPosition;

        //　マウスの左クリックがあるとき
        if (Input.GetMouseButtonDown(0))
        {
            //　つかむ
            GameManagement.manager.HoldOnArm();
            ob.renderer.sprite = sprites[1];
        }
        //　左クリックが離されるとき
        else if (Input.GetMouseButtonUp(0))
        {
            //　放す
            GameManagement.manager.SetFree();
            ob.renderer.sprite = sprites[0];
        }
    }
}
