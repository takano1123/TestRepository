//----------------------------------------------------------
//　作成日　2018/10/18
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　砲弾関連
//----------------------------------------------------------
//　作成時　2018/10/18　砲弾の移動
//----------------------------------------------------------
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    public ObjectBase ob;
    public Vector3 vector3;		
    public float moveSpeed;       //　弾の時の速さ
    public int num;

    void Start ()
    {
        ob = GetComponent<ObjectBase>();
        vector3 = transform.position;
        for(int i = 0; i < GameManagement.amNum ;i++)
        {
            if(GameManagement.manager.c_Ammunition[i] == null)
            {
                GameManagement.manager.c_Ammunition[i] = ob;
                break;
            }
        }
	}

    void Update()
    {
        vector3.y += moveSpeed;
        transform.position = vector3;

        if(transform.position.y > GameMap.heightSize)
        {
            Destroy(gameObject);
        }
    }
}
