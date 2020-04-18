using UnityEngine;

public class PipeSetter : PuzzleSetter
{
    [SerializeField] public GameObject horizontalPipe;
    [SerializeField] public GameObject cornerPipe;
    [SerializeField] public GameObject container;

    [SerializeField] public Vector2[] cor;
    [SerializeField] public Vector2[] hor;

    private static readonly Vector2[] cor1 = new Vector2[13] { new Vector2(-5.3f, 1.2f), new Vector2(-3.3f, 1.2f), new Vector2(-1.3f, 1.2f), new Vector2(0.7f, 1.2f), new Vector2(-3.3f, 3.2f), new Vector2(-1.3f, 3.2f), new Vector2(0.7f, 3.2f), new Vector2(-3.3f, -1.8f), new Vector2(-1.3f, -0.3f), new Vector2(0.7f, -0.8f), new Vector2(5.7f, 0.2f), new Vector2(3.7f, 0.2f), new Vector2(3.7f, 3.2f) };
    private static readonly Vector2[] hor1 = new Vector2[7] { new Vector2(-3.3f, -0.3f), new Vector2(-1.8f, -1.8f), new Vector2(-0.8f, -1.8f), new Vector2(2.2f, -0.8f), new Vector2(3.2f, -0.8f), new Vector2(3.7f, 1.7f), new Vector2(2.2f, 3.2f) };

    private static readonly Vector2[] cor2 = new Vector2[14] { new Vector2(-5.3f, 1.2f), new Vector2(-3.3f, 1.2f), new Vector2(-3.3f, -2.7f), new Vector2(-1.3f, -2.7f), new Vector2(-1.3f, -0.7f), new Vector2(-3.3f, 3.2f), new Vector2(-0.3f, 3.2f), new Vector2(1.7f, -2.7f), new Vector2(4.7f, -2.7f), new Vector2(1.7f, -0.7f), new Vector2(3.7f, -0.7f), new Vector2(5.7f, -0.7f), new Vector2(3.7f, 1.3f), new Vector2(1.7f, 1.3f) };
    private static readonly Vector2[] hor2 = new Vector2[7] { new Vector2(-3.3f, -0.3f), new Vector2(-3.3f, -1.2f), new Vector2(0.2f, -0.7f), new Vector2(5.7f, 0.8f), new Vector2(3.2f, -2.7f), new Vector2(-1.8f, 3.2f), new Vector2(-0.3f, 1.7f) };

    private static readonly Vector2[] cor3 = new Vector2[14] { new Vector2(-5.3f, 1.2f), new Vector2(-3.3f, 1.2f), new Vector2(-3.3f, 4.2f), new Vector2(0.7f, 4.2f), new Vector2(0.7f, 2.2f), new Vector2(-1.3f,2.2f), new Vector2(3.7f, 2.2f), new Vector2(3.7f, 4.2f), new Vector2(-1.3f, 0.2f), new Vector2(0.7f, 0.2f), new Vector2(0.7f, -1.8f), new Vector2(-3.3f, -1.8f), new Vector2(2.7f, 0.3f), new Vector2(5.7f, -1.8f) };
    private static readonly Vector2[] hor3 = new Vector2[12] { new Vector2(-3.3f, 2.7f), new Vector2(-1.8f, 4.2f), new Vector2(-0.8f, 4.2f), new Vector2(2.2f, 2.2f), new Vector2(-0.8f, -1.8f), new Vector2(-1.8f, -1.8f), new Vector2(-3.3f, -0.3f), new Vector2(2.2f, -1.8f), new Vector2(3.2f, -1.8f), new Vector2(4.2f, -1.8f), new Vector2(5.7f,-0.3f), new Vector2(5.7f, 0.7f)};

    private static float RandomRotation => 90f * Random.Range(0, 4);

    protected override void SetPuzzle(int selector)
    {
        switch (selector) {
            case 0:
                cor = cor1;
                hor = hor1;
                break;
            case 1:
                cor = cor2;
                hor = hor2;
                break;
            case 2:
                cor = cor3;
                hor = hor3;
                break;
        }
        for (int i = 0; i <cor.Length; i++)
        {
            GameObject pipe = Instantiate(cornerPipe) as GameObject;
            pipe.transform.SetParent(container.GetComponent<Transform>());
            pipe.transform.position = new Vector3(cor[i][0], cor[i][1], cornerPipe.transform.position.z);
            pipe.transform.rotation = new Quaternion(0f, 0f, RandomRotation, 0f);
        }
        for (int i = 0; i < hor.Length; i++) {
            GameObject pipe = Instantiate(horizontalPipe) as GameObject;
            pipe.transform.SetParent(container.GetComponent<Transform>());
            pipe.transform.position = new Vector3(hor[i][0], hor[i][1], horizontalPipe.transform.position.z);
            pipe.transform.rotation = new Quaternion(0f, 0f, RandomRotation, 0f);
        }
    }


}
