using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Othello : MonoBehaviour
{

    [Header("盤面イメージコンポーネント")]

    public Image OthelloImage;

    [Header("メインスクリプト")]
    [SerializeField]MainScript mainScript;


    [Header("駒の状態")]
    // Start is called before the first frame update
    public Pieces PieceStatus;


    [Header("縦の位置")]
    public int vertical;
    [Header("横の位置")]
    public int horizontal;

    [Header("AudioClip")]
    [SerializeField] AudioClip ClickClip;
    [SerializeField] AudioClip SelectClip;

    [SerializeField] AudioSource audiosource;


    public bool isCurrented;//そのコマが選択されているか

    bool iscan_put;//ひっくりかえせる駒があるかどうか

    public enum Pieces
    {
        None =0,
        Black =1,
        White =2,
    }


    void Awake()
    {
        mainScript = GameObject.FindGameObjectWithTag("MainScript").GetComponent<MainScript>();
    }

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnter()//カーソルが駒を選択しているとき
    {

        OthelloImage.color = Color.gray;//選択した駒を灰色に
        audiosource.clip = SelectClip;
        audiosource.Play();
        if (PieceStatus == Pieces.None)//駒に何も置かれていない場合 八方向の挟める駒があるか検索
        {
            iscan_put = mainScript.SerchChengePieces(vertical, horizontal);
            Debug.Log("iscan_put" + iscan_put);
          
        }
        else
        {
            Debug.Log("iscan_put" + iscan_put);
            
        }


       

    }

    public bool AIisOnEnter()//AIにbool値を返す確認用
    {
        if (PieceStatus == Pieces.None)//駒に何も置かれていない場合 八方向の挟める駒があるか検索
        {
            return true;
        }
        else
        {
            Debug.Log("iscan_put" + iscan_put);
            return false;
        }
    }



    public void OnExit()//カーソルが駒から離れた時
    {
       
        OthelloImage.color = Color.white;//自身の選択を解除
        mainScript.SerchCanceled();//挟める駒の選択状態の解除
    }


    

    public void OnClick()//駒を選択したとき
    {

        switch (PieceStatus)
        {
            case Pieces.Black://自身が黒の場合

                Debug.Log("現在の色は黒です。配置できません");

            break;

            case Pieces.White://自身が白の場合

                Debug.Log("現在の色は白です。配置できません");

                break;


            case Pieces.None://何も置かれていない場合

                
                
                if (iscan_put)//八方向に置ける駒がある場合
                {

                    Debug.Log("空なので配置します");
                    switch (mainScript.Order)//ターンを検索　
                    {
                        case MainScript.Othello_Order.Black://黒のターン

                            Debug.Log("黒を配置");
                            OthelloImage.color = Color.white;//自身の選択を解除
                            OthelloImage.sprite = mainScript.pieceImage[1].sprite;//選択していた駒に黒を設置

                            audiosource.clip = ClickClip;
                            audiosource.Play();

                            PieceStatus = Pieces.Black;//状態を変更
                            mainScript.ChangeOthelloBoard(vertical, horizontal, (int)PieceStatus);//挟む駒の状態変更
                            break;


                        case MainScript.Othello_Order.White://白のターン

                            Debug.Log("白を配置");
                            OthelloImage.color = Color.white;
                            OthelloImage.sprite = mainScript.pieceImage[2].sprite;//選択していた駒に白を設置

                            audiosource.clip = ClickClip;
                            audiosource.Play();

                            PieceStatus = Pieces.White;//状態を変更
                            mainScript.ChangeOthelloBoard(vertical, horizontal, (int)PieceStatus);//挟む駒の状態変更
                            break;



                    }
                }
                else
                {


                    Debug.Log("返せる駒がないので配置できません");

                }

                break;





        }

        Debug.Log("クリック");
        

    }

   
}
