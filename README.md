# Slay the Spire Design

A learning project exploring game design patterns through a Slay the Spire-inspired combat system, built in C#.

## About

This is at present a console-based deck-building combat system that recreates the core mechanics of Slay the Spire. The focus is on understanding and implementing design patterns like Command and Observer in the context of a turn-based card game, as I wanted to explore the usage in actual use rather than a theoretical understanding. 

## Current Features

- Deck system with draw, discard, hand, and exhaust piles
- Card playing with energy costs
- Block and damage resolution
- Enemy turns with randomized intent
- Turn-based combat loop with win/lose conditions

## Running

Requires .NET 8.0. Clone the repo and run:

```
dotnet run --project "Slay the Spire Design/Slay the Spire Design"
```

## Devlog

Development notes and write-ups on the design process:

- [01 - Getting Started](devlog/01-getting-started.md)
