/* SpriteAnimator.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
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
            lastPlayState = GameController.instance.isPlaying;
            renderer = GetComponent<SpriteRenderer>();
            SetAnimation();
        }

        void Update()
        {
            if(lastPlayState != GameController.instance.isPlaying) {
                //if the play state changes, then either start or stop animation..
                lastPlayState = GameController.instance.isPlaying;
                SetAnimation();
            } 
        }

        private void SetAnimation() {
            if(sprites.Length <= 1) {
                return;
            }

            if (GameController.instance.isPlaying) {
                //start animation
                StartCoroutine(Animate());
            } else {
                //stop animation
                StopCoroutine(Animate());
            }
        }

        private IEnumerator Animate() {
            int index = 0;
            while (GameController.instance.isPlaying) {
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