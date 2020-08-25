using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will contain the game's save data
//When the game is saved, an instance of this class will be written to the user's device
//When the game is loaded, an instance of this class will will be leaded inot ram to update game values
[System.Serializable]
public class GameSaveData {

    //Add any data to be saved here (make sure its public)

    //Test data
    public int[] testData1 = new int[5];
    public int[] testData2 = new int[5];


    //Not currently used
    void Start(){}

    // Any data being saved that should be updated every frame should be added here
    public void updateRegularSaveData(){

        /*e.g
         * 
         * this.playerPosition = GameManager.playerPosition;
         * 
         */
        
        //Test data
        int randomIndex = Random.Range(0, testData1.Length);
        int randomValue = Random.Range(0, 500);
        testData1[randomIndex] = randomValue;
    }

    // Any data being saved irreguarly (at checkpoints, etc.) Should be updated here
    public void UpdateCheckpoint(){

        /*e.g
         * 
         * this.playerScore = GameManager.playerScore;
         * 
         */

        //Test data
        int randomIndex = Random.Range(0, testData2.Length);
        int randomValue = Random.Range(0, 500);
        testData2[randomIndex] = randomValue;
    }
}
