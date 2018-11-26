//----------------------------------------------------------
//　作成日　2018/10/09
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　関数表
//----------------------------------------------------------
//　作成時　2018/10/16　X軸、Y軸の同時計算（同じ値が足される）
//                                  表示順の設定　の作成
//----------------------------------------------------------
using UnityEngine;

public class FC : MonoBehaviour
{
    //--------------------------------------------------------
    //　X軸、Y軸の同時計算（同じ値が足される）
    //--------------------------------------------------------
    static public Vector2 XYCalculation(Vector2 vector, int i)
    {
        vector.x += (float)i;
        vector.y += (float)i;
        return vector;
    }
    static public Vector2 XYCalculation(Vector2 vector, double d)
    {
        vector.x += (float)d;
        vector.y += (float)d;
        return vector;
    }
    static public Vector2 XYCalculation(Vector2 vector, float f)
    {
        vector.x += f;
        vector.y += f;
        return vector;
    }

    //--------------------------------------------------------
    //　表示順の設定
    //--------------------------------------------------------
    static public void SettingOrderInLayer(GameObject obj, int order)
    {
        SpriteRenderer s = obj.GetComponent<SpriteRenderer>();
        s.sortingOrder = order;
    }
    static public void SettingOrderInLayer(GameObject obj, GameMap.Order order)
    {
        SpriteRenderer s = obj.GetComponent<SpriteRenderer>();
        s.sortingOrder = (int)order;
    }

    //--------------------------------------------------------
    //　コライダー
    //--------------------------------------------------------
    static public bool Collider(ObjectBase.ColliderSet a, ObjectBase.ColliderSet b)
    {
        float zeA = Mathf.Abs((a.pos.position.x + a.myRect.x) - (b.pos.position.x + b.myRect.x));
        float zeB = Mathf.Abs((a.pos.position.y + a.myRect.y) - (b.pos.position.y + b.myRect.y));
        //　衝突していたら
        if (zeA < ((a.myRect.width / 2) + (b.myRect.width / 2)) && zeB < (a.myRect.height / 2 + b.myRect.height / 2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //--------------------------------------------------------
    //　子持ちの確認
    //--------------------------------------------------------
    static public bool ConfirmationOfChild(Transform t)
    {
        bool b = t.childCount > 0;
        return b;
    }
}

