# 8080-invaders
An Intel 8080 and Space Invaders emulator with Windows Form app and .NET Standard core.

![Screen Recording](https://raw.githubusercontent.com/jmcd/8080-invaders/master/recording.gif)

Full Intel 8080 emulaton, passing third-party machine-code tests:

* `8080PRE` preliminary tests,
* `8080EX1` instruction set exerciser,
* "MICROCOSM ASSOCIATES  8080/8085 CPU DIAGNOSTIC VERSION 1.0  1980",
* something called "CPUTEST.COM", which I don't know the title of

Janky sound implementation via `WAV` files played through win32 MCI.

Configurable:

* emulator speed,
* color palette,
* number of lifes,
* extra ship score.

## Usage

Build, then place ROM files in the same directory as `.exe`. 

ROM files are easily found via search engine, and should be named: `invaders.e`, `invaders.f`, `invaders.g`, `invaders.h	`.

Keys:

* `z` left
* `x` right
* `enter` fire
* `c` insert coin
* `1` player-one start
