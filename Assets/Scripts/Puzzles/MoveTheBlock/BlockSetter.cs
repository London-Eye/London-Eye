using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSetter : MonoBehaviour
{
    [SerializeField] public GameObject container;
    [SerializeField] public BlockMovement originalBlock;
    public int selector;

    private readonly Vector2[] bP1 = new Vector2[11] { new Vector2(-2.5f, -2.5f), new Vector2(-0.5f, -3f), new Vector2(2f, -3f), new Vector2(2f, -2f), new Vector2(-0.5f, -1.5f), new Vector2(0.5f, -1.5f), new Vector2(2.5f, 0f), new Vector2(1.5f, 1.5f), new Vector2(0.5f, 1.5f), new Vector2(-1f, 1f), new Vector2(-2.5f, 1.5f) };
    private readonly Vector2[] bS1 = new Vector2[11] { new Vector2(1f, 2f), new Vector2(3f, 1f), new Vector2(2f, 1f), new Vector2(2f, 1f), new Vector2(1f, 2f), new Vector2(1f, 2f), new Vector2(1f, 3f), new Vector2(1f, 2f), new Vector2(1f, 2f), new Vector2(2f, 1f), new Vector2(1f, 2f) };

    private readonly Vector2[] bP2 = new Vector2[11] { new Vector2(-2.5f, -1.5f ), new Vector2(-1f,-2f ), new Vector2(0.5f,-3f ),new Vector2(2.5f,-2.5f ),new Vector2(1.5f,-1f ),new Vector2(0.5f, -0.5f ),new Vector2(-0.5f, -0.5f  ),new Vector2(2.5f,0.5f ), new Vector2(1.5f, 2f ), new Vector2(-0.5f,1f ),new Vector2(-2f,2f ) };
    private readonly Vector2[] bS2 = new Vector2[11] { new Vector2(1f, 2f ), new Vector2(2f, 1f ), new Vector2(3f, 1f ), new Vector2(1f, 2f ), new Vector2(1f, 3f ),new Vector2(1f, 2f ),new Vector2(1f,2f ),new Vector2(1f, 2f ), new Vector2(3f, 1f ), new Vector2(3f, 1f ), new Vector2(2f,1f ) };

    private readonly Vector2[] bP3 = new Vector2[11] { new Vector2(-2.5f, -2.5f),new Vector2(-1f, -3f), new Vector2(2f, -3f), new Vector2(0.5f, -2f), new Vector2(2.5f, -1.5f),new Vector2(0f, -1f), new Vector2(1.5f, -0.5f),new Vector2(-0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 2f), new Vector2(2.5f, 1.5f) };
    private readonly Vector2[] bS3 = new Vector2[11] { new Vector2(1f, 2f), new Vector2(2f, 1f),new Vector2(2f, 1f), new Vector2(3f, 1f), new Vector2(1f, 2f), new Vector2(2f, 1f), new Vector2(1f, 2f), new Vector2(1f, 2f),  new Vector2(1f, 2f), new Vector2(3f, 1f), new Vector2(1f, 2f) };

    private readonly Vector2[] bP4 = new Vector2[11] { new Vector2(-2.5f, -2f), new Vector2(-0.5f, -3f), new Vector2(-0.5f, -2f),new Vector2(2.5f, -2f),new Vector2(0.5f, -1f),new Vector2(2.5f, 0.5f), new Vector2(1.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(2f,2f),new Vector2(-2f, 1f), new Vector2(-1.5f, 2f) };
    private readonly Vector2[] bS4 = new Vector2[11] { new Vector2(1f, 3f), new Vector2(3f, 1f), new Vector2(3f, 1f), new Vector2(1f, 3f),  new Vector2(3f, 1f), new Vector2(1f, 2f), new Vector2(1f, 2f), new Vector2(1f, 2f), new Vector2(2f, 1f), new Vector2(2f, 1f), new Vector2(3f, 1f) };

    private Vector2[] blockPosition = new Vector2[11];
    private  Vector2[] blockScale = new Vector2[11];

    public void SetBlocks() {
        blockPosition = bP4;
        blockScale = bS4;
        selector = UnityEngine.Random.Range(1, 5);
        switch (selector) {
            case 1:
                blockPosition = bP1;
                blockScale = bS1;
                break;
            case 2:
                blockPosition = bP2;
                blockScale = bS2;
                break;
            case 3:
                blockPosition = bP3;
                blockScale = bS3;
                break;
            case 4:
                blockPosition = bP4;
                blockScale = bS4;
                break;
        }

        for (int i = 0; i < blockPosition.Length; i++)
        {
            BlockMovement block = Instantiate(originalBlock) as BlockMovement;
            block.transform.SetParent(container.GetComponent<Transform>());
            block.transform.position = new Vector3(blockPosition[i][0], blockPosition[i][1], originalBlock.transform.position.z);
            block.transform.GetChild(0).localScale = new Vector3(blockScale[i][0], blockScale[i][1], 1);
        }
    }
}
