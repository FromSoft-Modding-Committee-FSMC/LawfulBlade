using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Lawful Runtime/Game Data")]
public class GameData : ScriptableObject
{
	[field: SerializeField] public LevelTable[] LevelTableList    { get; private set; }
	[field: SerializeField] public LevelTable   CurrentLevelTable { get; private set; }
}
