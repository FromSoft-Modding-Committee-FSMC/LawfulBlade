using UnityEngine;

using Lawful.IO;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System;

public class MapEvents
{
    /// <summary>
    /// Configuration for what triggers the event/event page
    /// </summary>
    public struct EVTCondition
    {
        public ushort mode;             // The comparison mode.
        public ushort itemID;           // Either a counter, or something else...
        public int compareTo;           // The value the value of the item/etc ID is compared to
        public ushort comparisonType;   // The type of comparison to perform. (==, !=, <, >)
    }

    /// <summary>
    /// Operation is a single command in the EVT file<br/>
    /// TODO: This should probably be some interface struct?..
    /// </summary>
    public struct EVTOperation
    {
        public short  opcode;
        public ushort length;
        public byte[] arguments;
    }

    /// <summary>
    /// Pages store many operations.
    /// </summary>
    public struct EVTPage
    {
        public uint operationsOffset;
        public EVTCondition startCondition;
        public EVTOperation[] operations;
    }

    /// <summary>
    /// Used to store an event declaration
    /// </summary>
    public struct EVTDeclaration
    {
        public string eventName;
        public byte targetType;     // 0xFE = system
        public short targetID;
        public byte u8x22;
        public byte u8x23;
        public ushort interactionCone;
        public ushort u16x26;
        public float xwArea;
        public float yhArea;
        public float radius;
        public EVTCondition startCondition;
        public EVTPage[] pages;
    }

    public List<EVTDeclaration> events = new List<EVTDeclaration>();

    /// <summary>
    /// Loads an .EVT file
    /// </summary>
    /// <param name="filepath"></param>
    public void LoadFromFile(string filepath)
    {
        using (FileInputStream finStream = new FileInputStream(filepath))
            Load(finStream);
    }

    /// <summary>
    /// Internal load is used for both string and memory operations
    /// </summary>
    /// <param name="finStream"></param>
    void Load(FileInputStream finStream)
    {
        // Seek to the start of our stream
        finStream.SeekBegin(0);

        //
        // EVT Header
        //
        uint numEvent = finStream.ReadU32();

        //
        // EVT Events
        //
        do
        {
            // Read Each Event...
            EVTDeclaration evtDecl = new EVTDeclaration
            {
                eventName       = finStream.ReadFixedString(31, Encoding.GetEncoding(932)),
                targetType      = finStream.ReadU8(),
                targetID        = finStream.ReadS16(),
                u8x22           = finStream.ReadU8(),
                u8x23           = finStream.ReadU8(),
                interactionCone = finStream.ReadU16(),
                u16x26          = finStream.ReadU16(),
                xwArea          = finStream.ReadF32(),
                yhArea          = finStream.ReadF32(),
                radius          = finStream.ReadF32(),

                startCondition = new EVTCondition
                {
                    mode = finStream.ReadU16(),
                    itemID = finStream.ReadU16(),
                    compareTo = finStream.ReadS16(),
                    comparisonType = finStream.ReadU16()
                },
                pages = ReadPages(finStream, 12)
            };

            // Print the name of our event
            Logger.Info(evtDecl.eventName);

            // Add to the list of event declarations
            events.Add(evtDecl);

            // Decrement our event count
            numEvent--;

        } while (numEvent > 0);
    }

    /// <summary>
    /// Loads pages from the evt file
    /// </summary>
    EVTPage[] ReadPages(FileInputStream finStream, int count)
    {
        EVTPage[] pages = new EVTPage[count];

        for (int i = 0; i < count; ++i)
        {
            // Read a single page...
            pages[i] = new EVTPage
            {
                operationsOffset = finStream.ReadU32(),
                startCondition = new EVTCondition
                {
                    mode = finStream.ReadU16(),
                    itemID = finStream.ReadU16(),
                    compareTo = finStream.ReadS16(),
                    comparisonType = finStream.ReadU16()
                }
            };

            if (pages[i].operationsOffset != 0)
            {
                finStream.Jump(pages[i].operationsOffset);
                pages[i].operations = ReadOperations(finStream);
                finStream.Return();
            }
        }

        return pages;
    }

    /// <summary>
    /// Loads operations for a page
    /// </summary>
    EVTOperation[] ReadOperations(FileInputStream finStream)
    {
        List<EVTOperation> operations = new List<EVTOperation>();

        short  operation;
        ushort length;

        do
        {
            operation = finStream.ReadS16();
            length    = (ushort)(finStream.ReadU16() - 4);

            EVTOperation op = new ()
            {
                opcode = operation,
                length = length
            };

            if (length > 0)
                op.arguments = finStream.ReadU8Array(length);
            else
                op.arguments = Array.Empty<byte>();

            operations.Add(op);

            if (operation == -1)
                break;

        } while (true);

        return operations.ToArray();
    }
}
