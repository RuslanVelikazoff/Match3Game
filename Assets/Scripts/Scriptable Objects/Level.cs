using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "World", menuName = "Level")]
public class Level : ScriptableObject
{
    [Header("Размер поля")]
    public int width;
    public int height;

    [Header("Начальные плитки")]
    public TileType[] boardLayout;

    [Header("Начальные бонусы")]
    public PieceType[] pieceLayout;

    [Header("Разновидности точек")]
    public GameObject[] dots;

    [Header("Конечная цель уровня")]
    public EndGameRequirements endGameRequirements;
    public BlankGoal[] levelGoals;
}
