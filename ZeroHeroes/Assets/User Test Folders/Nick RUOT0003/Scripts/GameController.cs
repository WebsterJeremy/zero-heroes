/* GameController.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using UnityEngine;
using Assets.Scripts.world;
using Assets.Scripts.util.resource;
using Assets.Scripts.player;
using System.Collections;
using Assets.Scripts.ai.path;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;
        private PathRequestManager pathRequestManager;
        private PathFinder pathFinder;


        public Sprite MISSING_SPRITE;
        
        public int width, height;

        [Header("Containers")]
        public Transform objectContainer;
        public Transform tileContainer;
        public Transform itemContainer;
        public Transform entityContainer;


        [Header("Object Templates")]
        public GameObject tileTemplate;
        public GameObject entityTemplate;
        public GameObject objectTemplate;
        public GameObject itemTemplate;

        public bool isPlaying = true;


        private World world;
        private ResourceManager resourceManager;


        private Player player;

        
        public void Awake() {
            instance = this;

            InitializeGame();
        }

        private void InitializeGame() {
            //todo any required gather game settings, 

            //initialize game resource manager
            resourceManager = new ResourceManager();
            resourceManager.LoadAll();

            //set up the pathfinding manager and pathfinder
            pathRequestManager = new PathRequestManager();
            pathFinder = new PathFinder();

            world = new World();

            //todo this is a test... eventually allow user to choose to either generate new world or load via UI on menu.
            world.GenerateWorld(width, height);

            //todo test player data,  get from disk data later...
            player = new Player("0", new Position(8, 2));
            player.Inventory.Add("0", GameController.instance.ResourceManager.ItemResourceManager.GetFromAssetName("stick").Id, 4);
        }

        #region Game Loop

        public Coroutine StartChildCoroutine(IEnumerator coroutineMethod) {
            return StartCoroutine(coroutineMethod);
        }

        public void StopChildCoroutine(Coroutine coroutineMethod) {
            StopCoroutine(coroutineMethod);
        }

        #endregion


        public ResourceManager ResourceManager {
            get { return resourceManager; }
        }

        public World World {
            get { return world; }
        }

        public Player Player {
            get { return player; }
        }

        public PathFinder PathFinder {
            get { return pathFinder; }
        }

        public PathRequestManager PathRequestManager {
            get { return pathRequestManager; }
        }

    }
}
