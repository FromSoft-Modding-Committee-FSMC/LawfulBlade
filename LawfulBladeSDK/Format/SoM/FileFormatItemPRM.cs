using LawfulBladeSDK.Extensions;
using LawfulBladeSDK.IO;

namespace LawfulBladeSDK.Format.SoM
{
    public class FileFormatItemPRM
    {
        public List<ItemPRM> Parameters;
        public int ParameterCount => Parameters.Count;

        /// <summary>
        /// Loads a Item PRM from a file.
        /// </summary>
        public static bool LoadFromFile(string filepath, out FileFormatItemPRM prm)
        {
            try
            {
                using FileInputStream fis = new FileInputStream(filepath);
                prm = Load(fis);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load Item PRM '{Path.GetFileName(filepath)}': {ex.Message}".Colourise(0xF08080));
            }

            prm = null;
            return false;
        }

        /// <summary>
        /// Saves the content of the PRM to a buffer
        /// </summary>
        public bool SaveToBuffer(out byte[] buffer)
        {
            using MemoryStream ms = new MemoryStream();
            using FileOutputStream fos = new FileOutputStream(ms);

            foreach (ItemPRM prm in Parameters)
                fos.WriteStruct(prm);

            buffer = ms.ToArray();
            return true;
        }

        /// <summary>
        /// Internal loading for Item PRM format.
        /// </summary>
        static FileFormatItemPRM Load(FileInputStream fis)
        {
            FileFormatItemPRM result = new FileFormatItemPRM();

            // Load each PRF from the stream
            result.Parameters = new List<ItemPRM>(250);

            for (int i = 0; i < 250; ++i)
                result.Parameters.Add(fis.ReadStruct<ItemPRM>());

            return result;
        }

        /// <summary>
        /// Deduplicates Item PRFs using the PRF index in each PRM.
        /// </summary>
        public int DeduplicateProfiles(ref FileFormatItemPR2 pr2Data)
        {
            // We use the PRF ID as the key, and a queue of PRM IDs
            Dictionary<int, Queue<int>> prfReferenceCount = new Dictionary<int, Queue<int>>();

            for (int i = 0; i < Parameters.Count; ++i)
            {
                // Skip unused parameter files
                if (Parameters[i].pr2Index < 0)
                    continue;

                // Make sure the reference list exists...
                if (!prfReferenceCount.TryGetValue(Parameters[i].pr2Index, out Queue<int> refCounted))
                    prfReferenceCount[Parameters[i].pr2Index] = new Queue<int>();
                else
                    // Since we else, only duplicates will be stored in the queue
                    prfReferenceCount[Parameters[i].pr2Index].Enqueue(i);
            }

            // After the scan for duplicate references, we can proceed to fix by de-duplication
            int deduplicatedPRFs = 0;

            foreach (int refKey in prfReferenceCount.Keys)
            {
                // Get the list of duplicate references
                Queue<int> duplicateRefs = prfReferenceCount[refKey];

                // Loop while there are duplicate references
                while (duplicateRefs.Count > 0)
                {
                    // deduplicate
                    int prmID = duplicateRefs.Dequeue();

                    // Add a duplicate profile by copying the original to a new slot
                    pr2Data.Profiles.Add(pr2Data.Profiles[refKey]);

                    // We can now store the new PRF index on to the PRM
                    ItemPRM prmCopy = Parameters[prmID];
                    prmCopy.pr2Index = (short)(pr2Data.Profiles.Count - 1);
                    Parameters[prmID] = prmCopy;

                    // Increment the number of deduplicated PRFs
                    deduplicatedPRFs++;
                }
            }

            return deduplicatedPRFs;
        }
    }
}
