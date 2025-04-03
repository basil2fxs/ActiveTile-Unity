# Active Tiles – Unity Game Prototype

This Unity project is a prototype game designed for interactive floor tiles, used in the first active games in WA.

## 🎮 Features
- Basic level implementation for player interaction via RGB floor tiles.
- UDP communication layer for sending and receiving tile data.
- In-room monitor UI for immersive feedback.

## 🛠 Tech Stack
- Unity (C#)
- Custom hardware over UDP (interfaced with floor tiles)
- Supports future modular game expansion.

## 🔧 Getting Started
1. Clone the repo: https://github.com/basil2fxs/unity-cubetown
2. Open the project in Unity (recommended version: `2022.x.x`).
3. Run the scene: `Assets/Scenes/Main.unity`.

## 📡 Communication Protocol
- UDP messages are sent to and from the tile controller.
- Tile data includes RGB state and pressure input flags.

## 🧠 Future Plans
- Add support for additional modules and puzzle interactions.
- Port finalized system to Godot (see [Next Gen Godot repo](#)).

## 👤 Author
Basil Toufexis  
Student Engineer @ Curtin University  
[LinkedIn](https://www.linkedin.com/in/basiltoufexis) | [Email](mailto:basil2fxs@gmail.com)
