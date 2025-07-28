namespace Ingweland.Fog.Application.Core.Helpers;

public static class TreasureHuntBattleEncounters
{
    private static readonly Dictionary<(int difficulty, int stage), List<string>> _encounters =
        new()
        {
            // Difficulty 0
            {
                (0, 0),
                [
                    "Encounter_0_0_0", "Encounter_0_0_2", "Encounter_0_0_4", "Encounter_0_0_6", "Encounter_0_0_8",
                    "Encounter_0_0_11", "Encounter_0_0_13", "Encounter_0_0_15", "Encounter_0_0_17", "Encounter_0_0_19",
                    "Encounter_0_0_22", "Encounter_0_0_24", "Encounter_0_0_26", "Encounter_0_0_28", "Encounter_0_0_30",
                    "Encounter_0_0_33", "Encounter_0_0_35", "Encounter_0_0_37", "Encounter_0_0_39", "Encounter_0_0_41",
                ]
            },
            {
                (0, 1),
                [
                    "Encounter_0_1_1", "Encounter_0_1_3", "Encounter_0_1_5", "Encounter_0_1_7", "Encounter_0_1_9",
                    "Encounter_0_1_12", "Encounter_0_1_14", "Encounter_0_1_16", "Encounter_0_1_18", "Encounter_0_1_20",
                    "Encounter_0_1_23", "Encounter_0_1_25", "Encounter_0_1_27", "Encounter_0_1_29", "Encounter_0_1_31",
                    "Encounter_0_1_34", "Encounter_0_1_36", "Encounter_0_1_38", "Encounter_0_1_40", "Encounter_0_1_42",
                ]
            },
            {
                (0, 2),
                [
                    "Encounter_0_2_1", "Encounter_0_2_3", "Encounter_0_2_5", "Encounter_0_2_7", "Encounter_0_2_9",
                    "Encounter_0_2_12", "Encounter_0_2_14", "Encounter_0_2_16", "Encounter_0_2_18", "Encounter_0_2_20",
                    "Encounter_0_2_23", "Encounter_0_2_25", "Encounter_0_2_27", "Encounter_0_2_29", "Encounter_0_2_31",
                    "Encounter_0_2_34", "Encounter_0_2_36", "Encounter_0_2_38", "Encounter_0_2_40", "Encounter_0_2_42",
                ]
            },
            {
                (0, 3),
                [
                    "Encounter_0_3_1", "Encounter_0_3_3", "Encounter_0_3_5", "Encounter_0_3_8", "Encounter_0_3_10",
                    "Encounter_0_3_13", "Encounter_0_3_15", "Encounter_0_3_17", "Encounter_0_3_20", "Encounter_0_3_22",
                    "Encounter_0_3_25", "Encounter_0_3_27", "Encounter_0_3_29", "Encounter_0_3_32", "Encounter_0_3_34",
                    "Encounter_0_3_37", "Encounter_0_3_39", "Encounter_0_3_41", "Encounter_0_3_44", "Encounter_0_3_46",
                ]
            },

            // Difficulty 1
            {
                (1, 0),
                [
                    "Encounter_1_0_0", "Encounter_1_0_2", "Encounter_1_0_4", "Encounter_1_0_6", "Encounter_1_0_8",
                    "Encounter_1_0_11", "Encounter_1_0_13", "Encounter_1_0_15", "Encounter_1_0_17", "Encounter_1_0_19",
                    "Encounter_1_0_22", "Encounter_1_0_24", "Encounter_1_0_26", "Encounter_1_0_28", "Encounter_1_0_30",
                    "Encounter_1_0_33", "Encounter_1_0_35", "Encounter_1_0_37", "Encounter_1_0_39", "Encounter_1_0_41",
                ]
            },
            {
                (1, 1),
                [
                    "Encounter_1_1_1", "Encounter_1_1_3", "Encounter_1_1_5", "Encounter_1_1_7", "Encounter_1_1_9",
                    "Encounter_1_1_12", "Encounter_1_1_14", "Encounter_1_1_16", "Encounter_1_1_18", "Encounter_1_1_20",
                    "Encounter_1_1_23", "Encounter_1_1_25", "Encounter_1_1_27", "Encounter_1_1_29", "Encounter_1_1_31",
                    "Encounter_1_1_34", "Encounter_1_1_36", "Encounter_1_1_38", "Encounter_1_1_40", "Encounter_1_1_42",
                ]
            },
            {
                (1, 2),
                [
                    "Encounter_1_2_1", "Encounter_1_2_3", "Encounter_1_2_5", "Encounter_1_2_7", "Encounter_1_2_9",
                    "Encounter_1_2_12", "Encounter_1_2_14", "Encounter_1_2_16", "Encounter_1_2_18", "Encounter_1_2_20",
                    "Encounter_1_2_23", "Encounter_1_2_25", "Encounter_1_2_27", "Encounter_1_2_29", "Encounter_1_2_31",
                    "Encounter_1_2_34", "Encounter_1_2_36", "Encounter_1_2_38", "Encounter_1_2_40", "Encounter_1_2_42",
                ]
            },
            {
                (1, 3),
                [
                    "Encounter_1_3_1", "Encounter_1_3_3", "Encounter_1_3_5", "Encounter_1_3_8", "Encounter_1_3_10",
                    "Encounter_1_3_13", "Encounter_1_3_15", "Encounter_1_3_17", "Encounter_1_3_20", "Encounter_1_3_22",
                    "Encounter_1_3_25", "Encounter_1_3_27", "Encounter_1_3_29", "Encounter_1_3_32", "Encounter_1_3_34",
                    "Encounter_1_3_37", "Encounter_1_3_39", "Encounter_1_3_41", "Encounter_1_3_44", "Encounter_1_3_46",
                ]
            },

            // Difficulty 2
            {
                (2, 0),
                [
                    "Encounter_2_0_0", "Encounter_2_0_2", "Encounter_2_0_4", "Encounter_2_0_6", "Encounter_2_0_8",
                    "Encounter_2_0_11", "Encounter_2_0_13", "Encounter_2_0_15", "Encounter_2_0_17", "Encounter_2_0_19",
                    "Encounter_2_0_22", "Encounter_2_0_24", "Encounter_2_0_26", "Encounter_2_0_28", "Encounter_2_0_30",
                    "Encounter_2_0_33", "Encounter_2_0_35", "Encounter_2_0_37", "Encounter_2_0_39", "Encounter_2_0_41",
                ]
            },
            {
                (2, 1),
                [
                    "Encounter_2_1_1", "Encounter_2_1_3", "Encounter_2_1_5", "Encounter_2_1_7", "Encounter_2_1_9",
                    "Encounter_2_1_12", "Encounter_2_1_14", "Encounter_2_1_16", "Encounter_2_1_18", "Encounter_2_1_20",
                    "Encounter_2_1_23", "Encounter_2_1_25", "Encounter_2_1_27", "Encounter_2_1_29", "Encounter_2_1_31",
                    "Encounter_2_1_34", "Encounter_2_1_36", "Encounter_2_1_38", "Encounter_2_1_40", "Encounter_2_1_42",
                ]
            },
            {
                (2, 2),
                [
                    "Encounter_2_2_1", "Encounter_2_2_3", "Encounter_2_2_5", "Encounter_2_2_7", "Encounter_2_2_9",
                    "Encounter_2_2_12", "Encounter_2_2_14", "Encounter_2_2_16", "Encounter_2_2_18", "Encounter_2_2_20",
                    "Encounter_2_2_23", "Encounter_2_2_25", "Encounter_2_2_27", "Encounter_2_2_29", "Encounter_2_2_31",
                    "Encounter_2_2_34", "Encounter_2_2_36", "Encounter_2_2_38", "Encounter_2_2_40", "Encounter_2_2_42",
                ]
            },
            {
                (2, 3),
                [
                    "Encounter_2_3_1", "Encounter_2_3_3", "Encounter_2_3_5", "Encounter_2_3_8", "Encounter_2_3_10",
                    "Encounter_2_3_13", "Encounter_2_3_15", "Encounter_2_3_17", "Encounter_2_3_20", "Encounter_2_3_22",
                    "Encounter_2_3_25", "Encounter_2_3_27", "Encounter_2_3_29", "Encounter_2_3_32", "Encounter_2_3_34",
                    "Encounter_2_3_37", "Encounter_2_3_39", "Encounter_2_3_41", "Encounter_2_3_44", "Encounter_2_3_46",
                ]
            },

            // Difficulty 3
            {
                (3, 0),
                [
                    "Encounter_3_0_0", "Encounter_3_0_2", "Encounter_3_0_4", "Encounter_3_0_6", "Encounter_3_0_8",
                    "Encounter_3_0_11", "Encounter_3_0_13", "Encounter_3_0_15", "Encounter_3_0_17", "Encounter_3_0_19",
                    "Encounter_3_0_22", "Encounter_3_0_24", "Encounter_3_0_26", "Encounter_3_0_28", "Encounter_3_0_30",
                    "Encounter_3_0_33", "Encounter_3_0_35", "Encounter_3_0_37", "Encounter_3_0_39", "Encounter_3_0_41",
                ]
            },
            {
                (3, 1),
                [
                    "Encounter_3_1_1", "Encounter_3_1_3", "Encounter_3_1_5", "Encounter_3_1_7", "Encounter_3_1_9",
                    "Encounter_3_1_12", "Encounter_3_1_14", "Encounter_3_1_16", "Encounter_3_1_18", "Encounter_3_1_20",
                    "Encounter_3_1_23", "Encounter_3_1_25", "Encounter_3_1_27", "Encounter_3_1_29", "Encounter_3_1_31",
                    "Encounter_3_1_34", "Encounter_3_1_36", "Encounter_3_1_38", "Encounter_3_1_40", "Encounter_3_1_42",
                ]
            },
            {
                (3, 2),
                [
                    "Encounter_3_2_1", "Encounter_3_2_3", "Encounter_3_2_5", "Encounter_3_2_7", "Encounter_3_2_9",
                    "Encounter_3_2_12", "Encounter_3_2_14", "Encounter_3_2_16", "Encounter_3_2_18", "Encounter_3_2_20",
                    "Encounter_3_2_23", "Encounter_3_2_25", "Encounter_3_2_27", "Encounter_3_2_29", "Encounter_3_2_31",
                    "Encounter_3_2_34", "Encounter_3_2_36", "Encounter_3_2_38", "Encounter_3_2_40", "Encounter_3_2_42",
                ]
            },
            {
                (3, 3),
                [
                    "Encounter_3_3_1", "Encounter_3_3_3", "Encounter_3_3_5", "Encounter_3_3_8", "Encounter_3_3_10",
                    "Encounter_3_3_13", "Encounter_3_3_15", "Encounter_3_3_17", "Encounter_3_3_20", "Encounter_3_3_22",
                    "Encounter_3_3_25", "Encounter_3_3_27", "Encounter_3_3_29", "Encounter_3_3_32", "Encounter_3_3_34",
                    "Encounter_3_3_37", "Encounter_3_3_39", "Encounter_3_3_41", "Encounter_3_3_44", "Encounter_3_3_46",
                ]
            },

            // Difficulty 4
            {
                (4, 0),
                [
                    "Encounter_4_0_0", "Encounter_4_0_2", "Encounter_4_0_4", "Encounter_4_0_6", "Encounter_4_0_8",
                    "Encounter_4_0_11", "Encounter_4_0_13", "Encounter_4_0_15", "Encounter_4_0_17", "Encounter_4_0_19",
                    "Encounter_4_0_22", "Encounter_4_0_24", "Encounter_4_0_26", "Encounter_4_0_28", "Encounter_4_0_30",
                    "Encounter_4_0_33", "Encounter_4_0_35", "Encounter_4_0_37", "Encounter_4_0_39", "Encounter_4_0_41",
                ]
            },
            {
                (4, 1),
                [
                    "Encounter_4_1_1", "Encounter_4_1_3", "Encounter_4_1_5", "Encounter_4_1_7", "Encounter_4_1_9",
                    "Encounter_4_1_12", "Encounter_4_1_14", "Encounter_4_1_16", "Encounter_4_1_18", "Encounter_4_1_20",
                    "Encounter_4_1_23", "Encounter_4_1_25", "Encounter_4_1_27", "Encounter_4_1_29", "Encounter_4_1_31",
                    "Encounter_4_1_34", "Encounter_4_1_36", "Encounter_4_1_38", "Encounter_4_1_40", "Encounter_4_1_42",
                ]
            },
            {
                (4, 2),
                [
                    "Encounter_4_2_1", "Encounter_4_2_3", "Encounter_4_2_5", "Encounter_4_2_7", "Encounter_4_2_9",
                    "Encounter_4_2_12", "Encounter_4_2_14", "Encounter_4_2_16", "Encounter_4_2_18", "Encounter_4_2_20",
                    "Encounter_4_2_23", "Encounter_4_2_25", "Encounter_4_2_27", "Encounter_4_2_29", "Encounter_4_2_31",
                    "Encounter_4_2_34", "Encounter_4_2_36", "Encounter_4_2_38", "Encounter_4_2_40", "Encounter_4_2_42",
                ]
            },
            {
                (4, 3),
                [
                    "Encounter_4_3_1", "Encounter_4_3_3", "Encounter_4_3_5", "Encounter_4_3_8", "Encounter_4_3_10",
                    "Encounter_4_3_13", "Encounter_4_3_15", "Encounter_4_3_17", "Encounter_4_3_20", "Encounter_4_3_22",
                    "Encounter_4_3_25", "Encounter_4_3_27", "Encounter_4_3_29", "Encounter_4_3_32", "Encounter_4_3_34",
                    "Encounter_4_3_37", "Encounter_4_3_39", "Encounter_4_3_41", "Encounter_4_3_44", "Encounter_4_3_46",
                ]
            },

            // Difficulty 5
            {
                (5, 0),
                [
                    "Encounter_5_0_0", "Encounter_5_0_2", "Encounter_5_0_4", "Encounter_5_0_6", "Encounter_5_0_8",
                    "Encounter_5_0_11", "Encounter_5_0_13", "Encounter_5_0_15", "Encounter_5_0_17", "Encounter_5_0_19",
                    "Encounter_5_0_22", "Encounter_5_0_24", "Encounter_5_0_26", "Encounter_5_0_28", "Encounter_5_0_30",
                    "Encounter_5_0_33", "Encounter_5_0_35", "Encounter_5_0_37", "Encounter_5_0_39", "Encounter_5_0_41",
                ]
            },
            {
                (5, 1),
                [
                    "Encounter_5_1_1", "Encounter_5_1_3", "Encounter_5_1_5", "Encounter_5_1_7", "Encounter_5_1_9",
                    "Encounter_5_1_12", "Encounter_5_1_14", "Encounter_5_1_16", "Encounter_5_1_18", "Encounter_5_1_20",
                    "Encounter_5_1_23", "Encounter_5_1_25", "Encounter_5_1_27", "Encounter_5_1_29", "Encounter_5_1_31",
                    "Encounter_5_1_34", "Encounter_5_1_36", "Encounter_5_1_38", "Encounter_5_1_40", "Encounter_5_1_42",
                ]
            },
            {
                (5, 2),
                [
                    "Encounter_5_2_1", "Encounter_5_2_3", "Encounter_5_2_5", "Encounter_5_2_7", "Encounter_5_2_9",
                    "Encounter_5_2_12", "Encounter_5_2_14", "Encounter_5_2_16", "Encounter_5_2_18", "Encounter_5_2_20",
                    "Encounter_5_2_23", "Encounter_5_2_25", "Encounter_5_2_27", "Encounter_5_2_29", "Encounter_5_2_31",
                    "Encounter_5_2_34", "Encounter_5_2_36", "Encounter_5_2_38", "Encounter_5_2_40", "Encounter_5_2_42",
                ]
            },
            {
                (5, 3),
                [
                    "Encounter_5_3_1", "Encounter_5_3_3", "Encounter_5_3_5", "Encounter_5_3_8", "Encounter_5_3_10",
                    "Encounter_5_3_13", "Encounter_5_3_15", "Encounter_5_3_17", "Encounter_5_3_20", "Encounter_5_3_22",
                    "Encounter_5_3_25", "Encounter_5_3_27", "Encounter_5_3_29", "Encounter_5_3_32", "Encounter_5_3_34",
                    "Encounter_5_3_37", "Encounter_5_3_39", "Encounter_5_3_41", "Encounter_5_3_44", "Encounter_5_3_46",
                ]
            },

            // Difficulty 6
            {
                (6, 0),
                [
                    "Encounter_6_0_0", "Encounter_6_0_2", "Encounter_6_0_4", "Encounter_6_0_6", "Encounter_6_0_8",
                    "Encounter_6_0_11", "Encounter_6_0_13", "Encounter_6_0_15", "Encounter_6_0_17", "Encounter_6_0_19",
                    "Encounter_6_0_22", "Encounter_6_0_24", "Encounter_6_0_26", "Encounter_6_0_28", "Encounter_6_0_30",
                    "Encounter_6_0_33", "Encounter_6_0_35", "Encounter_6_0_37", "Encounter_6_0_39", "Encounter_6_0_41",
                ]
            },
            {
                (6, 1),
                [
                    "Encounter_6_1_1", "Encounter_6_1_3", "Encounter_6_1_5", "Encounter_6_1_7", "Encounter_6_1_9",
                    "Encounter_6_1_12", "Encounter_6_1_14", "Encounter_6_1_16", "Encounter_6_1_18", "Encounter_6_1_20",
                    "Encounter_6_1_23", "Encounter_6_1_25", "Encounter_6_1_27", "Encounter_6_1_29", "Encounter_6_1_31",
                    "Encounter_6_1_34", "Encounter_6_1_36", "Encounter_6_1_38", "Encounter_6_1_40", "Encounter_6_1_42",
                ]
            },
            {
                (6, 2),
                [
                    "Encounter_6_2_1", "Encounter_6_2_3", "Encounter_6_2_5", "Encounter_6_2_7", "Encounter_6_2_9",
                    "Encounter_6_2_12", "Encounter_6_2_14", "Encounter_6_2_16", "Encounter_6_2_18", "Encounter_6_2_20",
                    "Encounter_6_2_23", "Encounter_6_2_25", "Encounter_6_2_27", "Encounter_6_2_29", "Encounter_6_2_31",
                    "Encounter_6_2_34", "Encounter_6_2_36", "Encounter_6_2_38", "Encounter_6_2_40", "Encounter_6_2_42",
                ]
            },
            {
                (6, 3),
                [
                    "Encounter_6_3_1", "Encounter_6_3_3", "Encounter_6_3_5", "Encounter_6_3_8", "Encounter_6_3_10",
                    "Encounter_6_3_13", "Encounter_6_3_15", "Encounter_6_3_17", "Encounter_6_3_20", "Encounter_6_3_22",
                    "Encounter_6_3_25", "Encounter_6_3_27", "Encounter_6_3_29", "Encounter_6_3_32", "Encounter_6_3_34",
                    "Encounter_6_3_37", "Encounter_6_3_39", "Encounter_6_3_41", "Encounter_6_3_44", "Encounter_6_3_46",
                ]
            },

            // Difficulty 7
            {
                (7, 0),
                [
                    "Encounter_7_0_0", "Encounter_7_0_2", "Encounter_7_0_4", "Encounter_7_0_6", "Encounter_7_0_8",
                    "Encounter_7_0_11", "Encounter_7_0_13", "Encounter_7_0_15", "Encounter_7_0_17", "Encounter_7_0_19",
                    "Encounter_7_0_22", "Encounter_7_0_24", "Encounter_7_0_26", "Encounter_7_0_28", "Encounter_7_0_30",
                    "Encounter_7_0_33", "Encounter_7_0_35", "Encounter_7_0_37", "Encounter_7_0_39", "Encounter_7_0_41",
                ]
            },
            {
                (7, 1),
                [
                    "Encounter_7_1_1", "Encounter_7_1_3", "Encounter_7_1_5", "Encounter_7_1_7", "Encounter_7_1_9",
                    "Encounter_7_1_12", "Encounter_7_1_14", "Encounter_7_1_16", "Encounter_7_1_18", "Encounter_7_1_20",
                    "Encounter_7_1_23", "Encounter_7_1_25", "Encounter_7_1_27", "Encounter_7_1_29", "Encounter_7_1_31",
                    "Encounter_7_1_34", "Encounter_7_1_36", "Encounter_7_1_38", "Encounter_7_1_40", "Encounter_7_1_42",
                ]
            },
            {
                (7, 2),
                [
                    "Encounter_7_2_1", "Encounter_7_2_3", "Encounter_7_2_5", "Encounter_7_2_7", "Encounter_7_2_9",
                    "Encounter_7_2_12", "Encounter_7_2_14", "Encounter_7_2_16", "Encounter_7_2_18", "Encounter_7_2_20",
                    "Encounter_7_2_23", "Encounter_7_2_25", "Encounter_7_2_27", "Encounter_7_2_29", "Encounter_7_2_31",
                    "Encounter_7_2_34", "Encounter_7_2_36", "Encounter_7_2_38", "Encounter_7_2_40", "Encounter_7_2_42",
                ]
            },
            {
                (7, 3),
                [
                    "Encounter_7_3_1", "Encounter_7_3_3", "Encounter_7_3_5", "Encounter_7_3_8", "Encounter_7_3_10",
                    "Encounter_7_3_13", "Encounter_7_3_15", "Encounter_7_3_17", "Encounter_7_3_20", "Encounter_7_3_22",
                    "Encounter_7_3_25", "Encounter_7_3_27", "Encounter_7_3_29", "Encounter_7_3_32", "Encounter_7_3_34",
                    "Encounter_7_3_37", "Encounter_7_3_39", "Encounter_7_3_41", "Encounter_7_3_44", "Encounter_7_3_46",
                ]
            },

            // Difficulty 8
            {
                (8, 0),
                [
                    "Encounter_8_0_0", "Encounter_8_0_2", "Encounter_8_0_4", "Encounter_8_0_6", "Encounter_8_0_8",
                    "Encounter_8_0_11", "Encounter_8_0_13", "Encounter_8_0_15", "Encounter_8_0_17", "Encounter_8_0_19",
                    "Encounter_8_0_22", "Encounter_8_0_24", "Encounter_8_0_26", "Encounter_8_0_28", "Encounter_8_0_30",
                    "Encounter_8_0_33", "Encounter_8_0_35", "Encounter_8_0_37", "Encounter_8_0_39", "Encounter_8_0_41",
                ]
            },
            {
                (8, 1),
                [
                    "Encounter_8_1_1", "Encounter_8_1_3", "Encounter_8_1_5", "Encounter_8_1_7", "Encounter_8_1_9",
                    "Encounter_8_1_12", "Encounter_8_1_14", "Encounter_8_1_16", "Encounter_8_1_18", "Encounter_8_1_20",
                    "Encounter_8_1_23", "Encounter_8_1_25", "Encounter_8_1_27", "Encounter_8_1_29", "Encounter_8_1_31",
                    "Encounter_8_1_34", "Encounter_8_1_36", "Encounter_8_1_38", "Encounter_8_1_40", "Encounter_8_1_42",
                ]
            },
            {
                (8, 2),
                [
                    "Encounter_8_2_1", "Encounter_8_2_3", "Encounter_8_2_5", "Encounter_8_2_7", "Encounter_8_2_9",
                    "Encounter_8_2_12", "Encounter_8_2_14", "Encounter_8_2_16", "Encounter_8_2_18", "Encounter_8_2_20",
                    "Encounter_8_2_23", "Encounter_8_2_25", "Encounter_8_2_27", "Encounter_8_2_29", "Encounter_8_2_31",
                    "Encounter_8_2_34", "Encounter_8_2_36", "Encounter_8_2_38", "Encounter_8_2_40", "Encounter_8_2_42",
                ]
            },
            {
                (8, 3),
                [
                    "Encounter_8_3_1", "Encounter_8_3_3", "Encounter_8_3_5", "Encounter_8_3_8", "Encounter_8_3_10",
                    "Encounter_8_3_13", "Encounter_8_3_15", "Encounter_8_3_17", "Encounter_8_3_20", "Encounter_8_3_22",
                    "Encounter_8_3_25", "Encounter_8_3_27", "Encounter_8_3_29", "Encounter_8_3_32", "Encounter_8_3_34",
                    "Encounter_8_3_37", "Encounter_8_3_39", "Encounter_8_3_41", "Encounter_8_3_44", "Encounter_8_3_46",
                ]
            },

            // Difficulty 9
            {
                (9, 0),
                [
                    "Encounter_9_0_0", "Encounter_9_0_2", "Encounter_9_0_4", "Encounter_9_0_6", "Encounter_9_0_8",
                    "Encounter_9_0_11", "Encounter_9_0_13", "Encounter_9_0_15", "Encounter_9_0_17", "Encounter_9_0_19",
                    "Encounter_9_0_22", "Encounter_9_0_24", "Encounter_9_0_26", "Encounter_9_0_28", "Encounter_9_0_30",
                    "Encounter_9_0_33", "Encounter_9_0_35", "Encounter_9_0_37", "Encounter_9_0_39", "Encounter_9_0_41",
                ]
            },
            {
                (9, 1),
                [
                    "Encounter_9_1_1", "Encounter_9_1_3", "Encounter_9_1_5", "Encounter_9_1_7", "Encounter_9_1_9",
                    "Encounter_9_1_12", "Encounter_9_1_14", "Encounter_9_1_16", "Encounter_9_1_18", "Encounter_9_1_20",
                    "Encounter_9_1_23", "Encounter_9_1_25", "Encounter_9_1_27", "Encounter_9_1_29", "Encounter_9_1_31",
                    "Encounter_9_1_34", "Encounter_9_1_36", "Encounter_9_1_38", "Encounter_9_1_40", "Encounter_9_1_42",
                ]
            },
            {
                (9, 2),
                [
                    "Encounter_9_2_1", "Encounter_9_2_3", "Encounter_9_2_5", "Encounter_9_2_7", "Encounter_9_2_9",
                    "Encounter_9_2_12", "Encounter_9_2_14", "Encounter_9_2_16", "Encounter_9_2_18", "Encounter_9_2_20",
                    "Encounter_9_2_23", "Encounter_9_2_25", "Encounter_9_2_27", "Encounter_9_2_29", "Encounter_9_2_31",
                    "Encounter_9_2_34", "Encounter_9_2_36", "Encounter_9_2_38", "Encounter_9_2_40", "Encounter_9_2_42",
                ]
            },
            {
                (9, 3),
                [
                    "Encounter_9_3_1", "Encounter_9_3_3", "Encounter_9_3_5", "Encounter_9_3_8", "Encounter_9_3_10",
                    "Encounter_9_3_13", "Encounter_9_3_15", "Encounter_9_3_17", "Encounter_9_3_20", "Encounter_9_3_22",
                    "Encounter_9_3_25", "Encounter_9_3_27", "Encounter_9_3_29", "Encounter_9_3_32", "Encounter_9_3_34",
                    "Encounter_9_3_37", "Encounter_9_3_39", "Encounter_9_3_41", "Encounter_9_3_44", "Encounter_9_3_46",
                ]
            },
        };

    public static IReadOnlyCollection<string> GetStage(int difficulty, int stage)
    {
        return _encounters.GetValueOrDefault((difficulty, stage), []);
    }
}
