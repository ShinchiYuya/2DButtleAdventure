using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DiggingMethod : MonoBehaviour
{
    /* �ݒ肷��l */
    [SerializeField, Header("�c���̃T�C�Y ���K����ɂ��邱��")] public int max; //�c���̃T�C�Y ���K����ɂ��邱��
    public GameObject wall;    //�Ǘp�I�u�W�F�N�g
    public GameObject floor;    //���p�I�u�W�F�N�g
    public GameObject start;   //�X�^�[�g�n�_�ɔz�u����I�u�W�F�N�g
    public GameObject goal;    //�S�[���n�_�ɔz�u����I�u�W�F�N�g

    /* �����p�����[�^ */
    private int[,] walls;      //�}�b�v�̏�� 0�F�� 1�F�ʘH
    private int[] startPos;    //�X�^�[�g�̍��W
    private int[] goalPos;     //�S�[���̍��W

    void Start()
    {
        //�}�b�v��ԏ�����
        walls = new int[max, max];

        //�X�^�[�g�n�_�̎擾
        startPos = GetStartPosition();

        //�ʘH�̐���
        //����̓S�[���n�_��ݒ肷��
        goalPos = MakeDungeonMap(startPos);
        //�ʘH�������J��Ԃ��đ܏��H�����炷
        int[] tmpStart = goalPos;
        for (int i = 0; i < max * 5; i++)
        {
            MakeDungeonMap(tmpStart);
            tmpStart = GetStartPosition();
        }

        //�}�b�v�̏�Ԃɉ����ĕǂƒʘH�𐶐�����
        BuildDungeon();

        //�X�^�[�g�n�_�ƃS�[���n�_�ɃI�u�W�F�N�g��z�u����
        //����Ŏ擾�����X�^�[�g�n�_�ƃS�[���n�_�͕K���Ȃ����Ă���̂Ŕj�]���Ȃ�
        GameObject startObj = Instantiate(start, new Vector3(startPos[0], 1, startPos[1]), Quaternion.identity) as GameObject;
        GameObject goalObj = Instantiate(goal, new Vector3(goalPos[0], 1, goalPos[1]), Quaternion.identity) as GameObject;
        startObj.transform.parent = transform;
        goalObj.transform.parent = transform;
    }

    /* �X�^�[�g�n�_�̎擾 */
    int[] GetStartPosition()
    {
        //�����_����x,y��ݒ�
        int randx = Random.Range(0, max);
        int randy = Random.Range(0, max);

        //x�Ay�������������ɂȂ�܂ŌJ��Ԃ�
        while (randx % 2 != 0 || randy % 2 != 0)
        {
            randx = Mathf.RoundToInt(Random.Range(0, max));
            randy = Mathf.RoundToInt(Random.Range(0, max));
        }

        return new int[] { randx, randy };
    }

    /* �}�b�v���� */
    int[] MakeDungeonMap(int[] _startPos)
    {
        // �X�^�[�g�ʒu�z��𕡐�
        int[] tmpStartPos = new int[2];
        _startPos.CopyTo(tmpStartPos, 0);
        // �ړ��\�ȍ��W�̃��X�g���擾
        Dictionary<int, int[]> movePos = GetPosition(tmpStartPos);

        // �ړ��\�ȍ��W���Ȃ��Ȃ�܂ŒT�����J��Ԃ�
        while (movePos != null)
        {
            // �ړ��\�ȍ��W���烉���_����1�擾���ʘH�ɂ���
            int[] tmpPos = movePos[Random.Range(0, movePos.Count)];
            walls[tmpPos[0], tmpPos[1]] = 1;

            // ���̒n�_�ƒʘH�ɂ������W�̊Ԃ�ʘH�ɂ���
            int xPos = tmpPos[0] + (tmpStartPos[0] - tmpPos[0]) / 2;
            int yPos = tmpPos[1] + (tmpStartPos[1] - tmpPos[1]) / 2;
            walls[xPos, yPos] = 1;

            // �ړ���̍��W���ꎞ�ϐ��Ɋi�[���A�ēx�ړ��\�ȍ��W��T������
            tmpStartPos = tmpPos;
            movePos = GetPosition(tmpStartPos);

            // �S�[�����W�ɓ��B�����ꍇ�A�o�H���X�^�[�g����S�[���܂ő����Ă��邩�m�F
            /*if (tmpStartPos[0] == goalPos[0] && tmpStartPos[1] == goalPos[1] && !IsConnected(startPos, goalPos))
            {
                // �X�^�[�g����S�[���܂ő����Ȃ��ꍇ�A�ʘH�����ɖ߂�
                walls[tmpPos[0], tmpPos[1]] = 0;
                int xPosRevert = tmpPos[0] + (tmpStartPos[0] - tmpPos[0]) / 2;
                int yPosRevert = tmpPos[1] + (tmpStartPos[1] - tmpPos[1]) / 2;
                walls[xPosRevert, yPosRevert] = 0;
                tmpStartPos = startPos;
            }*/
        }
        // �T���I�����̍��W��Ԃ�
        return tmpStartPos;
    }

    bool IsConnected(int[] start, int[] goal)
    {
        // 2�����z�� visited �͒T���ς݂̈ʒu���L�^���܂��B
        bool[,] visited = new bool[max, max];

        // �[���D��T�������s���܂��B
        return DFS(start[0], start[1], goal[0], goal[1], visited);
    }

    bool DFS(int x, int y, int goalX, int goalY, bool[,] visited)
    {
        // �S�[���ɓ��B�����ꍇ�A�o�H�������Ă���Ƃ݂Ȃ��܂��B
        if (x == goalX && y == goalY)
        {
            return true;
        }

        // ���݈ʒu��T���ς݂ƃ}�[�N���܂��B
        visited[x, y] = true;

        // �㉺���E��4������T�����܂��B
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int newX = x + dx[i];
            int newY = y + dy[i];

            // �V�����ʒu���͈͓��ŒʘH�ł���A�܂��T�����Ă��Ȃ��ꍇ�A�ċA�I�ɒT�����܂��B
            if (IsInRange(newX, newY) && walls[newX, newY] == 1 && !visited[newX, newY])
            {
                if (DFS(newX, newY, goalX, goalY, visited))
                {
                    return true; // �S�[���܂ł̌o�H�����������ꍇ�Atrue ��Ԃ��܂��B
                }
            }
        }

        return false; // �S�[���܂ł̌o�H��������Ȃ������ꍇ�Afalse ��Ԃ��܂��B
    }

    bool IsInRange(int x, int y)
    {
        return x >= 0 && x < max && y >= 0 && y < max;
    }

    /* �ړ��\�ȍ��W�̃��X�g���擾���� */
    Dictionary<int, int[]> GetPosition(int[] _startPos)
    {
        //�ǐ��̂��ߍ��W��ϐ��Ɋi�[
        int x = _startPos[0];
        int y = _startPos[1];

        //�ړ���������2���x,y���W�����v�Z
        List<int[]> position = new List<int[]> {
            new int[] {x, y + 2},
            new int[] {x, y - 2},
            new int[] {x + 2, y},
            new int[] {x - 2, y}
        };

        //�ړ��������Ɉړ���̍��W���͈͓����ǂł��邩�𔻒肷��
        //�^�ł���΁A�ԋp�p���X�g�ɒǉ�����
        Dictionary<int, int[]> positions = position.Where(p => !IsOutOfRange(p[0], p[1]) && walls[p[0], p[1]] == 0).Select((p, i) => new { p, i }).ToDictionary(p => p.i, p => p.p);
        //�ړ��\�ȏꏊ�����݂��Ȃ��ꍇnull��Ԃ�
        return positions.Count() != 0 ? positions : null;
    }

    //�^����ꂽx�Ay���W���͈͊O�̏ꍇ�^��Ԃ�
    bool IsOutOfRange(int x, int y)
    {
        return (x < 0 || y < 0 || x >= max || y >= max);
    }

    //�p�����[�^�ɉ����ăI�u�W�F�N�g�𐶐�����
    void BuildDungeon()
    {
        //�c��1�}�X���傫�����[�v���񂵁A�ǂƂ���
        for (int i = -1; i <= max; i++)
        {
            for (int j = -1; j <= max; j++)
            {
                //�͈͊O�A�܂��͕ǂ̏ꍇ�ɕǃI�u�W�F�N�g�𐶐�����
                if (IsOutOfRange(i, j) || walls[i, j] == 0)
                {
                    if (!IsNearExistingWall(i, j))
                    {
                        GameObject wallObj = Instantiate(wall, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        wallObj.transform.parent = transform;
                    }
                }

                //�S�Ă̏ꏊ�ɏ��I�u�W�F�N�g�𐶐�
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

                // �͈͓����ǂ̏ꍇ�A�����̕ǂ�1�}�X�ȓ��ɑ��݂���
                if (!IsOutOfRange(checkX, checkY) && walls[checkX, checkY] == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
