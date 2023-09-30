using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 穴掘り法

public class MakaMaze : MonoBehaviour
{
    public enum MazeData : int
    {
        WALL = 0,
        ROAD = 1,
    }

    [Header("Maze Size")]
    [Range(5, 101)]
    [SerializeField] private int m_width = 5;
    [Range(5, 101)]
    [SerializeField] private int m_height = 5;

    [Header("Test Map Object")]
    [SerializeField] List<GameObject> m_wall_deco_object = new List<GameObject>();
    [SerializeField] GameObject m_road_object = null;

    // 迷路データ
    private MazeData[] m_map; // マップデータ
    private int m_point_num;  // 穴を掘るチェックポイント数

    // up, down, right, left
    private int[] m_x_table = { 0, 0, +2, -2 };
    private int[] m_y_table = { -2, +2, 0, 0 };

    int idx; 

    // Start is called before the first frame update
    void Start()
    {
        // マップサイズを奇数にする
        if ((m_width % 2) == 0) { ++m_width; }
        if ((m_height % 2) == 0) { ++m_height; }

        // 外周(壁)の分を考慮して，マップを生成
        m_map = new MazeData[m_width * m_height];
        // 掘るべきポイント数
        m_point_num = ((m_width - 1) / 2) * ((m_height - 1) / 2) - 1;
        // 穴掘り法
        DigMap();

        // マップ生成
        GenerateMap();
    }

    // 穴掘り法
    private void DigMap()
    {
        int[] rnd_idx = new int[] { 0, 1, 2, 3 };

        // スタック
        Stack<Vector2> stack = new Stack<Vector2>();

        // 座標(奇数)
        int x = (int)UnityEngine.Random.Range(0, m_width / 2) * 2 + 1;
        int y = (int)UnityEngine.Random.Range(0, m_height / 2) * 2 + 1;

        while (m_point_num > 0)
        {
            // ランダムな方向を決定
            int[] idx = rnd_idx.OrderBy(i => Guid.NewGuid()).ToArray();

            // 掘ったか判定
            bool is_dig = false;

            for (int i = 0; i < 4; ++i)
            {
                // 範囲内を掘れるか
                bool is_ok = CheckRange(x, y, idx[i]);
                // 掘れる
                if (is_ok)
                {
                    int current = y * m_width + x;
                    int next1 = (y + m_y_table[idx[i]] / 2) * m_width + (x + m_x_table[idx[i]] / 2);
                    int next2 = (y + m_y_table[idx[i]]) * m_width + (x + m_x_table[idx[i]]);

                    // 2マス先が掘れるか
                    if (m_map[next2] == MazeData.WALL)
                    {
                        m_map[current] = MazeData.ROAD;
                        m_map[next1] = MazeData.ROAD;
                        m_map[next2] = MazeData.ROAD;

                        x = x + m_x_table[idx[i]];
                        y = y + m_y_table[idx[i]];

                        stack.Push(new Vector2(x, y));

                        is_dig = true;

                        --m_point_num;

                        break;
                    }
                }
            }

            // 掘れていない場合は，ランダムな位置を選択
            if (!is_dig)
            {
                var vec = stack.Pop();
                x = (int)vec.x;
                y = (int)vec.y;
            }
            if (m_point_num <= 0) { break; }
        }
    }
    // 範囲チェック
    private bool CheckRange(int x, int y, int idx)
    {
        int nx = x + m_x_table[idx];
        if (nx < 0 || nx >= m_width) { return false; }
        int ny = y + m_y_table[idx];
        if (ny < 0 || ny >= m_height) { return false; }

        return true;
    }

    // マップ生成
    private void GenerateMap()
    {
        // 床の中心地
        float x_center = (m_width - 1) / 2.0f;
        float y_center = (m_height - 1) / 2.0f;

        for (int i = 0; i < (m_height); ++i)
        {
            for (int j = 0; j < (m_width); ++j)
            {
                var idx = i * (m_width) + j;

                GameObject game_object = null;
                if (m_map[idx] == MazeData.WALL)
                {
                    CreateWall(new Vector3(j - x_center, i - y_center, 0));
                }
                else if (m_map[idx] == MazeData.ROAD)
                {
                    game_object = Instantiate(m_road_object, new Vector3(j - x_center, i - y_center, 0), Quaternion.identity);
                    game_object.transform.parent = this.transform.Find("Roads");
                }
            }
        }

        // 壁生成
        void CreateWall(Vector3 pos)
        {
            GameObject game_object = null;

            game_object = Instantiate(m_wall_deco_object[idx], pos, Quaternion.identity);
            game_object.transform.parent = this.transform.Find("Walls");
        }
    }
}