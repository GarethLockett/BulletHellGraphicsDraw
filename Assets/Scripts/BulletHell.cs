using UnityEngine;
using UnityEngine.UI;

/*
    Script: BulletHell
    Author: Gareth Lockett
    Version: 1.0
    Description:    Super simple demonstration of using Graphics.DrawMesh to draw objects/textures in the scene without requiring a GameObject each (eg better performance on mobile!)

                    NOTES:
                            - Gets around 60fps with 5000 bullets on Samsung S10e.
                            - Could possibly be faster to use Unity Jobs for this sort of task (and/or ECS / Burst compiler)
                            - Could also be worth trying with Graphics.DrawMeshInstanced or Graphics.DrawMeshInstancedIndirect.
*/

public class BulletHell : MonoBehaviour
{
    // Structs
    [System.Serializable]
    public struct Bullet                            // Basic bullet struct. Properties for position, rotation, and speed.
    {
        public Vector3 position;
        public Quaternion rotation;
        public float moveSpeed;
    }

    // Properties
    public int numberOfBullets = 1000;              // Total number of bullets to spawn.
    public Vector2 randomSpeedRange;                // Will reset bullet move speed to somewhere within this range (eg from randomSpeedRange.x to randomSpeedRange.y)
    public float bulletScale = 0.1f;                // Amount to scale the bullet mesh.
    public float maximumDistance = 10f;             // Maximum distance before resetting the bullet.

    public Mesh bulletMesh;                         // Mesh to render each bullet with.
    public Material bulletMaterial;                 // The material to render each bullet with.

    public GameObject player;                       // Player object to test for collisions.
    public Vector2 collisionBoxSize;                // Quick test to see if a bullet is within a box around the player.

    public Text numberText;                         // Output the number of bullets text.

    private Bullet[] allBullets;                    // Array of all bullets (eg object pool)

    // Methods
    private void Start()
    {
        // Make it run as fast as possible (NOTE: just for testing ... will suck down battery!!)
        Application.targetFrameRate = 0;

        // Create all the bullets.
        this.allBullets = new Bullet[ this.numberOfBullets ];

        // Position and randomize all bullets.
        for( int i = 0; i < this.allBullets.Length; i++ ) { this.ResetBullet( ref this.allBullets[ i ] ); }

        // Output number of bullets text.
        if( this.numberText != null ) { this.numberText.text = "#Bullets: " +this.numberOfBullets; }
    }

    // This method recycles the bullet.
    private void ResetBullet( ref Bullet bullet )
    {
        // Reset the bullet position.
        bullet.position = Vector3.zero;

        // Give the bullet a random rotation.
        bullet.rotation = Quaternion.Euler( 0f, 0f, Random.Range( 0f, 360f ) );

        // Randomize the bullet move speed.
        bullet.moveSpeed = Random.Range( this.randomSpeedRange.x, this.randomSpeedRange.y );
    }

    private void Update()
    {
        // Sanity checks.
        if( this.allBullets == null ) { return; }
        if( this.bulletMesh == null || this.bulletMaterial == null ) { return; }

        // Loop through all the bullets.
        Vector3 lastBulletPosition;
        for( int i=0; i<this.allBullets.Length; i++ )
        {
            // Record the current bullet position.
            lastBulletPosition = this.allBullets[ i ].position;

            // Move the bullet.
            this.allBullets[ i ].position += ( this.allBullets[ i ].rotation *Vector3.up ) * Time.deltaTime * this.allBullets[ i ].moveSpeed;

            // Test for a player collision.
            if( this.player != null )
            {
                // Check within x axis.
                if( this.allBullets[i].position.x > this.player.transform.position.x - this.collisionBoxSize.x 
                            && this.allBullets[i].position.x < this.player.transform.position.x + this.collisionBoxSize.x )
                {
                    // Check within y axis.
                    if( this.allBullets[ i ].position.y > this.player.transform.position.y - this.collisionBoxSize.y
                            && this.allBullets[ i ].position.y < this.player.transform.position.y + this.collisionBoxSize.y )
                    {
                        this.ResetBullet( ref this.allBullets[ i ] );

                        /*
                        // OPTIONAL: Spherecast to check if bullet hit player.
                        if( Physics.SphereCast( lastBulletPosition, this.bulletScale, ( this.allBullets[ i ].position - lastBulletPosition ).normalized, 
                                                                            out RaycastHit hit, ( this.allBullets[ i ].position - lastBulletPosition ).magnitude ) == true )
                        {
                            Debug.DrawRay( this.allBullets[ i ].position, Vector3.up, Color.red, 0.25f );
                            
                            // Reset the bullet on collision. (NOTE: Could test 'hit' to see what and where it was hit .. perhaps spawn a hit effect/sound)
                            this.ResetBullet( ref this.allBullets[ i ] );

                            continue;
                        }
                        */
                    }
                }
            }

            // Create a 4x4 transform matrix for the bullet.
            Matrix4x4 matrix = Matrix4x4.TRS( this.allBullets[ i ].position, this.allBullets[ i ].rotation, Vector3.one * this.bulletScale );

            // Draw the bullet mesh with a material.
            Graphics.DrawMesh( this.bulletMesh, matrix, this.bulletMaterial, 0 );

            // Check distance for a reset.
            if( this.allBullets[ i ].position.magnitude >= this.maximumDistance ) { this.ResetBullet( ref this.allBullets[ i ] ); }
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        if( Application.isEditor == false ) { return; }

        // Visualize the player collision box in the editor.
        if( this.player != null )
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube( this.player.transform.position, new Vector3( this.collisionBoxSize.x *2f, this.collisionBoxSize.y *2f, 0f ) );
        }
    }
}
