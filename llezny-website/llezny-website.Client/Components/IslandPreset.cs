using Hearthglade.Core.World;

namespace llezny_website.Client.Components;

/// <summary>
/// A hand-tuned stand-in for the game's world data: the real thing lives in ScriptableObjects that are not
/// part of the public repo. The generator itself (HearthgladeCore/) is the game's own Unity-free code.
/// </summary>
public static class IslandPreset
{
    public const int Size = 128;

    /// <summary>Colours indexed by biome id, same order as <see cref="Rules"/>.</summary>
    public static readonly (byte R, byte G, byte B)[] Palette =
    {
        (0x2f, 0x55, 0x68), // deep water
        (0x4b, 0x84, 0x93), // shallows
        (0xe4, 0xcd, 0x9c), // beach
        (0xf1, 0xed, 0xe4), // snow
        (0x8a, 0x7a, 0x6e), // rock
        (0x56, 0x78, 0x3a), // forest
        (0x9a, 0xb5, 0x59), // meadow
        (0xcf, 0xa6, 0x5a), // dry land
    };

    private static readonly bool[] Liquid = { true, true, false, false, false, false, false, false };

    private static readonly BiomeRule[] Rules =
    {
        Rule(-1.01f, -0.60f),                          // deep water
        Rule(-0.60f, -0.40f),                          // shallows
        Rule(-0.40f, -0.25f),                          // beach
        Rule(0.55f, 1.01f, -1.01f, -0.20f),            // snow
        Rule(0.55f, 1.01f),                            // rock
        Rule(-0.25f, 0.55f, -1.01f, 1.01f, 0.10f, 1.01f),   // forest: wet
        Rule(-0.25f, 0.55f, -1.01f, 0.50f),            // meadow: dry and not hot
        Rule(-0.25f, 0.55f),                           // dry land: hot and dry
    };

    public static GeneratedTerrain Generate(int seed)
    {
        var shape = new WorldShapeSettings
        {
            IslandRadius = 0.8, IslandFalloff = 0.2, InteriorFloor = -0.3,
            WarpScale = 24, WarpStrength = 6,
            ClimateJitter = 0.08, ClimateJitterScale = 5,
            KeepMainIslandOnly = true, MinLakeSize = 12,
        };

        return TerrainGenerator.Generate(
            Size, seed,
            new NoiseSettings(4, 0.5, 2, 30),
            new NoiseSettings(2, 0.5, 2, 60),
            new NoiseSettings(3, 0.5, 2, 40),
            Rules, shape, Liquid);
    }

    private static BiomeRule Rule(float h0, float h1, float t0 = -1.01f, float t1 = 1.01f, float m0 = -1.01f, float m1 = 1.01f) =>
        new()
        {
            HeightFrom = h0, HeightTo = h1,
            TemperatureFrom = t0, TemperatureTo = t1,
            HumidityFrom = m0, HumidityTo = m1,
        };
}
