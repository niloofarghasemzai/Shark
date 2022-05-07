using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private float startSpawnTime = 3f;
    [SerializeField] private float hardship = 10;
    
    [SerializeField] private SharkContoller sharkContoller;
    [SerializeField] private ObstacleRemover obstacleRemover;
    [SerializeField] private List<Obstacle> deActiveObstacles;
    [SerializeField] private List<Obstacle> activeObstacles;

    private float _spawnTimeSpend = 0;
    private float _spawnPos;
    private Camera _camera;

    public void Setup(float spawnPos)
    {
        _spawnPos = spawnPos;
        _camera = Camera.main;
        sharkContoller.OnDestroyObstacle += DestroyObstacle;
        obstacleRemover.OnDestroyObstacle += DestroyObstacle;
    }

    private void Update()
    {
        _spawnTimeSpend += Time.deltaTime;
        if (_spawnTimeSpend <= startSpawnTime) return;
        _spawnTimeSpend = 0;

        if (deActiveObstacles.Count > 0)
        {
            startSpawnTime -= Time.deltaTime * hardship;
            var randomIndex = Random.Range(0, deActiveObstacles.Count);
            var orthographicSize = _camera.orthographicSize;
            var randomYPos = Random.Range(-orthographicSize + 2f, orthographicSize - 2f);
            var obstacle = deActiveObstacles[randomIndex];
            obstacle.transform.position = new Vector3(_spawnPos, randomYPos, 0);
            obstacle.gameObject.SetActive(true);
            deActiveObstacles.Remove(obstacle);
            activeObstacles.Add(obstacle);
        }
    }

    private void DestroyObstacle(Obstacle target)
    {
        print("remove");
        activeObstacles.Remove(target);
        deActiveObstacles.Add(target);
        target.gameObject.SetActive(false);
    }
}