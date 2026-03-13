# BIO2 RNDX

A Resident Evil 2 (PC) randomizer and modding tool that shuffles items, enemies, puzzles, and scenarios to create fresh, unpredictable playthroughs. Originally developed for the 1998 classic, this tool extends the game's replayability far beyond its original scope.

![BIO2 RNDX](https://github.com/n0rthw1ndz/BIO2_RNDX/blob/master/image.png)

## Overview

BIO2 RNDX is a comprehensive randomization suite for both Leon A/B and Claire A/B scenarios. It reads directly from the game's RDT files, parses bytecode-level logic, and rewrites item placements, enemy spawns, and puzzle solutions while maintaining game stability.

## Features

**Item Randomization**
- Full item pool shuffling across all rooms
- Category-based key placement (Blue Cards, Suit Keys, Object Keys, Sewer Keys, Lab Keys)
- Integrity checking to ensure seeds remain completable
- Per-character item lists for Leon and Claire

**Enemy Randomization**
- EMD data swapping across 40+ tested rooms
- Zombie, licker, and monster placement shuffling
- Scenario-aware enemy layouts (A/B, Hunk/Tofu)
- Crash prevention through room-specific filtering

**Puzzle Randomization**
- Statue puzzle solutions
- Safe combinations
- Torch puzzle layouts
- Timer puzzle variations

**Additional Modifications**
- Item box availability toggling
- Handgun disable/enable on game start
- Cutscene enemy swaps
- Mr. X spawn condition adjustments
- Backup and restore system for original game files

## Usage

Run from command line with the following arguments:

| Argument | Description |
|----------|-------------|
| `/L` | Randomize Leon A/B scenarios |
| `/C` | Randomize Claire A/B scenarios |
| `/R` | Restore original game files from backup |

The tool will prompt for additional randomization options:
- Item shuffle mode (common only / common+keys / keys only)
- Enemy randomization
- Puzzle randomization
- Item box disabling
- Scenario layout mixing
- Cutscene modifications
- Handgun startup toggle

## How It Works

BIO2 RNDX parses the game's RDT files directly, navigating complex bytecode structures that include:

- CK condition checks (character, scenario, difficulty, Mr. X spawns)
- 0x4E item placement operations
- 0x44 EMD (enemy) data blocks
- Nested conditional blocks for Leon/Claire, A/B, and Easy/Normal paths

The tool builds an in-memory representation of all items, shuffles them according to user-selected rules, and rewrites the original files with the new placements—all while maintaining the game's original structure.

## Supported Rooms

Enemy randomization works across dozens of hand-tested rooms including:
- RPD main areas
- Kendo's gun shop
- Sewer and laboratory sections
- Streets and parking lots
- Train and basement areas

Each room was individually tested to ensure stability—rooms known to cause crashes are excluded or handled with special cases.

## Building

Open the solution in Visual Studio and build. The project targets .NET Framework and uses Figgle for ASCII art rendering.

## Credits

Created by n0rthw1ndz (dchaps)

Built through extensive reverse engineering of the original 1998 release. No SDKs or official documentation were used—just hex editors, persistence, and a lot of trial and error.

## License

This project is for educational and archival purposes. Original game files are required for use.
