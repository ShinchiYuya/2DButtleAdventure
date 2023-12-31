using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DiggingMethod : MonoBehaviour
{
    /* 設定する値 */
    [SerializeField, Header("縦横のサイズ ※必ず奇数にすること")] public int max; //縦横のサイズ ※必ず奇数にすること
    public GameObject wall;    //壁用オブジェクト
    public GameObject floor;    //床用オブジェクト
    public GameObject start;   //スタート地点に配置するオブジェクト
    public GameObject goal;    //ゴール地点に配置するオブジェクト

    /* 内部パラメータ */
    private int[,] walls;      //マップの状態 0：壁 1：通路
    private int[] startPos;    //スタートの座標
    private int[] goalPos;     //ゴールの座標

    void Start()
    {
        //マップ状態初期化
        walls = new int[max, max];

        //スタート地点の取得
        startPos = GetStartPosition();

        //通路の生成
        //初回はゴール地点を設定する
        goalPos = MakeDungeonMap(startPos);
        //通路生成を繰り返して袋小路を減らす
        int[] tmpStart = goalPos;
        for (int i = 0; i < max * 5; i++)
        {
            MakeDungeonMap(tmpStart);
            tmpStart = GetStartPosition();
        }

        //マップの状態に応じて壁と通路を生成する
        BuildDungeon();

        //スタート地点とゴール地点にオブジェクトを配置する
        //初回で取得したスタート地点とゴール地点は必ずつながっているので破綻しない
        GameObject startObj = Instantiate(start, new Vector3(startPos[0], 1, startPos[1]), Quaternion.identity) as GameObject;
        GameObject goalObj = Instantiate(goal, new Vector3(goalPos[0], 1, goalPos[1]), Quaternion.identity) as GameObject;
        startObj.transform.parent = transform;
        goalObj.transform.parent = transform;
    }

    /* スタート地点の取得 */
    int[] GetStartPosition()
    {
        //ランダムでx,yを設定
        int randx = Random.Range(0, max);
        int randy = Random.Range(0, max);

        //x、yが両方共偶数になるまで繰り返す
        while (randx % 2 != 0 || randy % 2 != 0)
        {
            randx = Mathf.RoundToInt(Random.Range(0, max));
            randy = Mathf.RoundToInt(Random.Range(0, max));
        }

        return new int[] { randx, randy };
    }

    /* マップ生成 */
    int[] MakeDungeonMap(int[] _startPos)
    {
        // スタート位置配列を複製
        int[] tmpStartPos = new int[2];
        _startPos.CopyTo(tmpStartPos, 0);
        // 移動可能な座標のリストを取得
        Dictionary<int, int[]> movePos = GetPosition(tmpStartPos);

        // 移動可能な座標がなくなるまで探索を繰り返す
        while (movePos != null)
        {
            // 移動可能な座標からランダムで1つ取得し通路にする
            int[] tmpPos = movePos[Random.Range(0, movePos.Count)];
            walls[tmpPos[0], tmpPos[1]] = 1;

            // 元の地点と通路にした座標の間を通路にする
            int xPos = tmpPos[0] + (tmpStartPos[0] - tmpPos[0]) / 2;
            int yPos = tmpPos[1] + (tmpStartPos[1] - tmpPos[1]) / 2;
            walls[xPos, yPos] = 1;

            // 移動後の座標を一時変数に格納し、再度移動可能な座標を探索する
            tmpStartPos = tmpPos;
            movePos = GetPosition(tmpStartPos);

            // ゴール座標に到達した場合、経路がスタートからゴールまで続いているか確認
            /*if (tmpStartPos[0] == goalPos[0] && tmpStartPos[1] == goalPos[1] && !IsConnected(startPos, goalPos))
            {
                // スタートからゴールまで続かない場合、通路を元に戻す
                walls[tmpPos[0], tmpPos[1]] = 0;
                int xPosRevert = tmpPos[0] + (tmpStartPos[0] - tmpPos[0]) / 2;
                int yPosRevert = tmpPos[1] + (tmpStartPos[1] - tmpPos[1]) / 2;
                walls[xPosRevert, yPosRevert] = 0;
                tmpStartPos = startPos;
            }*/
        }
        // 探索終了時の座標を返す
        return tmpStartPos;
    }

    bool IsConnected(int[] start, int[] goal)
    {
        // 2次元配列 visited は探索済みの位置を記録します。
        bool[,] visited = new bool[max, max];

        // 深さ優先探索を実行します。
        return DFS(start[0], start[1], goal[0], goal[1], visited);
    }

    bool DFS(int x, int y, int goalX, int goalY, bool[,] visited)
    {
        // ゴールに到達した場合、経路が続いているとみなします。
        if (x == goalX && y == goalY)
        {
            return true;
        }

        // 現在位置を探索済みとマークします。
        visited[x, y] = true;

        // 上下左右の4方向を探索します。
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int newX = x + dx[i];
            int newY = y + dy[i];

            // 新しい位置が範囲内で通路であり、まだ探索していない場合、再帰的に探索します。
            if (IsInRange(newX, newY) && walls[newX, newY] == 1 && !visited[newX, newY])
            {
                if (DFS(newX, newY, goalX, goalY, visited))
                {
                    return true; // ゴールまでの経路が見つかった場合、true を返します。
                }
            }
        }

        return false; // ゴールまでの経路が見つからなかった場合、false を返します。
    }

    bool IsInRange(int x, int y)
    {
        return x >= 0 && x < max && y >= 0 && y < max;
    }

    /* 移動可能な座標のリストを取得する */
    Dictionary<int, int[]> GetPosition(int[] _startPos)
    {
        //可読性のため座標を変数に格納
        int x = _startPos[0];
        int y = _startPos[1];

        //移動方向毎に2つ先のx,y座標を仮計算
        List<int[]> position = new List<int[]> {
            new int[] {x, y + 2},
            new int[] {x, y - 2},
            new int[] {x + 2, y},
            new int[] {x - 2, y}
        };

        //移動方向毎に移動先の座標が範囲内かつ壁であるかを判定する
        //真であれば、返却用リストに追加する
        Dictionary<int, int[]> positions = position.Where(p => !IsOutOfRange(p[0], p[1]) && walls[p[0], p[1]] == 0).Select((p, i) => new { p, i }).ToDictionary(p => p.i, p => p.p);
        //移動可能な場所が存在しない場合nullを返す
        return positions.Count() != 0 ? positions : null;
    }

    //与えられたx、y座標が範囲外の場合真を返す
    bool IsOutOfRange(int x, int y)
    {
        return (x < 0 || y < 0 || x >= max || y >= max);
    }

    //パラメータに応じてオブジェクトを生成する
    void BuildDungeon()
    {
        //縦横1マスずつ大きくループを回し、壁とする
        for (int i = -1; i <= max; i++)
        {
            for (int j = -1; j <= max; j++)
            {
                //範囲外、または壁の場合に壁オブジェクトを生成する
                if (IsOutOfRange(i, j) || walls[i, j] == 0)
                {
                    if (!IsNearExistingWall(i, j))
                    {
                        GameObject wallObj = Instantiate(wall, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        wallObj.transform.parent = transform;
                    }
                }

                //全ての場所に床オブジェクトを生成
                GameObject floorObj = Instantiate(floor, new Vector3(i, j, 1), Quaternion.identity) as GameObject;
                floorObj.transform.parent = transform;
            }
        }
    }

    bool IsNearExistingWall(int x, int y)
    {
        for (int i = 1; i <= 1; i++)
        {
            for (int j = 1; j <= 1; j++)
            {
                int checkX = x + i;
                int checkY = y + j;

                // 範囲内かつ壁の場合、既存の壁が1マス以内に存在する
                if (!IsOutOfRange(checkX, checkY) && walls[checkX, checkY] == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
