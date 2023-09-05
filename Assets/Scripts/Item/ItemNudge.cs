using System.Collections;
using UnityEngine;

public class ItemNudge : MonoBehaviour
{
    private WaitForSeconds pause;  // Pause duration for animations
    private bool isAnimating = false;  // Flag to track if an animation is currently in progress

    private void Awake()
    {
        pause = new WaitForSeconds(0.04f);  // Set the pause duration for animations
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAnimating == false)
        {
            // Check the relative positions of the two colliding objects and trigger the appropriate rotation animation
            if (gameObject.transform.position.x < collision.gameObject.transform.position.x)
            {
                StartCoroutine(RotateAntiClock());  // Rotate counterclockwise
            }
            else
            {
                StartCoroutine(RotateClock());  // Rotate clockwise
            }

            // Play a rustling sound if the colliding object has the "Player" tag
           // if (collision.gameObject.tag == "Player")
           // {
          //      AudioManager.Instance.PlaySound(SoundName.effectRustle);
          //  }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isAnimating == false)
        {
            // Check the relative positions of the two colliding objects and trigger the appropriate rotation animation
            if (gameObject.transform.position.x > collision.gameObject.transform.position.x)
            {
                StartCoroutine(RotateAntiClock());  // Rotate counterclockwise
            }
            else
            {
                StartCoroutine(RotateClock());  // Rotate clockwise
            }

            // Play a rustling sound if the colliding object has the "Player" tag
          //  if (collision.gameObject.tag == "Player")
          //  {
         //       AudioManager.Instance.PlaySound(SoundName.effectRustle);
           // }
        }
    }

    private IEnumerator RotateAntiClock()
    {
        isAnimating = true;  // Set the animation flag to true to prevent concurrent animations

        // Rotate the child object counterclockwise
        for (int i = 0; i < 4; i++)
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);  // Rotate by 2 degrees

            yield return pause;  // Pause for a short duration
        }

        // Rotate the child object clockwise
        for (int i = 0; i < 5; i++)
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);  // Rotate by -2 degrees

            yield return pause;  // Pause for a short duration
        }

        gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);  // Final rotation

        yield return pause;  // Pause for a short duration

        isAnimating = false;  // Reset the animation flag to allow for new animations
    }

    private IEnumerator RotateClock()
    {
        isAnimating = true;  // Set the animation flag to true to prevent concurrent animations

        // Rotate the child object clockwise
        for (int i = 0; i < 4; i++)
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);  // Rotate by -2 degrees

            yield return pause;  // Pause for a short duration
        }

        // Rotate the child object counterclockwise
        for (int i = 0; i < 5; i++)
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);  // Rotate by 2 degrees

            yield return pause;  // Pause for a short duration
        }

        gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);  // Final rotation

        yield return pause;  // Pause for a short duration

        isAnimating = false;  // Reset the animation flag to allow for new animations
    }
}
