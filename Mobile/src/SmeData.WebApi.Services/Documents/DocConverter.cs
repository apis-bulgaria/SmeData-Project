using HtmlAgilityPack;
using Newtonsoft.Json;
using SmeData.SharedModels.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace SmeData.WebApi.Services.Documents
{
    public static class DocConverter
    {
        #region Arts to Recitals relations data dictionary
        private static readonly Dictionary<string, List<string>> artsAndRecs = new Dictionary<string, List<string>>()
        {
            { "art_1", new List<string>() { "rec_1", "rec_2", "rec_3", "rec_4", "rec_5", "rec_6", "rec_7", } },
            { "art_2__subPar_1", new List<string>() { "rec_15", } },
            { "art_2__subPar_2", new List<string>() { "rec_16", "rec_18", "rec_19", } },
            { "art_2__subPar_2__let_a", new List<string>() { "rec_16", } },
            { "art_2__subPar_2__let_а", new List<string>() { "rec_16", } },
            { "art_2__subPar_2__let_b", new List<string>() { "rec_16", } },
            { "art_2__subPar_2__let_б", new List<string>() { "rec_16", } },
            { "art_2__subPar_2__let_c", new List<string>() { "rec_18", } },
            { "art_2__subPar_2__let_в", new List<string>() { "rec_18", } },
            { "art_2__subPar_2__let_d", new List<string>() { "rec_19", } },
            { "art_2__subPar_2__let_г", new List<string>() { "rec_19", } },
            { "art_2__subPar_3", new List<string>() { "rec_17", "rec_19", } },
            { "art_2__subPar_4", new List<string>() { "rec_21", } },
            { "art_3__subPar_1", new List<string>() { "rec_22", } },
            { "art_3__subPar_2__let_a", new List<string>() { "rec_23", } },
            { "art_3__subPar_2__let_а", new List<string>() { "rec_23", } },
            { "art_3__subPar_2__let_b", new List<string>() { "rec_24", } },
            { "art_3__subPar_2__let_б", new List<string>() { "rec_24", } },
            { "art_3__subPar_3", new List<string>() { "rec_25", } },
            { "art_4__subPar_1", new List<string>() { "rec_27", "rec_158", "rec_160", } },
            { "art_4__subPar_3", new List<string>() { "rec_67", "rec_156", } },
            { "art_4__subPar_4", new List<string>() { "rec_24", "rec_60", "rec_63", "rec_70", "rec_71", "rec_72", "rec_73", "rec_91", } },
            { "art_4__subPar_5", new List<string>() { "rec_26", "rec_28", "rec_29", "rec_75", "rec_78", "rec_85", "rec_156", } },
            { "art_4__subPar_6", new List<string>() { "rec_15", "rec_31", "rec_67", } },
            { "art_4__subPar_7", new List<string>() { "rec_1", "rec_27", "rec_79", } },
            { "art_4__subPar_11", new List<string>() { "rec_25", "rec_32", "rec_42", } },
            { "art_4__subPar_12", new List<string>() { "rec_73", "rec_85", "rec_86", "rec_87", "rec_88", } },
            { "art_4__subPar_13", new List<string>() { "rec_34", "rec_35", "rec_53", "rec_75", } },
            { "art_4__subPar_14", new List<string>() { "rec_51", "rec_53", "rec_91", } },
            { "art_4__subPar_15", new List<string>() { "rec_35", "rec_53", "rec_54", "rec_75", } },
            { "art_4__subPar_16", new List<string>() { "rec_36", } },
            { "art_4__subPar_17", new List<string>() { "rec_80", } },
            { "art_4__subPar_18", new List<string>() { "rec_13", } },
            { "art_4__subPar_19", new List<string>() { "rec_36", "rec_37", "rec_48", "rec_110", } },
            { "art_4__subPar_20", new List<string>() { "rec_107", "rec_108", "rec_110", "rec_168", } },
            { "art_4__subPar_21", new List<string>() { "rec_20", "rec_36", } },
            { "art_4__subPar_22", new List<string>() { "rec_36", "rec_91", "rec_124", "rec_128", "rec_135", "rec_136", } },
            { "art_4__subPar_23", new List<string>() { "rec_5", "rec_53", "rec_116", "rec_135", "rec_136", "rec_137", "rec_138", } },
            { "art_4__subPar_24", new List<string>() { "rec_124", } },
            { "art_4__subPar_25", new List<string>() { "rec_21", } },
            { "art_4__subPar_26", new List<string>() { "rec_6", "rec_101", "rec_102", "rec_103", "rec_105", "rec_106", "rec_107", "rec_108", "rec_112", "rec_139", "rec_153", "rec_168", "rec_169", } },
            { "art_5__subPar_1__let_a", new List<string>() { "rec_39", "rec_58", "rec_60", } },
            { "art_5__subPar_1__let_а", new List<string>() { "rec_39", "rec_58", "rec_60", } },
            { "art_5__subPar_1__let_b", new List<string>() { "rec_50", } },
            { "art_5__subPar_1__let_б", new List<string>() { "rec_50", } },
            { "art_5__subPar_1__let_c", new List<string>() { "rec_39", } },
            { "art_5__subPar_1__let_в", new List<string>() { "rec_39", } },
            { "art_5__subPar_1__let_d", new List<string>() { "rec_39", "rec_59", "rec_65", "rec_73", } },
            { "art_5__subPar_1__let_г", new List<string>() { "rec_39", "rec_59", "rec_65", "rec_73", } },
            { "art_5__subPar_1__let_e", new List<string>() { "rec_39", } },
            { "art_5__subPar_1__let_д", new List<string>() { "rec_39", } },
            { "art_5__subPar_1__let_f", new List<string>() { "rec_29", "rec_71", "rec_156", } },
            { "art_5__subPar_1__let_е", new List<string>() { "rec_29", "rec_71", "rec_156", } },
            { "art_5__subPar_2", new List<string>() { "rec_85", } },
            { "art_6__subPar_1", new List<string>() { "rec_39", "rec_40", "rec_41", } },
            { "art_6__subPar_1__let_a", new List<string>() { "rec_32", "rec_42", "rec_43", } },
            { "art_6__subPar_1__let_а", new List<string>() { "rec_32", "rec_42", "rec_43", } },
            { "art_6__subPar_1__let_b", new List<string>() { "rec_44", } },
            { "art_6__subPar_1__let_б", new List<string>() { "rec_44", } },
            { "art_6__subPar_1__let_c", new List<string>() { "rec_45", } },
            { "art_6__subPar_1__let_в", new List<string>() { "rec_45", } },
            { "art_6__subPar_1__let_d", new List<string>() { "rec_46", } },
            { "art_6__subPar_1__let_г", new List<string>() { "rec_46", } },
            { "art_6__subPar_1__let_e", new List<string>() { "rec_45", } },
            { "art_6__subPar_1__let_д", new List<string>() { "rec_45", } },
            { "art_6__subPar_1__let_f", new List<string>() { "rec_47", "rec_48", } },
            { "art_6__subPar_1__let_е", new List<string>() { "rec_47", "rec_48", } },
            { "art_6__subPar_2", new List<string>() { "rec_40", } },
            { "art_6__subPar_3", new List<string>() { "rec_45", } },
            { "art_6__subPar_4", new List<string>() { "rec_50", } },
            { "art_6__subPar_4__let_e", new List<string>() { "rec_26", "rec_28", "rec_29", "rec_75", "rec_78", "rec_156", } },
            { "art_6__subPar_4__let_д", new List<string>() { "rec_26", "rec_28", "rec_29", "rec_75", "rec_78", "rec_156", } },
            { "art_7", new List<string>() { "rec_32", } },
            { "art_7__subPar_1", new List<string>() { "rec_32", "rec_42", } },
            { "art_7__subPar_3", new List<string>() { "rec_42", "rec_65", } },
            { "art_7__subPar_4", new List<string>() { "rec_32", "rec_43", } },
            { "art_8", new List<string>() { "rec_38", "rec_58", "rec_65", "rec_71", "rec_75", } },
            { "art_9", new List<string>() { "rec_51", "rec_52", "rec_53", "rec_54", "rec_55", "rec_56", } },
            { "art_9__subPar_1", new List<string>() { "rec_10", "rec_34", "rec_35", "rec_51", } },
            { "art_9__subPar_2__let_a", new List<string>() { "rec_32", "rec_42", "rec_51", } },
            { "art_9__subPar_2__let_а", new List<string>() { "rec_32", "rec_42", "rec_51", } },
            { "art_9__subPar_2__let_b", new List<string>() { "rec_52", "rec_127", "rec_155", } },
            { "art_9__subPar_2__let_б", new List<string>() { "rec_52", "rec_127", "rec_155", } },
            { "art_9__subPar_2__let_c", new List<string>() { "rec_46", "rec_51", } },
            { "art_9__subPar_2__let_в", new List<string>() { "rec_46", "rec_51", } },
            { "art_9__subPar_2__let_d", new List<string>() { "rec_51", } },
            { "art_9__subPar_2__let_г", new List<string>() { "rec_51", } },
            { "art_9__subPar_2__let_f", new List<string>() { "rec_52", } },
            { "art_9__subPar_2__let_е", new List<string>() { "rec_52", } },
            { "art_9__subPar_2__let_g", new List<string>() { "rec_52", } },
            { "art_9__subPar_2__let_ж", new List<string>() { "rec_52", } },
            { "art_9__subPar_2__let_h", new List<string>() { "rec_52", "rec_53", } },
            { "art_9__subPar_2__let_з", new List<string>() { "rec_52", "rec_53", } },
            { "art_9__subPar_2__let_i", new List<string>() { "rec_50", "rec_53", "rec_75", "rec_85", "rec_164", } },
            { "art_9__subPar_2__let_и", new List<string>() { "rec_50", "rec_53", "rec_75", "rec_85", "rec_164", } },
            { "art_9__subPar_2__let_j", new List<string>() { "rec_50", "rec_52", "rec_53", "rec_62", "rec_65", "rec_156", "rec_158", } },
            { "art_9__subPar_2__let_й", new List<string>() { "rec_50", "rec_52", "rec_53", "rec_62", "rec_65", "rec_156", "rec_158", } },
            { "art_9__subPar_3", new List<string>() { "rec_50", "rec_53", "rec_75", "rec_85", "rec_164", } },
            { "art_10", new List<string>() { "rec_19", "rec_50", "rec_73", "rec_80", "rec_91", "rec_97", } },
            { "art_11", new List<string>() { "rec_57", } },
            { "art_12", new List<string>() { "rec_39", "rec_58", "rec_60", } },
            { "art_12__subPar_1", new List<string>() { "rec_58", } },
            { "art_12__subPar_2", new List<string>() { "rec_57", "rec_59", "rec_64", } },
            { "art_12__subPar_3", new List<string>() { "rec_59", } },
            { "art_12__subPar_4", new List<string>() { "rec_59", } },
            { "art_12__subPar_5", new List<string>() { "rec_59", } },
            { "art_12__subPar_6", new List<string>() { "rec_57", "rec_64", } },
            { "art_12__subPar_7", new List<string>() { "rec_60", } },
            { "art_12__subPar_8", new List<string>() { "rec_166", "rec_167", } },
            { "art_13", new List<string>() { "rec_39", } },
            { "art_13__subPar_2__let_b", new List<string>() { "rec_59", "rec_66", "rec_68", } },
            { "art_13__subPar_2__let_б", new List<string>() { "rec_59", "rec_66", "rec_68", } },
            { "art_14", new List<string>() { "rec_39", "rec_58", "rec_60", } },
            { "art_14__subPar_2__let_c", new List<string>() { "rec_59", "rec_66", "rec_68", } },
            { "art_14__subPar_2__let_в", new List<string>() { "rec_59", "rec_66", "rec_68", } },
            { "art_14__subPar_5__let_d", new List<string>() { "rec_50", "rec_53", "rec_75", "rec_85", "rec_164", } },
            { "art_14__subPar_5__let_г", new List<string>() { "rec_50", "rec_53", "rec_75", "rec_85", "rec_164", } },
            { "art_15", new List<string>() { "rec_63", } },
            { "art_15__subPar_1__let_e", new List<string>() { "rec_59", "rec_66", "rec_68", } },
            { "art_15__subPar_1__let_д", new List<string>() { "rec_59", "rec_66", "rec_68", } },
            { "art_15__subPar_3", new List<string>() { "rec_59", } },
            { "art_15__subPar_4", new List<string>() { "rec_59", } },
            { "art_16", new List<string>() { "rec_39", "rec_59", "rec_65", "rec_66", "rec_68", "rec_73", } },
            { "art_17", new List<string>() { "rec_65", "rec_66", "rec_68", "rec_65", "rec_153", } },
            { "art_17__subPar_2", new List<string>() { "rec_62", } },
            { "art_17__subPar_3", new List<string>() { "rec_4", } },
            { "art_18", new List<string>() { "rec_67", "rec_156", } },
            { "art_19", new List<string>() { "rec_62", } },
            { "art_20", new List<string>() { "rec_68", "rec_73", } },
            { "art_21", new List<string>() { "rec_50", "rec_59", "rec_69", "rec_70", "rec_73", } },
            { "art_21__subPar_2", new List<string>() { "rec_70", } },
            { "art_21__subPar_3", new List<string>() { "rec_70", } },
            { "art_21__subPar_6", new List<string>() { "rec_156", } },
            { "art_22", new List<string>() { "rec_71", "rec_75", } },
            { "art_24", new List<string>() { "rec_74", } },
            { "art_24__subPar_1", new List<string>() { "rec_29", "rec_71", "rec_156", } },
            { "art_24__subPar_3", new List<string>() { "rec_77", } },
            { "art_25", new List<string>() { "rec_78", } },
            { "art_25__subPar_1", new List<string>() { "rec_26", "rec_28", "rec_29", "rec_71", "rec_75", "rec_78", "rec_156", } },
            { "art_25__subPar_2", new List<string>() { "rec_29", "rec_71", "rec_156", } },
            { "art_26", new List<string>() { "rec_79", } },
            { "art_26__subPar_3", new List<string>() { "rec_79", "rec_146", } },
            { "art_27", new List<string>() { "rec_80", } },
            { "art_28", new List<string>() { "rec_29", "rec_71", "rec_156", } },
            { "art_28__subPar_1", new List<string>() { "rec_81", } },
            { "art_28__subPar_2", new List<string>() { "rec_81", } },
            { "art_28__subPar_3", new List<string>() { "rec_81", } },
            { "art_28__subPar_3__let_b", new List<string>() { "rec_81", "rec_83", } },
            { "art_28__subPar_3__let_б", new List<string>() { "rec_81", "rec_83", } },
            { "art_28__subPar_3__let_e", new List<string>() { "rec_81", "rec_83", } },
            { "art_28__subPar_3__let_д", new List<string>() { "rec_81", "rec_83", } },
            { "art_28__subPar_3__let_h", new List<string>() { "rec_81", "rec_82", } },
            { "art_28__subPar_3__let_з", new List<string>() { "rec_81", "rec_82", } },
            { "art_28__subPar_4", new List<string>() { "rec_81", } },
            { "art_28__subPar_5", new List<string>() { "rec_77", } },
            { "art_28__subPar_6", new List<string>() { "rec_81", "rec_108", "rec_109", } },
            { "art_28__subPar_7", new List<string>() { "rec_81", "rec_108", "rec_109", } },
            { "art_28__subPar_8", new List<string>() { "rec_81", "rec_108", "rec_109", } },
            { "art_30", new List<string>() { "rec_82", "rec_89", } },
            { "art_30__subPar_2", new List<string>() { "rec_82", } },
            { "art_31", new List<string>() { "rec_82", } },
            { "art_32", new List<string>() { "rec_29", "rec_71", "rec_83", "rec_156", } },
            { "art_32__subPar_1__let_a", new List<string>() { "rec_26", "rec_28", "rec_29", "rec_75", "rec_78", "rec_156", } },
            { "art_32__subPar_1__let_а", new List<string>() { "rec_26", "rec_28", "rec_29", "rec_75", "rec_78", "rec_156", } },
            { "art_33", new List<string>() { "rec_73", "rec_85", "rec_86", "rec_87", "rec_88", } },
            { "art_33__subPar_2", new List<string>() { "rec_85", } },
            { "art_34", new List<string>() { "rec_73", "rec_86", "rec_87", "rec_88", } },
            { "art_35", new List<string>() { "rec_84", "rec_90", "rec_91", "rec_92", "rec_93", "rec_94", "rec_95", "rec_95", "rec_95", } },
            { "art_35__subPar_4", new List<string>() { "rec_94", } },
            { "art_35__subPar_5", new List<string>() { "rec_94", } },
            { "art_35__subPar_6", new List<string>() { "rec_94", } },
            { "art_35__subPar_8", new List<string>() { "rec_77", } },
            { "art_36", new List<string>() { "rec_94", "rec_95", } },
            { "art_37", new List<string>() { "rec_97", } },
            { "art_37__subPar_5", new List<string>() { "rec_97", } },
            { "art_37__subPar_6", new List<string>() { "rec_97", } },
            { "art_38__subPar_1", new List<string>() { "rec_97", } },
            { "art_38__subPar_2", new List<string>() { "rec_97", } },
            { "art_38__subPar_3", new List<string>() { "rec_97", } },
            { "art_38__subPar_4", new List<string>() { "rec_97", } },
            { "art_38__subPar_5", new List<string>() { "rec_97", } },
            { "art_38__subPar_6", new List<string>() { "rec_97", } },
            { "art_39", new List<string>() { "rec_29", "rec_71", "rec_97", "rec_156", } },
            { "art_40", new List<string>() { "rec_98", "rec_99", "rec_108", } },
            { "art_40__subPar_1", new List<string>() { "rec_98", } },
            { "art_40__subPar_2", new List<string>() { "rec_77", "rec_81", "rec_98", "rec_99", "rec_28", "rec_29", "rec_75", "rec_78", "rec_156", } },
            { "art_40__subPar_2__let_d", new List<string>() { "rec_26", } },
            { "art_40__subPar_2__let_г", new List<string>() { "rec_26", } },
            { "art_40__subPar_2__let_j", new List<string>() { "rec_81", } },
            { "art_40__subPar_2__let_й", new List<string>() { "rec_81", } },
            { "art_40__subPar_3", new List<string>() { "rec_81", } },
            { "art_41", new List<string>() { "rec_108", } },
            { "art_42", new List<string>() { "rec_77", "rec_81", "rec_108", } },
            { "art_42__subPar_1", new List<string>() { "rec_100", } },
            { "art_42__subPar_8", new List<string>() { "rec_100", } },
            { "art_43", new List<string>() { "rec_77", "rec_81", "rec_100", "rec_108", } },
            { "art_44", new List<string>() { "rec_101", "rec_102", "rec_103", "rec_104", "rec_105", "rec_106", "rec_107", "rec_108", "rec_109", "rec_110", "rec_111", "rec_112", "rec_113", "rec_114", "rec_115", "rec_116", } },
            { "art_45", new List<string>() { "rec_101", "rec_102", "rec_103", "rec_104", "rec_105", "rec_106", "rec_107", "rec_108", "rec_109", "rec_110", "rec_111", "rec_112", "rec_113", "rec_114", "rec_115", "rec_116", "rec_107", } },
            { "art_45__subPar_3", new List<string>() { "rec_106", "rec_107", } },
            { "art_45__subPar_4", new List<string>() { "rec_106", "rec_107", } },
            { "art_45__subPar_5", new List<string>() { "rec_106", } },
            { "art_46", new List<string>() { "rec_108", } },
            { "art_46__subPar_2__let_a", new List<string>() { "rec_108", } },
            { "art_46__subPar_2__let_а", new List<string>() { "rec_108", } },
            { "art_46__subPar_2__let_b", new List<string>() { "rec_108", } },
            { "art_46__subPar_2__let_б", new List<string>() { "rec_108", } },
            { "art_46__subPar_2__let_c", new List<string>() { "rec_81", "rec_108", "rec_109", } },
            { "art_46__subPar_2__let_в", new List<string>() { "rec_81", "rec_108", "rec_109", } },
            { "art_46__subPar_2__let_d", new List<string>() { "rec_108", "rec_109", } },
            { "art_46__subPar_2__let_г", new List<string>() { "rec_108", "rec_109", } },
            { "art_46__subPar_2__let_e", new List<string>() { "rec_77", "rec_81", "rec_108", } },
            { "art_46__subPar_2__let_д", new List<string>() { "rec_77", "rec_81", "rec_108", } },
            { "art_46__subPar_2__let_f", new List<string>() { "rec_108", } },
            { "art_46__subPar_2__let_е", new List<string>() { "rec_108", } },
            { "art_46__subPar_3__let_a", new List<string>() { "rec_108", } },
            { "art_46__subPar_3__let_а", new List<string>() { "rec_108", } },
            { "art_46__subPar_3__let_b", new List<string>() { "rec_108", } },
            { "art_46__subPar_3__let_б", new List<string>() { "rec_108", } },
            { "art_46__subPar_4", new List<string>() { "rec_108", } },
            { "art_47", new List<string>() { "rec_108", "rec_110", } },
            { "art_47__subPar_1", new List<string>() { "rec_108", "rec_110", } },
            { "art_47__subPar_2", new List<string>() { "rec_108", "rec_110", } },
            { "art_47__subPar_3", new List<string>() { "rec_108", "rec_110", "rec_116", "rec_168", } },
            { "art_48", new List<string>() { "rec_102", "rec_115", } },
            { "art_49__subPar_1", new List<string>() { "rec_113", } },
            { "art_49__subPar_1__let_a", new List<string>() { "rec_111", } },
            { "art_49__subPar_1__let_а", new List<string>() { "rec_111", } },
            { "art_49__subPar_1__let_b", new List<string>() { "rec_111", } },
            { "art_49__subPar_1__let_б", new List<string>() { "rec_111", } },
            { "art_49__subPar_1__let_c", new List<string>() { "rec_111", } },
            { "art_49__subPar_1__let_в", new List<string>() { "rec_111", } },
            { "art_49__subPar_1__let_d", new List<string>() { "rec_111", "rec_112", } },
            { "art_49__subPar_1__let_г", new List<string>() { "rec_111", "rec_112", } },
            { "art_49__subPar_1__let_e", new List<string>() { "rec_111", } },
            { "art_49__subPar_1__let_д", new List<string>() { "rec_111", } },
            { "art_49__subPar_1__let_f", new List<string>() { "rec_111", "rec_112", } },
            { "art_49__subPar_1__let_е", new List<string>() { "rec_111", "rec_112", } },
            { "art_49__subPar_1__let_g", new List<string>() { "rec_111", } },
            { "art_49__subPar_1__let_ж", new List<string>() { "rec_111", } },
            { "art_49__subPar_2", new List<string>() { "rec_111", } },
            { "art_49__subPar_3", new List<string>() { "rec_111", "rec_112", "rec_113", } },
            { "art_49__subPar_5", new List<string>() { "rec_111", } },
            { "art_49__subPar_6", new List<string>() { "rec_113", } },
            { "art_50", new List<string>() { "rec_116", "rec_168", } },
            { "art_51", new List<string>() { "rec_117", "rec_124", } },
            { "art_52", new List<string>() { "rec_117", "rec_118", "rec_121", } },
            { "art_53", new List<string>() { "rec_121", } },
            { "art_54", new List<string>() { "rec_121", } },
            { "art_54__subPar_2", new List<string>() { "rec_50", "rec_53", "rec_75", "rec_85", "rec_164", } },
            { "art_55", new List<string>() { "rec_122", "rec_123", "rec_124", "rec_125", "rec_126", "rec_127", "rec_128", } },
            { "art_56", new List<string>() { "rec_124", "rec_125", "rec_126", "rec_127", "rec_128", "rec_135", "rec_136", "rec_137", "rec_138", } },
            { "art_57", new List<string>() { "rec_122", "rec_123", } },
            { "art_57__subPar_1__let_j", new List<string>() { "rec_81", "rec_108", "rec_109", } },
            { "art_57__subPar_1__let_й", new List<string>() { "rec_81", "rec_108", "rec_109", } },
            { "art_57__subPar_1__let_m", new List<string>() { "rec_98", } },
            { "art_57__subPar_1__let_м", new List<string>() { "rec_98", } },
            { "art_57__subPar_1__let_o", new List<string>() { "rec_98", } },
            { "art_57__subPar_1__let_о", new List<string>() { "rec_98", } },
            { "art_57__subPar_1__let_p", new List<string>() { "rec_98", } },
            { "art_57__subPar_1__let_п", new List<string>() { "rec_98", } },
            { "art_57__subPar_1__let_r", new List<string>() { "rec_81", "rec_108", "rec_109", } },
            { "art_57__subPar_1__let_с", new List<string>() { "rec_81", "rec_108", "rec_109", } },
            { "art_57__subPar_1__let_s", new List<string>() { "rec_108", "rec_110", } },
            { "art_57__subPar_1__let_т", new List<string>() { "rec_108", "rec_110", } },
            { "art_58", new List<string>() { "rec_129", } },
            { "art_60", new List<string>() { "rec_116", "rec_168", } },
            { "art_61", new List<string>() { "rec_133", "rec_134", "rec_168", } },
            { "art_61__subPar_3", new List<string>() { "rec_116", "rec_168", } },
            { "art_61__subPar_9", new List<string>() { "rec_116", } },
            { "art_62", new List<string>() { "rec_133", "rec_134", } },
            { "art_63", new List<string>() { "rec_108", "rec_132", "rec_133", "rec_134", "rec_135", "rec_136", "rec_137", "rec_138", } },
            { "art_64", new List<string>() { "rec_135", "rec_136", "rec_137", "rec_138", } },
            { "art_64__subPar_1__let_d", new List<string>() { "rec_108", "rec_109", } },
            { "art_64__subPar_1__let_г", new List<string>() { "rec_108", "rec_109", } },
            { "art_64__subPar_2", new List<string>() { "rec_132", "rec_133", "rec_134", } },
            { "art_65", new List<string>() { "rec_135", "rec_136", "rec_137", "rec_138", } },
            { "art_66", new List<string>() { "rec_135", "rec_136", "rec_137", "rec_138", } },
            { "art_67", new List<string>() { "rec_116", "rec_135", "rec_136", "rec_137", "rec_138", "rec_168", } },
            { "art_70__subPar_1__let_c", new List<string>() { "rec_116", "rec_168", } },
            { "art_70__subPar_1__let_в", new List<string>() { "rec_116", "rec_168", } },
            { "art_70__subPar_1__let_u", new List<string>() { "rec_116", "rec_168", } },
            { "art_70__subPar_1__let_ф", new List<string>() { "rec_116", "rec_168", } },
            { "art_70__subPar_1__let_v", new List<string>() { "rec_116", "rec_168", } },
            { "art_70__subPar_1__let_х", new List<string>() { "rec_116", "rec_168", } },
            { "art_70__subPar_1__let_w", new List<string>() { "rec_116", "rec_168", } },
            { "art_70__subPar_1__let_ц", new List<string>() { "rec_116", "rec_168", } },
            { "art_78", new List<string>() { "rec_143", } },
            { "art_78__subPar_3", new List<string>() { "rec_143", } },
            { "art_79", new List<string>() { "rec_143", } },
            { "art_79__subPar_2", new List<string>() { "rec_143", } },
            { "art_80", new List<string>() { "rec_142", } },
            { "art_81", new List<string>() { "rec_144", } },
            { "art_82__subPar_1", new List<string>() { "rec_146", "rec_147", } },
            { "art_82__subPar_2", new List<string>() { "rec_146", "rec_147", } },
            { "art_82__subPar_3", new List<string>() { "rec_79", "rec_146", } },
            { "art_82__subPar_4", new List<string>() { "rec_79", "rec_146", "rec_147", } },
            { "art_82__subPar_5", new List<string>() { "rec_79", "rec_146", } },
            { "art_83", new List<string>() { "rec_150", "rec_152", } },
            { "art_83__subPar_1", new List<string>() { "rec_156", } },
            { "art_83__subPar_2", new List<string>() { "rec_150", } },
            { "art_83__subPar_2__let_j", new List<string>() { "rec_77", } },
            { "art_83__subPar_2__let_й", new List<string>() { "rec_77", } },
            { "art_83__subPar_5", new List<string>() { "rec_150", } },
            { "art_83__subPar_6", new List<string>() { "rec_150", } },
            { "art_84", new List<string>() { "rec_149", "rec_152", } },
            { "art_85", new List<string>() { "rec_4", "rec_65", "rec_153", } },
            { "art_88", new List<string>() { "rec_52", "rec_127", "rec_155", } },
            { "art_89__subPar_1", new List<string>() { "rec_26", "rec_28", "rec_29", "rec_75", "rec_78", "rec_156", } },
            { "art_89__subPar_2", new List<string>() { "rec_156", } },
            { "art_90", new List<string>() { "rec_50", "rec_53", "rec_75", "rec_85", "rec_164", } },
            { "art_91", new List<string>() { "rec_55", "rec_165", } },
            { "art_92", new List<string>() { "rec_166", "rec_167", } },
            { "art_93__subPar_2", new List<string>() { "rec_81", "rec_106", "rec_107", "rec_108", "rec_109", } },
            { "art_93__subPar_3", new List<string>() { "rec_106", } },
            { "art_93__subPar_4", new List<string>() { "rec_107", } },
            { "art_94", new List<string>() { "rec_171", } },
            { "art_95", new List<string>() { "rec_173", } },
            { "art_96", new List<string>() { "rec_102", "rec_115", } },
            { "art_97", new List<string>() { "rec_98", } },
            { "art_99", new List<string>() { "rec_171", } },
        };

        private static readonly Dictionary<string, List<string>> artsAndOldArts = new Dictionary<string, List<string>>()
        {
            { "art_1", new List<string>() { "art_1", } },
            { "art_2__subPar_1", new List<string>() { "art_3", } },
            { "art_2__subPar_2", new List<string>() { "art_3", } },
            { "art_2__subPar_2__let_a", new List<string>() { "art_3", } },
            { "art_2__subPar_2__let_а", new List<string>() { "art_3", } },
            { "art_2__subPar_2__let_b", new List<string>() { "art_3", } },
            { "art_2__subPar_2__let_б", new List<string>() { "art_3", } },
            { "art_2__subPar_2__let_c", new List<string>() { "art_3", } },
            { "art_2__subPar_2__let_в", new List<string>() { "art_3", } },
            { "art_2__subPar_2__let_d", new List<string>() { "art_3", } },
            { "art_2__subPar_2__let_г", new List<string>() { "art_3", } },
            { "art_2__subPar_3", new List<string>() { "art_3", } },
            { "art_3__subPar_1", new List<string>() { "art_4", } },
            { "art_3__subPar_2__let_a", new List<string>() { "art_4__subPar_1__let_c", "art_4__subPar_1__let_в", } },
            { "art_3__subPar_2__let_а", new List<string>() { "art_4__subPar_1__let_c", "art_4__subPar_1__let_в", } },
            { "art_3__subPar_3", new List<string>() { "art_4__subPar_1__let_b", "art_4__subPar_1__let_б", } },
            { "art_4__subPar_1", new List<string>() { "art_2__let_a", "art_2__let_а", } },
            { "art_4__subPar_2", new List<string>() { "art_2__let_b", "art_2__let_б", } },
            { "art_4__subPar_6", new List<string>() { "art_2__let_c", "art_2__let_в", } },
            { "art_4__subPar_7", new List<string>() { "art_1", "art_2__let_d", "art_2__let_г", } },
            { "art_4__subPar_8", new List<string>() { "art_2__let_e", "art_2__let_д", } },
            { "art_4__subPar_9", new List<string>() { "art_2__let_g", "art_2__let_ж", } },
            { "art_4__subPar_10", new List<string>() { "art_2__let_f", "art_2__let_е", } },
            { "art_4__subPar_11", new List<string>() { "art_2__let_h", "art_2__let_з", } },
            { "art_4__subPar_12", new List<string>() { "art_17__subPar_1", } },
            { "art_4__subPar_17", new List<string>() { "art_4__subPar_2", } },
            { "art_4__subPar_20", new List<string>() { "art_26__subPar_2", } },
            { "art_5__subPar_1__let_a", new List<string>() { "art_6__subPar_1__let_a", "art_6__subPar_1__let_а", "art_10", } },
            { "art_5__subPar_1__let_а", new List<string>() { "art_6__subPar_1__let_a", "art_6__subPar_1__let_а", "art_10", } },
            { "art_5__subPar_1__let_b", new List<string>() { "art_6__subPar_1__let_b", "art_6__subPar_1__let_б", } },
            { "art_5__subPar_1__let_б", new List<string>() { "art_6__subPar_1__let_b", "art_6__subPar_1__let_б", } },
            { "art_5__subPar_1__let_c", new List<string>() { "art_6__subPar_1__let_c", "art_6__subPar_1__let_в", } },
            { "art_5__subPar_1__let_в", new List<string>() { "art_6__subPar_1__let_c", "art_6__subPar_1__let_в", } },
            { "art_5__subPar_1__let_d", new List<string>() { "art_6__subPar_1__let_d", "art_6__subPar_1__let_г", "art_12__let_b", "art_12__let_б", } },
            { "art_5__subPar_1__let_г", new List<string>() { "art_6__subPar_1__let_d", "art_6__subPar_1__let_г", "art_12__let_b", "art_12__let_б", } },
            { "art_5__subPar_1__let_e", new List<string>() { "art_6__subPar_1__let_e", "art_6__subPar_1__let_д", } },
            { "art_5__subPar_1__let_д", new List<string>() { "art_6__subPar_1__let_e", "art_6__subPar_1__let_д", } },
            { "art_5__subPar_1__let_f", new List<string>() { "art_17__subPar_1", } },
            { "art_5__subPar_1__let_е", new List<string>() { "art_17__subPar_1", } },
            { "art_5__subPar_2", new List<string>() { "art_6__subPar_2", } },
            { "art_6__subPar_1", new List<string>() { "art_7", } },
            { "art_6__subPar_1__let_a", new List<string>() { "art_2__let_h", "art_2__let_з", "art_7__let_a", "art_7__let_а", } },
            { "art_6__subPar_1__let_а", new List<string>() { "art_2__let_h", "art_2__let_з", "art_7__let_a", "art_7__let_а", } },
            { "art_6__subPar_1__let_b", new List<string>() { "art_7__let_b", "art_7__let_б", } },
            { "art_6__subPar_1__let_б", new List<string>() { "art_7__let_b", "art_7__let_б", } },
            { "art_6__subPar_1__let_c", new List<string>() { "art_7__let_c", "art_7__let_в", } },
            { "art_6__subPar_1__let_в", new List<string>() { "art_7__let_c", "art_7__let_в", } },
            { "art_6__subPar_1__let_d", new List<string>() { "art_7__let_d", "art_7__let_г", } },
            { "art_6__subPar_1__let_г", new List<string>() { "art_7__let_d", "art_7__let_г", } },
            { "art_6__subPar_1__let_e", new List<string>() { "art_7__let_e", "art_7__let_д", } },
            { "art_6__subPar_1__let_д", new List<string>() { "art_7__let_e", "art_7__let_д", } },
            { "art_6__subPar_1__let_f", new List<string>() { "art_7__let_f", "art_7__let_е", } },
            { "art_6__subPar_1__let_е", new List<string>() { "art_7__let_f", "art_7__let_е", } },
            { "art_6__subPar_4", new List<string>() { "art_6__subPar_1__let_b", "art_6__subPar_1__let_б", } },
            { "art_7", new List<string>() { "art_2__let_h", "art_2__let_з", } },
            { "art_7__subPar_1", new List<string>() { "art_2__let_h", "art_2__let_з", } },
            { "art_9", new List<string>() { "art_8", } },
            { "art_9__subPar_1", new List<string>() { "art_8__subPar_1", } },
            { "art_9__subPar_2__let_a", new List<string>() { "art_8__subPar_2", } },
            { "art_9__subPar_2__let_а", new List<string>() { "art_8__subPar_2", } },
            { "art_9__subPar_2__let_b", new List<string>() { "art_8__subPar_2__let_b", "art_8__subPar_2__let_б", } },
            { "art_9__subPar_2__let_б", new List<string>() { "art_8__subPar_2__let_b", "art_8__subPar_2__let_б", } },
            { "art_9__subPar_2__let_c", new List<string>() { "art_8__subPar_2__let_c", "art_8__subPar_2__let_в", } },
            { "art_9__subPar_2__let_в", new List<string>() { "art_8__subPar_2__let_c", "art_8__subPar_2__let_в", } },
            { "art_9__subPar_2__let_d", new List<string>() { "art_8__subPar_2__let_d", "art_8__subPar_2__let_г", } },
            { "art_9__subPar_2__let_г", new List<string>() { "art_8__subPar_2__let_d", "art_8__subPar_2__let_г", } },
            { "art_9__subPar_2__let_e", new List<string>() { "art_8__subPar_2__let_e", "art_8__subPar_2__let_д", } },
            { "art_9__subPar_2__let_д", new List<string>() { "art_8__subPar_2__let_e", "art_8__subPar_2__let_д", } },
            { "art_9__subPar_2__let_f", new List<string>() { "art_8__subPar_5", } },
            { "art_9__subPar_2__let_е", new List<string>() { "art_8__subPar_5", } },
            { "art_9__subPar_2__let_h", new List<string>() { "art_8__subPar_3", } },
            { "art_9__subPar_2__let_з", new List<string>() { "art_8__subPar_3", } },
            { "art_9__subPar_3", new List<string>() { "art_8__subPar_3", } },
            { "art_10", new List<string>() { "art_8__subPar_5", } },
            { "art_12", new List<string>() { "art_10", } },
            { "art_12__subPar_5", new List<string>() { "art_12", } },
            { "art_13", new List<string>() { "art_10", } },
            { "art_14", new List<string>() { "art_10", "art_11", } },
            { "art_15", new List<string>() { "art_12", } },
            { "art_15__subPar_3", new List<string>() { "art_12", } },
            { "art_15__subPar_4", new List<string>() { "art_12", } },
            { "art_16", new List<string>() { "art_6", "art_12__let_b", "art_12__let_б", } },
            { "art_17", new List<string>() { "art_12__let_b", "art_12__let_б", } },
            { "art_17__subPar_2", new List<string>() { "art_12__let_c", "art_12__let_в", } },
            { "art_17__subPar_3", new List<string>() { "art_9", } },
            { "art_19", new List<string>() { "art_12__let_c", "art_12__let_в", } },
            { "art_21", new List<string>() { "art_14__let_a", "art_14__let_а", } },
            { "art_21__subPar_2", new List<string>() { "art_14__let_b", "art_14__let_б", } },
            { "art_21__subPar_3", new List<string>() { "art_14__let_b", "art_14__let_б", } },
            { "art_22", new List<string>() { "art_15", } },
            { "art_24", new List<string>() { "art_6__subPar_2", } },
            { "art_24__subPar_1", new List<string>() { "art_17__subPar_1", } },
            { "art_24__subPar_3", new List<string>() { "art_27", } },
            { "art_25__subPar_1", new List<string>() { "art_17__subPar_1", } },
            { "art_25__subPar_2", new List<string>() { "art_17__subPar_1", } },
            { "art_26", new List<string>() { "art_2__let_b", "art_2__let_б", } },
            { "art_26__subPar_3", new List<string>() { "art_23__subPar_2", } },
            { "art_27", new List<string>() { "art_4__subPar_2", } },
            { "art_28", new List<string>() { "art_17", } },
            { "art_28__subPar_1", new List<string>() { "art_17", } },
            { "art_28__subPar_2", new List<string>() { "art_16", } },
            { "art_28__subPar_3", new List<string>() { "art_17", } },
            { "art_28__subPar_3__let_b", new List<string>() { "art_16", } },
            { "art_28__subPar_3__let_б", new List<string>() { "art_16", } },
            { "art_28__subPar_4", new List<string>() { "art_16", } },
            { "art_28__subPar_5", new List<string>() { "art_27", } },
            { "art_28__subPar_6", new List<string>() { "art_26", "art_31", } },
            { "art_28__subPar_7", new List<string>() { "art_26", "art_31__subPar_2", } },
            { "art_28__subPar_8", new List<string>() { "art_26", "art_31__subPar_2", } },
            { "art_29", new List<string>() { "art_16", } },
            { "art_30", new List<string>() { "art_18", } },
            { "art_32", new List<string>() { "art_17__subPar_1", } },
            { "art_35__subPar_4", new List<string>() { "art_20", } },
            { "art_35__subPar_5", new List<string>() { "art_20", } },
            { "art_35__subPar_6", new List<string>() { "art_20", } },
            { "art_35__subPar_8", new List<string>() { "art_27", } },
            { "art_36", new List<string>() { "art_20", } },
            { "art_39", new List<string>() { "art_17__subPar_1", } },
            { "art_40", new List<string>() { "art_27", } },
            { "art_40__subPar_1", new List<string>() { "art_27__subPar_1", } },
            { "art_40__subPar_2", new List<string>() { "art_27", } },
            { "art_40__subPar_4", new List<string>() { "art_27", } },
            { "art_40__subPar_5", new List<string>() { "art_27__subPar_2", } },
            { "art_40__subPar_8", new List<string>() { "art_27__subPar_3", } },
            { "art_41", new List<string>() { "art_27", } },
            { "art_44", new List<string>() { "art_25", "art_26", "art_31", } },
            { "art_45", new List<string>() { "art_25", "art_26", "art_31", } },
            { "art_46__subPar_2__let_b", new List<string>() { "art_26__subPar_2", } },
            { "art_46__subPar_2__let_б", new List<string>() { "art_26__subPar_2", } },
            { "art_46__subPar_2__let_c", new List<string>() { "art_26", "art_31", } },
            { "art_46__subPar_2__let_в", new List<string>() { "art_26", "art_31", } },
            { "art_46__subPar_2__let_e", new List<string>() { "art_27", } },
            { "art_46__subPar_2__let_д", new List<string>() { "art_27", } },
            { "art_46__subPar_3__let_a", new List<string>() { "art_26", } },
            { "art_46__subPar_3__let_а", new List<string>() { "art_26", } },
            { "art_46__subPar_4", new List<string>() { "art_26", } },
            { "art_47", new List<string>() { "art_26__subPar_2", } },
            { "art_49__subPar_1__let_a", new List<string>() { "art_26__subPar_1__let_a", "art_26__subPar_1__let_а", } },
            { "art_49__subPar_1__let_а", new List<string>() { "art_26__subPar_1__let_a", "art_26__subPar_1__let_а", } },
            { "art_49__subPar_1__let_b", new List<string>() { "art_26__subPar_1__let_b", "art_26__subPar_1__let_б", } },
            { "art_49__subPar_1__let_б", new List<string>() { "art_26__subPar_1__let_b", "art_26__subPar_1__let_б", } },
            { "art_49__subPar_1__let_c", new List<string>() { "art_26__subPar_1__let_c", "art_26__subPar_1__let_в", } },
            { "art_49__subPar_1__let_в", new List<string>() { "art_26__subPar_1__let_c", "art_26__subPar_1__let_в", } },
            { "art_49__subPar_1__let_d", new List<string>() { "art_26__subPar_1__let_d", "art_26__subPar_1__let_г", } },
            { "art_49__subPar_1__let_г", new List<string>() { "art_26__subPar_1__let_d", "art_26__subPar_1__let_г", } },
            { "art_49__subPar_1__let_e", new List<string>() { "art_26__subPar_1__let_d", "art_26__subPar_1__let_г", } },
            { "art_49__subPar_1__let_д", new List<string>() { "art_26__subPar_1__let_d", "art_26__subPar_1__let_г", } },
            { "art_49__subPar_1__let_f", new List<string>() { "art_26__subPar_1__let_e", "art_26__subPar_1__let_д", } },
            { "art_49__subPar_1__let_е", new List<string>() { "art_26__subPar_1__let_e", "art_26__subPar_1__let_д", } },
            { "art_49__subPar_1__let_g", new List<string>() { "art_26__subPar_1__let_f", "art_26__subPar_1__let_е", } },
            { "art_49__subPar_1__let_ж", new List<string>() { "art_26__subPar_1__let_f", "art_26__subPar_1__let_е", } },
            { "art_49__subPar_2", new List<string>() { "art_26__subPar_1__let_f", "art_26__subPar_1__let_е", } },
            { "art_49__subPar_3", new List<string>() { "art_26__subPar_1", } },
            { "art_51", new List<string>() { "art_28", } },
            { "art_52", new List<string>() { "art_28__subPar_1", } },
            { "art_53", new List<string>() { "art_28", } },
            { "art_54", new List<string>() { "art_28", } },
            { "art_55", new List<string>() { "art_28", } },
            { "art_56", new List<string>() { "art_28", } },
            { "art_57", new List<string>() { "art_28__subPar_4", } },
            { "art_57__subPar_1__let_j", new List<string>() { "art_26", } },
            { "art_57__subPar_1__let_й", new List<string>() { "art_26", } },
            { "art_57__subPar_1__let_m", new List<string>() { "art_27__subPar_1", } },
            { "art_57__subPar_1__let_м", new List<string>() { "art_27__subPar_1", } },
            { "art_57__subPar_1__let_o", new List<string>() { "art_27__subPar_1", } },
            { "art_57__subPar_1__let_о", new List<string>() { "art_27__subPar_1", } },
            { "art_57__subPar_1__let_p", new List<string>() { "art_27", } },
            { "art_57__subPar_1__let_п", new List<string>() { "art_27", } },
            { "art_57__subPar_1__let_r", new List<string>() { "art_26", "art_31__subPar_2", } },
            { "art_57__subPar_1__let_с", new List<string>() { "art_26", "art_31__subPar_2", } },
            { "art_58", new List<string>() { "art_28", } },
            { "art_58__subPar_3__let_d", new List<string>() { "art_27__subPar_2", } },
            { "art_58__subPar_3__let_г", new List<string>() { "art_27__subPar_2", } },
            { "art_59", new List<string>() { "art_28__subPar_5", } },
            { "art_61", new List<string>() { "art_29", } },
            { "art_62", new List<string>() { "art_29", } },
            { "art_63", new List<string>() { "art_26", "art_28__subPar_1", "art_31__subPar_2", } },
            { "art_64__subPar_1__let_b", new List<string>() { "art_27__subPar_3", } },
            { "art_64__subPar_1__let_б", new List<string>() { "art_27__subPar_3", } },
            { "art_64__subPar_2", new List<string>() { "art_28__subPar_1", } },
            { "art_68", new List<string>() { "art_29", } },
            { "art_69", new List<string>() { "art_29", } },
            { "art_70", new List<string>() { "art_29", } },
            { "art_71", new List<string>() { "art_29", } },
            { "art_72", new List<string>() { "art_29", } },
            { "art_73", new List<string>() { "art_29", } },
            { "art_74", new List<string>() { "art_29", } },
            { "art_75", new List<string>() { "art_29", } },
            { "art_76", new List<string>() { "art_29", } },
            { "art_77", new List<string>() { "art_28__subPar_4", } },
            { "art_78", new List<string>() { "art_28__subPar_3", } },
            { "art_78__subPar_3", new List<string>() { "art_22", } },
            { "art_79", new List<string>() { "art_28__subPar_3", } },
            { "art_79__subPar_2", new List<string>() { "art_22", } },
            { "art_80", new List<string>() { "art_27", } },
            { "art_82__subPar_1", new List<string>() { "art_23", } },
            { "art_82__subPar_2", new List<string>() { "art_23", } },
            { "art_82__subPar_3", new List<string>() { "art_23__subPar_2", } },
            { "art_82__subPar_4", new List<string>() { "art_23__subPar_1", "art_23__subPar_2", } },
            { "art_82__subPar_5", new List<string>() { "art_23__subPar_2", } },
            { "art_83", new List<string>() { "art_8__subPar_3", "art_24", } },
            { "art_83__subPar_2__let_j", new List<string>() { "art_27", } },
            { "art_83__subPar_2__let_й", new List<string>() { "art_27", } },
            { "art_83__subPar_5", new List<string>() { "art_24", } },
            { "art_83__subPar_6", new List<string>() { "art_24", } },
            { "art_84", new List<string>() { "art_24", } },
            { "art_86", new List<string>() { "art_7__let_e", "art_7__let_д", } },
            { "art_87", new List<string>() { "art_8__subPar_7", } },
            { "art_88", new List<string>() { "art_8__subPar_2__let_b", "art_8__subPar_2__let_б", } },
            { "art_89__subPar_1", new List<string>() { "art_6__subPar_1__let_a", "art_6__subPar_1__let_а", "art_6__subPar_1__let_e", "art_6__subPar_1__let_д", "art_11__subPar_2", "art_13__subPar_2", } },
            { "art_91", new List<string>() { "art_8__subPar_2", } },
            { "art_93__subPar_2", new List<string>() { "art_26", "art_31__subPar_2", } },
            { "art_97", new List<string>() { "art_33", } },
            { "art_98", new List<string>() { "art_33", } },
            { "art_99", new List<string>() { "art_32", } },
        };

        #endregion

        public static SmeDocMeta GetSmeDocMeta(string xmlDoc)
        {
            var docMeta = new SmeDocMeta();//<FRBRalias value="Общ регламент относно защитата на данните" name="#ShortTitle" />
            docMeta.Title = Regex.Match(xmlDoc, @"\<FRBRname[^\<\>]*\svalue\s?\=\s?""([^""]+)""")?.Groups[1]?.Value.Replace("&#xD;", " ").Replace("&#xA;", " ").Replace("  ", " ");
            docMeta.ShortTitle = Regex.Match(xmlDoc, @"\<FRBRalias[^\<\>]*\svalue\s?\=\s?""([^""]+)""\s[^\<\>]*name\s?\=\s?""\s?\#ShortTitle\s?""")?.Groups[1]?.Value.Replace("&#xD;", " ").Replace("&#xA;", " ").Replace("  ", " ");
            if (docMeta.ShortTitle == string.Empty)
            {
                docMeta.ShortTitle = Regex.Match(xmlDoc, @"\<FRBRalias[^\<\>]*\sname\s?\=\s?""\s?\#ShortTitle\s?""\s[^\<\>]*value\s?\=\s?""([^""]+)""")?.Groups[1]?.Value.Replace("&#xD;", " ").Replace("&#xA;", " ").Replace("  ", " ");
            }

            docMeta.Language = Regex.Match(xmlDoc, @"\<FRBRlanguage[^\<\>]*\slanguage\s?\=\s?""([^""]+)""").Groups[1].Value;
            docMeta.Country = Regex.Match(xmlDoc, @"\<FRBRcountry[^\<\>]*\svalue\s?\=\s?""([^""]+)""").Groups[1].Value;
            docMeta.Idenitifier = Regex.Match(xmlDoc, @"\<[^\<\>]*class\s?=\s?""\#DocumentIdentifier""[^\<\>]*\>([^\<\>]+)").Groups[1].Value;
            docMeta.DocNumber = Regex.Match(xmlDoc, @"\<FRBRnumber[^\<\>]*eId\s?=\s?""#ConsLegNr""[^\<\>]*\svalue\s?\=\s?""([^""]+)""").Groups[1].Value;
            if (String.IsNullOrEmpty(docMeta.DocNumber))
            {
                docMeta.DocNumber = Regex.Match(xmlDoc, @"\<FRBRnumber[^\<\>]*eId\s?=\s?""#DocNr""[^\<\>]*\svalue\s?\=\s?""([^""]+)""").Groups[1].Value;
            }

            Match publicationDateMatch = Regex.Match(xmlDoc, @"\<FRBRdate[^\<\>]*\sdate\s?\=\s?""([^""]+)""[^\<\>]+name\s?\=\s?""\#DatePublication""");
            if (publicationDateMatch.Success && DateTime.TryParse(publicationDateMatch.Groups[1].Value, out var publDate))
            {
                docMeta.PublicationDate = publDate;
            }

            Match actDateMatch = Regex.Match(xmlDoc, @"\<FRBRdate[^\<\>]*\sdate\s?\=\s?""([^""]+)""[^\<\>]+name\s?\=\s?""\#DateDocument""");
            if (actDateMatch.Success && DateTime.TryParse(actDateMatch.Groups[1].Value, out var actDate))
            {
                docMeta.ActDate = actDate;
            }

            Match lastChangedDateMatch = Regex.Match(xmlDoc, @"\<FRBRdate[^\<\>]*\sdate\s?\=\s?""([^""]+)""[^\<\>]+name\s?\=\s?""\#DateLastChangedEUCases""");
            if (lastChangedDateMatch.Success && DateTime.TryParse(lastChangedDateMatch.Groups[1].Value, out var lastChangedDate))
            {
                docMeta.LastChangeDate = lastChangedDate;
            }

            Match matchDocType = Regex.Match(xmlDoc, @"\<([^\<\>\s]+)[^\<\>]*\>\s*\<meta[\s\/\>]");
            if (matchDocType.Success)
            {
                switch (matchDocType.Groups[1].Value)
                {
                    case "judgment":
                        docMeta.DocType = 1;
                        break;
                    case "act":
                        docMeta.DocType = 2;
                        break;
                    case "doc":
                        docMeta.DocType = 3;
                        break;
                    default:
                        break;
                }
            }
            return docMeta;

        }

        public static SmeDoc DoDocConvert(HtmlDocument htmlDoc, string xmlDoc)
        {
            SmeDoc devidedDoc = new SmeDoc();

            devidedDoc.Meta = GetSmeDocMeta(xmlDoc);

            var mainDiv = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'d-judgmentBody')]|//div[contains(@class,'d-mainBody')]|//div[contains(@class,'d-body')]");

            if (mainDiv != null && devidedDoc.Meta.DocType != 2)
            {
                SmeDocItem mainEl = new SmeDocItem();
                mainEl.Text = mainDiv.OuterHtml;
                mainEl.TreeLevel = 0;

                devidedDoc.Items = new List<SmeDocItem>();
                devidedDoc.Items.Add(mainEl);
            }
            else
            {
                var gdprConsVersionCheck = Regex.Match(xmlDoc, @"\<FRBRnumber[^\<\>]*eId\s?=\s?""#ConsLegNr""[^\<\>]*\svalue\s?\=\s?""02016R0679");
                bool isConsGdpr = gdprConsVersionCheck.Success;

                devidedDoc.Items = GetRecitalsFromPrefaceHtml(htmlDoc);
                devidedDoc.Items.AddRange(GetDevidedLegalPartFromBodyHtml(htmlDoc, devidedDoc.Meta.Idenitifier));
            }

            if (devidedDoc.Meta.Idenitifier != null && Regex.IsMatch(devidedDoc.Meta.DocNumber, @"^[30]2016R0679", RegexOptions.IgnoreCase))
            {
                var smeItems = devidedDoc.Items;
                GetRecitalsAndOldArticlesRelations(artsAndRecs, artsAndOldArts, smeItems);
                devidedDoc.Items = smeItems;
            }

            //var returnedDocs = JsonConvert.SerializeObject(devidedDoc);

            return devidedDoc;
        }

        private static void GetRecitalsAndOldArticlesRelations(Dictionary<string, List<string>> artsAndRecs, Dictionary<string, List<string>> artsAndOldArts, List<SmeDocItem> smeItems)
        {
            foreach (var item in smeItems)
            {
                if (artsAndRecs.ContainsKey(item.Id))
                {
                    item.HasRecitals = true;
                    item.Recitals.AddRange(artsAndRecs[item.Id]);
                }

                if (artsAndOldArts.ContainsKey(item.Id))
                {
                    item.OldArticles.AddRange(artsAndOldArts[item.Id]);
                }

                if (artsAndRecs.Values.Any(x => x.Contains(item.Id)))
                {
                    item.Articles.AddRange(artsAndRecs.Where(x => x.Value.Contains(item.Id)).Select(x => x.Key).ToList());
                }

                if (item.Childs != null && item.Childs.Count > 0)
                {
                    GetRecitalsAndOldArticlesRelations(artsAndRecs, artsAndOldArts, item.Childs);
                }
            }
        }

        public static List<SmeDocItem> GetRecitalsFromPrefaceHtml(HtmlDocument htmlDoc)
        {
            List<SmeDocItem> resultItems = new List<SmeDocItem>();

            //var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@class,'docTitle')]");
            //if (titleNode != null && !string.IsNullOrWhiteSpace(titleNode.InnerText))
            //{
            //    SmeDocItem titleItem = new SmeDocItem();
            //    titleItem.Type = SmeDocItemType.Title;
            //    titleItem.Text = titleNode.OuterHtml;
            //    resultItems.Add(titleItem);
            //}

            var allContentDivs = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class,'d-recital')]");

            if (allContentDivs == null)
            {
                allContentDivs = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class,'d-preamble')]//table");
            }

            if (allContentDivs == null)
            {
                allContentDivs = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class,'d-preface')]//table");

                if (allContentDivs == null || !allContentDivs.Any(x => Regex.IsMatch(x.InnerHtml, @"d-recitals")))
                {
                    var preface = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'d-preface')]");
                    if (preface != null && !string.IsNullOrWhiteSpace(preface.InnerText))
                    {
                        SmeDocItem prefaceEl = new SmeDocItem();
                        prefaceEl.Text = preface.OuterHtml;
                        prefaceEl.Type = SmeDocItemType.Preface;
                        resultItems.Add(prefaceEl);

                        return resultItems;
                    }
                }
            }
            else
            {
                var preface = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'d-preface')]");
                if (preface != null && !string.IsNullOrWhiteSpace(preface.InnerText))
                {
                    SmeDocItem prefaceEl = new SmeDocItem();
                    prefaceEl.Text = preface.OuterHtml;
                    prefaceEl.Type = SmeDocItemType.Preface;
                    resultItems.Add(prefaceEl);
                }
            }

            SmeDocItem notRecItem = new SmeDocItem();
            notRecItem.Type = SmeDocItemType.Text;

            if (allContentDivs != null)
            {
                foreach (HtmlNode divNode in allContentDivs)
                {
                    if (divNode.Attributes[@"class"].Value.Contains("d-recitals"))
                    {
                        continue;
                    }

                    Match matchRec = Regex.Match(divNode.InnerText, @"^\s*\((\d+)\)\s*");

                    if (matchRec.Success)
                    {
                        if (notRecItem != null && !string.IsNullOrWhiteSpace(notRecItem.Text))
                        {
                            resultItems.Add(notRecItem);
                            notRecItem = null;
                        }

                        SmeDocItem recItem = new SmeDocItem();
                        recItem.Type = SmeDocItemType.Recital;
                        recItem.Id = $"rec_{matchRec.Groups[1].Value}";
                        recItem.Text = divNode.OuterHtml;

                        resultItems.Add(recItem);
                    }
                    else if (!string.IsNullOrWhiteSpace(divNode.OuterHtml.Trim()))
                    {
                        if (notRecItem == null)
                        {
                            notRecItem = new SmeDocItem();
                            notRecItem.Type = SmeDocItemType.Text;
                        }

                        if (!string.IsNullOrWhiteSpace(divNode.InnerText))
                        {
                            notRecItem.Text += divNode.OuterHtml;
                        }
                    }
                }

                if (notRecItem != null && !string.IsNullOrWhiteSpace(notRecItem.Text))
                {
                    resultItems.Add(notRecItem);
                }
            }

            //if (isConsGdpr && !resultItems.Any(x => x.Id.StartsWith("rec_")))
            //{
            //    var url = @"https://smedata.apis.bg/api/values/gdprRecitalsEn";

            //    switch (language)
            //    {
            //        case "bul":
            //            url = @"https://smedata.apis.bg/api/values/gdprRecitalsBg";
            //            break;
            //        case "ita":
            //            url = @"https://smedata.apis.bg/api/values/gdprRecitalsIt";
            //            break;
            //        default:
            //            break;
            //    }

            //    using (HttpClient client = new HttpClient())
            //    {
            //        var response = client.GetAsync(url);
            //        var json = response.Result.Content.ReadAsStringAsync().Result;

            //        resultItems = JsonConvert.DeserializeObject<List<SmeDocItem>>(json);
            //    }
            //}

            if (resultItems == null)
            {
                return new List<SmeDocItem>();
            }

            return resultItems;
        }

        public static List<SmeDocItem> GetDevidedLegalPartFromBodyHtml(HtmlDocument htmlDoc, string docIdent)
        {
            List<SmeDocItem> resultItems = new List<SmeDocItem>();

            var mainAnchors = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'d-body')]")?.SelectNodes("./a[@class='doc-anchor']");

            if (mainAnchors != null)
            {
                //foreach (HtmlNode currAnchor in mainAnchors)
                for (int i = 0; i < mainAnchors.Count; i++)
                {
                    string currBaseEId = mainAnchors[i].Attributes[@"eId"]?.Value;
                    if (!string.IsNullOrWhiteSpace(currBaseEId))
                    {
                        SmeDocItem currentBaseEl = new SmeDocItem();
                        currentBaseEl.Text += mainAnchors[i].OuterHtml;
                        currentBaseEl.TreeLevel = 0;
                        SetElementIdAndType(currBaseEId, currentBaseEl);

                        var currBaseNode = mainAnchors[i].NextSibling;
                        if (currBaseNode.Name == "#text" || string.IsNullOrWhiteSpace(currBaseNode.InnerText))
                        {
                            currBaseNode = currBaseNode.NextSibling;
                        }

                        GetAndSetChildsNodes(currentBaseEl, currBaseNode, docIdent);

                        if (Regex.IsMatch(Regex.Replace(currentBaseEl.Text, @"\<[^\<\>]*\>", string.Empty).Trim(), @"[\p{L}\d]"))
                        {
                            resultItems.Add(currentBaseEl);
                        }
                    }
                }
            }
            else
            {
                mainAnchors = htmlDoc.DocumentNode.SelectNodes("//div/p|//div/span|//div/ul");

                foreach (HtmlNode currAnchor in mainAnchors)
                {
                    SmeDocItem currentBaseEl = new SmeDocItem();
                    currentBaseEl.Text += currAnchor.OuterHtml;
                    currentBaseEl.TreeLevel = 0;

                    if (Regex.IsMatch(Regex.Replace(currentBaseEl.Text, @"\<[^\<\>]*\>", string.Empty).Trim(), @"[\p{L}\d]"))
                    {
                        resultItems.Add(currentBaseEl);
                    }
                }
            }

            var additonalDocuments = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class,'d-mainBody')]");

            if (additonalDocuments?.Count > 0)
            {
                foreach (HtmlNode additionalDoc in additonalDocuments)
                {
                    SmeDocItem additionalDocElement = new SmeDocItem();
                    additionalDocElement.Text = additionalDoc.OuterHtml;
                    additionalDocElement.TreeLevel = 0;

                    if (Regex.IsMatch(Regex.Replace(additionalDocElement.Text, @"\<[^\<\>]*\>", string.Empty).Trim(), @"[\p{L}\d]"))
                    {
                        resultItems.Add(additionalDocElement);
                    }
                }
            }

            return resultItems;
        }

        private static void GetAndSetChildsNodes(SmeDocItem currentBaseEl, HtmlNode currBaseNode, string docIdent)
        {
            if (currBaseNode != null)
            {
                currentBaseEl.Text += Regex.Match(currBaseNode.OuterHtml, @"^\s*\<[^\<\>]+\>").Value;

                bool isChildNodeNext = false;

                for (int i = 0; i < currBaseNode.ChildNodes.Count; i++)
                {
                    if (isChildNodeNext)
                    {
                        isChildNodeNext = false;
                        continue;
                    }

                    if (currBaseNode.ChildNodes[i].Attributes[@"class"] != null)
                    {
                        if (currBaseNode.ChildNodes[i].Attributes[@"class"].Value.Contains("d-num"))
                        {
                            currentBaseEl.Heading = currBaseNode.ChildNodes[i].InnerText.Trim();
                        }
                        else if (currBaseNode.ChildNodes[i].Attributes[@"class"].Value.Contains("d-heading"))
                        {
                            currentBaseEl.SubHeading = currBaseNode.ChildNodes[i].InnerText.Trim();
                        }
                    }

                    if (currBaseNode.ChildNodes[i].Name.ToLower() != "a")
                    {
                        currentBaseEl.Text += currBaseNode.ChildNodes[i].OuterHtml;
                    }
                    else
                    {
                        string childEId = currBaseNode.ChildNodes[i].Attributes[@"eId"]?.Value;
                        if (!string.IsNullOrWhiteSpace(childEId))
                        {
                            SmeDocItem childBaseEl = new SmeDocItem();
                            childBaseEl.TreeLevel = currentBaseEl.TreeLevel + 1;
                            childBaseEl.Text += currBaseNode.ChildNodes[i].OuterHtml;
                            SetElementIdAndType(childEId, childBaseEl);
                            childBaseEl.FullId = childEId;
                            var childBaseNode = currBaseNode.ChildNodes[i].NextSibling;
                            if (childBaseNode.Name == "#text" || string.IsNullOrWhiteSpace(childBaseNode.InnerText))
                            {
                                childBaseNode = childBaseNode?.NextSibling;
                                i++;
                            }

                            isChildNodeNext = true;

                            if (Regex.IsMatch(childBaseEl.Id, @"^art_", RegexOptions.IgnoreCase))
                            {
                                childBaseEl.Text += Regex.Match(childBaseNode.OuterHtml, @"^\s*\<[^\<\>]+\>").Value;

                                foreach (var artChild in childBaseNode.ChildNodes)
                                {
                                    if (artChild.Attributes[@"class"] != null)
                                    {
                                        if (artChild.Attributes[@"class"].Value.Contains("d-num"))
                                        {
                                            childBaseEl.Heading = artChild.InnerText.Trim();
                                        }
                                        else
                                        {
                                            Match matchSubHeading = Regex.Match(artChild.InnerHtml, @"\<[^\<\>]+class\s?=\s?['""][^'""]*?d-c-sti-art[^'""]*?['""][^\<\>]*\>([^\<\>]+)\<");

                                            if (matchSubHeading.Success)
                                            {
                                                childBaseEl.SubHeading = matchSubHeading.Groups[1].Value;
                                            }
                                        }
                                    }

                                    if (artChild.Name.ToLower() != "div")
                                    {
                                        childBaseEl.Text += artChild.OuterHtml;
                                    }
                                    else
                                    {
                                        childBaseEl.Text += Regex.Match(artChild.OuterHtml, @"^\s*\<[^\<\>]+\>").Value;

                                        foreach (var divChild in artChild.ChildNodes)
                                        {
                                            if (divChild.Name.ToLower() == "p" || (docIdent != null && (docIdent == "51300c8d-df7b-4924-9e00-7538863d5324" || docIdent == "634976e7-84bd-4723-b818-66bd5b5d6473" || docIdent == "64147400-389d-4212-b1ab-eba5e522a30d") && Regex.IsMatch(divChild.InnerText.Trim(), @"^\(?\d+\)")))
                                            {
                                                Match matchNumber = Regex.Match(divChild.InnerText.Trim(), @"^\d+(?=\.)");

                                                if (!matchNumber.Success)
                                                {
                                                    matchNumber = Regex.Match(divChild.InnerText.Trim(), @"(?<=^\(|^)\d+(?=\))");
                                                }

                                                if (matchNumber.Success)
                                                {
                                                    SmeDocItem parEl = new SmeDocItem();
                                                    parEl.TreeLevel = childBaseEl.TreeLevel + 1;
                                                    parEl.Text = divChild.OuterHtml;
                                                    var parEId = $"{childBaseEl.Id}__subPar_{matchNumber.Value}";
                                                    SetElementIdAndType(parEId, parEl);
                                                    parEl.FullId = $"{childBaseEl.FullId}__subPar_{matchNumber.Value}";

                                                    childBaseEl.Childs.Add(parEl);
                                                }
                                                else
                                                {
                                                    if (childBaseEl.Childs.Count == 0)
                                                    {
                                                        childBaseEl.Text += divChild.OuterHtml;
                                                    }
                                                    else
                                                    {
                                                        if (childBaseEl.Childs.Last().Childs.Count == 0)
                                                        {
                                                            childBaseEl.Childs.Last().Text += divChild.OuterHtml;
                                                        }
                                                        else
                                                        {
                                                            if (childBaseEl.Childs.Last().Childs.Last().Childs.Count == 0)
                                                            {
                                                                childBaseEl.Childs.Last().Childs.Last().Text += divChild.OuterHtml;
                                                            }
                                                            else
                                                            {
                                                                childBaseEl.Childs.Last().Childs.Last().Childs.Last().Text += divChild.OuterHtml;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else if (divChild.Name.ToLower() == "table")
                                            {
                                                SmeDocItem currEl = new SmeDocItem();
                                                currEl.TreeLevel = childBaseEl.TreeLevel + 1;
                                                currEl.Text = divChild.OuterHtml;
                                                var currEId = string.Empty;

                                                Match matchBegin = Regex.Match(divChild.InnerText.Trim(), @"^\(?(\p{L})\)");

                                                if (matchBegin.Success)
                                                {
                                                    currEId = $"{(childBaseEl.Childs.Count > 0 ? childBaseEl.Childs.Last().Id : childBaseEl.Id)}__let_{matchBegin.Groups[1].Value}";
                                                }
                                                else
                                                {
                                                    matchBegin = Regex.Match(divChild.InnerText.Trim(), @"^\(?(\d+)\)");

                                                    if (matchBegin.Success)
                                                    {
                                                        currEId = $"{(childBaseEl.Childs.Count > 0 ? childBaseEl.Childs.Last().Id : childBaseEl.Id)}__pt_{matchBegin.Groups[1].Value}";
                                                    }
                                                    else
                                                    {
                                                        if (childBaseEl.Childs.Count == 0)
                                                        {
                                                            childBaseEl.Text += divChild.OuterHtml;
                                                        }
                                                        else
                                                        {
                                                            if (childBaseEl.Childs.Last().Childs.Count == 0)
                                                            {
                                                                childBaseEl.Childs.Last().Text += divChild.OuterHtml;
                                                            }
                                                            else
                                                            {
                                                                if (childBaseEl.Childs.Last().Childs.Last().Childs.Count == 0)
                                                                {
                                                                    childBaseEl.Childs.Last().Childs.Last().Text += divChild.OuterHtml;
                                                                }
                                                                else
                                                                {
                                                                    childBaseEl.Childs.Last().Childs.Last().Childs.Last().Text += divChild.OuterHtml;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (!string.IsNullOrWhiteSpace(currEId))
                                                {
                                                    SetElementIdAndType(currEId, currEl);

                                                    currEl.FullId = $"{childBaseEl.FullId}__{currEId}";
                                                    currEl.FullId = Regex.Replace(currEl.FullId, @"(art_[^-]+)__art_[^_]+", @"$1");
                                                    currEl.FullId = Regex.Replace(currEl.FullId, @"(par_[^-]+)__par_[^_]+", @"$1");

                                                    if (childBaseEl.Childs.Count > 0)
                                                    {
                                                        currEl.TreeLevel = childBaseEl.Childs.Last().TreeLevel + 1;
                                                        childBaseEl.Childs.Last().Childs.Add(currEl);
                                                    }
                                                    else
                                                    {
                                                        childBaseEl.Childs.Add(currEl);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (childBaseEl.Childs.Count == 0)
                                                {
                                                    childBaseEl.Text += divChild.OuterHtml;
                                                }
                                                else
                                                {
                                                    if (childBaseEl.Childs.Last().Childs.Count == 0)
                                                    {
                                                        childBaseEl.Childs.Last().Text += divChild.OuterHtml;
                                                    }
                                                    else
                                                    {
                                                        if (childBaseEl.Childs.Last().Childs.Last().Childs.Count == 0)
                                                        {
                                                            childBaseEl.Childs.Last().Childs.Last().Text += divChild.OuterHtml;
                                                        }
                                                        else
                                                        {
                                                            childBaseEl.Childs.Last().Childs.Last().Childs.Last().Text += divChild.OuterHtml;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        childBaseEl.Text += Regex.Match(artChild.OuterHtml, @"\<\/[^\<\>]+\>\s*$").Value;
                                    }
                                }

                                childBaseEl.Text += Regex.Match(childBaseNode.OuterHtml, @"\<\/[^\<\>]+\>\s*$").Value;
                            }
                            else
                            {
                                GetAndSetChildsNodes(childBaseEl, childBaseNode, docIdent);
                            }

                            currentBaseEl.Childs.Add(childBaseEl);
                        }
                    }
                }

                currentBaseEl.Text += Regex.Match(currBaseNode.OuterHtml, @"\<\/[^\<\>]+\>\s*$").Value;
            }
        }

        private static void SetElementIdAndType(string currBaseEId, SmeDocItem currentBaseEl)
        {
            Match matchType = Regex.Match(currBaseEId, @"([^_]+)_([^_]+)$");

            if (matchType.Success)
            {
                if (currBaseEId.StartsWith("art_") || currBaseEId.StartsWith("par_"))
                {
                    currentBaseEl.Id = currBaseEId;
                }
                else
                {
                    currentBaseEl.Id = matchType.Value;
                }

                switch (matchType.Groups[1].Value)
                {
                    case "tit":
                        currentBaseEl.Type = SmeDocItemType.Title;
                        break;
                    case "rec":
                        currentBaseEl.Type = SmeDocItemType.Recital;
                        break;
                    case "sec":
                        currentBaseEl.Type = SmeDocItemType.Section;
                        break;
                    case "chap":
                        currentBaseEl.Type = SmeDocItemType.Chapter;
                        break;
                    case "part":
                        currentBaseEl.Type = SmeDocItemType.Part;
                        break;
                    case "art":
                        currentBaseEl.Type = SmeDocItemType.Article;
                        break;
                    case "subPar":
                        currentBaseEl.Type = SmeDocItemType.SubParagraph;
                        break;
                    case "par":
                        currentBaseEl.Type = SmeDocItemType.Paragraph;
                        break;
                    case "pt":
                        currentBaseEl.Type = SmeDocItemType.Point;
                        break;
                    case "sent":
                        currentBaseEl.Type = SmeDocItemType.Sentence;
                        break;
                    case "let":
                        currentBaseEl.Type = SmeDocItemType.Letter;
                        break;
                    case "num":
                        currentBaseEl.Type = SmeDocItemType.Number;
                        break;
                    default:
                        currentBaseEl.Type = SmeDocItemType.Text;
                        break;
                }
            }
        }
    }
}
