using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
   [SerializeField] private float speed = 2f;
   public ObstacleType type;

   private void Update()
   {
      transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
   }
}
public enum ObstacleType
{
   GoodFish,
   BadFish
}