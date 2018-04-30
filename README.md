# AI-Component

Link to repo: https://github.com/jack-maber/AI-Component

The Image of my script is in the Scripts folder

I tried implementing a Behaviour tree system but this proved much more involved that I thought as the preset actions didn't match up with the system I created, also it caused it to crash every time the game was played, so I went with a much simpler, checkpoint based system instead, and although it only involves one script, it is very effective and can be scaled up with the tracks in the game as it uses NavMesh to avoid obstacles on the tracks

The implemenation can be found in the demo track of the main game as I found it was easier to develop on a simpler track as it was easier to test but it can easily be seen that it will work on the other tracks, and  started development on it before I found out that it needed to be in another branch so it was easier to move it over that way. 

The script basically has a set of public transforms where the cubes that I am using a checkpoints plug in, which is done in editor, the loop then just checks if the chair has come inot contact with a tagged checkpoint and if it lines up, it sets the NavMeshAgents next destiantion as the next checkpoint, all while adjusting the speed of the chair using the raycast function at the bottom of the script to make them slow down when they are in close vicinty to other racers so that they don't bunch up as it looks unrealistic. 

Oh and by the way, my old repo was private as the BAs made it, so my progress can be seen over there once they have turned it public
