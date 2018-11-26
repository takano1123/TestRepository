//----------------------------------------------------------
//　作成日　2018/10/16
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　オブジェクトの持つ共通の情報
//----------------------------------------------------------
//　作成時　2018/10/16　情報の整理
//　　追加　2018/10/17　子供の情報を取得格納
//　　追加　2018/10/18　親の情報を取得格納
//　　追加　2018/10/18　Sprite 変更用の SpriteRenderer を実装
//　　追加　2018/10/20　自分を削除する関数追加
//----------------------------------------------------------
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    //　コライダー用の情報
    [System.Serializable]
    public struct ColliderSet
    {
        public Rect myRect;         //　矩形
        public Transform pos;       //　トランスフォーム
    }

    public ColliderSet colliderSet;         //　自分のコライダー用の情報
    public ColliderSet[] childSet;          //　子供のコライダー用の情報
    public ColliderSet ParentSet;           //　親のコライダー用の情報
    public new SpriteRenderer renderer;     //  レンダラー
	public GameObject newObject;			//　自分が消える直前に生成するオブジェクト

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        colliderSet.pos = GetComponent<Transform>();
        childSet = null;
        //　子供がいるとき
        if (FC.ConfirmationOfChild(transform))
        {
            //　子供の情報獲得
            AcquisitionOfChildren();
        }
        //　親がいるとき
        if (transform.parent != null)
        {
            //　親の情報獲得
            AcquireParents();
        }
    }

    private void Update()
    {
        //　親がいるとき
        if (transform.parent != null)
        {
            //　親が変わったとき
            if (transform.parent != ParentSet.pos)
            {
                //　親の情報獲得
                AcquireParents();
            }
        }
    }

    //　子供の情報獲得
    public void AcquisitionOfChildren()
    {
        if (childSet == null)
        {
            childSet = new ColliderSet[transform.childCount];       //　人数分のコライダー用の情報を確保
        }

        ObjectBase[] temp = new ObjectBase[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            temp[i] = transform.GetChild(i).GetComponent<ObjectBase>();
        }
        //　子供の代入
        for (int i = 0; i < transform.childCount; i++)
        {
            //　childSetとchildObjの配列の中身がヌルのとき
            if (childSet[i].pos == null)
            {
                childSet[i] = temp[i].colliderSet;
            }
        }
    }


    //　親の情報獲得
    public void AcquireParents()
    {
        ParentSet = transform.parent.GetComponent<ObjectBase>().colliderSet;
    }

    //　オブジェクト削除
    public void ObjectDestroy()
    {
		if(newObject != null)
		{
			Instantiate(newObject, transform.position, Quaternion.identity);
		}
        Destroy(gameObject);
    }
}