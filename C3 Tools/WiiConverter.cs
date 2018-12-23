﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using C3Tools.Properties;
using C3Tools.x360;
using SearchOption = System.IO.SearchOption;
using NAudio.Midi;
using System.Drawing;

namespace C3Tools
{
    public partial class WiiConverter : Form
    {
        private static string inputDir;
        private static List<string> inputFiles;
        private DateTime endTime;
        private DateTime startTime;
        private string wiifolder;
        private readonly string wimgt;
        private readonly string rar;
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        private bool VenueFixed;
        private string songmeta;
        private string songdata;
        private string songextracted;
        private string songname;

        public WiiConverter(Color ButtonBackColor, Color ButtonTextColor, string Folder)
        {
            InitializeComponent();
            
            Tools = new NemoTools();
            Parser = new DTAParser();
            inputFiles = new List<string>();

            if (Folder != "")
            {
                inputDir = Folder;
            }
            else
            {
                inputDir = Application.StartupPath + "\\wiiprep_input\\";
                if (!Directory.Exists(inputDir))
                {
                    Directory.CreateDirectory(inputDir);
                } 
            }

            wimgt = Application.StartupPath + "\\bin\\wimgt.exe";
            if (!File.Exists(wimgt))
            {
                MessageBox.Show("Can't find wimgt.exe ... I won't be able to convert the album art without it", "Missing Executable",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            rar = Application.StartupPath + "\\bin\\rar.exe";
            if (!File.Exists(rar))
            {
                MessageBox.Show("Can't find rar.exe ... I won't be able to create RAR files for your songs without it",
                                "Missing Executable", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                chkRAR.Checked = false;
                chkRAR.Enabled = false;
            }

            var formButtons = new List<Button> { btnFolder, btnRefresh, btnReset, btnBegin };
            foreach (var button in formButtons)
            {
                button.BackColor = ButtonBackColor;
                button.ForeColor = ButtonTextColor;
                button.FlatAppearance.MouseOverBackColor = button.BackColor == Color.Transparent ? Color.FromArgb(127, Color.AliceBlue.R, Color.AliceBlue.G, Color.AliceBlue.B) : Tools.LightenColor(button.BackColor);
            }

            toolTip1.SetToolTip(btnBegin, "Click to begin process");
            toolTip1.SetToolTip(btnFolder, "Click to select the input folder");
            toolTip1.SetToolTip(btnRefresh, "Click to refresh if the contents of the folder have changed");
            toolTip1.SetToolTip(txtFolder, "This is the working directory");
            toolTip1.SetToolTip(lstLog, "This is the application log. Right click to export");
            toolTip1.SetToolTip(chkDummy,"Enable this to replace the song's animation file with a basic one that works!");
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            //if user selects new folder, assign that value
            //if user cancels or selects same folder, this forces the text_changed event to run again
            var tFolder = txtFolder.Text;

            var folderUser = new FolderBrowserDialog
                {
                    SelectedPath = txtFolder.Text, 
                    Description = "Select the folder where your CON files are",
                };
            txtFolder.Text = "";
            var result = folderUser.ShowDialog();
            txtFolder.Text = result == DialogResult.OK ? folderUser.SelectedPath : tFolder;
        }

        private void Log(string message)
        {
            if (lstLog.InvokeRequired)
            {
                lstLog.Invoke(new MethodInvoker(() => lstLog.Items.Add(message)));
                lstLog.Invoke(new MethodInvoker(() => lstLog.SelectedIndex = lstLog.Items.Count - 1));
            }
            else
            {
                lstLog.Items.Add(message);
                lstLog.SelectedIndex = lstLog.Items.Count - 1;
            }
        }
        
        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            if (picWorking.Visible) return;
            inputFiles.Clear();

            if (string.IsNullOrWhiteSpace(txtFolder.Text))
            {
                btnRefresh.Visible = false;
            }
            btnRefresh.Visible = true;

            if (txtFolder.Text != "")
            {
                Tools.CurrentFolder = txtFolder.Text;
                Log("");
                Log("Reading input directory ... hang on");

                try
                {
                    var inFiles = Directory.GetFiles(txtFolder.Text);
                    foreach (var file in inFiles)
                    {
                        try
                        {
                            if (VariousFunctions.ReadFileType(file) == XboxFileType.STFS)
                            {
                                inputFiles.Add(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (Path.GetExtension(file) != "") continue;
                            Log("There was a problem accessing file " + Path.GetFileName(file));
                            Log("The error says: " + ex.Message);
                        }
                    }
                    if (!inputFiles.Any())
                    {
                        Log("Did not find any CON files ... try a different directory");
                        Log("You can also drag and drop CON files here");
                        Log("Ready");
                        btnBegin.Visible = false;
                        btnRefresh.Visible = true;
                    }
                    else
                    {
                        Log("Found " + inputFiles.Count + " CON " + (inputFiles.Count > 1 ? "files" : "file"));
                        Log("Ready to begin");
                        btnBegin.Visible = true;
                        btnRefresh.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    Log("There was an error: " + ex.Message);
                }
            }
            else
            {
                btnBegin.Visible = false;
                btnRefresh.Visible = false;
            }
            txtFolder.Focus();
        }

        private void ProcessDTA(string rawdta)
        {
            var umlaut = false;
            if (!File.Exists(rawdta)) return;
            var sr = new StreamReader(rawdta, Encoding.Default);
            var sw = new StreamWriter(Path.GetDirectoryName(rawdta) + "\\songs.dta", false, Encoding.Default);
            while (sr.Peek() >= 0)
            {
                var line = sr.ReadLine();
                if (line.Contains("ÿ") || line.Contains("Ÿ"))
                {
                    umlaut = true;
                }
                if (line.Contains("songs/"))
                {
                    line = line.Replace("songs/", "dlc/sZAE/000/content/songs/");
                }
                else if (line.Contains("utf8") && !umlaut)
                {
                    line = line.Replace("utf8", "latin1");
                }
                else if ((line.ToLowerInvariant().Contains("'version'") || line.ToLowerInvariant().Contains("(version")) && VenueFixed)
                {
                    line = "   ('version' 1)";
                }
                sw.WriteLine(line);
            }
            sr.Dispose();
            sw.Dispose();
            Tools.DeleteFile(rawdta);
            Log("Processed songs.dta file successfully");
        }
        
        private bool FixMIDI(string input, string output)
        {
            var rbn2 = Tools.NemoLoadMIDI(input);
            if (rbn2 == null)
            {
                Log("Failed to read the MIDI file, venue was not modified");
                return false;
            }

            var note_length = rbn2.DeltaTicksPerQuarterNote/4; //16th note
            var to_remove = new List<MidiEvent>();
            var to_add = new List<MidiEvent>();
            var venue = -1;
            long lastevent = (note_length * -1) -1; //this ensures (lastevent + note_length) starts at -1
            long final_event = 0;

            for (var i = 0; i < rbn2.Events.Tracks; i++)
            {
                if (!rbn2.Events[i][0].ToString().Contains("VENUE")) continue;
                venue = i;
                long last_first = 0;
                long last_next = 0;
                long last_proc_time = 0;
                var last_proc_note = 0;

                to_add.Add(new TextEvent("[verse]",MetaEventType.TextEvent, 0));
                foreach (var venue_events in rbn2.Events[i])
                {
                    final_event = venue_events.AbsoluteTime;
                    if (venue_events.CommandCode == MidiCommandCode.MetaEvent && venue_events.ToString().Contains("["))
                    {
                        var venue_event = (MetaEvent) venue_events;
                        var index = venue_event.ToString().IndexOf("[", StringComparison.Ordinal);
                        var new_event = venue_event.ToString().Substring(index, venue_event.ToString().Length - index).Trim();
                        
                        if (new_event.Contains("[directed"))
                        {
                            new_event = new_event.Replace("[directed_vocals_cam_pt]", "[directed_vocals_cam]");
                            new_event = new_event.Replace("[directed_vocals_cam_pr]", "[directed_vocals_cam]");
                            new_event = new_event.Replace("[directed_guitar_cam_pt]", "[directed_guitar_cam]");
                            new_event = new_event.Replace("[directed_guitar_cam_pr]", "[directed_guitar_cam]");
                            new_event = new_event.Replace("[directed_crowd]", "[directed_crowd_g]");
                            new_event = new_event.Replace("[directed_duo_drums]", "[directed_drums]");

                            new_event = new_event.Replace("[directed_duo_kv]", "[directed_duo_guitar]");
                                //keys not supported
                            new_event = new_event.Replace("[directed_duo_kb]", "[directed_duo_gb]");
                            new_event = new_event.Replace("[directed_duo_kg]", "[directed_duo_gb]");
                                //all instances replaced
                            new_event = new_event.Replace("[directed_keys]", "[directed_crowd_b]");
                            new_event = new_event.Replace("[directed_keys_cam]", "[directed_crowd_b]");
                                //with arbitrary choices
                            new_event = new_event.Replace("[directed_keys_np]", "[directed_crowd_b]");

                            new_event = new_event.Replace("[directed", "[do_directed_cut directed");
                            to_add.Add(new TextEvent(new_event, MetaEventType.TextEvent, venue_events.AbsoluteTime));
                        }
                        else if (new_event.Contains("[lighting") && venue_events.AbsoluteTime == 0)
                        {
                            new_event = "[lighting ()]";
                            to_add.Add(new TextEvent(new_event, MetaEventType.TextEvent, venue_events.AbsoluteTime));
                        }
                        else if (new_event.Contains("[lighting (manual") || new_event.Contains("[lighting (dischord)]"))
                        {
                            if (venue_events.AbsoluteTime <= last_next)
                            {
                                to_remove.Add(venue_events);
                                continue;
                            }

                            //add First Frame note as found in most RBN1 MIDIs
                            var note = new NoteOnEvent(venue_events.AbsoluteTime, 1, 50, 96, note_length);
                            to_add.Add(note);
                            to_add.Add(new NoteEvent(note.AbsoluteTime + note.NoteLength,note.Channel,MidiCommandCode.NoteOff, note.NoteNumber,0));
                            last_first = note.AbsoluteTime + note.NoteLength; //to prevent having both Next and First events in the same spot
                            continue;
                        }
                        else if (new_event.Contains("[lighting (verse)]"))
                        {
                            new_event = "[verse]";
                            to_add.Add(new TextEvent(new_event, MetaEventType.TextEvent, venue_events.AbsoluteTime));
                        }
                        else if (new_event.Contains("[lighting (chorus)]"))
                        {
                            new_event = "[chorus]";
                            to_add.Add(new TextEvent(new_event, MetaEventType.TextEvent, venue_events.AbsoluteTime));
                        }
                        else if (new_event.Contains("[lighting (intro)]"))
                        {
                            new_event = "[lighting ()]";
                            to_add.Add(new TextEvent(new_event, MetaEventType.TextEvent, venue_events.AbsoluteTime));
                        }
                        else if (new_event.Contains("[lighting (blackout_spot)]"))
                        {
                            new_event = "[lighting (silhouettes_spot)]";
                            to_add.Add(new TextEvent(new_event, MetaEventType.TextEvent, venue_events.AbsoluteTime));
                        }
                        else if (new_event.Contains("[next]"))
                        {
                            if (venue_events.AbsoluteTime <= last_first)
                            {
                                to_remove.Add(venue_events);
                                continue;
                            }

                            var note = new NoteOnEvent(venue_events.AbsoluteTime, 1, 48, 96, note_length);
                            to_add.Add(note);
                            to_add.Add(new NoteEvent(note.AbsoluteTime + note.NoteLength,note.Channel,MidiCommandCode.NoteOff, note.NoteNumber,0));
                            last_next = note.AbsoluteTime + note.NoteLength; //to prevent having both Next and First events in the same spot
                        }
                        else if (new_event.Contains(".pp]"))
                        {
                            if (!attemptToConvertPostprocEvents.Enabled || !attemptToConvertPostprocEvents.Checked)
                            {
                                to_remove.Add(venue_events);
                                continue;
                            }
                            
                            var note = new NoteOnEvent(venue_events.AbsoluteTime, 1, 0, 96, note_length);
                            switch (new_event)
                            {
                                case "[ProFilm_a.pp]":
                                case "[ProFilm_b.pp]":
                                    note.NoteNumber = 96;
                                    break;
                                case "[film_contrast.pp]":
                                case "[film_contrast_green.pp]":
                                case "[film_contrast_red.pp]":
                                case "[contrast_a.pp]":
                                    note.NoteNumber = 97;
                                    break;
                                case "[desat_posterize_trails.pp]":
                                case "[film_16mm.pp]":
                                    note.NoteNumber = 98;
                                    break;
                                case "[film_sepia_ink.pp]":
                                    note.NoteNumber = 99;
                                    break;
                                case "[film_silvertone.pp]":
                                    note.NoteNumber = 100;
                                    break;
                                case "[horror_movie_special.pp]":
                                case "[ProFilm_psychedelic_blue_red.pp]":
                                case "[photo_negative.pp]":
                                    note.NoteNumber = 101;
                                    break;
                                case "[photocopy.pp]":
                                    note.NoteNumber = 102;
                                    break;
                                case "[posterize.pp]":
                                case "[bloom.pp]":
                                    note.NoteNumber = 103;
                                    break;
                                case "[bright.pp]":
                                    note.NoteNumber = 104;
                                    break;
                                case "[ProFilm_mirror_a.pp]":
                                    note.NoteNumber = 105;
                                    break;
                                case "[desat_blue.pp]":
                                case "[film_contrast_blue.pp]":
                                case "[film_blue_filter.pp]":
                                    note.NoteNumber = 106;
                                    break;
                                case "[video_a.pp]":
                                    note.NoteNumber = 107;
                                    break;
                                case "[video_bw.pp]":
                                case "[film_b+w.pp]":
                                    note.NoteNumber = 108;
                                    break;
                                case "[shitty_tv.pp]":
                                case "[video_security.pp]":
                                    note.NoteNumber = 109;
                                    break;
                                case "[video_trails.pp]":
                                case "[flicker_trails.pp]":
                                case "[space_woosh.pp]":
                                case "[clean_trails.pp]":
                                    note.NoteNumber = 110;
                                    break;
                            }
                                                        
                            //reduces instances of pp notes to bare minimum
                            if (note.NoteNumber > 0 && note.NoteNumber != last_proc_note && note.AbsoluteTime >= last_proc_time)
                            {
                                to_add.Add(note);
                                to_add.Add(new NoteEvent(note.AbsoluteTime + note.NoteLength,note.Channel,MidiCommandCode.NoteOff, note.NoteNumber,0));
                            }

                            //we want at least 1 measure between pp effects
                            last_proc_time = note.AbsoluteTime + (rbn2.DeltaTicksPerQuarterNote * 4);
                            //we don't want to put multiple PP notes for the same effect
                            last_proc_note = note.NoteNumber;
                        }
                        else if (new_event.Contains("[coop"))
                        {
                            if (venue_events.AbsoluteTime <= (lastevent + note_length)) //to avoid double notes)
                            {
                                to_remove.Add(venue_events);
                                continue;
                            }

                            var cameranotes = new NoteOnEvent[9];
                            var enabled = new bool[9];
                            lastevent = venue_events.AbsoluteTime;

                            const int cameracut = 0; //60
                            const int bass = 1; //61
                            const int drummer = 2; //62
                            const int guitar = 3; //63
                            const int vocals = 4; //64
                            const int nobehind = 5; //70
                            const int onlyfar = 6; //71
                            const int onlyclose = 7; //72
                            const int noclose = 8; //73

                            cameranotes[cameracut] = new NoteOnEvent(venue_events.AbsoluteTime, 1, 60, 96, note_length);
                            cameranotes[bass] = new NoteOnEvent(venue_events.AbsoluteTime, 1, 61, 96, note_length);
                            cameranotes[drummer] = new NoteOnEvent(venue_events.AbsoluteTime, 1, 62, 96, note_length);
                            cameranotes[guitar] = new NoteOnEvent(venue_events.AbsoluteTime, 1, 63, 96, note_length);
                            cameranotes[vocals] = new NoteOnEvent(venue_events.AbsoluteTime, 1, 64, 96, note_length);
                            cameranotes[nobehind] = new NoteOnEvent(venue_events.AbsoluteTime, 1, 70, 96, note_length);
                            cameranotes[onlyfar] = new NoteOnEvent(venue_events.AbsoluteTime, 1, 71, 96, note_length);
                            cameranotes[onlyclose] = new NoteOnEvent(venue_events.AbsoluteTime, 1, 72, 96, note_length);
                            cameranotes[noclose] = new NoteOnEvent(venue_events.AbsoluteTime, 1, 73, 96, note_length);

                            enabled[cameracut] = true; //always enabled for [coop shots

                            //players
                            if (new_event.Contains("all_"))
                            {
                                enabled[drummer] = true;
                                enabled[bass] = true;
                                enabled[guitar] = true;
                                enabled[vocals] = true;
                            }
                            else if (new_event.Contains("front_"))
                            {
                                enabled[bass] = true;
                                enabled[guitar] = true;
                                enabled[vocals] = true;
                            }
                            else if (new_event.Contains("v_") || new_event.Contains("_v"))
                                //this allows for single or duo shots
                            {
                                enabled[vocals] = true;
                            }
                            else if (new_event.Contains("g_") || new_event.Contains("_g"))
                            {
                                enabled[guitar] = true;
                            }
                            else if ((new_event.Contains("b_") || new_event.Contains("_b")) &&
                                     !new_event.Contains("behind"))
                            {
                                enabled[bass] = true;
                            }
                            else if (new_event.Contains("d_") || new_event.Contains("_d"))
                            {
                                enabled[drummer] = true;
                            }
                            else if (new_event.Contains("k_") || new_event.Contains("_k"))
                            {
                                enabled[guitar] = true; //keys not supported
                            }

                            //camera placement
                            if (new_event.Contains("behind"))
                            {
                                enabled[noclose] = true;
                            }
                            else if (new_event.Contains("near") || new_event.Contains("closeup"))
                            {
                                enabled[nobehind] = true;
                                enabled[onlyclose] = true;
                            }
                            else if (new_event.Contains("far"))
                            {
                                enabled[nobehind] = true;
                                enabled[noclose] = true;
                                enabled[onlyfar] = true;
                            }

                            //add the notes that are enabled
                            for (var c = 0; c < 9; c++)
                            {
                                if (!enabled[c]) continue;
                                to_add.Add(cameranotes[c]);
                                to_add.Add(new NoteEvent(cameranotes[c].AbsoluteTime + cameranotes[c].NoteLength, cameranotes[c].Channel, MidiCommandCode.NoteOff, cameranotes[c].NoteNumber,0));
                            }
                        }
                        else
                        {
                            continue;
                        }
                        to_remove.Add(venue_events);
                    }
                    else if (venue_events.CommandCode == MidiCommandCode.MetaEvent)
                    {
                        var venue_event = (MetaEvent)venue_events;
                        if (venue_event.MetaEventType == MetaEventType.EndTrack)
                        {
                            to_remove.Add(venue_events);
                        }
                    }
                    else switch (venue_events.CommandCode)
                    {
                        case MidiCommandCode.NoteOn:
                            {
                                var note = (NoteOnEvent) venue_events;
                                if (note.NoteNumber == 41) //can't have keys spotlight
                                {
                                    to_remove.Add(note);
                                }
                            }
                            break;
                        case MidiCommandCode.NoteOff:
                            {
                                var note = (NoteEvent)venue_events;
                                if (note.NoteNumber == 41) //can't have keys spotlight
                                {
                                    to_remove.Add(note);
                                }
                            }
                            break;
                    }
                }
            }

            if (venue == -1)
            {
                Log("No VENUE track found in MIDI file " + Path.GetFileName(input));
                return false;
            }

            foreach (var remove in to_remove)
            {
                rbn2.Events[venue].Remove(remove);
            }
            foreach (var add in to_add)
            {
                rbn2.Events[venue].Add(add);
            }
            rbn2.Events[venue].Add(new MetaEvent(MetaEventType.EndTrack, 0,final_event + (note_length * 2)));
            
            try
            {
                MidiFile.Export(output, rbn2.Events);
            }
            catch (Exception)
            {
                return false; //if exporting fails
            }
            return true;
        }

        private bool ProcessFiles()
        {
            songdata = "";
            songextracted = "";
            songmeta = "";

            var counter = 0;
            var success = 0;

            foreach (var file in inputFiles.Where(File.Exists).TakeWhile(file => !backgroundWorker1.CancellationPending))
            {
                try
                {
                    if (VariousFunctions.ReadFileType(file) != XboxFileType.STFS) continue;
                    try
                    {
                        if (!Directory.Exists(wiifolder))
                        {
                            Directory.CreateDirectory(wiifolder);
                        }
                        counter++;

                        songname = "";
                        Parser.ExtractDTA(file);
                        Parser.ReadDTA(Parser.DTA);
                        if (Parser.Songs.Count > 1)
                        {
                            Log("File " + Path.GetFileName(file) + " is a pack, try dePACKing first, skipping...");
                            continue;
                        }
                        if (!Parser.Songs.Any())
                        {
                            Log("There was an error processing the songs.dta file");
                            continue;
                        }

                        var instruct = Application.StartupPath + "\\bin\\wii.txt";
                        if (File.Exists(instruct))
                        {
                            if (!File.Exists(wiifolder + "Instructions.txt"))
                            {
                                File.Copy(instruct, wiifolder + "Instructions.txt");
                                Log("Copied instructions file successfully");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No instructions file found, so I didn't include it with the song files\n\nRemember this file must be placed in the /bin subdirectory to where C3Tools.exe is found and be named wii.txt", Text, MessageBoxButtons.OK,
                                            MessageBoxIcon.Exclamation);
                        }
                        
                        if (Parser.Songs.Count > 0)
                        {
                            songname = Parser.Songs[0].InternalName;
                            Log("Internal name for file #" + counter + " (" + (Path.GetFileName(file)) + ") is '" + songname + "'");
                        }
                        else
                        {
                            songname = Path.GetFileNameWithoutExtension(file);
                            songname = songname.Replace(" ", "").Replace("-","").Replace("'","").Replace("(","").Replace(")","").Trim();
                            Log("Could not get internal name from the songs.dta file");
                            Log("Defaulting to filename '" + songname + "' for song name");
                        }

                        songextracted = wiifolder + songname + "_extracted\\";
                        songmeta = wiifolder + "000_00000000_" + songname + "_meta\\content\\songs\\";
                        songdata = wiifolder + "000_00000000_" + songname + "_song\\content\\songs\\" + songname + "\\";

                        Tools.DeleteFolder(songextracted,true);
                        Tools.DeleteFolder(songmeta, true);
                        Tools.DeleteFolder(songdata, true);

                        var xPackage = new STFSPackage(file);
                        if (!xPackage.ParseSuccess)
                        {
                            Log("Failed to parse '" + Path.GetFileName(file) + "'");
                            Log("Skipping this file");
                            continue;
                        }
                        xPackage.ExtractPayload(songextracted, true, false);
                        xPackage.CloseIO();

                        Directory.CreateDirectory(songmeta + songname + "\\gen\\");
                        Directory.CreateDirectory(songdata + "\\gen\\");

                        var midi = Directory.GetFiles(songextracted, "*.mid", SearchOption.AllDirectories);
                        if (midi.Count() != 0)
                        {
                            var newmidi = songdata + Path.GetFileName(midi[0]);
                            Tools.DeleteFile(newmidi);
                            
                            if (convertVenueDuringSongConversion.Checked)
                            {
                                VenueFixed = FixMIDI(midi[0], newmidi);
                                if (VenueFixed) //will convert RBN2 venue to RBN1 and export in right folder
                                {
                                    Log("Converted RBN2 venue to RBN1 successfully ... using modified MIDI file");
                                }
                                else
                                {
                                    Log("Failed at converting RBN2 venue to RBN1 ... using original MIDI file");
                                    Tools.MoveFile(midi[0], newmidi);
                                }
                            }
                            else
                            {
                                Log("Venue not converted ... using unmodified MIDI file");
                                Tools.MoveFile(midi[0], newmidi);
                            }
                            if (File.Exists(newmidi))
                            {
                                Log("Extracted MIDI file " + Path.GetFileName(midi[0] + " successfully"));
                            }
                            else
                            {
                                Log("There was a problem extracting the MIDI file");
                            }
                        }
                        else
                        {
                            Log("WARNING: Did not find a MIDI file in the extracted folder");
                        }

                        var dtaOut = Directory.GetFiles(songextracted, "songs.dta", SearchOption.AllDirectories);
                        if (dtaOut.Count() != 0)
                        {
                            var rawdta = songmeta + "raw.dta";
                            Tools.DeleteFile(rawdta);
                            if (Tools.MoveFile(dtaOut[0], rawdta))
                            {
                                Log("Extracted songs.dta file successfully");
                                ProcessDTA(rawdta);
                            }
                            else
                            {
                                Log("There was a problem extracting the songs.dta file");
                            }
                        }
                        else
                        {
                            Log("WARNING: Did not find a songs.dta file in the extracted folder");
                        }
                            
                        var art = Directory.GetFiles(songextracted, "*.png_xbox", SearchOption.AllDirectories);
                        if (art.Count() != 0)
                        {
                            var newart = songmeta + songname + "\\gen\\" + Path.GetFileName(art[0]);
                            Tools.DeleteFile(newart);
                            if (Tools.MoveFile(art[0], newart))
                            {
                                Log("Extracted album art file " + Path.GetFileName(art[0]) + " successfully");
                                if (Tools.ConvertImagetoWii(wimgt, newart, newart, true))
                                {
                                    Log("Converted album art file to png_wii successfully");
                                }
                            }
                            else
                            {
                                Log("There was a problem extracting the album art file");
                            }
                        }
                        else
                        {
                            Log("WARNING: Did not find album art file in the extracted folder");
                        }
                           
                        var mogg = Directory.GetFiles(songextracted, "*.mogg", SearchOption.AllDirectories);
                        if (mogg.Count() != 0)
                        {
                            var newmogg = songdata + Path.GetFileName(mogg[0]);
                            Tools.DeleteFile(newmogg);
                            if (Tools.MoveFile(mogg[0], newmogg))
                            {
                                Log("Extracted mogg file " + Path.GetFileName(mogg[0]) + " successfully");
                                if (chkCreatePreview.Checked)
                                {
                                    Log("Creating preview clip...");
                                    CreatePreviewClip(file, newmogg, Parser.Songs[0].PreviewStart / 1000);
                                }
                                if (chkEncrypt.Checked)
                                {
                                    Log("Encrypting mogg file...");
                                    Log(Tools.EncM(File.ReadAllBytes(newmogg), newmogg) ? "Encrypted mogg file successfully" : "Failed to encrypt mogg file");
                                }
                            }
                            else
                            {
                                Log("There was a problem extracting the mogg file");
                            }
                        }
                        else
                        {
                            Log("WARNING: Did not find a mogg file in the extracted folder");
                        }
                            
                        var milo = Directory.GetFiles(songextracted, "*.milo_xbox", SearchOption.AllDirectories);
                        if (milo.Count() != 0)
                        {
                            var wiimilo = songdata + "gen\\" + Path.GetFileNameWithoutExtension(milo[0]) +".milo_wii";
                            Tools.DeleteFile(wiimilo);
                            if (Tools.MoveFile(milo[0], wiimilo))
                            {
                                Log("Extracted and renamed milo file " + Path.GetFileName(wiimilo) + " successfully");
                            }
                            else
                            {
                                Log("There was a problem extracting the milo file");
                            }
                        }
                        else
                        {
                            Log("WARNING: Did not find a milo file in the extracted folder");
                        }

                        Tools.DeleteFolder(songextracted, true);
                        success++;
                        if (!chkRAR.Checked || backgroundWorker1.CancellationPending) continue;
                        var meta = wiifolder + "000_00000000_" + songname + "_meta";
                        var song = wiifolder + "000_00000000_" + songname + "_song";
                        var archive = Path.GetFileName(file);
                        archive = archive.Replace(" ", "").Replace("-", "_").Replace("\\", "").Replace("'", "").Replace(",", "").Replace("_rb3con", "");
                        archive = Tools.CleanString(archive, false);
                        archive = wiifolder + archive + "_wii.rar";

                        var inst = "";
                        if (File.Exists(wiifolder + "Instructions.txt"))
                        {
                            inst = " \"" + wiifolder + "Instructions.txt\"";
                        }
                                
                        Log("Creating RAR archive for " + songname);
                        
                        bool rarSuccess = Tools.CreateRAR(new NemoTools.RAROptions
                        {
                            Compression = 5,
                            PathMode = NemoTools.RAROptions.FilePathMode.ExcludeBase,
                            InputPaths = new[] { meta, song, inst },
                            OutputPath = archive,
                            Recurse = true
                        });

                        Log(rarSuccess ? "Created RAR archive successfully" : "RAR archive creation failed");
                    }
                    catch (Exception ex)
                    {
                        Log("There was an error: " + ex.Message);
                        Log("Attempting to continue with the next file");
                    }
                }
                catch (Exception ex)
                {
                    Log("There was a problem accessing file " + Path.GetFileName(file));
                    Log("The error says: " + ex.Message);
                }
            }
            Log("Successfully processed " + success + " of " + counter + " files");
            return true;
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var tFolder = txtFolder.Text;
            txtFolder.Text = "";
            txtFolder.Text = tFolder;
        }
        
        private void CreatePreviewClip(string con, string m, double time)
        {
            var p_o = Path.GetTempPath() + "preview.ogg";
            var splitter = new MoggSplitter();
            splitter.DownmixMogg(con, p_o, MoggSplitter.MoggSplitFormat.OGG, "3", true, time, (double)numLength.Value, (int)numFadeIn.Value, (int)numFadeOut.Value, (double)numAttenuation.Value);
            if (!File.Exists(p_o))
            {
                Log("Creating preview clip failed");
                return;
            }
            var p_m = songmeta + songname + "\\" + Path.GetFileName(m.Replace(".mo", "_prev.mo"));
            AddMHeader(p_o, p_m);
            Tools.DeleteFile(p_o);
            if (!File.Exists(p_m))
            {
                Log("Creating preview clip failed");
                return;
            }
            Log("Created preview clip successfully");
        }

        private void AddMHeader(string o, string m)
        {
            var header = Application.StartupPath + "\\bin\\mogg_header";
            if (!File.Exists(header))
            {
                Log("Can't find mogg_header file in the \bin directory");
                Log("Can't create preview clip without it");
                return;
            }
            Tools.DeleteFile(m);
            using (var bw = new BinaryWriter(new FileStream(m, FileMode.Create, FileAccess.Write)))
            {
                var input = File.ReadAllBytes(header);
                bw.Write(input);
                input = File.ReadAllBytes(o);
                bw.Write(input);
            }
        }

        private void HandleDragDrop(object sender, DragEventArgs e)
        {
            if (picWorking.Visible) return;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (btnReset.Visible)
            {
                btnReset.PerformClick();
            }
            if (File.GetAttributes(files[0]).HasFlag(FileAttributes.Directory))
            {
                doSmartMerge(files.ToList());
            }
            else if (VariousFunctions.ReadFileType(files[0]) == XboxFileType.STFS)
            {
                txtFolder.Text = Path.GetDirectoryName(files[0]);
                Tools.CurrentFolder = txtFolder.Text;
            }
            else if (files[0].ToLowerInvariant().EndsWith(".bin", StringComparison.Ordinal))
            {
                Tools.CurrentFolder = Path.GetDirectoryName(files[0]);
                Tools.ProcessBINFile(files[0], Text);
            }
            else
            {
                MessageBox.Show("That's not a valid file to drop here", Text, MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }
        
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var message = Tools.ReadHelpFile("wii");
            var help = new HelpForm(Text + " - Help", message);
            help.ShowDialog();
        }
        
        private void exportLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.ExportLog(Text,lstLog.Items);
        }

        private void WiiPrep_Resize(object sender, EventArgs e)
        {
            btnRefresh.Left = txtFolder.Left + txtFolder.Width - btnRefresh.Width;
            btnBegin.Left = txtFolder.Left + txtFolder.Width - btnBegin.Width;
            picWorking.Left = (Width / 2) - (picWorking.Width / 2);
        }
        
        private void tutorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://customscreators.com/index.php?/topic/14630-how-to-playing-customs-on-wii/");
        }
        
        private static string GetCounterpart(string folder)
        {
            //default value in case the search below fails
            var name = folder.Replace(Directory.GetParent(folder) + "\\","");
            if (name.EndsWith("_meta", StringComparison.Ordinal))
            {
                name = name.Substring(0, name.Length - 4) + "song";
            }
            else if (name.EndsWith("_song", StringComparison.Ordinal))
            {
                name = name.Substring(0, name.Length - 4) + "meta";
            }
            else
            {
                return "";
            }
            var index = name.IndexOf("_", StringComparison.Ordinal);
            var index2 = name.IndexOf("_", index + 1, StringComparison.Ordinal);
            var search = name.Substring(index2 + 1, name.Length - index2 - 1);
            foreach (var folders in Directory.GetDirectories(Directory.GetParent(folder).ToString()).Where(folders => folders.ToString(CultureInfo.InvariantCulture).Contains(search)))
            {
                name = folders.ToString(CultureInfo.InvariantCulture);
                break;
            }
            return name;
        }

        private void doSmartMerge(List<string> input)
        {
            var input_folders = new List<string>();
            var destination_path = Tools.CurrentFolder;
            var saved_dir = Application.StartupPath + "\\wiidir.c3";
            var error = false;
            startTime = DateTime.Now;
            if (!input.Any() || string.IsNullOrWhiteSpace(input[0]) || !Directory.Exists(input[0]))
            {
                var ofd = new FolderBrowserDialog
                    {
                        SelectedPath = Tools.CurrentFolder,
                        Description = "Select the '_meta' or '_song' folder that you want to merge from",
                        ShowNewFolderButton = false
                    };
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    Log("Smart Folder Merge cancelled");
                    return;
                }
                if (ofd.SelectedPath.EndsWith("_meta", StringComparison.Ordinal) || ofd.SelectedPath.EndsWith("_song", StringComparison.Ordinal))
                {
                    input_folders.Add( ofd.SelectedPath);
                }
                else
                {
                    Log("Source folder is not in the correct format");
                    return;
                }
            }
            else
            {
                input_folders = input;
            }
            var mergePath = "";
            foreach (var folder in input_folders)
            {
                string source_meta;
                string source_song;
                string destination_meta;
                string destination_song;

                if (folder.EndsWith("_meta", StringComparison.Ordinal))
                {
                    source_meta = folder;
                    source_song = GetCounterpart(folder);
                    if (!Directory.Exists(source_song))
                    {
                        Log("Missing '_song' source directory");
                        Log("Can't continue without it");
                        Log("Smart Folder Merge cancelled");
                        return;
                    }
                }
                else if (folder.EndsWith("_song", StringComparison.Ordinal))
                {
                    source_song = folder;
                    source_meta = GetCounterpart(folder);
                    if (!Directory.Exists(source_meta))
                    {
                        Log("Missing '_meta' source directory");
                        Log("Can't continue without it");
                        Log("Smart Folder Merge cancelled");
                        return;
                    }
                }
                else
                {
                    Log("Source folder is not in the correct format");
                    return;
                }
                Log("Source folder selected successfully");
                if (File.Exists(saved_dir))
                {
                    try
                    {
                        var sr = new StreamReader(saved_dir, Encoding.Default);
                        destination_path = sr.ReadLine();
                        sr.Dispose();
                        if (destination_path == null)
                        {
                            destination_path = Tools.CurrentFolder;
                            Tools.DeleteFile(saved_dir);
                        }
                        else if (!Directory.Exists(destination_path))
                        {
                            destination_path = Tools.CurrentFolder;
                            Tools.DeleteFile(saved_dir);
                        }
                    }
                    catch (Exception)
                    {
                        destination_path = Tools.CurrentFolder;
                        Tools.DeleteFile(saved_dir);
                    }
                }
                if (string.IsNullOrWhiteSpace(mergePath))
                {
                    var ofd2 = new FolderBrowserDialog
                    {
                        SelectedPath = destination_path,
                        Description = "Select the '_meta' or '_song' folder that you want to merge to",
                        ShowNewFolderButton = false
                    };
                    if (ofd2.ShowDialog() != DialogResult.OK)
                    {
                        Log("Smart Folder Merge cancelled");
                        return;
                    }
                    mergePath = ofd2.SelectedPath;
                }
                if (mergePath.EndsWith("_meta", StringComparison.Ordinal))
                {
                    destination_meta = mergePath;
                    destination_song = GetCounterpart(mergePath);
                    if (!Directory.Exists(destination_song))
                    {
                        Log("Missing '_song' destination directory");
                        Log("Can't continue without it");
                        Log("Smart Folder Merge cancelled");
                        return;
                    }
                    Log("Destination folder selected successfully");
                }
                else if (mergePath.EndsWith("_song", StringComparison.Ordinal))
                {
                    destination_song = mergePath;
                    destination_meta = GetCounterpart(mergePath);
                    if (!Directory.Exists(destination_meta))
                    {
                        Log("Missing '_meta' destination directory");
                        Log("Can't continue without it");
                        Log("Smart Folder Merge cancelled");
                        return;
                    }
                    Log("Destination folder selected successfully");
                }
                else
                {
                    Log("Destination folder is not in the correct format");
                    return;
                }
                if (source_meta == destination_meta || source_song == destination_song)
                {
                    Log("Source and destination directories can't be the same");
                    Log("Smart Folder Merge cancelled");
                    return;
                }
                //save this parent directory for next time
                var new_dest = Directory.GetParent(destination_meta);
                var sw = new StreamWriter(saved_dir, false, Encoding.Default);
                sw.WriteLine(new_dest.FullName);
                sw.Dispose();
                Log("Creating subdirectories in destination _meta folder");
                var source_folders = Directory.GetDirectories(source_meta, "*.*", SearchOption.AllDirectories);
                foreach (
                    var folders in
                        source_folders.Where(
                            folders => !Directory.Exists(folders.Replace(source_meta, destination_meta))))
                {
                    Directory.CreateDirectory(folders.Replace(source_meta, destination_meta));
                }
                Log("Creating subdirectories in destination _song folder");
                source_folders = Directory.GetDirectories(source_song, "*.*", SearchOption.AllDirectories);
                foreach (
                    var folders in
                        source_folders.Where(
                            folders => !Directory.Exists(folders.Replace(source_song, destination_song))))
                {
                    Directory.CreateDirectory(folders.Replace(source_song, destination_song));
                }
                Log("Moving files in _meta folder");
                var meta_files = Directory.GetFiles(source_meta, "*.*", SearchOption.AllDirectories);
                foreach (var file in meta_files)
                {
                    try
                    {
                        if (Path.GetFileName(file) != "songs.dta")
                        {
                            Log("Moving file " + Path.GetFileName(file));

                            if (File.Exists(file.Replace(source_meta, destination_meta)))
                            {
                                Log(Path.GetFileName(file) + " already exists ... overwriting");
                                Tools.DeleteFile(file.Replace(source_meta, destination_meta));
                            }
                            File.Copy(file, file.Replace(source_meta, destination_meta));


                            if (File.Exists(file.Replace(source_meta, destination_meta)))
                            {
                                Log("Moved file " + Path.GetFileName(file) + " successfully");
                            }
                            else
                            {
                                Log("Moving file " + Path.GetFileName(file) + " failed");
                                error = true;
                            }
                        }
                        else
                        {
                            if (File.Exists(file.Replace(source_meta, destination_meta)))
                            {
                                var dlc_info = "sZAE";
                                var bin_number =
                                    destination_meta.Replace(Directory.GetParent(destination_meta) + "\\", "")
                                        .Substring(0, 3);

                                Log("Found existing songs.dta file ... merging");

                                var bin_reader = new StreamReader(file.Replace(source_meta, destination_meta),
                                    Encoding.Default);
                                while (bin_reader.Peek() >= 0)
                                {
                                    var line = bin_reader.ReadLine();
                                    if (!line.Contains("dlc/")) continue;
                                    var index = line.IndexOf("dlc/", StringComparison.Ordinal) + 4;
                                    dlc_info = line.Substring(index, 4);
                                    break;
                                }
                                bin_reader.Dispose();

                                var sr = new StreamReader(file, Encoding.Default);
                                var sw2 = new StreamWriter(file.Replace(source_meta, destination_meta), true,
                                    Encoding.Default);

                                while (sr.Peek() >= 0)
                                {
                                    var line = sr.ReadLine();

                                    if (line.Contains("dlc/"))
                                    {
                                        var index = line.IndexOf("dlc/", StringComparison.Ordinal) + 4;
                                        var replace = line.Substring(index, 4);
                                        line = line.Replace(replace, dlc_info);
                                        line = line.Replace("000", bin_number);
                                    }
                                    sw2.WriteLine(line);
                                }
                                sr.Dispose();
                                sw2.Dispose();
                                Log("Merged songs.dta files successfully");
                            }
                            else
                            {
                                Log("No existing songs.dta file found, copying new one");
                                File.Copy(file, file.Replace(source_meta, destination_meta));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log("There was an error:");
                        Log(ex.Message);
                        error = true;
                    }
                }
                Log("Moved all files in _meta folder successfully");

                if (error)
                {
                    Log("There was at least one error in the process");
                    Log("Leaving source _meta folder intact");
                }
                else
                {
                    if (chkSmartDelete.Checked)
                    {
                        Log("Sending source _meta folder to Recycle Bin");
                        Tools.SendtoTrash(source_meta, true);
                    }
                }

                Log("Moving files in _song folder");
                var song_files = Directory.GetFiles(source_song, "*.*", SearchOption.AllDirectories);
                foreach (var file in song_files)
                {
                    try
                    {
                        if (file.EndsWith(".milo_wii", StringComparison.Ordinal) && chkDummy.Checked)
                        {
                            var dummy = Application.StartupPath + "\\bin\\dummy.milo_wii";

                            if (File.Exists(dummy))
                            {
                                Log("Backing up original .milo_wii file and replacing with working dummy.milo_wii");
                                var wii_backup = Application.StartupPath + "\\wii_backup\\";
                                if (!Directory.Exists(wii_backup))
                                {
                                    Directory.CreateDirectory(wii_backup);
                                }
                                File.Copy(file, wii_backup + Path.GetFileName(file));

                                if (File.Exists(wii_backup + Path.GetFileName(file)))
                                {
                                    Log("Original .milo_wii file backed up successfully");
                                    Log("You can find it in the 'wii_backup' folder in the root directory");
                                    Log("Using working dummy .milo_wii file");
                                    Tools.DeleteFile(file);
                                    File.Copy(dummy, file);
                                }
                                else
                                {
                                    Log(
                                        "Backing up original .milo_wii file failed, will not replace with dummy.milo_wii");
                                    Log("Continuing with original .milo_wii file instead");
                                }
                            }
                            else
                            {
                                Log("You told me to use the basic animation file dummy.milo_wii but I can't find it!");
                                Log("Continuing with original .milo_wii file instead");
                            }
                        }
                        else
                        {
                            Log("Moving file " + Path.GetFileName(file));
                        }
                        if (File.Exists(file.Replace(source_song, destination_song)))
                        {
                            Log(Path.GetFileName(file) + " already exists ... overwriting");
                            Tools.DeleteFile(file.Replace(source_song, destination_song));
                        }
                        File.Copy(file, file.Replace(source_song, destination_song));


                        if (File.Exists(file.Replace(source_song, destination_song)))
                        {
                            Log("Moved file " + Path.GetFileName(file) + " successfully");
                        }
                        else
                        {
                            Log("Moving file " + Path.GetFileName(file) + " failed");
                            error = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log("There was an error:");
                        Log(ex.Message);
                        error = true;
                    }
                }
                Log("Moved all files in _song folder successfully");

                if (error)
                {
                    Log("There was at least one error in the process");
                    Log("Leaving source _song folder intact");
                }
                else
                {
                    if (!chkSmartDelete.Checked) continue;
                    Log("Sending source _meta folder to Recycle Bin");
                    Tools.SendtoTrash(source_song, true);
                }
            }
            endTime = DateTime.Now;
            var timeDiff = endTime - startTime;
            MessageBox.Show("Smart Folder Merge completed in " + timeDiff.Minutes + " minutes and " + timeDiff.Seconds +
                        " seconds",Text,MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Log("Resetting...");
            inputFiles.Clear();
            EnableDisable(true);
            btnBegin.Visible = true;
            btnBegin.Enabled = true;
            btnReset.Visible = false;
            btnFolder.Enabled = true;
            btnRefresh.Enabled = true;
            btnRefresh.PerformClick();
        }

        private void EnableDisable(bool enabled)
        {
            btnFolder.Enabled = enabled;
            btnRefresh.Enabled = enabled;
            menuStrip1.Enabled = enabled;
            txtFolder.Enabled = enabled;
            chkRAR.Enabled = enabled;
            chkCreatePreview.Enabled = enabled;
            numAttenuation.Enabled = enabled;
            numFadeIn.Enabled = enabled;
            numFadeOut.Enabled = enabled;
            numLength.Enabled = enabled;
            picWorking.Visible = !enabled;
            btnSmartMerge.Enabled = enabled;
            chkDummy.Enabled = enabled;
            chkSmartDelete.Enabled = enabled;
            picHelp.Visible = enabled;
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            if (btnBegin.Text == "Cancel")
            {
                backgroundWorker1.CancelAsync();
                Log("User cancelled process...stopping as soon as possible");
                btnBegin.Enabled = false;
                return;
            } 
            
            startTime = DateTime.Now;
            wiifolder = txtFolder.Text + "\\WiiFiles\\";
            Tools.CurrentFolder = txtFolder.Text;
            EnableDisable(false);
            
            if (!Directory.Exists(wiifolder))
            {
                Directory.CreateDirectory(wiifolder);
            }

            try
            {
                var files = Directory.GetFiles(txtFolder.Text);
                if (files.Count() != 0)
                {
                    btnBegin.Text = "Cancel";
                    toolTip1.SetToolTip(btnBegin, "Click to cancel process");
                    backgroundWorker1.RunWorkerAsync();
                }
                else
                {
                    Log("No files found ... there's nothing to do");
                    EnableDisable(true);
                }
            }
            catch (Exception ex)
            {
                Log("Error retrieving files to process");
                Log("The error says:" + ex.Message);
                EnableDisable(true);
            }
        }
        
        private void btnSmartMerge_Click(object sender, EventArgs e)
        {
            doSmartMerge(new List<string>());
        }
        
        private void WiiPrep_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!picWorking.Visible)
            {
                Tools.DeleteFolder(Application.StartupPath + "\\wiiprep_input\\");
                return;
            }
            MessageBox.Show("Please wait until the current process finishes", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
        }

        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }

        private void WiiPrep_Shown(object sender, EventArgs e)
        {
            if (Tools.IsAuthorized())
            {
                chkRAR.Checked = true;
                chkEncrypt.Checked = true;
            }

            Log("Welcome to " + Text);
            Log("Drag and drop the CON /LIVE file(s) to be converted here");
            Log("Or click 'Change Input Folder' to select the files");
            Log("Ready to begin");
            

            txtFolder.Text = inputDir;
        }

        private void convertVENUEFromRBN2ToRBN1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "MIDI File (*.mid)|*.mid",
                InitialDirectory = Tools.CurrentFolder,
                Title = "Select MIDI file(s) to process",
                Multiselect = true,
            };

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            Tools.CurrentFolder = Path.GetDirectoryName(ofd.FileNames[0]);

            var counter = 0;
            foreach (var midi in ofd.FileNames)
            {
                if (!midi.ToLowerInvariant().EndsWith(".mid", StringComparison.Ordinal) || !File.Exists(midi)) continue;

                var backup = Path.GetDirectoryName(midi) + "\\" + Path.GetFileNameWithoutExtension(midi) + "_backup.mid";
                Tools.DeleteFile(backup);
                if (Tools.MoveFile(midi, backup))
                {
                    Log("Backed up MIDI file " + Path.GetFileName(midi) + " to " + backup);
                }
                else
                {
                    Log("Failed to create back up for MIDI file " + Path.GetFileName(midi));

                    backup = File.Exists(midi) ? midi : "";
                }

                if (string.IsNullOrWhiteSpace(backup))
                {
                    Log("Failed to convert VENUE for MIDI file " + Path.GetFileName(midi));
                    return;
                }

                if (FixMIDI(backup, midi))
                {
                    Log("Converted VENUE successfully for MIDI file " + Path.GetFileName(midi));
                    counter++;
                }
                else
                {
                    Log("Failed to convert VENUE for MIDI file " + Path.GetFileName(midi));
                }
            }

            if (counter == 0)
            {
                Log("None of the MIDI files were modified, sorry");
                Log("Refer to the log for possible error details");
            }
            else
            {
                var message = "Successfully converted VENUE " + (ofd.FileNames.Count() > 1 ? "tracks" : "track") + " for " + counter + " of " + ofd.FileNames.Count() + " MIDI " + (ofd.FileNames.Count() > 1 ? "files" : "file");
                var message2 = "Make sure to set the 'version' " + (ofd.FileNames.Count() > 1 ? "values" : "value") + " in the " + (ofd.FileNames.Count() > 1 ? "songs'" : "song's") + " DTA " + (ofd.FileNames.Count() > 1 ? "files" : "file") + " to 1";
                Log(message);
                Log(message2);
                MessageBox.Show(message + "\n\n" + message2, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void picHelp_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MessageBox.Show("Please refer to the Tutorial link at the top before attempting to do this\n\nBoth your source and destination folders must be formatted as the Tutorial instructs:\n\nxxx_xxxxxxxx_songname_meta\nand\nxxx_xxxxxxxx_songname_song\n\nIf your folders are not formatted correctly, this process will fail...",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void convertVenueDuringSongConversion_Click(object sender, EventArgs e)
        {
            attemptToConvertPostprocEvents.Enabled = convertVenueDuringSongConversion.Checked;
        }

        private void chkCreatePreview_CheckedChanged(object sender, EventArgs e)
        {
            numLength.Enabled = chkCreatePreview.Checked;
            numFadeIn.Enabled = chkCreatePreview.Checked;
            numFadeOut.Enabled = chkCreatePreview.Checked;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (ProcessFiles()) return;
            Log("There was an error processing the files ... stopping here");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Log("Done!");
            endTime = DateTime.Now;
            var timeDiff = endTime - startTime;
            Log("Process took " + timeDiff.Minutes + (timeDiff.Minutes == 1 ? " minute" : " minutes") + " and " + (timeDiff.Minutes == 0 && timeDiff.Seconds == 0 ? "1 second" : timeDiff.Seconds + " seconds"));
            Log("Click 'Reset' to start again or just close me down");

            //clear up any leftover png_xbox files
            var xbox_files = Directory.GetFiles(txtFolder.Text, "*.png_xbox", SearchOption.AllDirectories);
            foreach (var xbox_file in xbox_files)
            {
                Tools.DeleteFile(xbox_file);
            }

            picWorking.Visible = false;
            btnReset.Enabled = true;
            btnReset.Visible = true;
            lstLog.Cursor = Cursors.Default;
            Cursor = lstLog.Cursor; 
            toolTip1.SetToolTip(btnBegin, "Click to begin");
            btnBegin.Text = "&Begin";
        }

        private void autodownmix_Click(object sender, EventArgs e)
        {
            autodownmix.Checked = true;
            dualMonoChannels.Checked = false;
        }

        private void dualMonoChannels_Click(object sender, EventArgs e)
        {
            autodownmix.Checked = false;
            dualMonoChannels.Checked = true;
        }

        private void grabNgidFromBINFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "BIN File (*.bin)|*.bin",
                InitialDirectory = Tools.CurrentFolder,
                Title = "Select BIN file",
                Multiselect = false
            };

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            Tools.CurrentFolder = Path.GetDirectoryName(ofd.FileName);
            Tools.ProcessBINFile(ofd.FileName, Text);
        }

        private void picPin_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            switch (picPin.Tag.ToString())
            {
                case "pinned":
                    picPin.Image = Resources.unpinned;
                    picPin.Tag = "unpinned";
                    break;
                case "unpinned":
                    picPin.Image = Resources.pinned;
                    picPin.Tag = "pinned";
                    break;
            }
            TopMost = picPin.Tag.ToString() == "pinned";
        }
    }
}