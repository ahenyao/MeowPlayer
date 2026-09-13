using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia.Controls;

namespace MeowPlayer.Utils;

public class ID3Reader {


    public record FrameInfo(string ID3v22, string ID3v23, string ID3v24) {
        public string this[int version] => version switch {
            2 => ID3v22,
            3 => ID3v23,
            4 => ID3v24,
            _ => throw new ArgumentOutOfRangeException(nameof(version), "ID3 version must be 2, 3 or 4.")
        };
    };
    
    private static class TagContainer {
        public static FrameInfo Title { get; } = new FrameInfo("TT2", "TIT2", "TIT2");
        public static FrameInfo Artist { get; } = new FrameInfo("TP1", "TPE1", "TPE1");
        public static FrameInfo Album { get; } = new FrameInfo("TAL", "TALB", "TALB");
        public static FrameInfo TrackNumber { get; } = new FrameInfo("TRK", "TRCK", "TRCK");
        public static FrameInfo DiscNumber { get; } = new FrameInfo("TPA", "TPOS", "TPOS");
        public static FrameInfo Band { get; } = new FrameInfo("TP2", "TPE2", "TPE2");
        public static FrameInfo Composer { get; } = new FrameInfo("TCM", "TCOM", "TCOM");
        public static FrameInfo Genre { get; } = new FrameInfo("TCO", "TCON", "TCON");
        public static FrameInfo Year { get; } = new FrameInfo("TYE", "TYER", "(Merged into TDRC)");
        public static FrameInfo Date { get; } = new FrameInfo("TDA", "TDAT", "(Merged into TDRC)");
        public static FrameInfo Time { get; } = new FrameInfo("TIM", "TIME", "(Merged into TDRC)");
        public static FrameInfo RecordDate { get; } = new FrameInfo("TRD", "TRDA", "(Merged into TDRC)");
        public static FrameInfo RecordTime { get; } = new FrameInfo("None", "None", "TDRC");
        public static FrameInfo ReleaseTime { get; } = new FrameInfo("None", "None", "TDRL");
        public static FrameInfo OrigReleaseYear { get; } = new FrameInfo("TOR", "TORY", "(Merged into TDOR)");
        public static FrameInfo OrigReleaseTime { get; } = new FrameInfo("None", "None", "TDOR");
        public static FrameInfo EncodingTime { get; } = new FrameInfo("None", "None", "TDEN");
        public static FrameInfo TaggingTime { get; } = new FrameInfo("None", "None", "TDTG");
        public static FrameInfo Comments { get; } = new FrameInfo("COM", "COMM", "COMM");
        public static FrameInfo Artwork { get; } = new FrameInfo("PIC", "APIC", "APIC");
        public static FrameInfo UnSyncLyrics { get; } = new FrameInfo("ULT", "USLT", "USLT");
        public static FrameInfo SyncLyrics { get; } = new FrameInfo("SLT", "SYLT", "SYLT");
        public static FrameInfo BPM { get; } = new FrameInfo("TBP", "TBPM", "TBPM");
        public static FrameInfo InitialKey { get; } = new FrameInfo("TKE", "TKEY", "TKEY");
        public static FrameInfo Length { get; } = new FrameInfo("TLE", "TLEN", "TLEN");
        public static FrameInfo Conductor { get; } = new FrameInfo("TP3", "TPE3", "TPE3");
        public static FrameInfo ModifiedBy { get; } = new FrameInfo("TP4", "TPE4", "TPE4");
        public static FrameInfo Lyricist { get; } = new FrameInfo("TXT", "TEXT", "TEXT");
        public static FrameInfo OrigLyricist { get; } = new FrameInfo("TOL", "TOLY", "TOLY");
        public static FrameInfo OrigArtist { get; } = new FrameInfo("TOA", "TOPE", "TOPE");
        public static FrameInfo OrigAlbum { get; } = new FrameInfo("TOT", "TOAL", "TOAL");
        public static FrameInfo OrigFilename { get; } = new FrameInfo("TOF", "TOFN", "TOFN");
        public static FrameInfo ContentGroupDescription { get; } = new FrameInfo("TT1", "TIT1", "TIT1");
        public static FrameInfo SubtitleDescriptionRefinement { get; } = new FrameInfo("TT3", "TIT3", "TIT3");
        public static FrameInfo DiscTitle { get; } = new FrameInfo("None", "None", "TSST");
        public static FrameInfo Publisher { get; } = new FrameInfo("TPB", "TPUB", "TPUB");
        public static FrameInfo ProducedNotice { get; } = new FrameInfo("None", "None", "TPRO");
        public static FrameInfo Copyright { get; } = new FrameInfo("TCR", "TCOP", "TCOP");
        public static FrameInfo EncodedBy { get; } = new FrameInfo("TEN", "TENC", "TENC");
        public static FrameInfo SoftwareHardwareSettings { get; } = new FrameInfo("TSS", "TSSE", "TSSE");
        public static FrameInfo FileType { get; } = new FrameInfo("TFT", "TFLT", "TFLT");
        public static FrameInfo MediaType { get; } = new FrameInfo("TMT", "TMED", "TMED");
        public static FrameInfo ISRC { get; } = new FrameInfo("TRC", "TSRC", "TSRC");
        public static FrameInfo InvolvedPeopleList { get; } = new FrameInfo("IPL", "IPLS", "TIPL");
        public static FrameInfo MusicianCreditsList { get; } = new FrameInfo("None", "None", "TMCL");
        public static FrameInfo TitleSortOrder { get; } = new FrameInfo("None", "TSOT", "TSOT");
        public static FrameInfo PerformerSortOrder { get; } = new FrameInfo("None", "TSOP", "TSOP");
        public static FrameInfo AlbumSortOrder { get; } = new FrameInfo("None", "TSOA", "TSOA");
        public static FrameInfo AlbumArtistSortOrder { get; } = new FrameInfo("None", "TSO2 (non-std)", "TSOP2 / TSO2 (non-std)");
        public static FrameInfo ComposerSortOrder { get; } = new FrameInfo("None", "TSOC", "TSOC");
        public static FrameInfo BandSortOrder { get; } = new FrameInfo("None", "None", "TSOP");
        public static FrameInfo UserDefinedTextInfo { get; } = new FrameInfo("TXX", "TXXX", "TXXX");
        public static FrameInfo CommercialInfoUrl { get; } = new FrameInfo("WCM", "WCOM", "WCOM");
        public static FrameInfo CopyrightLegalInfoUrl { get; } = new FrameInfo("WCP", "WCOP", "WCOP");
        public static FrameInfo OfficialAudioWebpage { get; } = new FrameInfo("WAF", "WOAF", "WOAF");
        public static FrameInfo OfficialArtistWebpage { get; } = new FrameInfo("WAR", "WOAR", "WOAR");
        public static FrameInfo OfficialAudioSrcWebpage { get; } = new FrameInfo("WAS", "WOAS", "WOAS");
        public static FrameInfo OfficialInternetRadioWebpage { get; } = new FrameInfo("WRS", "WORS", "WORS");
        public static FrameInfo PaymentUrl { get; } = new FrameInfo("WPAY", "WPAY", "WPAY");
        public static FrameInfo OfficialPublisherWebpage { get; } = new FrameInfo("WPB", "WPUB", "WPUB");
        public static FrameInfo UserUrl { get; } = new FrameInfo("WXX", "WXXX", "WXXX");
        public static FrameInfo Popularimeter { get; } = new FrameInfo("POP", "POPM", "POPM");
        public static FrameInfo PlayCounter { get; } = new FrameInfo("CNT", "PCNT", "PCNT");
        public static FrameInfo RelativeVolumeAdjustment { get; } = new FrameInfo("RVA", "RVAD", "RVA2");
        public static FrameInfo Equalization { get; } = new FrameInfo("EQU", "EQUA", "EQU2");
        public static FrameInfo Reverb { get; } = new FrameInfo("REV", "RVRB", "RVRB");
        public static FrameInfo MusicCdIdentifier { get; } = new FrameInfo("MCI", "MCDI", "MCDI");
        public static FrameInfo EventTimingCodes { get; } = new FrameInfo("ETC", "ETCO", "ETCO");
        public static FrameInfo MpegLookupTable { get; } = new FrameInfo("MLL", "MLLT", "MLLT");
        public static FrameInfo SyncTempoCodes { get; } = new FrameInfo("STC", "SYTC", "SYTC");
        public static FrameInfo GeneralEncapsulatedObject { get; } = new FrameInfo("GEO", "GEOB", "GEOB");
        public static FrameInfo PrivateFrame { get; } = new FrameInfo("None", "PRIV", "PRIV");
        public static FrameInfo UniqFileId { get; } = new FrameInfo("UFI", "UFID", "UFID");
        public static FrameInfo TermsOfUse { get; } = new FrameInfo("None", "USER", "USER");
        public static FrameInfo CommercialFrame { get; } = new FrameInfo("None", "COMR", "COMR");
        public static FrameInfo OwnershipFrame { get; } = new FrameInfo("None", "OWNE", "OWNE");
        public static FrameInfo EncryptionMethodRegistration { get; } = new FrameInfo("CRM", "ENCR", "ENCR");
        public static FrameInfo GroupIdRegistration { get; } = new FrameInfo("None", "GRID", "GRID");
        public static FrameInfo Encryption { get; } = new FrameInfo("CRA", "AENC", "AENC");
        public static FrameInfo SignatureFrame { get; } = new FrameInfo("None", "None", "SIGN");
        public static FrameInfo SeekFrame { get; } = new FrameInfo("None", "None", "SEEK");
        public static FrameInfo AudioSeekPointIndex { get; } = new FrameInfo("None", "None", "ASPI");
    }
    
