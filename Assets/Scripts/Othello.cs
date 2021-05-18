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


    public bool isCurrented;

    bool iscan_put;

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

    public void OnEnter()
    {

        OthelloImage.color = Color.gray;

        if (PieceStatus == Pieces.None)
        {
            iscan_put = mainScript.SerchChengePieces(vertical, horizontal, (int)PieceStatus);
        }       
        Debug.Log(iscan_put);

    }

    public void OnExit()
    {
       
        OthelloImage.color = Color.white;
        mainScript.SerchCanceled();
    }


    

    public void OnClick()
    {

        switch (PieceStatus)
        {
            case Pieces.Black:

                Debug.Log("現在の色は黒です。配置できません");

            break;

            case Pieces.White:

                Debug.Log("現在の色は白です。配置できません");

                break;


            case Pieces.None:

                
                
                if (iscan_put)
                {

                    Debug.Log("空なので配置します");
                    switch (mainScript.Order)
                    {
                        case MainScript.Othello_Order.Black:

                            Debug.Log("黒を配置");
                            OthelloImage.color = Color.white;
                            OthelloImage.sprite = mainScript.pieceImage[1].sprite;



                            PieceStatus = Pieces.Black;
                            mainScript.ChangeOthelloBoard(vertical, horizontal, (int)PieceStatus);
                            break;


                        case MainScript.Othello_Order.White:

                            Debug.Log("白を配置");
                            OthelloImage.color = Color.white;
                            OthelloImage.sprite = mainScript.pieceImage[2].sprite;



                            PieceStatus = Pieces.White;
                            mainScript.ChangeOthelloBoard(vertical, horizontal, (int)PieceStatus);
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
