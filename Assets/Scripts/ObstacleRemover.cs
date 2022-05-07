using System;
using UnityEngine;

public class ObstacleRemover : MonoBehaviour
{
   public Action<Obstacle> OnDestroyObstacle;
   public void Setup(float posX)
   {
      transform.position = new Vector3(posX, 0, 0);
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      var otherObstacle = other.GetComponent<Obstacle>();
      if (!otherObstacle) return;
      OnDestroyObstacle.Invoke(otherObstacle);
   }
}
