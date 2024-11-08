using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelTable", menuName = "Lawful Runtime/Level Table")]
public class LevelTable : ScriptableObject
{
    [Serializable]
	public struct LevelTableEntry
	{
		/// <summary> The amount of experience required to gain this level </summary>
		public uint experienceRequired;
		
		/// <summary> How much max hp is added </summary>
		public byte hpDelta;
		
		/// <summary> How much max mp is added </summary>
		public byte mpDelta;
		
		/// <summary> How much strength is added </summary>
		public byte stDelta;
		
		/// <summary> How much magic is added </summary>
		public byte mgDelta;
	}
	
	[field: SerializeField] public List<LevelTableEntry> Levels { get; private set; }
	[field: SerializeField] public string SourceFile 			{ get; private set; }
	[field: SerializeField] public bool LoadInEditor 			{ get; private set; }
	
	void OnValidate()
	{
		if (LoadInEditor)
		{
			string tablePath = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), SourceFile);
			
			Levels.Clear();
			
			using (BinaryReader br = new BinaryReader(File.OpenRead(tablePath)))
			{
				do 
				{
					Levels.Add(new LevelTableEntry 
					{
						experienceRequired = br.ReadUInt32(),
						hpDelta = br.ReadByte(),
						mpDelta = br.ReadByte(),
						stDelta = br.ReadByte(),
						mgDelta = br.ReadByte()
					});
				} while (br.BaseStream.Position != br.BaseStream.Length);
			}
			
			LoadInEditor = false;
		}
	}
}
