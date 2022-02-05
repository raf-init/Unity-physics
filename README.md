# Unity-physics
## Particle physics from scratch

For the specific project the Unity game engine was used. The particle system, as well as the collision detection system were not implemented. Everything was written from scratch, taking advantage of Euler's formulas and based on the principle of the energy conservation.
The project is divided into 5 tasks:
• Task I: The particles are created with random starting velocities and are affected by gravity and the drag force, which is acting opposite to the relative motion. These particles bounce on the walls of an invisible cube surrounding them.

• Task II: The particles are also affected by the pulling force of two game objects that act like magnets.

• Task III: A specific amount of particles, surrounded by an invisible sphere, affect each other with pushing forces.

• Task IV: Implementing bird flocking.

• Task V: Smoke appears on scene.

## I & II
For the first and second task, gravity, linear drag, energy loss after collision and the pulling forces of the 'magnets' were taken into consideration. Given that, the gravity force only affects the acceleration on the Y-axis, the linear drag (fdrag = -kd * v) is always acting opposite to the relative motion and every time there is a collision, a percentage of energy is lost (KE' = a * KE, where a is the coefficient of energy loss). The value of g is set to 10, whilst the drag coefficient is set to 0.1. The force of each 'magnet' is set to 5. The energy loss coefficient is set to 0.1.
We know that the future position of the particle is given by the equation: x' = x + v * dt
We also know that the future velocity of the particle is given by the equation: v' = v + a * dt
The acceleration of the particle is given by the formula: a = ΣF / m
Using the FixedUpdate() method, since it can run once, zero, or several times per frame, we determine whether the particle approaches the game object that is used as a magnet, in order to specify if the object accelerates or decelerates. Then, we decompose each pulling force to determine the forces on each axis. The final velocity of the object is also defined by the collision with the walls of the cube. This was done by checking the distance between the particle and the cube's sides. Below, we demonstrate the pulling force decomposition:
### Fy = F * cosθ 

### Fx =AB * cosφ 

### Fz =AB * sinφ

## III
For the third part, gravity and linear drag forces were not taken into consideration. Moreover, the kinetic energy remains the same. The important part here was to detect whether the particles were approaching each other or not. The pushing force against each other is inversely proportional to their distance. Here we decided not to decompose the force, since it is not a consistent force. So the acceleration of the particle is affected, on each axis, by the distance between the particles.

## IV & V
Lastly, we implemented a bird flocking prefab that was downloaded from the Unity's Asset store and we also decorated the scene with lanterns that were lit by a particle system of special effects (such as lightning and smoke).

