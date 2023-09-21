
# Godot 3D Trail in C#

Because I've seen a lot of Trail made in GDScript and none in C#, I figured I might as well just rewrite the existing ones in C# and make them available to everyone.

I translated [metanoia83's Motion Trail](https://github.com/metanoia83/Godot-4.0-Motion-Trail) which was based on [dbp8890's Godot 3's Motion Trail](https://github.com/dbp8890/motion-trails).


## Demo

Here's a GIF demo of how it'd look like:

![](https://github.com/q8geek/Godot-4-Basic-3D-Trail/blob/main/demo.gif)
## Features

- A boolean value to enable/disable trailing
- Setting start and end's colours and widths
- Easing trail's width
- Determine distance needed to add a new segment to the line
- Lifespan for each segment


## Installation

To use this script:

1. Copy `Trail.cs` to your project's folder
2. Create a `MeshInstance3D` Node
3. Attach `Trail.cs` to the created node
4. Have fun!
## Acknowledgements

 - [metanoia83](https://github.com/metanoia83/Godot-4.0-Motion-Trail)
 - [dbp8890](https://github.com/dbp8890/motion-trails/tree/master/MotionTrail)
 - [Katherine Oelsner](https://readme.so/) (Her readme.so made it easier to write this readme file)


## License

[The Unlicense](https://choosealicense.com/licenses/unlicense/)