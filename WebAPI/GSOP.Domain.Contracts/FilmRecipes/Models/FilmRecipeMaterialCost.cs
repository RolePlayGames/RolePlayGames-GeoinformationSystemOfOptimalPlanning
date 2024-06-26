﻿namespace GSOP.Domain.Contracts.FilmRecipes.Models;

public record FilmRecipeMaterialCost
{
    public const int Min = 0;

    private readonly double _materialCost;

    public FilmRecipeMaterialCost(double materialCost)
    {
        if (materialCost <= Min)
            throw new ArgumentOutOfRangeException(nameof(materialCost), $"Material cost should be greater than {Min}");

        _materialCost = materialCost;
    }

    public static implicit operator double(FilmRecipeMaterialCost materialCost) => materialCost._materialCost;

    public static explicit operator FilmRecipeMaterialCost(double materialCost) => new(materialCost);
}
