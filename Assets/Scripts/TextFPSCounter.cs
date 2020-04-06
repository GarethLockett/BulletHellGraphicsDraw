using UnityEngine;
using UnityEngine.UI;

/*
    Script: TextFPSCounter
    Author: Gareth Lockett
    Version: 1.0
    Description:    Display framerate (FPS) on a Unity UI Text.
                    Usage: Press 'F' key to toggle show/hide.
*/

public class TextFPSCounter : MonoBehaviour 
{
	public Text text;                               // Reference to text component for outputing framerate as a string.
	public bool show = false;                       // Display the FPS.

    private const int targetFPS = 60;               // Target framerate (60 for Android)
	private const float updateInterval = 0.5f;      // How often to update the displayed framerate (Don't do too often else it is hard to read)

	private int framesCount;                        // Internal frame count.
	private float framesTime;                       // Internal time passed.

	void Start()
	{ 
		// No text object set? See if our gameobject has one to use
		if( this.text == null ) { this.text = this.gameObject.GetComponent<Text>(); }
	}
	
	void Update()
	{
        // Check for display toggle.
        if( Input.GetKeyDown( KeyCode.F ) == true ) { this.show = !this.show; }

        // Accumilate frame counter and the total time.
        this.framesCount++;
        this.framesTime += Time.unscaledDeltaTime; 

		// Check if interval ended, so calculate FPS and display.
		if( this.framesTime > updateInterval )
		{
			if( this.text != null )
			{
				if( this.show )
				{
					float fps = this.framesCount / this.framesTime;
                    this.text.text = fps.ToString( "F2" ) + " FPS";
                    this.text.color = Color.Lerp( Color.red, Color.green, fps / targetFPS );
				}
				else { this.text.text = ""; }
			}

            // Reset for the next interval to measure
            this.framesCount = 0;
            this.framesTime = 0;
		}
		
	}
}
