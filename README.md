# Belt-Proof Boots

BepInEx-6-IL2CPP-Mod für **Moon Sweepers**: Solange die Gravity Boots an sind, trägt dich ein Förderband nicht mehr mit. Du kannst also auf Bändern stehen, bauen und quer darüber laufen, ohne weggeschoben zu werden.

> Status: Beta. Im Einzelspieler mit Spielversion 1.0.5 getestet, Koop noch nicht.

## Installation
1. [BepInEx 6.0.0-be.791](https://builds.bepinex.dev/projects/bepinex_be) (Unity.IL2CPP win-x64) ins Spielverzeichnis entpacken.
2. Den [MoonSweepers BepInEx Patcher](https://www.nexusmods.com/moonsweepers/mods/9) installieren.
3. Spiel einmal starten, damit BepInEx die Interop-Dateien erzeugt, dann beenden.
4. `BeltProofBoots.dll` nach `<Spiel>\BepInEx\plugins\BeltProofBoots\` kopieren.

Im Koop wirkt die Mod nur für Spieler, die sie selbst installiert haben. Die anderen brauchen sie nicht.
Verträgt sich mit Belt Tiers.

## Einstellungen
`BepInEx\config\moonsweepers.beltproofboots.cfg`:

| Option | Standard | Wirkung |
|---|---|---|
| `Enabled` | true | Mod an/aus |
| `RequireGravityBoots` | true | Nur mit eingeschalteten Gravity Boots. `false` = Bänder tragen dich zu Fuß nie mit |
| `FootReach` | 2.5 | Abstand (m) zwischen Kontaktpunkt am Band und Astronaut, ab dem du als „auf dem Band“ zählst |

Fahrzeuge und Roboter, auf denen du fährst, laufen weiter mit dem Band.

## Bauen
```
dotnet build -c Release
```
Kopiert `BeltProofBoots.dll` nach `<Spiel>\BepInEx\plugins\BeltProofBoots\`. Spielordner wie bei Belt Tiers über `GamePath.props` oder `-p:GamePath="..."`.

## Funktionsweise
`MoonWorld.BeltSurfaceVelocity` liefert die Geschwindigkeit der Bandoberfläche, sagt aber nicht, wer darauf steht. Die Mod setzt sie auf null, wenn der Kontaktpunkt beim lokalen Astronauten liegt, er nicht fährt und die Gravity Boots an sind.

## Lizenz
GPL-3.0-or-later
