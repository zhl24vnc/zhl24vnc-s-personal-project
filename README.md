# 🃏 XR Tabletop Card Game (VR)

![Unity](https://img.shields.io/badge/Engine-Unity%202023+-blue.svg?logo=unity)
![Platform](https://img.shields.io/badge/Platform-Oculus%20Quest%202%2B-lightgrey?logo=oculus)
![License](https://img.shields.io/badge/License-Academic%20Project-green)

> A VR tabletop card battle game inspired by collectible card games like *Hearthstone*, designed with **Unity** and **Meta XR SDK**.  
> Players summon creatures and spells on a virtual table, engage in turn-based combat, and experience immersive interactions with cards in 3D space.

---

## 📖 Overview
This project demonstrates how traditional **card game mechanics** can be reimagined in **virtual reality**.  
By leveraging Unity and the Meta XR framework, players interact with cards naturally using VR controllers, with immersive effects and real-time AI opponents.  

- 🎮 **Genre**: VR Tabletop Strategy  
- 🛠 **Tech Stack**: Unity + Meta XR Interaction SDK (OVR)  
- 🕶 **Device**: Oculus Quest 2 and above  
- 🤖 **Mode**: Single-player with AI opponent  

---

## ✨ Key Features
- **Immersive VR Interaction**: Natural grabbing, placing, and playing cards using VR controllers.  
- **Strategic Depth**: Mana, health, and minion mechanics inspired by classic CCGs.  
- **Cinematic Effects**: Particle systems, rainstorms, and spell animations bring the table alive.  
- **AI Opponent**: Simple yet functional AI with “play first viable card, attack hero” logic.  
- **Optimized Performance**: Object pooling, LODs, and reduced draw calls ensure stable VR framerates.  

---

## 🛠 Project Structure
- **Card & Deck System**: ScriptableObject-based data handling, shuffling (Fisher-Yates), and card instantiation.  
- **Board & Minions**: Minion lifecycle (summoning, attack, death) with “summoning sickness” mechanic.  
- **Turn Manager**: Handles round rotation, timers, and player/AI flow.  
- **AI Controller**: Attack-first principle, extendable for advanced strategies.  
- **VR Interaction**: Raycasting and grab interactions for precise feedback.  

---

## 🚀 Development Journey
- **Week 1–2**: Rapid prototyping with Unity primitives to validate mechanics.  
- **Week 3–4**: VR interaction implementation, fixing pointer precision issues with raycasting.  
- **Week 5–6**: Graphic enhancement with Blender-edited 3D assets, particles, and shaders.  
- **Week 6–7**: AI logic integration.  
- **Week 7–8**: Performance optimization (object pooling, culling, URP fixes).  

---

## 📷 Screenshots
> *(Replace these placeholders with your actual game screenshots)*

| Card Battle Scene | VR Interaction | Special Effects |
|------------------|----------------|-----------------|
| ![screenshot1](docs/images/screen1.png) | ![screenshot2](docs/images/screen2.png) | ![screenshot3](docs/images/screen3.png) |

---

## 🔧 Requirements
- Oculus Quest 2 or newer  
- Unity 2023+ with Meta XR SDK  
- Oculus Link / Air Link for testing, or standalone build  

---

## 🌟 Future Work
- **Multiplayer Support**: Online battles with social interaction.  
- **Narrative Expansion**: Story-driven missions and campaign.  
- **Smarter AI**: Monte Carlo tree search & ML-based adaptive difficulty.  
- **Haptic Feedback**: Controller vibration and advanced haptic suits.  

---

## 📚 Resources & References
- **Portals package 1.0.2.unitypackage**  
- **Fantasy Animated Characters Pack [1.0]**  
- **Kingdom Rush Tutorial** – [https://www.sikiedu.com/course/1930](https://www.sikiedu.com/course/1930)  
- **735.Realistic Rain Storm**  
- **Grasslands - Stylized Nature 1.0**  

---

## 📜 License
This project is developed as part of an **academic research project**.  
Please check the resource licenses before using them in commercial projects.  

---
