//----------------------------------------------------------
//　作成日　2018/10/21
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　スコアの表示関連
//----------------------------------------------------------
//　作成時　2018/10/18　スコアの表示と加算
//　　追加　2018/10/23　ゲームクリア時のスコア表示
//　　追加　2018/10/24　シーンごとに表示位置とフォントサイズの変更
//----------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public RectTransform myRect;        //　自分の RectTransform
    public CanvasScaler myScaler;       //　自分の CanvasScaler
    public Text text;						//　Textコンポーネント保存用
    public RectTransform rTransform;         //　Textの RectTransform
    public static int score = 0;				//　スコア保存用

	//public float animeSpeed;		//　アニメーションのスピード
	//public float time;					//　アニメ管理の時間

	void Start()
	{
        myRect = GetComponent<RectTransform>();
        myRect.sizeDelta = new Vector2(GameMap.widthSize, GameMap.heightSize);
        myRect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        myScaler = GetComponent<CanvasScaler>();
        myScaler.referenceResolution = new Vector2(GameMap.widthSize, GameMap.heightSize);

		text = transform.GetChild(0).GetComponent<Text>();
        rTransform = text.GetComponent<RectTransform>();

        //　クリアシーン
        if (GameMap.currentScene == GameMap.SceneName.ClearScene)
        {
            //　テキストの表示位置とフォントのサイズ
            rTransform.anchoredPosition = new Vector3(570.0f, -120.0f, 0.0f);
            rTransform.sizeDelta = new Vector2(1941.0f, 328.0f);
            text.fontSize = 230;
        }
        //　ゲームシーン
        else if (GameMap.currentScene == GameMap.SceneName.GameScene)
        {
            GameManagement.manager.score = GetComponent<ScoreDisplay>();
            //　テキストの表示位置とフォントのサイズ
            rTransform.anchoredPosition = new Vector3(632.0f, 352.0f, 0.0f);
            rTransform.sizeDelta = new Vector2(480.0f, 68.0f);
            text.fontSize = 45;

            score = 0;
		}
	}
	
	void Update ()
    {
		switch(GameMap.currentScene)
		{
			case GameMap.SceneName.GameScene:
				UpdateGame();
				break;
			case GameMap.SceneName.ClearScene:
				UpdateClear();
				break;
		}
	}

    //　スコア加算関数
    public void plusScore()
    {
        score += 100;
    }

	//　ゲーム画面の時の処理
	private void UpdateGame()
	{
		text.text = "Score：" + score.ToString("D8");
	}

	//　クリア画面時の処理
	private void UpdateClear()
	{
		text.text =　score.ToString("D9");
	}
}
