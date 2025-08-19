using UnityEngine;

[CreateAssetMenu(fileName = nameof(ShapeStyle), menuName = "ScriptableObjects/Styles/Shape")]
public class ShapeStyle : Style
{
    public AssetReference ShapeReference;
    public AssetReference SecondaryShapeReference;
}
