﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;

namespace ProjetManhattan.Sources
{
    internal class FichierDeLogIIS : IFichierDeLog
    {
        private string _cheminDeFichier;
        private string[] _lignes;
        private int _currentLine;

        public FichierDeLogIIS(BaseConfig config)
        {
            _cheminDeFichier = GetCheminFichierLogAndModifyingDate(config);
            _lignes = File.ReadAllLines(_cheminDeFichier);
            _currentLine = 0;
        }

        private static string GetCheminFichierLogAndModifyingDate(BaseConfig config)
        {
            // Format type fichier log C:\inetpub\logs\LogFiles\W3SVC1\u_ex{YY}{MM}{DD}.log
            string fileName = config.CheminFichierLog;
            fileName = fileName
                        .Replace("YY", config.DateTraitement.ToString("yy"))
                        .Replace("MM", config.DateTraitement.ToString("MM"))
                        .Replace("DD", config.DateTraitement.ToString("dd"));

            if (!File.Exists(fileName))
            {                
                throw new TraitementExecutionException($"Le fichier de log {fileName} n'existe pas");
            }
            else
            {
                return fileName;
            }
        }

        public bool HasLines()
        {
            return _currentLine < _lignes.Length;
        }

        public LigneDeLog? ReadLine()
        {
            string line = _lignes[_currentLine];
            _currentLine++;

            if (line[0] != '#')
            {
                string[] champs = line.Split(' ');
                LigneDeLog parsedLine = new LigneDeLog(champs);
                return parsedLine;
            }
            else
            {
                return null;
            }
        }
    }
}
