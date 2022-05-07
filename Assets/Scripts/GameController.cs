using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private SharkContoller sharkContoller;
    [SerializeField] private ObstacleRemover obstacleRemover;
    [SerializeField] private ObstacleController obstacleController;
    

    private void Awake()
    {
        const float defaultScreenRatio = 4f / 3f;
        var ratio = Camera.main.aspect / defaultScreenRatio;

        sharkContoller.Setup(-Camera.main.orthographicSize * ratio);
        obstacleRemover.Setup(-Camera.main.orthographicSize * ratio - 3);
        obstacleController.Setup(Camera.main.orthographicSize * ratio + 5);
    }

}
