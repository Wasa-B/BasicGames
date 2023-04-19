using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WasabiGame
{
    public class Tile : MonoBehaviour
    {
        public GameObject wallPrefabs;
        public GameObject currentObject;
        GameObject wall;
        int lifeTime = 20;
        internal Vector2Int pos;
        System.Action moveUpdate;
        // Start is called before the first frame update
        void Start()
        {
            moveUpdate = MoveUpdate;
            currentObject = transform.GetChild(0).gameObject;
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void GenerateWall()
        {
            if(currentObject)
                currentObject.SetActive(false);
            if(wall == false)
            {
                wall = Instantiate(wallPrefabs, transform);
                wall.transform.localPosition = Vector3.zero;
            }
            else
            {
                wall.SetActive(true);
            }
            
            GetComponent<AudioSource>().Play();
            GameBoard.instance.moveUpdate += moveUpdate;
        }

        void MoveUpdate()
        {
            lifeTime -= 1;
            if(lifeTime <= 0)
            {
                wall.SetActive(false);
                lifeTime = 20;
                GameBoard.instance.WallDelete(pos);
                GameBoard.instance.moveUpdate -= moveUpdate;
            }
        }
    }
}