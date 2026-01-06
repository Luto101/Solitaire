# Introduction
This is a Solitaire game in console and it was a project qualifying for the final of the nationwide Gigathon 2025 competition.

# Requirements
Visual Studio 2022 (or newer) with .NET 8 support  
.NET 8 SDK (https://dotnet.microsoft.com/download/dotnet/8.0)

# How to Run the Project
1. Open the Solitaire.sln file in Visual Studio.
2. Run the application (F5 or Ctrl + F5).

# Solitaire – User Manual
The goal is to move all cards into the foundation piles (top-right) by suit, from Ace to King.

## Controls
- ← / A - Move left between piles  
- → / D	- Move right between piles  
- ↑ / W	- Move up within a Tableau column  
- ↓ / S	- Move down within a Tableau column  
- 1–7 - Jump directly to Tableau column 1–7  
- Enter / Space - Select or place a card  
- Tab - Focus on Stock  
- Esc - Cancel current move  
- Ctrl + Z - Undo last move  
- Q	- Quit a game

## Gameplay Overview
Tableau (bottom rows): Where you organize cards in descending order, alternating colors. Only face-up cards can be moved.

Foundation (top-right): If a card can be legally placed on the Foundation, it will be move there automatically by only picking it.

Stock & Talon (top-left): Click card on Stock to draw cards into the Talon. Use Talon pile to place cards in Tabelau.

Use ↑ to dig deeper into face-up cards in a Tableau stack or move to Stock.

Use ↓ to unselect higher cards or move back down.

You can jump quickly to a specific Tableau column using keys 1 to 7.


![Title screen](https://github.com/user-attachments/assets/a02a03df-2b21-4633-b030-b286be1d9758)
![Gameplay](https://github.com/user-attachments/assets/c72a4ff3-ce95-4061-903f-c5c9b7c4c72f)