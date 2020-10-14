using System;
using UnityEditor;

[CustomEditor(typeof(PositionerDemo.CardAsset)), CanEditMultipleObjects]
public class CardAssetCustomInspector : Editor
{
    public SerializedProperty
    ID_prop,
    Description_prop,
    Tags_prop,
    Rarity_prop,
    CardImage_prop,
    CardName_prop,
    TypeOfCard_prop,
    ActivavationType_prop,
    AmountPerDeck_prop,
    OverrideLimit_prop,
    IsChainable_prop,
    PosibleTargets_prop,
    Filtters_prop,
    IsDarkCard_prop,
    DarkPoints_prop
    ;

    void OnEnable()
    {
        // Setup the SerializedProperties

        // all the common general fields
        ID_prop = serializedObject.FindProperty("ID");
        Description_prop = serializedObject.FindProperty("Description");
        Tags_prop = serializedObject.FindProperty("Tags");
        Rarity_prop = serializedObject.FindProperty("CardRarity");
        CardImage_prop = serializedObject.FindProperty("CardImage");
        CardName_prop = serializedObject.FindProperty("CardName");
        TypeOfCard_prop = serializedObject.FindProperty("CardType");
        ActivavationType_prop = serializedObject.FindProperty("ActivationType");
        AmountPerDeck_prop = serializedObject.FindProperty("AmountPerDeck");
        OverrideLimit_prop = serializedObject.FindProperty("OverrideLimitOfThisCardInDeck");
        IsChainable_prop = serializedObject.FindProperty("IsChainable");
        PosibleTargets_prop = serializedObject.FindProperty("PosibleTargets");
        Filtters_prop = serializedObject.FindProperty("Filtters");
        IsDarkCard_prop = serializedObject.FindProperty("IsDarkCard");
        DarkPoints_prop = serializedObject.FindProperty("DarkPoints");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(ID_prop);
        EditorGUILayout.PropertyField(Description_prop);
        EditorGUILayout.PropertyField(Tags_prop);
        EditorGUILayout.PropertyField(Rarity_prop);
        EditorGUILayout.PropertyField(CardImage_prop);
        EditorGUILayout.PropertyField(CardName_prop);
        EditorGUILayout.PropertyField(TypeOfCard_prop);
        EditorGUILayout.PropertyField(ActivavationType_prop);
        EditorGUILayout.PropertyField(AmountPerDeck_prop);
        EditorGUILayout.PropertyField(OverrideLimit_prop);
        EditorGUILayout.PropertyField(IsChainable_prop);
        EditorGUILayout.PropertyField(PosibleTargets_prop);
        EditorGUILayout.PropertyField(Filtters_prop);
        EditorGUILayout.PropertyField(IsDarkCard_prop);

        bool isDarkCard = Convert.ToBoolean(IsDarkCard_prop.boolValue);

        if (isDarkCard)
        {
            EditorGUILayout.PropertyField(DarkPoints_prop);
        }

        serializedObject.ApplyModifiedProperties();
    }

}
