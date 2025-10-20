using System;
using System.Runtime.InteropServices;
using LawfulBladeSDK.IO;

namespace LawfulBladeSDK.Format.Windows
{
    // Thanks to the following:
    //   https://0xrick.github.io/win-internals/pe3/
    //   https://learn.microsoft.com/en-us/windows/win32/debug/pe-format
    //   http://webserver2.tecgraf.puc-rio.br/~ismael/Cursos/YC++/apostilas/win32_xcoff_pe/tyne-example/Tiny%20PE.htm (mentions section offset calc)

    /// <summary>
    /// Handles the reading, writing and manipulation of a Portable Executable file.
    /// </summary>
    public class FileFormatPE
    {
        PortableExecutableDOSHeader DOSHeader;
        byte[] DosStubAndRich;
        uint Signature;

        PortableExecutableFileHeader FileHeader;
        PortableExecutableNTHeader32 NTHeader32;
        PortableExecutableNTHeader64 NTHeader64;
        PortableExecutableDataDirectory[] DataDirectories;
        PortableExecutableNTSection[] Sections;

        /// <summary>
        /// Opens and reads a portable executable.
        /// </summary>
        /// <param name="filepath">File path to a valid portable executable (not checked)</param>
        /// <returns>The data of the portable executable file</returns>
        public static FileFormatPE ReadFromFile(string filepath)
        {
            FileFormatPE result = new ();

            // Open the file for reading...
            using FileInputStream fis = new FileInputStream(filepath);

            // Read DOS Stub Header
            result.DOSHeader  = fis.ReadStruct<PortableExecutableDOSHeader>();

            // Read DOS Stub and Rich (MSVC) chunks
            result.DosStubAndRich = fis.ReadU8Array((int)(result.DOSHeader.e_lfanew - fis.Position));

            // Read NT Header
            result.Signature  = fis.ReadU32();
            result.FileHeader = fis.ReadStruct<PortableExecutableFileHeader>();

            if ((result.FileHeader.Characteristics & PortableExecutableCharacteristic.X32Machine) > 0)
            {
                result.NTHeader32 = fis.ReadStruct<PortableExecutableNTHeader32>();
                result.DataDirectories = new PortableExecutableDataDirectory[result.NTHeader32.NumberOfRvaAndSizes];
            } 
            else
            {
                result.NTHeader64 = fis.ReadStruct<PortableExecutableNTHeader64>();
                result.DataDirectories = new PortableExecutableDataDirectory[result.NTHeader64.NumberOfRvaAndSizes];
            }

            // Read NT data dictionaries
            for (int i = 0; i < result.DataDirectories.Length; ++i)
                result.DataDirectories[i] = fis.ReadStruct<PortableExecutableDataDirectory>();

            // Read NT Sections
            result.Sections = new PortableExecutableNTSection[result.FileHeader.NumberOfSections];

            for (int i = 0; i < result.FileHeader.NumberOfSections; ++i)
            {
                result.Sections[i] = new PortableExecutableNTSection
                {
                    header = fis.ReadStruct<PortableExecutableNTSectionHeader>()
                };

                fis.Jump(result.Sections[i].header.PointerToRawData);
                result.Sections[i].blob = fis.ReadU8Array((int)result.Sections[i].header.SizeOfRawData);
                fis.Return();
            }

            return result;
        }

        /// <summary>
        /// Writes the portable executable back to a file
        /// </summary>
        /// <param name="filename"></param>
        public void WriteToFile(string filename)
        {

        }

        /// <summary>
        /// Sets the 'LargeAddressAware' flag on the executable
        /// </summary>
        public void SetLAA(bool laaEnabled)
        {
            FileHeader.Characteristics &= ~PortableExecutableCharacteristic.LargeAddressAware;
            FileHeader.Characteristics |= laaEnabled ? PortableExecutableCharacteristic.LargeAddressAware : 0;
        }
    }

