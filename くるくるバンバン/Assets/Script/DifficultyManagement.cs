//----------------------------------------------------------
//　作成日　2018/11/23
//　作成者　高野紋杜
//----------------------------------------------------------
//　難易度によるゲーム内容の変化の管理
//----------------------------------------------------------
//　作成時　2018/11/23　作成
//  
//  MEMO
//  難易度の定義はどこでするのか
//  
//
//  爆弾の速度 : Bomb.moveSpeed
//  爆弾の個数 : BombCreater.Bomb.Entity
//----------------------------------------------------------
using UnityEngine;

public class DifficultyManagement : MonoBehaviour
{
    //難易度の定義
    public enum Difficulty
    {
        EASY,
        NORMAL,
        HARD,
    }

    public Difficulty difficulty;

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    void DifficultyManager()
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                break;

            case Difficulty.NORMAL:
                break;

            case Difficulty.HARD:
                break;

            default:
                break;
        }
    }
}
