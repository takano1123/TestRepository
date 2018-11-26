//----------------------------------------------------------
//　作成日　2018/10/09
//　作成者　諸岡勇樹
//----------------------------------------------------------
//　マップ関連
//----------------------------------------------------------
//　作成時　2018/10/09　ゲーム中、画面に必要なオブジェクトの生成
//　　追加　2018/10/23　ゲームクリア画面の生成
//　　追加　2018/10/23　シーン名のリスト追加、シーンの指定用の enum 追加
//　　修正　2018/10/23　クラス名変更　MapOperation -> GameMap
//　　追加　2018/10/24　GameManagement のシーンごとの有効、無効の判定
//　　修正　2018/10/24　シーン名のリスト削除
//　　　　　　　　　　　　シーンの指定用の enum と　SceneName型変数での
//　　　　　　　　　　　　シーン判定に変更
//　　追加　2018/10/27　フィーバー管理の生成追加
//----------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMap : MonoBehaviour
{
	//　各表示順（上後ろ、下前）
	public enum Order
	{
		orderBack,
		orderLane,
		orderClock,
		orderCannon,
		orderBomb,
		orderIMass,
		orerMouse,
	}

    //　シーンの指定用
    public enum SceneName
    {
        Title,          //　タイトルシーン
        GameScene,      //　ゲームシーン
        ClearScene,     //　クリアシーン
        GameOver,       //　ゲームオーバーシーン
    }

    public static SceneName currentScene;          //　現在のシーン数の取得

    public const int width = 16;            //　画面サイズ横比
	public const int height = 9;            //　画面サイズ縦比
	public const int mapChipSize = 96;      //　マップチップサイズ

	public const int widthSize = width * mapChipSize;       //　横ドット数個数
	public const int heightSize = height * mapChipSize;     //　縦ドット数個数

	public const int xNum = width + 2;      //　横の数
	public const int yNum = height + 2;     //　縦の数

	public GameObject background_Game;           //　ゲーム中の背景
	public GameObject background_Clear;				//　クリア画面の背景
    public GameObject background_Title;     //　タイトルの背景
    public GameObject background_GameOver;      //　ゲームオーバーの背景
    public GameObject lane;                     //　レーン
    public GameObject clock;                    //　時計
    public GameObject cannon;               //　大砲
    public GameObject bombCreater;      //　爆弾生成オブジェクト
    public GameObject massCreater;      //　指示マスパネル生成オブジェクト
    public GameObject arm;                  //　アーム（マウスのカーソールの見た目）
	public GameObject mousePointa;      //　ゲームシーン以外のマウスの見た目
    public GameObject scoreDisplay;     //　スコアディスプレイ
    public GameObject feverManagement;      //　フィーバー管理の生成

    //　ゲームオブジェクトの設置場所指定の配列
    private int[,] map = new int[yNum, xNum]  //　Y , X
    {
        { 0, 0, 0, 0, 3, 0, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 4, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 2, 0, 0, },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
    };

    private void Awake()
    {
        //　FPS固定
        Application.targetFrameRate = 60;
        //　マウス非表示
        Cursor.visible = false;
    }

    void Start()
    {
		currentScene 　= (SceneName)SceneManager.GetActiveScene().buildIndex;
		transform.position = new Vector3(widthSize / 2.0f, heightSize / 2.0f, -10.0f);    //　カメラのポジション設定

		//　現在のシーンによって生成物を変える
		switch (currentScene)
		{
            case SceneName.Title:
                TitleCreate();
                break;
            case SceneName.GameScene:
				GameSceneCreate();
				break;
			case SceneName.ClearScene:
				ClearSceneCreate();
                break;
            case SceneName.GameOver:
                GameOverCreate();
                break;
		}
    }
    //　オブジェクト生成まとめ関数
    private void CreateObject()
    {
        //　背景の生成
        CreateBackground(background_Game);

        //画面外にも設置
        Vector2 tempPos = new Vector2((0.0f - mapChipSize), (heightSize - (mapChipSize / 2) + mapChipSize));

        int mCCount = 0;
        int bCCount = 0;
        int cannonCount = 0;
        int lMCount = 0;

        for  (int i = 0; i < yNum; i++)
        {    
            for (int j = 0; j < xNum; j++)
            {
                GameObject obj = null;
                Order orderInLayer = 0;

                switch (map[i, j])
                {
                    //　何も生成しない
                    case 0:
                        break;
                    //　レーン
                    case 1:
                        if (lMCount < GameManagement.lMNum)
                        {
                            obj = Instantiate(lane, tempPos, Quaternion.identity);
                            GameManagement.manager.c_LaneManagement[lMCount] = obj.GetComponent<ObjectBase>();
                            orderInLayer = Order.orderLane;

                            lMCount++;
                        }
                        break;
                    //　時計
                    case 2:
                        obj = Instantiate(clock, tempPos, Quaternion.identity);
                        GameManagement.manager.c_Clock = GetComponent<ObjectBase>();
                        orderInLayer = Order.orderClock;
                        break;
                    //　爆弾クリエイター
                    case 3:
                        if (bCCount < GameManagement.bCNum)
                        {
                            GameObject o = Instantiate(bombCreater, tempPos, Quaternion.identity);
                            GameManagement.manager.c_BombCreater[bCCount] = o.GetComponent<ObjectBase>();
                            o.GetComponent<BombCreater>().myNum = bCCount;
                            bCCount++;
                        }
                        break;
                    //　大砲
                    case 4:
                        if (cannonCount < GameManagement.cannonNum)
                        {
                            obj = Instantiate(cannon, tempPos, Quaternion.identity);
                            GameManagement.manager.c_Cannon[cannonCount] = obj.GetComponent<ObjectBase>();
                            GameManagement.manager.cannonsScript[cannonCount] = obj.GetComponent<Cannon>();
                            orderInLayer = Order.orderCannon;

                            cannonCount++;
                        }
                        break;
                    //　マスパネルクリエイター
                    case 5:
                        if (mCCount < GameManagement.mCNum)
                        {
                            GameObject o = Instantiate(massCreater, tempPos, Quaternion.identity);
                            GameManagement.manager.c_MassCreater[mCCount] = o.GetComponent<ObjectBase>();
                            o.GetComponent<MassCreater>().myNum = mCCount;
                            mCCount++;
                        }
                        break;
                    default:
                        break;
                }

                if (obj != null)
                {
                    //　生成したオブジェクトの表示順の設定
                    FC.SettingOrderInLayer(obj, orderInLayer);
                }

                tempPos.x += mapChipSize;
            }
            tempPos.y -= mapChipSize;
            tempPos.x = (0.0f - mapChipSize);
        }
    }
	//　マウスのポインタ生成の関数
	private GameObject CreateMousePointa(GameObject mouse)
	{
		GameObject obj = Instantiate(mouse, transform.position, Quaternion.identity);
		FC.SettingOrderInLayer(obj, Order.orerMouse);
		return obj;
	}
	//　背景作成関数
	private void CreateBackground(GameObject back)
    {
        GameObject obj = null;
        Vector2 backPos = new Vector2(widthSize / 2.0f, heightSize / 2.0f);

        obj = Instantiate(back, backPos, Quaternion.identity);

        FC.SettingOrderInLayer(obj, (int)Order.orderBack);
    }
    //　ゲームシーンの生成まとめ関数
    private void GameSceneCreate()
    {
        //　フィーバの生成
        GameManagement.manager = GetComponent<GameManagement>();
        GameObject obj = Instantiate(feverManagement, transform.position, Quaternion.identity);
        GameManagement.manager.feverScript = obj.GetComponent<FeverManagement>();
        GameManagement.manager.c_Arm = CreateMousePointa(arm).GetComponent<ObjectBase>();
        Instantiate(scoreDisplay, transform.position, Quaternion.identity);
        CreateObject();  //　オブジェクトの生成
        for(int i =0;i < GameManagement.bCNum; i++)
        {
            GameManagement.manager.c_BombCreater[i].GetComponent<BombCreater>().theOtherCannon
                = GameManagement.manager.SetCannonAndBombCreatorPair(GameManagement.manager.c_BombCreater[i]);
        }
    }
	//　クリアシーンの生成まとめ関数
	private void ClearSceneCreate()
	{
        GetComponent<GameManagement>().enabled = false;
        Instantiate(scoreDisplay, transform.position, Quaternion.identity);
        CreateMousePointa(mousePointa);
		CreateBackground(background_Clear);
	}
    //　タイトルの生成まとめ関数
    private void TitleCreate()
    {
        GetComponent<GameManagement>().enabled = false;
        CreateBackground(background_Title);
        CreateMousePointa(mousePointa);
    }
    //　ゲームオーバーの生成まとめ関数
    private void GameOverCreate()
    {
        GetComponent<GameManagement>().enabled = false;
        CreateBackground(background_GameOver);
        CreateMousePointa(mousePointa);
    }
}