    /// <summary>
    /// Structure for the PE DOS stub
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct PortableExecutableDOSHeader
    {
        [FieldOffset(0x00)] public ushort e_magic;                     // Magic number
        [FieldOffset(0x02)] public ushort e_cblp;                      // Bytes on last page of file
        [FieldOffset(0x04)] public ushort e_cp;                        // Pages in file
        [FieldOffset(0x06)] public ushort e_crlc;                      // Relocations
        [FieldOffset(0x08)] public ushort e_cparhdr;                   // Size of header in paragraphs
        [FieldOffset(0x0A)] public ushort e_minalloc;                  // Minimum extra paragraphs needed
        [FieldOffset(0x0C)] public ushort e_maxalloc;                  // Maximum extra paragraphs needed
        [FieldOffset(0x0E)] public ushort e_ss;                        // Initial (relative) SS value
        [FieldOffset(0x10)] public ushort e_sp;                        // Initial SP value
        [FieldOffset(0x12)] public ushort e_csum;                      // Checksum
        [FieldOffset(0x14)] public ushort e_ip;                        // Initial IP value
        [FieldOffset(0x16)] public ushort e_cs;                        // Initial (relative) CS value
        [FieldOffset(0x18)] public ushort e_lfarlc;                    // File address of relocation table
        [FieldOffset(0x1A)] public ushort e_ovno;                      // Overlay number
        [FieldOffset(0x1C)] public fixed ushort e_res[4];              // Reserved words
        [FieldOffset(0x24)] public ushort e_oemid;                     // OEM identifier (for e_oeminfo)
        [FieldOffset(0x26)] public ushort e_oeminfo;                   // OEM information; e_oemid specific
        [FieldOffset(0x28)] public fixed ushort e_res2[10];            // Reserved words
        [FieldOffset(0x3C)] public uint e_lfanew;                      // File address of new exe header
    }