    // public string Title;
    // public string Album;
    // public string Artist;
    // public string ;
    // public string ;
    // public string ;

    private byte versionMajor = 0x00;
    private byte versionMinor = 0x00;
    
    List<(string Name, MemoryStream data)> frames = new();
    
    public ID3Reader(string? path = null, Stream? stream = null) {

        if (path == null && stream == null) return;
        
        Stream fileStream;
        fileStream = stream!;
        // fileStream = new FileStream(path, FileMode.Open);
        
        fileStream.Seek(0, SeekOrigin.Begin);

        byte[] header = new byte[10];
        fileStream.ReadExactly(header, 0, header.Length);

        int magicNumber = (header[0] << 16) | (header[1] << 8) | header[2];

        if (magicNumber == 0x494433) {
            byte major = header[3]; // ID3v2.X.0
            byte minor = header[4]; // ID3v2.0.X
            byte flags = header[5];
            int size = (header[6] << 21) | (header[7] << 14) | (header[8] << 7) | header [9]; // 28-bit synchsafe int

            versionMajor = major;
            versionMinor = minor;

            // Console.WriteLine($"ID3v2.{major}.{minor}\nMajor: {major}\nMinor: {minor}\nFlags: {flags:X2}\nSize: {size}\n\n=======\n\n");

            byte[] framesDataArray = new byte[size];
            fileStream.ReadExactly(framesDataArray, 0, framesDataArray.Length);
            MemoryStream framesData = new MemoryStream(framesDataArray);

            int frameNameSize = -1;
            if (major == 0x02) frameNameSize = 3;
            else if (major == 0x03 || major == 0x04) frameNameSize = 4;

            framesData.Seek(0, SeekOrigin.Begin);

            while (framesData.Position < framesData.Length) {
                byte[] frameNameBytes = new byte[frameNameSize];
                framesData.ReadExactly(frameNameBytes, 0, frameNameBytes.Length);
                string frameName = Encoding.ASCII.GetString(frameNameBytes, 0, frameNameBytes.Length);


                if (frameName[0] == '\0') {
                    break;
                }

                int frameSize = -1;
                if (major == 0x02)
                    frameSize = (framesData.ReadByte() << 16) | (framesData.ReadByte() << 8) | framesData.ReadByte(); // 24-bit int
                if (major == 0x03)
                    frameSize = (framesData.ReadByte() << 24) | (framesData.ReadByte() << 16) | (framesData.ReadByte() << 8) | framesData.ReadByte(); // 32-bit int
                if (major == 0x04)
                    frameSize = (framesData.ReadByte() << 21) | (framesData.ReadByte() << 14) | (framesData.ReadByte() << 7) | framesData.ReadByte(); // 28-bit synchsafe int

                if (major == 0x03 || major == 0x04) {
                    byte[] frameFlags = new byte[2];
                    framesData.ReadExactly(frameFlags, 0, frameFlags.Length);
                }

                byte[] data = new byte[frameSize];
                framesData.ReadExactly(data, 0, data.Length);

                byte encodingByte = data[0];
                string stringEncoding = encodingByte switch {
                    0x00 => "iso-8859-1",
                    0x01 => "utf-16",
                    0x02 => "utf-16be",
                    0x03 => "utf-8",
                    _ => "utf-8"
                };


                if (frameName is "PIC" or "APIC") {
                    if (major == 0x02) {
                        byte[] imageFormat = data[1..3];
                        byte imageType = data[4];

                        data = data.Skip(5).ToArray(); // encoding byte(1)  +  imageFormat(3)  +  imageType(1)
                        data = data.Skip(Array.IndexOf(data, (byte)0x00) + 1).ToArray(); // image desc
                    }
                    else {
                        int nulCharNumOfMime = Array.IndexOf(data, (byte)0x00, 1); // skip encoding byte
                        string mimeType = Encoding.ASCII.GetString(data[1..nulCharNumOfMime]);
                        byte imageType = data[nulCharNumOfMime + 1];

                        int nulCharNumOfDesc = -1;

                        if (encodingByte is 0x01 or 0x02) {
                            for (int i = nulCharNumOfMime + 2; i < data.Length - 1; i++) {
                                if (data[i] == 0x00 && data[i + 1] == 0x00) {
                                    nulCharNumOfDesc = i;
                                    break;
                                }
                            }
                        }
                        else {
                            for (int i = nulCharNumOfMime + 2; i < data.Length; i++) {
                                if (data[i] == 0x00) {
                                    nulCharNumOfDesc = i;
                                    break;
                                }
                            }
                        }

                        if (nulCharNumOfDesc == -1) continue;

                        string descType = Encoding.GetEncoding(stringEncoding).GetString(data[(nulCharNumOfMime + 2)..nulCharNumOfDesc]);


                        data = data.Skip(nulCharNumOfDesc + 1).ToArray();
                        if (encodingByte is 0x01 or 0x02) data = data.Skip(2).ToArray();
                    }

                    try {
                        Console.WriteLine(BitConverter.ToString(data).Substring(0, 50));
                        // img_AlbumArt.Source = new Bitmap(new MemoryStream(data));
                    } catch (Exception ex) {
                        Console.WriteLine(ex.ToString());
                    }
                }
                else {
                    data = data.Skip(1).ToArray(); // encoding byte

                    if (encodingByte is 0x01 or 0x02) {
                        if (data.Length >= 3) {
                            if (data[data.Length - 1] == 0x00 && data[data.Length - 2] == 0x00) {
                                data = data.SkipLast(2).ToArray(); // optional id3v2.3+ 2 byte nul terminator
                            }
                        }
                    }
                    else {
                        if (data.Length >= 2) {
                            if (data[data.Length - 1] == 0x00) {
                                data = data.SkipLast(1).ToArray(); // in id3v2.2 nul terminator must be present but just checking
                            }
                        }
                    }

                    if (frameNameSize == 4 && frameName == "COMM") {
                        byte[] lang = { data[0], data[1], data[2] };
                        data = data.Skip(3).ToArray();
                    }

                    // string dataText = Encoding.GetEncoding(stringEncoding).GetString(data);
                    //
                    // string a1 = $"\n{frameName}({frameSize}): \t\t\t{Encoding.ASCII.GetString(data)}";
                    // string a2 = $"\n{BitConverter.ToString(frameNameBytes)}({frameSize}): \t\t\t{BitConverter.ToString(data)}";
                    // string a3 = $"\n{frameName}({frameSize}): \t\t\t{dataText}";
                    //
                    // Console.Write(a2);
                    // Console.WriteLine(a3);
                    // Console.WriteLine(a3 + "\n");

                    MemoryStream textEncode = new MemoryStream([encodingByte, ..data]);
                    frames.Add((frameName, textEncode));
                }
            }
        }
        else {
            // Console.WriteLine("Selected file doesn't contain ID3 metadata.");
            fileStream.Position = 0;
        }
    }

    public object getValueFromTag(string tagHumanName) {
        
        string tagInternalName = "<null>";
        Console.WriteLine("id3 version: " + versionMajor);

        switch (tagHumanName.ToLower()) {
            case "title":
                tagInternalName = TagContainer.Title[versionMajor];
                MemoryStream? value = getTagValue(tagInternalName);
                if (value is not null)  return decodeID3String(value.ToArray());
                break;
            
        }
        
        return $"Tag {tagHumanName} ({tagInternalName}) was null.";
    }

    public MemoryStream? getTagValue(string ID3tagName) {
        MemoryStream ms = frames.FirstOrDefault(f => string.Equals(f.Name, ID3tagName, StringComparison.OrdinalIgnoreCase)).Item2;
        return ms;
    }

    public string decodeID3String(byte[] str) {
        
        string stringEncoding = str[0] switch {
            0x00 => "iso-8859-1",
            0x01 => "utf-16",
            0x02 => "utf-16be",
            0x03 => "utf-8",
            _ => "utf-8"
        };
        
        return Encoding.GetEncoding(stringEncoding).GetString(str[1..]);
    }
}