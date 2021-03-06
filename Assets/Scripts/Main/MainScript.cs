using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class MainScript : MonoBehaviour
{

    public event Action<MainScript> Turn;


    [Header("Script")]

    [SerializeField] PieceCounter pieceCounter;

    [SerializeField] AIBot aIBot;

    [Header("盤面の一番左上の位置")]

    [SerializeField] GameObject initPos;

    [Header("駒のImageオブジェクトを配置する親")]
    [SerializeField] GameObject Panel;

    [Header("駒の種類")]
    public Image[] pieceImage = new Image[3];

    [Header("各座標のオセロスクリプト")]
    public Othello[,] OthelloScriptsArrow = new Othello[8, 8];
    [Header("各座標のイメージコンポーネント")]
    [SerializeField] Image[,] OthelloImageArrow = new Image[8, 8];

    [Header("ターンテキスト")]

    [SerializeField] Text TurnText;



    Image[,] ImageBoard = new Image[8, 8];

    const int pieceDistance = 100;

    [Header("現在のターン色")]
    public Othello_Order Order;


    public Othello_Order PlayerTurn;

    public Othello_Order AITurn;



    [Header("勝敗")]

    [SerializeField] GameObject judge;

    [SerializeField] Text judgetext;


    bool clipUp;
    bool clipDown;
    bool clipLeft;
    bool clipRight;
    bool clipUpLeft;
    bool clipUpRight;
    bool clipDownLeft;
    bool clipDownRight;
    int Up = 0;
    int Down = 0;
    int Left = 0;
    int Right = 0;
    int UpLeft = 0;
    int UpRight = 0;
    int DownLeft = 0;
    int DownRight = 0;



    int interval = 4;



    public enum Othello_Order//オセロのターン
    {
        Black = 1,

        White = 2,

    }


    public int[,] OthelloBoard = new int[8, 8]
    {
        {0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0},
        {0,0,0,2,1,0,0,0},
        {0,0,0,1,2,0,0,0},
        {0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0}
    };




    void Awake()
    {

        Application.targetFrameRate = 60;

    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();


        ResetBoard();//盤面のりセット



        if (aIBot != null)
        {
            Debug.Log("CPU戦です");
            Ordering();//順番決め
            
        }

      
    }
    

    public void Ordering()
    {
        int rand= UnityEngine.Random.Range(1,3);

        switch (rand)
        {
            case 1:

                PlayerTurn = Othello_Order.Black;
                Debug.Log("先行はプレイヤーです");
                TurnText.text = "あなたのターン";
                break;
            case 2:

                PlayerTurn = Othello_Order.White;
                Debug.Log("CPが先行に選ばれました");
                TurnText.text = "CPのターン";

                break;
            default:

                Debug.LogError("例外の数字が入力されました:"+rand);

                break;
        }



        switch (PlayerTurn)
        {
            case Othello_Order.Black:

                AITurn = Othello_Order.White;
                Debug.Log("CPは白です");
                TurnText.text = "あなたのターン";
                break;

            case Othello_Order.White:

                AITurn = Othello_Order.Black;
                Debug.Log("CPは黒です");
                TurnText.text = "CPのターン";

                aIBot.AITurn();

                break;
            default:

                Debug.LogError("例外の数字が入力されました");

                break;

        }



   
    }

    private void Reset()
    {
        Application.targetFrameRate = 60;


        judge.SetActive(false);
    }



    public void ResetBoard()
    {


        string boardTxt = "";
        for (int v = 0; v < OthelloBoard.GetLength(0); v++)
        {



            for (int h = 0; h < OthelloBoard.GetLength(1); h++)
            {

                switch (OthelloBoard[v, h])
                {
                    case 0:
                        ImageBoard[v, h] = Instantiate(pieceImage[0], new Vector2(initPos.transform.position.x + pieceDistance * h + interval * h, initPos.transform.position.y - pieceDistance * v - interval * v), Quaternion.identity, Panel.transform);
                        OthelloScriptsArrow[v, h] = ImageBoard[v, h].GetComponent<Othello>();
                        OthelloImageArrow[v, h] = ImageBoard[v, h].GetComponent<Image>();
                        OthelloScriptsArrow[v, h].vertical = v;
                        OthelloScriptsArrow[v, h].horizontal = h;

                        OthelloScriptsArrow[v, h].PieceStatus = Othello.Pieces.None;


                        break;


                    case 1:
                        ImageBoard[v, h] = Instantiate(pieceImage[1], new Vector2(initPos.transform.position.x + pieceDistance * h + interval * h, initPos.transform.position.y - pieceDistance * v - interval * v), Quaternion.identity, Panel.transform);
                        OthelloScriptsArrow[v, h] = ImageBoard[v, h].GetComponent<Othello>();
                        OthelloImageArrow[v, h] = ImageBoard[v, h].GetComponent<Image>();
                        OthelloScriptsArrow[v, h].vertical = v;
                        OthelloScriptsArrow[v, h].horizontal = h;
                        OthelloScriptsArrow[v, h].PieceStatus = Othello.Pieces.Black;

                        break;

                    case 2:
                        ImageBoard[v, h] = Instantiate(pieceImage[2], new Vector2(initPos.transform.position.x + pieceDistance * h + interval * h, initPos.transform.position.y - pieceDistance * v - interval * v), Quaternion.identity, Panel.transform);
                        OthelloScriptsArrow[v, h] = ImageBoard[v, h].GetComponent<Othello>();
                        OthelloImageArrow[v, h] = ImageBoard[v, h].GetComponent<Image>();
                        OthelloScriptsArrow[v, h].vertical = v;
                        OthelloScriptsArrow[v, h].horizontal = h;
                        OthelloScriptsArrow[v, h].PieceStatus = Othello.Pieces.White;

                        break;


                    default:

                        Debug.LogError("未定義の数字が発生");
                        break;


                }


                boardTxt = boardTxt + OthelloBoard[v, h];


            }
            boardTxt = boardTxt + "\n";
        }
        Debug.Log(boardTxt);

        Order = Othello_Order.Black;
        

    }



    // Update is called once per frame
    void Update()
    {

    }



    public bool SerchChengePieces(int v, int h)//取れる駒を予測する
    {
        //各方向のおける数を記録
        //初期化
        Up = 0;
        Down = 0;
        Left = 0;
        Right = 0;
        UpLeft = 0;
        UpRight = 0;
        DownLeft = 0;
        DownRight = 0;
        clipUp = false;
        clipDown = false;
        clipLeft = false;
        clipRight = false;
        clipUpLeft = false;
        clipUpRight = false;
        clipDownLeft = false;
        clipDownRight = false;

        bool isbreak = false;



        //Debug.Log(v + "," + h + "を選択、検索を開始");

        //上方向
        //Debug.Log("上方向");
        for (int u = 1; u <= v; u++)
        {

            switch (Order)
            {
                case Othello_Order.Black:

                    switch (OthelloBoard[v - u, h])
                    {
                        case 0:
                            if (clipUp == false && isbreak == false)
                            {
                            // Debug.Log(v + "," + h + "途中空白が検出されました + 上方向");
                                Up = 0;
                                isbreak = true;
                            }
                            break;
                        case 1://間の駒 黒
                            if (Up > 0 && clipUp == false && isbreak == false)
                            {
                                clipUp = true;

                                isbreak = true;
                             // Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "上方向");
                            }
                            else if (Up == 0 && isbreak == false)
                            {
                             //Debug.Log(v + "," + h + "途中同じ駒を検出" + "上方向");

                                isbreak = true;
                            }

                            break;
                        case 2://間の駒 白
                            if (clipUp == false && isbreak == false)
                            {
                                Up++;
                            // Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "上方向");
                            }
                            break;

                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;
                    }
                    break;

                case Othello_Order.White:

                    switch (OthelloBoard[v - u, h])
                    {
                        case 0:
                            if (clipUp == false && isbreak == false)
                            {

                                Up = 0;
                                isbreak = true;
                              //  //Debug.Log(v + "," + h + "途中空白が検出されました + 上方向");
                            }

                            break;
                        case 1://間の駒 黒
                            if (clipUp == false && isbreak == false)
                            {
                                Up++;
                              //  //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "上方向");
                            }
                            break;
                        case 2://間の駒 白
                            if (Up > 0 && clipUp == false && isbreak == false)
                            {
                                clipUp = true;
                                isbreak = true;
                              //  //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "上方向");
                            }
                            else if (Up == 0 && isbreak == false)
                            {
                              //  //Debug.Log(v + "," + h + "途中同じ駒を検出" + "上方向");

                                isbreak = true;
                            }

                            break;

                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;
                    }
                    break;

                default:
                    //Debug.LogError("例外が発生,駒の数値が間違っています");
                    break;
            }


        }


        isbreak = false;

        //下方向
        //Debug.Log("下方向");
        for (int d = 1; d <= (OthelloBoard.GetLength(1) - 1) - v; d++)
        {
            //Debug.Log("座標" + (v + d) + "," + h + "を検索中");
            switch (Order)
            {
                case Othello_Order.Black:
              
                    switch (OthelloBoard[v + d, h])
                    {
                        case 0:
                            if (clipDown == false && isbreak == false)
                            {
                                Down = 0;
                                isbreak = true;
                              //  //Debug.Log(v + "," + h + "途中空白が検出されました + 下方向");
                            }
                            break;
                        case 1:
                            if (Down > 0 && clipDown == false && isbreak == false)
                            {
                                clipDown = true;
                                isbreak = true;
                              //  //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "下方向");

                            } else if (Down == 0 && isbreak == false)
                            {
                              //  //Debug.Log(v + "," + h + "途中同じ駒を検出" + "下方向");

                                isbreak = true;
                            }
                            break;
                        case 2:
                            if (clipDown == false && isbreak == false)
                            {
                                Down++;
                              //  //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "下方向");
                            }

                            break;
                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;

                    }

                    break;

                case Othello_Order.White:
                   
                    switch (OthelloBoard[v + d, h])
                    {
                        case 0:
                            if (clipDown == false && isbreak == false)
                            {
                                Down = 0;
                                isbreak = true;
                               // //Debug.Log(v + "," + h + "途中空白が検出されました + 下方向");
                            }

                            break;
                        case 1:
                            if (clipDown == false && isbreak == false)
                            {
                                Down++;
                               // //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "下方向");
                            }


                            break;
                        case 2:
                            if (Down >= 1 && clipDown == false && isbreak == false)
                            {
                                clipDown = true;
                                isbreak = true;
                               // //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "下方向");

                            }
                            else if (Down == 0 && isbreak == false)
                            {
                               // //Debug.Log(v + "," + h + "途中同じ駒を検出" + "下方向");

                                isbreak = true;
                            }


                            break;
                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;
                    }

                    break;

                default:

                    //Debug.LogError("例外が発生,駒の数値が間違っています");

                    break;

            }
        }

        isbreak = false;
        //左方向
        //Debug.Log("左方向");
        for (int l = 1; l <= h; l++)
        {
            //Debug.Log("座標" + v + "," + (h - l) + "を検索中");
            switch (Order)
            {
                case Othello_Order.Black:

                    switch (OthelloBoard[v, h - l])
                    {
                        case 0:
                            if (clipLeft == false && isbreak == false)
                            {
                                Left = 0;
                                isbreak = true;
                               // //Debug.Log(v + "," + h + "途中空白が検出されました + 左方向");
                            }
                            break;
                        case 1:
                            if (Left >= 1 && clipLeft == false && isbreak == false)
                            {
                                clipLeft = true;
                                isbreak = true;
                              //  //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "左方向");
                            }
                            else if (Left == 0 && isbreak == false)
                            {
                               // //Debug.Log(v + "," + h + "途中同じ駒を検出" + "左方向");

                                isbreak = true;
                            }
                            break;
                        case 2:

                            if (clipLeft == false && isbreak == false)
                            {
                                Left++;
                              //  //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "左方向");
                            }

                            break;

                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;

                    }

                    break;

                case Othello_Order.White:

                    switch (OthelloBoard[v, h - l])
                    {
                        case 0:
                            if (clipLeft == false && isbreak == false)
                            {
                                Left = 0;
                                isbreak = true;
                               // //Debug.Log(v + "," + h + "途中空白が検出されました + 左方向");
                            }

                            break;
                        case 1:
                            if (clipLeft == false && isbreak == false)
                            {

                                Left++;
                              //  //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "左方向");
                            }
                            break;
                        case 2:


                            if (Left >= 1 && clipLeft == false && isbreak == false)
                            {
                                isbreak = true;
                                clipLeft = true;
                               // //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "左方向");
                            }
                            else if (Left == 0 && isbreak == false)
                            {
                              //  //Debug.Log(v + "," + h + "途中同じ駒を検出" + "左方向");

                                isbreak = true;
                            }

                            break;
                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;


                    }

                    break;
                default:

                    //Debug.LogError("例外が発生,駒の数値が間違っています");

                    break;
            }
        }
        isbreak = false;
        //右方向
        //Debug.Log("右方向");
        for (int r = 1; r <= (OthelloBoard.GetLength(0) - 1) - h; r++)
        {
            //Debug.Log("座標" + v + "," + (h + r) + "を検索中");
            switch (Order)
            {
                case Othello_Order.Black:
                    switch (OthelloBoard[v, h + r])
                    {
                        case 0:
                            if (clipRight == false && isbreak == false)
                            {
                                Right = 0;
                                isbreak = true;
                              //  //Debug.Log(v + "," + h + "途中空白が検出されました + 右方向");
                            }
                            break;
                        case 1:
                            if (Right >= 1 && clipRight == false && isbreak == false)
                            {
                                clipRight = true;
                                isbreak = true;
                               // //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "右方向");
                            }
                            else if (Right == 0 && isbreak == false)
                            {
                              //  //Debug.Log(v + "," + h + "途中同じ駒を検出" + "右方向");

                                isbreak = true;
                            }
                            break;
                        case 2:
                            if (clipRight == false && isbreak == false)
                            {
                                Right++;
                              //  //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "右方向");
                            }
                            break;
                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;
                    }
                    break;
                case Othello_Order.White:
                    switch (OthelloBoard[v, h + r])
                    {
                        case 0:
                            if (clipRight == false && isbreak == false)
                            {
                                Right = 0;
                                isbreak = true;
                               // //Debug.Log(v + "," + h + "途中空白が検出されました + 右方向");
                            }
                            break;
                        case 1:
                            if (clipRight == false && isbreak == false)
                            {
                                Right++;
                               // //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "右方向");
                            }
                            break;
                        case 2:
                            if (Right >= 1 && clipRight == false && isbreak == false)
                            {
                                clipRight = true;
                                isbreak = true;
                               // //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "右方向");
                            }
                            else if (Right == 0 && isbreak == false)
                            {
                              //  //Debug.Log(v + "," + h + "途中同じ駒を検出" + "右方向");

                                isbreak = true;
                            }

                            break;

                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;
                    }
                    break;
                default:

                    //Debug.LogError("例外が発生,駒の数値が間違っています");

                    break;
            }
        }

        isbreak = false;
        //左上方向
        //Debug.Log("左上方向");
        for (int ul = 1; ul <= Calculation_LeftUp_Length(v, h); ul++)
        {
            //Debug.Log("座標" + (v - ul) + "," + (h - ul) + "を検索中");
            switch (Order)
            {
                case Othello_Order.Black:
                    switch (OthelloBoard[v - ul, h - ul])
                    {
                        case 0:
                            if (clipUpLeft == false && isbreak == false)
                            {
                                UpLeft = 0;
                                isbreak = true;
                               // //Debug.Log(v + "," + h + "途中空白が検出されました + 左上方向");
                            }
                            break;
                        case 1:
                            if (UpLeft >= 1 && clipUpLeft == false && isbreak == false)
                            {
                                clipUpLeft = true;
                                isbreak = true;
                              //  //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "左上方向");
                            }
                            else if (UpLeft == 0 && isbreak == false)
                            {
                              //  //Debug.Log(v + "," + h + "途中同じ駒を検出" + "左上方向");

                                isbreak = true;
                            }
                            break;
                        case 2:

                            if (clipUpLeft == false && isbreak == false)
                            {
                                UpLeft++;
                              // //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "左上方向");
                            }

                            break;
                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;




                    }



                    break;

                case Othello_Order.White:

                    switch (OthelloBoard[v - ul, h - ul])
                    {
                        case 0:

                            if (clipUpLeft == false && isbreak == false)
                            {
                                UpLeft = 0;
                                isbreak = true;
                              //  //Debug.Log(v + "," + h + "途中空白が検出されました + 左上方向");
                            }


                            break;

                        case 1:

                            if (clipUpRight == false && isbreak == false)
                            {
                                UpLeft++;
                              //  //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "左上方向");

                            }
                            break;


                        case 2:

                            if (UpLeft >= 1 && clipUpLeft == false && isbreak == false)
                            {
                                clipUpLeft = true;
                                isbreak = true;
                               // //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "左上方向");
                            }
                            else if (UpLeft == 0 && isbreak == false)
                            {
                              //  //Debug.Log(v + "," + h + "途中同じ駒を検出" + "左上方向");

                                isbreak = true;
                            }



                            break;
                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;




                    }

                    break;


                default:

                    //Debug.LogError("例外が発生,駒の数値が間違っています");

                    break;

            }



        }

        isbreak = false;

        //右上方向
        //Debug.Log("右上方向");
        for (int ur = 1; ur <= Calculation_RightUp_Length(v, h); ur++)
        {
            //Debug.Log("座標" + (v - ur) + "," + (h + ur) + "を検索中");
            switch (Order)
            {

                case Othello_Order.Black:

                    switch (OthelloBoard[v - ur, h + ur])
                    {
                        case 0:


                            if (clipUpRight == false && isbreak == false)
                            {
                                UpRight = 0;
                                isbreak = true;
                               // //Debug.Log(v + "," + h + "途中空白が検出されました + 右上方向");
                            }
                            break;

                        case 1:

                            if (UpRight >= 1 && clipUpRight == false && isbreak == false)
                            {
                                clipUpRight = true;
                                isbreak = true;
                              //  //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "右上方向");
                            }
                            else if (UpRight == 0 && isbreak == false)
                            {
                              //  //Debug.Log(v + "," + h + "途中同じ駒を検出" + "右上方向");

                                isbreak = true;
                            }


                            break;


                        case 2:

                            if (clipUpRight == false && isbreak == false)
                            {
                                UpRight++;
                              //  //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "右上方向");
                            }
                            break;
                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;

                    }



                    break;

                case Othello_Order.White:

                    switch (OthelloBoard[v - ur, h + ur])
                    {
                        case 0:


                            if (clipUpRight == false && isbreak == false)
                            {
                                UpRight = 0;
                                isbreak = true;
                               // //Debug.Log(v + "," + h + "途中空白が検出されました + 右上方向");
                            }

                            break;

                        case 1:

                            if (clipUpRight == false && isbreak == false)
                            {
                                UpRight++;
                              //  //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "右上方向");

                            }
                            break;


                        case 2:


                            if (UpRight >= 1 && clipUpRight == false && isbreak == false)
                            {
                                clipUpRight = true;
                                isbreak = true;
                               // //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "右上方向");
                            }
                            else if (UpRight == 0 && isbreak == false)
                            {
                               // //Debug.Log(v + "," + h + "途中同じ駒を検出" + "右上方向");

                                isbreak = true;
                            }


                            break;
                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;

                    }


                    break;


                default:

                    //Debug.LogError("例外が発生,駒の数値が間違っています");

                    break;

            }



        }

        isbreak = false;

        //左下方向
        //Debug.Log("左下方向");
        for (int dl = 1; dl <= Calculation_LeftDown_Length(v, h); dl++)
        {
            //Debug.Log("座標" + (v + dl) + "," + (h - dl) + "を検索中");
            switch (Order)
            {

                case Othello_Order.Black:

                    switch (OthelloBoard[v + dl, h - dl])
                    {
                        case 0:
                            if (clipDownLeft == false && isbreak == false)
                            {
                                DownLeft = 0;
                                isbreak = true;
                              //  //Debug.Log(v + "," + h + "途中空白が検出されました + 左下方向");
                            }

                            break;

                        case 1:

                            if (DownLeft >= 1 && clipDownLeft == false && isbreak == false)
                            {
                                clipDownLeft = true;
                                isbreak = true;
                              //  //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "左下方向");
                            }
                            else if (DownLeft == 0 && isbreak == false)
                            {
                              //  //Debug.Log(v + "," + h + "途中同じ駒を検出" + "左下方向");

                                isbreak = true;
                            }


                            break;


                        case 2:

                            if (clipDownLeft == false && isbreak == false)
                            {
                                DownLeft++;
                              //  //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "左下方向");
                            }
                            break;
                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;




                    }


                    break;

                case Othello_Order.White:

                    switch (OthelloBoard[v + dl, h - dl])
                    {
                        case 0:
                            if (!clipDownLeft && isbreak == false)
                            {
                                DownLeft = 0;
                                isbreak = true;
                              //  //Debug.Log(v + "," + h + "途中空白が検出されました + 左下方向");
                            }

                            break;

                        case 1:
                            if (!clipDownLeft && isbreak == false)
                            {
                                DownLeft++;
                              //  //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "左下方向");
                            }
                            break;


                        case 2:


                            if (DownLeft >= 1 && isbreak == false)
                            {
                                clipDownLeft = true;
                                isbreak = true;
                              //  //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "左下方向");

                            }
                            else if (DownLeft == 0 && isbreak == false)
                            {
                               // //Debug.Log(v + "," + h + "途中同じ駒を検出" + "左下方向");

                                isbreak = true;
                            }



                            break;
                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;

                    }

                    break;

                default:

                    //Debug.LogError("例外が発生,駒の数値が間違っています");

                    break;

            }



        }

        isbreak = false;
        //右下方向
        //Debug.Log("右下方向");
        for (int dr = 1; dr <= Calculation_RightDown_Length(v, h); dr++)
        {
            //Debug.Log("座標" + (v + dr) + "," + (h + dr) + "を検索中");
            switch (Order)
            {

                case Othello_Order.Black:
                    switch (OthelloBoard[v + dr, h + dr])
                    {
                        case 0:
                            if (clipDownRight == false && isbreak == false)
                            {
                                DownRight = 0;
                                isbreak = true;
                              //  //Debug.Log(v + "," + h + "途中空白が検出されました + 右下方向");
                            }

                            break;

                        case 1:

                            if (DownRight >= 1 && clipDownRight == false && isbreak == false)
                            {
                                clipDownRight = true;
                                isbreak = true;
                               // //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "右下方向");

                            }
                            else if (DownRight == 0 && isbreak == false)
                            {
                              //  //Debug.Log(v + "," + h + "途中同じ駒を検出" + "右下方向");

                                isbreak = true;
                            }

                            break;


                        case 2:

                            if (clipDownRight == false && isbreak == false)
                            {
                                DownRight++;
                             //   //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "右下方向");
                            }
                            break;
                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;




                    }



                    break;

                case Othello_Order.White:

                    switch (OthelloBoard[v + dr, h + dr])
                    {
                        case 0:

                            if (clipDownRight == false && isbreak == false)
                            {
                              //  //Debug.Log("途中空白が検出されました + 右下方向");
                                DownRight = 0;
                                isbreak = true;
                            }
                            break;

                        case 1:
                            if (clipDownRight == false)
                            {
                                DownRight++;
                              //  //Debug.Log(v + "," + h + "対の駒を発見" + "isbreak:" + isbreak + "右下方向");
                            }

                            break;


                        case 2:

                            if (DownRight >= 1 && clipDownRight == false && isbreak == false)
                            {
                                clipDownRight = true;
                                isbreak = true;
                             //   //Debug.Log(v + "," + h + "同じ駒を検出" + "isbreak:" + isbreak + "右下方向");
                            }
                            else if (DownRight == 0 && isbreak == false)
                            {
                             //   //Debug.Log(v + "," + h + "途中同じ駒を検出" + "右下方向");

                                isbreak = true;
                            }

                            break;
                        default:

                            //Debug.LogError("例外が発生,駒の数値が間違っています");

                            break;




                    }

                    break;


                default:

                    //Debug.LogError("例外が発生,駒の数値が間違っています");

                    break;

            }



        }



        //ひっくり返る各方向のマスの色を変える

        //Debug.Log("座標" + v + "," + h);

        Debug.Log("ターン" + Order.ToString() + "Up:" + Up + clipUp + "Down:" + Down + clipDown + "Left:" + Left + clipLeft + "Right:" + Right + clipRight + "UpLeft:" + UpLeft + clipUpLeft + "UpRight:" + UpRight + clipUpRight + "DownLeft:" + DownLeft + clipDownLeft + "DownRight:" + DownRight + clipDownRight);

        



        if (clipUp)
        {
            for (int a = 1; a <= Up; a++)
            {
                OthelloImageArrow[v - a, h].color = Color.gray;
                OthelloScriptsArrow[v - a, h].isCurrented = true;
            }
        }
        if (clipDown)
        {
            for (int b = 1; b <= Down; b++)
            {
                OthelloImageArrow[v + b, h].color = Color.gray;
                OthelloScriptsArrow[v + b, h].isCurrented = true;
            }

        }
        if (clipLeft)
        {

            for (int c = 1; c <= Left; c++)
            {
                OthelloImageArrow[v, h - c].color = Color.gray;
                OthelloScriptsArrow[v, h - c].isCurrented = true;
            }

        }
        if (clipRight)
        {
            for (int d = 1; d <= Right; d++)
            {
                OthelloImageArrow[v, h + d].color = Color.gray;
                OthelloScriptsArrow[v, h + d].isCurrented = true;
            }

        }
        if (clipUpLeft)
        {
            for (int e = 1; e <= UpLeft; e++)
            {
                OthelloImageArrow[v - e, h - e].color = Color.gray;
                OthelloScriptsArrow[v - e, h - e].isCurrented = true;
            }

        }
        if (clipUpRight)
        {
            for (int f = 1; f <= UpRight; f++)
            {
                OthelloImageArrow[v - f, h + f].color = Color.gray;
                OthelloScriptsArrow[v - f, h + f].isCurrented = true;
            }

        }
        if (clipDownLeft)
        {
            for (int g = 1; g <= DownLeft; g++)
            {
                OthelloImageArrow[v + g, h - g].color = Color.gray;
                OthelloScriptsArrow[v + g, h - g].isCurrented = true;
            }

        }
        if (clipDownRight)
        {
            for (int i = 1; i <= DownRight; i++)
            {
                OthelloImageArrow[v + i, h + i].color = Color.gray;
                OthelloScriptsArrow[v + i, h + i].isCurrented = true;
            }

        }

        if (clipUp == false && clipDown == false && clipLeft == false && clipRight == false && clipUpLeft == false && clipUpRight == false && clipDownLeft == false && clipDownRight == false)
        {

            return false;
        }
        else
        {
            return true;
        }


    }

    public void SerchCanceled()//予測した駒の選択状態を解除する
    {
        for (int i = 0; i < OthelloScriptsArrow.GetLength(0); i++)
        {

            for (int a = 0; a < OthelloScriptsArrow.GetLength(1); a++)
            {
                if (OthelloScriptsArrow[i, a].isCurrented)//配列の選択がされていれば選択を解除
                {
                    OthelloScriptsArrow[i, a].isCurrented = false;

                    OthelloImageArrow[i, a].color = Color.white;

                }
            }
        }
    }


    public void ChangeOthelloBoard(int v, int h, int PieceStatus)//挟む駒を変更する
    {

        OthelloBoard[v, h] = PieceStatus;//オセロの配列の状態を画像と同じように反映させる

        if (clipUp)//挟んでいるかを判定する
        {
            for (int a = 1; a <= Up; a++)//挟んでいる数分繰り返す
            {
                switch (Order)
                {
                    case Othello_Order.Black:

                        OthelloBoard[v - a, h] = 1;
                        OthelloImageArrow[v - a, h].sprite = pieceImage[1].sprite;
                        Debug.Log("黒に変える");
                        OthelloImageArrow[v - a, h].color = Color.white;
    
                        OthelloScriptsArrow[v - a, h].isCurrented = false;

                        break;


                    case Othello_Order.White:

                        OthelloBoard[v - a, h] = 2;
                        OthelloImageArrow[v - a, h].sprite = pieceImage[2].sprite;
                        Debug.Log("白に変える");
                        OthelloImageArrow[v - a, h].color = Color.white;
                        OthelloScriptsArrow[v - a, h].isCurrented = false;


                        break;


                }

            }
        }
        if (clipDown)
        {
            for (int b = 1; b <= Down; b++)
            {
                switch (Order)
                {
                    case Othello_Order.Black:

                        OthelloBoard[v + b, h] = 1;
                        OthelloImageArrow[v + b, h].sprite = pieceImage[1].sprite;
                        OthelloImageArrow[v + b, h].color = Color.white;
                        OthelloScriptsArrow[v + b, h].isCurrented = false;

                        break;


                    case Othello_Order.White:

                        OthelloBoard[v + b, h] = 2;
                        OthelloImageArrow[v + b, h].sprite = pieceImage[2].sprite;
                        OthelloImageArrow[v + b, h].color = Color.white;
                        OthelloScriptsArrow[v + b, h].isCurrented = false;


                        break;


                }

            }

        }
        if (clipLeft)
        {

            for (int c = 1; c <= Left; c++)
            {
                switch (Order)
                {
                    case Othello_Order.Black:

                        OthelloBoard[v, h - c] = 1;
                        OthelloImageArrow[v, h - c].sprite = pieceImage[1].sprite;
                        Debug.Log("黒に変える");
                        OthelloImageArrow[v, h - c].color = Color.white;
                        OthelloScriptsArrow[v, h - c].isCurrented = false;
                        break;


                    case Othello_Order.White:

                        OthelloBoard[v, h - c] = 2;
                        OthelloImageArrow[v, h - c].sprite = pieceImage[2].sprite;
                        Debug.Log("白に変える");
                        OthelloImageArrow[v, h - c].color = Color.white;
                        OthelloScriptsArrow[v, h - c].isCurrented = false;
                        break;


                }

            }

        }
        if (clipRight)
        {
            for (int d = 1; d <= Right; d++)
            {
                switch (Order)
                {
                    case Othello_Order.Black:

                        OthelloBoard[v, h + d] = 1;
                        OthelloImageArrow[v, h + d].sprite = pieceImage[1].sprite;
                        OthelloImageArrow[v, h + d].color = Color.white;
                        OthelloScriptsArrow[v, h + d].isCurrented = false;
                        break;


                    case Othello_Order.White:

                        OthelloBoard[v, h + d] = 2;
                        OthelloImageArrow[v, h + d].sprite = pieceImage[2].sprite;
                        OthelloImageArrow[v, h + d].color = Color.white;
                        OthelloScriptsArrow[v, h + d].isCurrented = false;
                        break;


                }

            }

        }
        if (clipUpLeft)
        {
            for (int e = 1; e <= UpLeft; e++)
            {

                switch (Order)
                {
                    case Othello_Order.Black:

                        OthelloBoard[v - e, h - e] = 1;
                        OthelloImageArrow[v - e, h - e].sprite = pieceImage[1].sprite;
                        OthelloImageArrow[v - e, h - e].color = Color.white;
                        OthelloScriptsArrow[v - e, h - e].isCurrented = false;
                        break;


                    case Othello_Order.White:

                        OthelloBoard[v - e, h - e] = 2;
                        OthelloImageArrow[v - e, h - e].sprite = pieceImage[2].sprite;
                        OthelloImageArrow[v - e, h - e].color = Color.white;
                        OthelloScriptsArrow[v - e, h - e].isCurrented = false;
                        break;


                }


            }

        }
        if (clipUpRight)
        {
            for (int f = 1; f <= UpRight; f++)
            {
                switch (Order)
                {
                    case Othello_Order.Black:

                        OthelloBoard[v - f, h + f] = 1;
                        OthelloImageArrow[v - f, h + f].sprite = pieceImage[1].sprite;
                        OthelloImageArrow[v - f, h + f].color = Color.white;
                        OthelloScriptsArrow[v - f, h + f].isCurrented = false;
                        break;


                    case Othello_Order.White:

                        OthelloBoard[v - f, h + f] = 2;
                        OthelloImageArrow[v - f, h + f].sprite = pieceImage[2].sprite;
                        OthelloImageArrow[v - f, h + f].color = Color.white;
                        OthelloScriptsArrow[v - f, h + f].isCurrented = false;
                        break;


                }

            }

        }
        if (clipDownLeft)
        {
            for (int g = 1; g <= DownLeft; g++)
            {
                switch (Order)
                {
                    case Othello_Order.Black:

                        OthelloBoard[v + g, h - g] = 1;
                        OthelloImageArrow[v + g, h - g].sprite = pieceImage[1].sprite;
                        OthelloImageArrow[v + g, h - g].color = Color.white;
                        OthelloScriptsArrow[v + g, h - g].isCurrented = false;
                        break;


                    case Othello_Order.White:

                        OthelloBoard[v + g, h - g] = 2;
                        OthelloImageArrow[v + g, h - g].sprite = pieceImage[2].sprite;
                        OthelloImageArrow[v + g, h - g].color = Color.white;
                        OthelloScriptsArrow[v + g, h - g].isCurrented = false;
                        break;


                }

            }

        }
        if (clipDownRight)
        {
            for (int i = 1; i <= DownRight; i++)
            {

                switch (Order)
                {
                    case Othello_Order.Black:

                        OthelloBoard[v + i, h + i] = 1;
                        OthelloImageArrow[v + i, h + i].sprite = pieceImage[1].sprite;
                        OthelloImageArrow[v + i, h + i].color = Color.white;
                        OthelloScriptsArrow[v + i, h + i].isCurrented = false;
                        break;


                    case Othello_Order.White:

                        OthelloBoard[v + i, h + i] = 2;
                        OthelloImageArrow[v + i, h + i].sprite = pieceImage[2].sprite;
                        OthelloImageArrow[v + i, h + i].color = Color.white;
                        OthelloScriptsArrow[v + i, h + i].isCurrented = false;
                        break;


                }

            }

        }

        switch (Order)
        {
            case Othello_Order.Black:

                Order = Othello_Order.White;
                if (PlayerTurn == Othello_Order.Black)
                {
                    TurnText.text = "CPのターン";
                    aIBot.AITurn();
                    
                }
                else if(PlayerTurn == Othello_Order.White)
                {
                    TurnText.text = "あなたのターン";
                }else if(aIBot ==null){

                    TurnText.text = "白のターン";


                }


                
                break;



            case Othello_Order.White:

                Order = Othello_Order.Black;
                if (PlayerTurn == Othello_Order.White)
                {
                    TurnText.text = "CPのターン";
                    aIBot.AITurn();
                    
                }
                else if (PlayerTurn == Othello_Order.Black)
                {
                    TurnText.text = "あなたのターン";
                }
                else if (aIBot == null)
                {
                    TurnText.text = "黒のターン";
                }
                break;




        }


        nowBoardStatus();


        pieceCounter.CheckOthelloBoard();


        Pass();


        JudgeWinorlose();
    }


    void JudgeWinorlose()
    {
        bool isJudged = false;

        int none = 0;

        int black = 0;

        int white = 0;

        (none, black, white) = CheckBoard_PieceNumber();

        if (none == 0)
        {
            isJudged = true;
            Debug.Log("通知:全ての盤面が埋まったため 終了します");

            if (black < white)
            {
                Debug.Log("白の勝ち");
                switch (PlayerTurn)
                {
                    case Othello_Order.Black:

                        judgetext.text = "CPの勝ち";

                        judge.SetActive(true);

                        break;
                    case Othello_Order.White:

                        judgetext.text = "あなたの勝ち";

                        judge.SetActive(true);

                        break;
                }
                

            }
            else
            {
                Debug.Log("黒の勝ち");

                switch (PlayerTurn)
                {
                    case Othello_Order.Black:

                        judgetext.text = "あなたの勝ち";

                        judge.SetActive(true);

                        break;
                    case Othello_Order.White:

                        judgetext.text = "CPの勝ち";

                        judge.SetActive(true);

                        break;
                }

            }

        }
        else if (black == 0)
        {
            isJudged = true;
            Debug.Log("通知:黒がなくなったため 終了します");
            Debug.Log("白の勝ち");
            switch (PlayerTurn)
            {
                case Othello_Order.Black:

                    judgetext.text = "CPの勝ち";

                    judge.SetActive(true);

                    break;
                case Othello_Order.White:

                    judgetext.text = "あなたの勝ち";

                    judge.SetActive(true);

                    break;
            }
        }
        else if (white == 0)
        {
            isJudged = true;
            Debug.Log("通知:白がなくなったため 終了します");
            Debug.Log("黒の勝ち");

            switch (PlayerTurn)
            {
                case Othello_Order.Black:

                    judgetext.text = "あなたの勝ち";

                    judge.SetActive(true);

                    break;
                case Othello_Order.White:

                    judgetext.text = "CPの勝ち";

                    judge.SetActive(true);

                    break;
            }
        }
        else
        {
            Debug.Log("通知:勝敗は決まっていません");
        }



        

    }

    void nowBoardStatus()//オセロボードの配列の状況を表示
    {
        string boardStatus = "";
        for (int a = 0; a < 8; a++)
        {

            for (int b = 0; b < 8; b++)
            {
                boardStatus = boardStatus + OthelloBoard[a, b];
            }
            boardStatus = boardStatus + "\n";

            Debug.Log("現在の状態");
            Debug.Log(boardStatus);
        }

    }

    //要素数計算//
    int Calculation_LeftUp_Length(int v, int h)//選択位置から左上のマス数を計算する
    {
        int Length = 0;

        int finishV = v;//1になるまで何回で小さくなるか

        int finishH = h;//1になるまで何回で小さくなるか


        if (finishV < finishH)
        {
            Length = finishV;
        }
        else if (finishH < finishV)
        {
            Length = finishH;
        }
        else
        {
            Length = finishH;
        }



        return Length;

    }


    int Calculation_RightUp_Length(int v, int h)//選択位置から右上のマス数を計算する
    {
        int Length = 0;

        int finishV = v;//1になるまで何回で小さくなるか

        int finishH = 7 - h;//7になるまで何回で大きくなるか


        if (finishV < finishH)
        {
            Length = finishV;
        }
        else if (finishH < finishV)
        {
            Length = finishH;
        }
        else
        {
            Length = finishH;
        }


        return Length;

    }

    int Calculation_LeftDown_Length(int v, int h)//選択位置から左下のマス数を計算する
    {
        int Length = 0;

        int finishV = 7 - v;//7になるまで何回で大きくなるか

        int finishH = h; //1になるまで何回で小さくなるか

        if (finishV < finishH)
        {
            Length = finishV;
        }
        else if (finishH < finishV)
        {
            Length = finishH;
        }
        else
        {
            Length = finishH;
        }

        return Length;

    }

    int Calculation_RightDown_Length(int v, int h)//選択位置からの右下のマス数を計算する
    {

        int Length = 0;

        int finishV = 7 - v;//7になるまで何回で大きくなるか

        int finishH = 7 - h; ;//7になるまで何回で大きくなるか

        if (finishV < finishH)
        {
            Length = finishV;
        }
        else if (finishH < finishV)
        {
            Length = finishH;
        }
        else
        {
            Length = finishH;
        }

        return Length;



        //選択位置からの右上のマス数を計算する

    }


    public (int, int, int) CheckBoard_PieceNumber()
    {
        int none=0;

        int black=0;

        int white=0;

        foreach (int tmp in OthelloBoard)
        {
            switch (tmp)
            {
                case 0:

                    none++;

                    break;

                case 1:

                    black++;

                    break;

                case 2:

                    white++;

                    break;
                default:

                    Debug.LogError("未定義の数字が含まれています");

                    break;
            }
        }

        return (none, black, white);


    }

    void Pass()
    {
        if (PlayerTurn == Order)
        {

            Debug.Log("置けないターンか検証します");

            switch (PlayerTurn)
            {
                case Othello_Order.Black:


                    if (SerchMinePieces(0) == false)
                    {
                        Debug.Log("パス");
                        Order = Othello_Order.White;
                        TurnText.text = "CPのターン";
                        aIBot.AITurn();
                    }
                    break;


                case Othello_Order.White:


                    if (SerchMinePieces(0) == false)
                    {
                        Debug.Log("パス");
                        Order = Othello_Order.Black;
                        TurnText.text = "CPのターン";
                        aIBot.AITurn();
                    }

                    break;


            }
        }
    }


    bool SerchMinePieces(int x)
    {

        int[,] tmp = OthelloBoard;

        int dim = 0;


        List<int[]> MyPiece = new List<int[]>();

        bool isPut= false;
       
            for (int v = 0; v < tmp.GetLength(0); v++)
            {
                for (int h = 0; h < tmp.GetLength(1); h++)
                {
                    if (tmp[v, h] == x)
                    {
                        MyPiece.Add(new int[2]);

                        MyPiece[dim][0] = v;

                        MyPiece[dim][1] = h;

                        dim++;
                    }

                }
            }


        for (int i = 0; i < MyPiece.Count; i++)
        {
            isPut = SerchChengePieces(MyPiece[i][0], MyPiece[i][1]);

            if (isPut)
            {
                break;
            }

        }

        return isPut;
        

    }

}


 
