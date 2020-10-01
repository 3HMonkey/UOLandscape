﻿namespace UOLandscape.Client
{
    internal enum ClientVersion
    {
        CV_OLD = (1 << 24) | (0 << 16) | (0 << 8) | 0,                 // Original game
        CV_200 = (2 << 24) | (0 << 16) | (0 << 8) | 0,                 // T2A Introduction. Adds screen dimensions packet
        CV_200X = (2 << 24) | (0 << 16) | (0 << 8) | ('x' & 0xFF),      // T2A Introduction. Adds screen dimensions packet
        CV_204C = (2 << 24) | (0 << 16) | (4 << 8) | ('c' & 0xFF),      // Adds *.def files
        CV_300 = (3 << 24) | (0 << 16) | (0 << 8) | 0,
        CV_305D = (3 << 24) | (0 << 16) | (5 << 8) | ('d' & 0xFF),      // Renaissance. Expanded character slots.
        CV_306E = (3 << 24) | (0 << 16) | (0 << 8) | ('e' & 0xFF),      // Adds a packet with the client type, switches to mp3 from midi for sound files
        CV_308 = (3 << 24) | (0 << 16) | (8 << 8) | 0,
        CV_308D = (3 << 24) | (0 << 16) | (8 << 8) | ('d' & 0xFF),      // Adds maximum stats to the status bar
        CV_308J = (3 << 24) | (0 << 16) | (8 << 8) | ('j' & 0xFF),      // Adds followers to the status bar
        CV_308Z = (3 << 24) | (0 << 16) | (8 << 8) | ('z' & 0xFF),      // Age of Shadows. Adds paladin, necromancer, custom housing, resists, profession selection window, removes save password checkbox
        CV_400B = (4 << 24) | (0 << 16) | (0 << 8) | ('b' & 0xFF),      // Deletes tooltips
        CV_405A = (4 << 24) | (0 << 16) | (5 << 8) | ('a' & 0xFF),      // Adds ninja, samurai
        CV_4011D = (4 << 24) | (0 << 16) | (11 << 8) | ('d' & 0xFF),      // Adds elven race
        CV_500A = (5 << 24) | (0 << 16) | (0 << 8) | ('a' & 0xFF),      // Paperdoll buttons journal becomes quests, chat becomes guild. Use mega FileManager.Cliloc. Removes verdata.mul.
        CV_5020 = (5 << 24) | (0 << 16) | (2 << 8) | 0,                 // Adds buff bar
        CV_5090 = (5 << 24) | (0 << 16) | (9 << 8) | 0,                 //
        CV_6000 = (6 << 24) | (0 << 16) | (0 << 8) | 0,                 // Adds colored guild/all chat and ignore system. New targeting systems, object properties and handles.
        CV_6013 = (6 << 24) | (0 << 16) | (1 << 8) | 3,                 //
        CV_6017 = (6 << 24) | (0 << 16) | (1 << 8) | 8,                 //
        CV_6040 = (6 << 24) | (0 << 16) | (4 << 8) | 0,                 // Increased number of player slots
        CV_6060 = (6 << 24) | (0 << 16) | (6 << 8) | 0,                 //
        CV_60142 = (6 << 24) | (0 << 16) | (14 << 8) | 2,                 //
        CV_60143 = (6 << 24) | (0 << 16) | (14 << 8) | 3,                 // Stygian Abyss
        CV_60144 = (6 << 24) | (0 << 16) | (14 << 8) | 4,                 // Adds gargoyle race.
        CV_7000 = (7 << 24) | (0 << 16) | (0 << 8) | 0,                 //
        CV_7090 = (7 << 24) | (0 << 16) | (9 << 8) | 0,                 // high seas
        CV_70130 = (7 << 24) | (0 << 16) | (13 << 8) | 0,                 //
        CV_70160 = (7 << 24) | (0 << 16) | (16 << 8) | 0,                 //
        CV_70180 = (7 << 24) | (0 << 16) | (18 << 8) | 0,                 //
        CV_70240 = (7 << 24) | (0 << 16) | (24 << 8) | 0,                 // *.mul -> *.uop
        CV_70331 = (7 << 24) | (0 << 16) | (33 << 8) | 1,                 //
        CV_704565 = (7 << 24) | (0 << 16) | (45 << 8) | 65,                //
        CV_706000 = (7 << 24) | (0 << 16) | (60 << 8) | 0,
        CV_706400 = (7 << 24) | (0 << 16) | (64 << 8) | 0,                 // Endless Journey background
        CV_70796 = (7 << 24) | (0 << 16) | (79 << 8) | 6                  // Display houses content option
    }
}
