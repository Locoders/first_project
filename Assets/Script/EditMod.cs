using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class EditMod : MonoBehaviour
{
    public Transform selected;
    public Transform selectbody;
    public Transform core;
    readonly List<int> cop = new() { 1, 1 };
    readonly List<List<int>> Maps = new()
    {
        new List<int>(){0,0,0},
        new List<int>(){0,9,0},
        new List<int>(){0,0,0},
    };
    void OnEnable()
    {
        Camera.main.GetComponent<Following>().enabled = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && selectbody != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            int x = Mathf.RoundToInt(mousePos.x);
            int y = Mathf.RoundToInt(mousePos.y);
            if (selectbody != null)
            {
                Install_check(x, y);
            }
        }
        if (Input.GetMouseButtonDown(1) && selectbody != null)
        {
            if (selectbody != null)
            {
                Destroy(selectbody.gameObject);
                selectbody = null;
                selected = null;
            }
        }
        if (Input.GetMouseButtonUp(2))
        {
            Del_Object();
        }
        if (selectbody != null)
        {
            selectbody.position = Input.mousePosition;
            selectbody.position = Camera.main.ScreenToWorldPoint(selectbody.position);
            selectbody.position = new Vector3(selectbody.position.x, selectbody.position.y, 0);
        }
    }
    public void Del_Object()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        int x = Mathf.RoundToInt(mousePos.x);
        int y = Mathf.RoundToInt(mousePos.y);
        if (Maps[cop[1] - y][cop[0] + x] != 9)
        {
            Maps[cop[1] - y][cop[0] + x] = 0;
            Vector2 point = new(x, y); // 월드 좌표
            print(AllOneConnectedToNine(Maps));
            if (AllOneConnectedToNine(Maps))
            {
                Collider2D[] hits = Physics2D.OverlapPointAll(point);
                foreach (var hit in hits)
                {
                    if (hit.CompareTag("block"))
                    {
                        Destroy(hit.gameObject);
                    }
                }
            }
            else
            {
                Maps[cop[1] - y][cop[0] + x] = 1;
            }
        }
    }
    public void Selected_Object(Transform A)
    {
        selected = A;
        if (!selectbody)
        {
            selectbody = new GameObject("SelectBody").transform;
            selectbody.name = selected.name;
            selectbody.AddComponent<SpriteRenderer>().sprite = selected.GetComponent<Info>()._instsprite;
        }
        else if (selected.name != selectbody.name)
        {
            selectbody.name = selected.name;
            selectbody.GetComponent<SpriteRenderer>().sprite = selected.GetComponent<Info>()._instsprite;
        }
    }
    void Install_check(int x, int y)
    {
        bool isgood = false;
        if (cop[0] + x < 0 || cop[1] - y < 0 || cop[0] + x >= Maps[0].Count || cop[1] - y >= Maps.Count )
        {
            return;
        }
        if (Maps[cop[1] - y][cop[0] + x] == 0)
            {
                // 상, 하, 좌, 우 방향 확인
                List<Vector2Int> directions = new()
            {
                new (0, 1), // 상
                new (-1, 0), // 좌
                new (0, -1),  // 하
                new (1, 0)   // 우
            };
                foreach (var dir in directions)
                {
                    int newY = cop[1] - y + dir.y;
                    int newX = cop[0] + x + dir.x;

                    // 경계 체크
                    if (newY >= 0 && newY < Maps.Count && newX >= 0 && newX < Maps[0].Count)
                    {
                        if (Maps[newY][newX] != 0 && Maps[newY][newX] != 2)
                        {
                            isgood = true;
                            break;
                        }
                    }
                }
            }
        // Maps 크기 조정
        if (isgood)
        {
            Maps[cop[1] - y][cop[0] + x] = 1;
            if (cop[1] - y == 0)
            {
                Maps.Insert(0, Enumerable.Repeat(0, Maps[0].Count).ToList());
                cop[1]++;
            }
            if (cop[0] + x == 0)
            {
                foreach (var row in Maps)
                {
                    row.Insert(0, 0);
                }
                cop[0]++;
            }
            if (cop[1] - y == Maps.Count - 1)
            {
                Maps.Add(Enumerable.Repeat(0, Maps[0].Count).ToList());
            }
            if (cop[0] + x == Maps[0].Count - 1)
            {
                foreach (var row in Maps)
                {
                    row.Add(0);
                }
            }
            // 설치
            Install_block(x, y);
        }
    }
    void Install_block(int x, int y)
    {
        Instantiate(selected, new Vector3(x, y, 0), Quaternion.identity, core.Find("Add"));
    }
    public bool AllOneConnectedToNine(List<List<int>> map)
    {
        int rows = map.Count;
        int cols = map[0].Count;
        bool[,] visited = new bool[rows, cols];
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        // 9에서부터 BFS 시작
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (map[y][x] == 9)
                {
                    queue.Enqueue(new Vector2Int(x, y));
                    visited[y, x] = true;
                }
            }
        }

        // 상하좌우
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        // 9에서 연결된 1 모두 방문
        while (queue.Count > 0)
        {
            Vector2Int cur = queue.Dequeue();
            foreach (var dir in dirs)
            {
                int nx = cur.x + dir.x;
                int ny = cur.y + dir.y;
                if (nx < 0 || ny < 0 || nx >= cols || ny >= rows)
                    continue;
                if (visited[ny, nx])
                    continue;
                if (map[ny][nx] == 1)
                {
                    queue.Enqueue(new Vector2Int(nx, ny));
                    visited[ny, nx] = true;
                }
            }
        }

        // 방문하지 않은 1이 있으면 false
        for (int y = 0; y < rows; y++)
            for (int x = 0; x < cols; x++)
                if (map[y][x] == 1 && !visited[y, x])
                    return false;

        return true;
    }
}