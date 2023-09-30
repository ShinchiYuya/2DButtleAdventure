using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ���@��@

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

    // ���H�f�[�^
    private MazeData[] m_map; // �}�b�v�f�[�^
    private int m_point_num;  // �����@��`�F�b�N�|�C���g��

    // up, down, right, left
    private int[] m_x_table = { 0, 0, +2, -2 };
    private int[] m_y_table = { -2, +2, 0, 0 };

    int idx; 

    // Start is called before the first frame update
    void Start()
    {
        // �}�b�v�T�C�Y����ɂ���
        if ((m_width % 2) == 0) { ++m_width; }
        if ((m_height % 2) == 0) { ++m_height; }

        // �O��(��)�̕����l�����āC�}�b�v�𐶐�
        m_map = new MazeData[m_width * m_height];
        // �@��ׂ��|�C���g��
        m_point_num = ((m_width - 1) / 2) * ((m_height - 1) / 2) - 1;
        // ���@��@
        DigMap();

        // �}�b�v����
        GenerateMap();
    }

    // ���@��@
    private void DigMap()
    {
        int[] rnd_idx = new int[] { 0, 1, 2, 3 };

        // �X�^�b�N
        Stack<Vector2> stack = new Stack<Vector2>();

        // ���W(�)
        int x = (int)UnityEngine.Random.Range(0, m_width / 2) * 2 + 1;
        int y = (int)UnityEngine.Random.Range(0, m_height / 2) * 2 + 1;

        while (m_point_num > 0)
        {
            // �����_���ȕ���������
            int[] idx = rnd_idx.OrderBy(i => Guid.NewGuid()).ToArray();

            // �@����������
            bool is_dig = false;

            for (int i = 0; i < 4; ++i)
            {
                // �͈͓����@��邩
                bool is_ok = CheckRange(x, y, idx[i]);
                // �@���
                if (is_ok)
                {
                    int current = y * m_width + x;
                    int next1 = (y + m_y_table[idx[i]] / 2) * m_width + (x + m_x_table[idx[i]] / 2);
                    int next2 = (y + m_y_table[idx[i]]) * m_width + (x + m_x_table[idx[i]]);

                    // 2�}�X�悪�@��邩
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

            // �@��Ă��Ȃ��ꍇ�́C�����_���Ȉʒu��I��
            if (!is_dig)
            {
                var vec = stack.Pop();
                x = (int)vec.x;
                y = (int)vec.y;
            }
            if (m_point_num <= 0) { break; }
        }
    }
    // �͈̓`�F�b�N
    private bool CheckRange(int x, int y, int idx)
    {
        int nx = x + m_x_table[idx];
        if (nx < 0 || nx >= m_width) { return false; }
        int ny = y + m_y_table[idx];
        if (ny < 0 || ny >= m_height) { return false; }

        return true;
    }

    // �}�b�v����
    private void GenerateMap()
    {
        // ���̒��S�n
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

        // �ǐ���
        void CreateWall(Vector3 pos)
        {
            GameObject game_object = null;

            game_object = Instantiate(m_wall_deco_object[idx], pos, Quaternion.identity);
            game_object.transform.parent = this.transform.Find("Walls");
        }
    }
}