    [Flags]
    public enum PortableExecutableCharacteristic : ushort
    {
        None              = 0       ,
        RelocsStripped    = (1 << 0),   // IMAGE_FILE_RELOCS_STRIPPED 
        Executable        = (1 << 1),   // IMAGE_FILE_EXECUTABLE_IMAGE
        LineNumsStripped  = (1 << 2),   // IMAGE_FILE_LINE_NUMS_STRIPPED
        LocalSymsStripped = (1 << 3),   // IMAGE_FILE_LOCAL_SYMS_STRIPPED
        AggressiveWSTrim  = (1 << 4),   // IMAGE_FILE_AGGRESSIVE_WS_TRIM
        LargeAddressAware = (1 << 5),   // IMAGE_FILE_LARGE_ADDRESS_AWARE
        Reserved0x0040    = (1 << 6),   // Reserved
        LittleEndian      = (1 << 7),   // IMAGE_FILE_BYTES_REVERSED_LO
        X32Machine        = (1 << 8),   // IMAGE_FILE_32BIT_MACHINE
        DebugStripped     = (1 << 9),   // IMAGE_FILE_DEBUG_STRIPPED
        RunInSwap         = (1 << 10),  // IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP
        NetworkRunInSwap  = (1 << 11),  // IMAGE_FILE_NET_RUN_FROM_SWAP
        SystemFile        = (1 << 12),  // IMAGE_FILE_SYSTEM
        DLL               = (1 << 13),  // IMAGE_FILE_DLL
        SystemOnly        = (1 << 14),  // IMAGE_FILE_UP_SYSTEM_ONLY
        BigEndian         = (1 << 15)   // IMAGE_FILE_BYTES_REVERSED_HI
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct PortableExecutableFileHeader
    {
        [FieldOffset(0x00)] public ushort Machine;
        [FieldOffset(0x02)] public ushort NumberOfSections;
        [FieldOffset(0x04)] public uint TimeDateStamp;
        [FieldOffset(0x08)] public uint PointerToSymbolTable;
        [FieldOffset(0x0C)] public uint NumberOfSymbols;
        [FieldOffset(0x10)] public ushort SizeOfOptionalHeader;
        [FieldOffset(0X12)] public PortableExecutableCharacteristic Characteristics;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct PortableExecutableDataDirectory
    {
        [FieldOffset(0x00)] public uint VirtualAddress;
        [FieldOffset(0x04)] public uint Size;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct PortableExecutableNTHeader32
    {
        [FieldOffset(0x00)] public ushort Magic;
        [FieldOffset(0x02)] public byte MajorLinkerVersion;
        [FieldOffset(0x03)] public byte MinorLinkerVersion;
        [FieldOffset(0x04)] public uint SizeOfCode;
        [FieldOffset(0x08)] public uint SizeOfInitializedData;
        [FieldOffset(0x0C)] public uint SizeOfUninitializedData;
        [FieldOffset(0x10)] public uint AddressOfEntryPoint;
        [FieldOffset(0x14)] public uint BaseOfCode;
        [FieldOffset(0x18)] public uint BaseOfData;
        [FieldOffset(0x1C)] public uint ImageBase;
        [FieldOffset(0x20)] public uint SectionAlignment;
        [FieldOffset(0x24)] public uint FileAlignment;
        [FieldOffset(0x28)] public ushort MajorOperatingSystemVersion;
        [FieldOffset(0x2A)] public ushort MinorOperatingSystemVersion;
        [FieldOffset(0x2C)] public ushort MajorImageVersion;
        [FieldOffset(0x2E)] public ushort MinorImageVersion;
        [FieldOffset(0x30)] public ushort MajorSubsystemVersion;
        [FieldOffset(0x32)] public ushort MinorSubsystemVersion;
        [FieldOffset(0x34)] public uint Win32VersionValue;
        [FieldOffset(0x38)] public uint SizeOfImage;
        [FieldOffset(0x3C)] public uint SizeOfHeaders;
        [FieldOffset(0x40)] public uint CheckSum;
        [FieldOffset(0x44)] public ushort Subsystem;
        [FieldOffset(0x46)] public ushort DllCharacteristics;
        [FieldOffset(0x48)] public uint SizeOfStackReserve;
        [FieldOffset(0x4C)] public uint SizeOfStackCommit;
        [FieldOffset(0x50)] public uint SizeOfHeapReserve;
        [FieldOffset(0x54)] public uint SizeOfHeapCommit;
        [FieldOffset(0x58)] public uint LoaderFlags;
        [FieldOffset(0x5C)] public uint NumberOfRvaAndSizes;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct PortableExecutableNTHeader64
    {
        [FieldOffset(0x00)] public ushort Magic;
        [FieldOffset(0x02)] public byte MajorLinkerVersion;
        [FieldOffset(0x03)] public byte MinorLinkerVersion;
        [FieldOffset(0x04)] public uint SizeOfCode;
        [FieldOffset(0x08)] public uint SizeOfInitializedData;
        [FieldOffset(0x0C)] public uint SizeOfUninitializedData;
        [FieldOffset(0x10)] public uint AddressOfEntryPoint;
        [FieldOffset(0x14)] public uint BaseOfCode;
        [FieldOffset(0x18)] public ulong ImageBase;
        [FieldOffset(0x20)] public uint SectionAlignment;
        [FieldOffset(0x24)] public uint FileAlignment;
        [FieldOffset(0x28)] public ushort MajorOperatingSystemVersion;
        [FieldOffset(0x2A)] public ushort MinorOperatingSystemVersion;
        [FieldOffset(0x2C)] public ushort MajorImageVersion;
        [FieldOffset(0x2E)] public ushort MinorImageVersion;
        [FieldOffset(0x30)] public ushort MajorSubsystemVersion;
        [FieldOffset(0x32)] public ushort MinorSubsystemVersion;
        [FieldOffset(0x34)] public uint Win32VersionValue;
        [FieldOffset(0x38)] public uint SizeOfImage;
        [FieldOffset(0x3C)] public uint SizeOfHeaders;
        [FieldOffset(0x40)] public uint CheckSum;
        [FieldOffset(0x44)] public ushort Subsystem;
        [FieldOffset(0x46)] public ushort DllCharacteristics;

        [FieldOffset(0x48)] public ulong SizeOfStackReserve;
        [FieldOffset(0x50)] public ulong SizeOfStackCommit;
        [FieldOffset(0x58)] public ulong SizeOfHeapReserve;
        [FieldOffset(0x60)] public ulong SizeOfHeapCommit;
        [FieldOffset(0x68)] public uint LoaderFlags;
        [FieldOffset(0x6C)] public uint NumberOfRvaAndSizes;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct PortableExecutableNTSectionHeader
    {
        [FieldOffset(0x00)] public fixed byte Name[8];
        [FieldOffset(0x08)] public uint PhysicalAddress;
        [FieldOffset(0x08)] public uint VirtualSize;
        [FieldOffset(0x0C)] public uint VirtualAddress;
        [FieldOffset(0x10)] public uint SizeOfRawData;
        [FieldOffset(0x14)] public uint PointerToRawData;
        [FieldOffset(0x18)] public uint PointerToRelocations;
        [FieldOffset(0x1C)] public uint PointerToLinenumbers;
        [FieldOffset(0x20)] public ushort NumberOfRelocations;
        [FieldOffset(0x22)] public ushort NumberOfLinenumbers;
        [FieldOffset(0x24)] public uint Characteristics;
    }

    public struct PortableExecutableNTSection
    {
        public PortableExecutableNTSectionHeader header;
        public byte[] blob;
    }
}