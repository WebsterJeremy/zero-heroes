/* SpriteAnimator.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Jeremy Webster
 * Last Updated:   30/08/2020
 */

using System.Collections;
using UnityEngine;

namespace Assets.Scripts.world { 
    public class SpriteAnimator : MonoBehaviour
    {

        Sprite[] sprites;

        bool lastPlayState;
        SpriteRenderer renderer;


        public void Initialize(Sprite[] _sprites) {
            this.sprites = _sprites;
            lastPlayState = (GameController.Instance.CurrentGameState == GameController.GameState.PLAYING);
            renderer = GetComponent<SpriteRenderer>();
            SetAnimation();
        }

        void Update()
        {
            if(lastPlayState != (GameController.Instance.CurrentGameState == GameController.GameState.PLAYING)) {
                //if the play state changes, then either start or stop animation..
                lastPlayState = (GameController.Instance.CurrentGameState == GameController.GameState.PLAYING);
                SetAnimation();
            } 
        }

        private void SetAnimation() {
            if(sprites.Length <= 1) {
                return;
            }

            if (GameController.Instance.CurrentGameState == GameController.GameState.PLAYING) {
                //start animation
                StartCoroutine(Animate());
            } else {
                //stop animation
                StopCoroutine(Animate());
            }
        }

        private IEnumerator Animate() {
            int index = 0;
            while (GameController.Instance.CurrentGameState == GameController.GameState.PLAYING) {
                renderer.sprite = sprites[index];
                index++;

                if (index == sprites.Length) {
                    //reset index to 0
                    index = 0;
                }
            
                yield return new WaitForSeconds(Constants.ANIMATION_SPEED);
            }
        }

    
    }

}