using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBot : MonoBehaviour
{
    [SerializeField] Othello othello;
    [SerializeField] MainScript main;

    int[,] NowOthelloBoard = new int[8, 8];
    int AIturn = 0;


    int[,] OThelloEvaluation = new int[8, 8]
   {
        {100,20,80,80,80,80,20,100},
        {20,30,30,30,30,30,30,20},
        {80,60,70,55,55,70,60,80},
        {80,60,55,50,50,55,60,80},
        {80,60,55,50,50,55,60,80},
        {80,60,70,55,55,70,60,80},
        {20,30,30,30,30,30,30,20},
        {100,20,80,80,80,80,20,100}
   };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public void AITurn()
    {
        bool isput = false;

        List<int[]> CanChangePieces = new List<int[]>();

        

        int change_piece_num = 0;

        NowOthelloBoard = main.OthelloBoard;//現在のオセロの盤面を持ってくる


        debugNowOthelloBoard();


        for (int v = 0; v < NowOthelloBoard.GetLength(0); v++)
        {
            for (int h = 0; h < NowOthelloBoard.GetLength(1); h++)
            {


                Debug.Log("AI:" + v + "," + h + "を検索中");
                isput = main.SerchChengePieces(v, h);
                if (isput)
                {
                    Debug.Log("AI:置ける場所が検出されました" + v + "," + h +"List" +change_piece_num+"行目に配置します");
                    CanChangePieces.Add(new int[2]);//out of range のエラー・・・なんで・・・
                    CanChangePieces[change_piece_num][0] = v;
                    CanChangePieces[change_piece_num][1] = h;
                    //  CanChangePieces.Add(h);
                    change_piece_num++;
                    main.SerchCanceled();
                }
                else
                {
                    main.SerchCanceled();
                }
            }
        }

        Debug.Log("置ける場所の評価をします");

        

        int max = 0;
        int vertical =0;
        int horizontal = 0;

        for (int i = 0; i < CanChangePieces.Count; i++)
        {

           
            if(OThelloEvaluation[CanChangePieces[i][0], CanChangePieces[i][1]] > max)
            {

                if(main.OthelloScriptsArrow[CanChangePieces[i][0], CanChangePieces[i][1]].AIisOnEnter() ==true)
                {
                    Debug.Log("配置検証:設置可能 一番評価の高い座標に代入します");
                    max = OThelloEvaluation[CanChangePieces[i][0], CanChangePieces[i][1]];

                    vertical = CanChangePieces[i][0];

                    horizontal = CanChangePieces[i][1];
                }
                else
                {
                    Debug.Log("配置検証結果:設置不可能他を探します");
                }
                    
            }
        }

        Debug.Log("最も評価の高い座標は" + vertical +"," + horizontal +"でした");
        Debug.Log("配置します");

        othello = main.OthelloScriptsArrow[vertical, horizontal];

        othello.OnEnter();
        othello.OnClick();

    }



    void debugNowOthelloBoard()
    {

        string boardStatus = "";
        for (int a = 0; a < 8; a++)
        {

            for (int b = 0; b < 8; b++)
            {
                boardStatus = boardStatus + NowOthelloBoard[a, b];
            }
            boardStatus = boardStatus + "\n";

            Debug.Log("AIが盤面を検出");
            Debug.Log(boardStatus);
        }
    }
}
